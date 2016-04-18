using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AtelierXNA
{
    public class NetworkServer : Microsoft.Xna.Framework.GameComponent
    {
        // Server object
        static NetServer Serveur;
        // Configuration object
        static NetPeerConfiguration Config;

        public string NomJeu { get; private set; }
        public int Port { get; private set; }
        public DateTime Temps { get; private set; }
        public TimeSpan IntervalleRafraichissement { get; private set; }
        NetIncomingMessage MessageInc { get; set; }
        public List<JoueurMultijoueur> ListeJoueurs { get; private set; }
        byte[] message { get; set; }
        public Mode1v1LAN PartieEnCours { get; set; }
        public long TempsServeurMaster { get; set; }


        public NetworkServer(Game jeu, string nomJeu, int port):base(jeu)
        {
            NomJeu = nomJeu;
            Port = port;
            Create(NomJeu, Port);
            IntervalleRafraichissement = new TimeSpan(0, 0, 0, 0, 30); //30 ms
            Console.WriteLine("Waiting for new connections and updateing world state to current ones");
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
                Console.WriteLine("Server Started" + Temps.ToString());
            }

            //Doit être amélioré pour ajouter d'autre exceptions, mais cest un début
            //Probablement revoir la structure
            catch(Exception)
            {
                Console.WriteLine("Exception Serveur");
                throw new Exception(""); //Envoie de l'exception vers network manager
            }
        }

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

                        //// Détermination tu type d'information
                        //if (byteEnum == (byte)PacketTypes.MOVE)
                        //{
                        //    //On regarde qui a envoyé le message
                        //    foreach (JoueurMultijoueur j in ListeJoueurs)
                        //    {
                        //        if (j.IP == MessageInc.SenderConnection)
                        //        {
                        //            //On gère le message tout dépendant de ce qu'il faut faire( switch/case)
                        //        }
                        //        //EnvoieNouveauMessage();
                        //    }
                        //}

                        if (byteEnum == (byte)PacketTypes.STARTGAME_INFO)
                        {
                            GérerStartGameInfo();
                        }

                        if(byteEnum == (byte)PacketTypes.ANIMATION)
                        {

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

        void EnvoieNouveauMessageWorldState()
        {
            //Création d'un message pouvant être envoyé
            NetOutgoingMessage MessageSortant = Serveur.CreateMessage();

            //Tout d'abord on dit quel sorte de message on envoir en Byte
            //Envoie d'un message renvoyant l'état du monde
            MessageSortant.Write((byte)PacketTypes.WORLDSTATE);

            //Ensuite on écrit l'information à être envoyée
            //Doit être modifié, probablemement un int servant à dire combien de joueurs il y a
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

            //Tout d'abord on dit quel sorte de message on envoir en Byte
            //Envoie d'un message renvoyant l'état du monde
            MessageSortant.Write((byte)typeInfo);

            //Ensuite on écrit l'information à être envoyée
            //Doit être modifié, probablemement un int servant à dire combien de joueurs il y a
            MessageSortant.Write(messageToSend);

            //Envoie du message à toutes les connections dans l'ordre qu'il a été envoyé
            //Serveur.SendMessage(MessageSortant, Serveur.Connections, NetDeliveryMethod.ReliableOrdered, 0);
            Serveur.SendMessage(MessageSortant, ListeJoueurs[indiceJoueur].IP, NetDeliveryMethod.ReliableOrdered, 0);
        }

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
            //Doit être modifié, probablemement un int servant à dire combien de joueurs il y a
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
                    if (!j.Client.EstMaster)
                        indiceJoueur = 1;

                    message = MessageInc.ReadBytes((int)MessageInc.LengthBytes - 1);

                    EnvoieNouveauMessage(PacketTypes.ANIMATION, message, indiceJoueur);
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
