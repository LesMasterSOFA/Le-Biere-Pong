using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
    public class NetworkServer : Microsoft.Xna.Framework.GameComponent
    {
        #region propriétés de la classe

        // Server object
        public NetServer Serveur { get; private set; }
        // Configuration serveur
        NetPeerConfiguration Config;

        public string NomJeu { get; private set; }
        public int Port { get; private set; }
        public DateTime Temps { get; private set; }
        public TimeSpan IntervalleRafraichissement { get; private set; }
        NetIncomingMessage MessageInc { get; set; }
        public List<JoueurMultijoueur> ListeJoueurs { get; private set; }
        byte[] message { get; set; }
        public Mode1v1LAN PartieEnCours { get; set; } //doit pouvoir être modifié dans le mode de jeu
        public long TempsServeurMaster { get; private set; }

        #endregion

        #region Création d'un serveur

        public NetworkServer(Game jeu, string nomJeu, int port):base(jeu)
        {
            NomJeu = nomJeu;
            Port = port;
            Create(NomJeu, Port);
            IntervalleRafraichissement = new TimeSpan(0, 0, 0, 0, 30); //30 ms
            Console.WriteLine("En attente d'une nouvelle connection et update de WorldState");
            ListeJoueurs = new List<JoueurMultijoueur>();
        }

        //Constructeur sérialiseur
        public NetworkServer(Game jeu, string nomJeu, int port, long tempsServeurMaster)
            : base(jeu)
        {
            NomJeu = nomJeu;
            Port = port;
            IntervalleRafraichissement = new TimeSpan(0, 0, 0, 0, 30); //30 ms
            TempsServeurMaster = tempsServeurMaster;
            Console.WriteLine("Waiting for new connections and updateing world state to current ones");
            ListeJoueurs = new List<JoueurMultijoueur>();
        }

        void Create(string nomJeu, int port)
        {
            try
            {
                Config = new NetPeerConfiguration(NomJeu);
                Config.Port = Port;
                Config.MaximumConnections = 2;
                Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
                Serveur = new NetServer(Config);
                Serveur.Start();
                Temps = DateTime.Now;
                Console.WriteLine("Serveur démaré" + Temps.ToString());
            }

            catch(Exception)
            {
                Console.WriteLine("Exception Serveur");
                throw new Exception(""); //Envoie de l'exception vers network manager
            }
        }

        #endregion

        #region Update

        //Fonction pouvant être appelée de l'extérieur de façon à updater le serveur tout le temps
        public void UpdateServeur()
        {
            if ((MessageInc = Serveur.ReadMessage()) != null)
            {
                switch (MessageInc.MessageType)
                {
                    //message reçu lors de la connection initiale
                    //Initialisation de joueur et de partie à faire ici
                    case NetIncomingMessageType.ConnectionApproval:

                        if (MessageInc.ReadByte() == (byte)PacketTypes.LOGIN)
                        {
                            GérerLogin();
                        }
                        break;


                    //Représente tout les messages envoyés manuellement par le client
                    case NetIncomingMessageType.Data:
                        
                        //Lit le type d'information
                        byte byteEnum = MessageInc.ReadByte();

                        if (byteEnum == (byte)PacketTypes.STARTGAME_INFO)
                        {
                            GérerStartGameInfo();
                        }

                        if(byteEnum == (byte)PacketTypes.ANIMATION)
                        {
                            GérerAnimation();
                        }

                        if(byteEnum == (byte)PacketTypes.POSITION_BALLE)
                        {
                            GérerInfoPositionBalle();
                        }

                        if(byteEnum == (byte)PacketTypes.EST_TOUR_JOUEUR_PRINCIPAL_INFO)
                        {
                            GérerInfoEstTourJoueurPrincipal();
                        }

                        if (byteEnum == (byte)PacketTypes.VERRE_À_ENLEVER)
                        {
                            GérerVerreÀEnlever();
                        }

                        if(byteEnum == (byte)PacketTypes.LANCER_BALLE_INFO)
                        {
                            GérerInfoLancerBalle();
                        }

                        break;

                    //S'il y a un message parmi: NetConnectionStatus.Connected, NetConnectionStatus.Connecting ,
                    //NetConnectionStatus.Disconnected, NetConnectionStatus.Disconnecting, NetConnectionStatus.None
                    case NetIncomingMessageType.StatusChanged:

                        GérerChangementStatutJoueur();
                        break;
                    //Pour tout autres types de messages
                    default:
                        Console.WriteLine("Message non géré(non important)");
                        break;
                }
            }
            // Si l'intervalle de temps est passé

            if ((Temps + IntervalleRafraichissement) < DateTime.Now)
            {
                //Regarde s'il y a au moins un client de connecté
                if (Serveur.ConnectionsCount != 0)
                {
                    EnvoieNouveauMessageWorldState();
                }
                //Update le temps
                Temps = DateTime.Now;
            }
        }

        public override void Update(GameTime gameTime)
        {
            UpdateServeur();
        }

        #endregion

        #region Envoie/réception de messages

        void EnvoieNouveauMessageWorldState()
        {
            //Création d'un message pouvant être envoyé
            NetOutgoingMessage MessageSortant = Serveur.CreateMessage();

            //Tout d'abord on dit quel sorte de message on envoie en Byte
            //Envoie d'un message renvoyant l'état du monde
            MessageSortant.Write((byte)PacketTypes.WORLDSTATE);

            //Ensuite on écrit l'information à être envoyée
            MessageSortant.Write(ListeJoueurs.Count);

            //Passe sur tous les joueurs dans le jeu
            foreach (JoueurMultijoueur j in ListeJoueurs)
            {
                // Écrit tous les propriété des objet 
                MessageSortant.WriteAllProperties(j);
            }

            //// Le message contient: le type d'information, le nombre de joueurs et l'information 
            //Envoie du message à toutes les connections dans l'ordre qu'il a été envoyé
            Serveur.SendMessage(MessageSortant, Serveur.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }

        void EnvoieNouveauMessage(PacketTypes typeInfo, byte[] messageToSend, int indiceJoueur)
        {
            //Création d'un message pouvant être envoyé
            NetOutgoingMessage MessageSortant = Serveur.CreateMessage();

            //Tout d'abord on dit quel sorte de message on envoie en Byte
            //Envoie d'un message renvoyant l'état du monde
            MessageSortant.Write((byte)typeInfo);

            //Ensuite on écrit l'information à être envoyée
            MessageSortant.Write(messageToSend);

            //Envoie le message à un joueur en particulier
            Serveur.SendMessage(MessageSortant, ListeJoueurs[indiceJoueur].IP, NetDeliveryMethod.ReliableOrdered, 0);
        }

        #endregion

        #region Fonctions pour Update

        void GérerLogin()
        {
            Console.WriteLine("Incoming LOGIN");

            //Ajout du client
            MessageInc.SenderConnection.Approve();

            //Initialisation de joueur
            //Reste à ajouter le joueur dans le WorldState
            var joueur = new JoueurMultijoueur(Game, MessageInc.SenderConnection);
            ListeJoueurs.Add(joueur);


            // Création d'un message pouvant être envoyé
            NetOutgoingMessage MessageSortant = Serveur.CreateMessage();

            // Tout d'abord on dit quel sorte de message on envoie en Byte
            //Envoie d'un message renvoyant l'état du monde
            //Update la liste de joueurs
            MessageSortant.Write((byte)PacketTypes.WORLDSTATE);

            // Ensuite on écrit l'information à être envoyée
            MessageSortant.Write(ListeJoueurs.Count);

            //Passe sur tous les joueurs dans le jeu
            foreach (JoueurMultijoueur j in ListeJoueurs)
            {
                // Écrit tous les propriété des objet 
                MessageSortant.WriteAllProperties(j);
            }

            //// Le message contient: le type d'information, le nombre de joueurs et l'information 

            //Envoie du message à la connection dans l'ordre qu'il a été envoyé
            Serveur.SendMessage(MessageSortant, MessageInc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);

            // Debug
            Console.WriteLine("Approved new connection and updated the world status");
        }

        void GérerStartGameInfo()
        {
            Console.WriteLine("STARTGAME_INFO recue _ Serveur");
            foreach (JoueurMultijoueur j in ListeJoueurs)
            {
                if (j.IP != MessageInc.SenderConnection)
                {
                    message = MessageInc.ReadBytes((int)MessageInc.LengthBytes - 1);
                    EnvoieNouveauMessage(PacketTypes.STARTGAME_INFO, message, 1);
                }
            }
        }

        void GérerChangementStatutJoueur()
        {
            Console.WriteLine(MessageInc.SenderConnection.ToString() + " status changed. " + (NetConnectionStatus)MessageInc.SenderConnection.Status);

            //À compléter
            if (MessageInc.SenderConnection.Status == NetConnectionStatus.Disconnected || MessageInc.SenderConnection.Status == NetConnectionStatus.Disconnecting)
            {
                Console.WriteLine("Un adversaire s'est déconnecté");
            }
        }

        void GérerAnimation()
        {
            Console.WriteLine("Changement AnimationJoueur");
            foreach (JoueurMultijoueur j in ListeJoueurs)
            {
                if (j.IP != MessageInc.SenderConnection)
                {
                    int indiceJoueur = 0;
                    if (j.IP != ListeJoueurs[0].IP) 
                        indiceJoueur = 1;

                    message = MessageInc.ReadBytes((int)MessageInc.LengthBytes - 1);

                    EnvoieNouveauMessage(PacketTypes.ANIMATION, message, indiceJoueur);
                }
            }
        }

        void GérerInfoPositionBalle()
        {
            Console.WriteLine("Update Position balle");
            foreach (JoueurMultijoueur j in ListeJoueurs)
            {
                if (j.IP != MessageInc.SenderConnection)
                {
                    int indiceJoueur = 0;
                    if (j.IP != ListeJoueurs[0].IP)
                        indiceJoueur = 1;

                    message = MessageInc.ReadBytes((int)MessageInc.LengthBytes - 1);

                    EnvoieNouveauMessage(PacketTypes.POSITION_BALLE, message, indiceJoueur);
                }
            }

        }

        void GérerInfoEstTourJoueurPrincipal()
        {
            Console.WriteLine("Update info est tour joueur principal");
            foreach (JoueurMultijoueur j in ListeJoueurs)
            {
                if (j.IP != MessageInc.SenderConnection)
                {
                    int indiceJoueur = 0;
                    if (j.IP != ListeJoueurs[0].IP)
                        indiceJoueur = 1;

                    message = MessageInc.ReadBytes((int)MessageInc.LengthBytes - 1);

                    EnvoieNouveauMessage(PacketTypes.EST_TOUR_JOUEUR_PRINCIPAL_INFO, message, indiceJoueur);
                }
            }
        }

        void GérerVerreÀEnlever()
        {
            Console.WriteLine("Update verre à enlever");
            foreach (JoueurMultijoueur j in ListeJoueurs)
            {
                if (j.IP != MessageInc.SenderConnection)
                {
                    int indiceJoueur = 0;
                    if (j.IP != ListeJoueurs[0].IP)
                        indiceJoueur = 1;

                    message = MessageInc.ReadBytes((int)MessageInc.LengthBytes - 1);

                    EnvoieNouveauMessage(PacketTypes.VERRE_À_ENLEVER, message, indiceJoueur);
                }
            }
        }

        void GérerInfoLancerBalle()
        {
            Console.WriteLine("Update lancer balle");
            foreach (JoueurMultijoueur j in ListeJoueurs)
            {
                if (j.IP != MessageInc.SenderConnection)
                {
                    int indiceJoueur = 0;
                    if (j.IP != ListeJoueurs[0].IP)
                        indiceJoueur = 1;

                    message = MessageInc.ReadBytes((int)MessageInc.LengthBytes - 1);

                    EnvoieNouveauMessage(PacketTypes.LANCER_BALLE_INFO, message, indiceJoueur);
                }
            }
        }

        #endregion

    }
    
    [Serializable]
    public class InfoNetworkServer
    {
        public int Port { get; private set; }
        public string NomJeu { get; private set; }
        public long TempsServeurMaster { get; set; }
        public InfoNetworkServer(int port, string nomJeu, DateTime tempsServeurMaster)
        {
            Port = port;
            NomJeu = nomJeu;
            TempsServeurMaster = tempsServeurMaster.ToBinary();
        }
    }
}
