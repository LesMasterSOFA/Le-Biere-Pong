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
    public class NetworkClient : Microsoft.Xna.Framework.GameComponent
    {
        //Objet Client
        static NetClient Client;

        //Liste de joueur
        static List<JoueurMultijoueur> ListeJoueurs;

        // indique si le client roule
        public static bool EstEnMarche = false;

        DateTime Temps { get; set; }
        public string NomJeu { get; private set; }
        public int Port { get; private set; }
        public string HostIP{ get; private set; } //ip de l'host
        NetIncomingMessage MessageInc { get; set; } //message entrant
        NetOutgoingMessage MessageOut { get; set; } //message sortant
        public string NomJoueur { get; private set; }
        TimeSpan IntervalleRafraichissement { get; set; }
        NetworkServer Serveur { get; set; }
        public bool EstMaster { get; private set; }

        public NetworkClient(Game jeu, string nomJeu, int port, string nomJoueur, NetworkServer serveur, bool estMaster):base(jeu)
        {
            NomJeu = nomJeu;
            Port = port;
            NomJoueur = nomJoueur;
            Serveur = serveur;
            EstMaster = estMaster;
            Create(NomJeu, Port);
            Connect();
            ListeJoueurs = new List<JoueurMultijoueur>();
            IntervalleRafraichissement = new TimeSpan(0, 0, 0, 0, 30); //30 ms
        }

        public NetworkClient(Game jeu, string nomJeu, string adresse, int port, string nomJoueur, NetworkServer serveur, bool estMaster)
            : base(jeu)
        {
            NomJeu = nomJeu;
            HostIP = adresse;
            Port = port;
            NomJoueur = nomJoueur;
            Serveur = serveur;
            EstMaster = estMaster;
            Create(NomJeu, Port);
            Connect();
            ListeJoueurs = new List<JoueurMultijoueur>();
            IntervalleRafraichissement = new TimeSpan(0, 0, 0, 0, 30); //30 ms
        }

        void Create(string nomJeu, int port)
        {
            // Demande l'ip si aucune adresse a été fournie
            if (HostIP == null)
            {
                Console.WriteLine("Enter IP To Connect");
                HostIP = Console.ReadLine();
            }

            //Crée la configuration du client -> doit avoir le même nom que le serveur
            NetPeerConfiguration Config = new NetPeerConfiguration(NomJeu);
            //Config.Port = Port;
            Client = new NetClient(Config);
            Client.Start();
            Temps = DateTime.Now;
            Console.WriteLine("Client créé à " + Temps.ToString());
        }

        void Connect()
        {
            try
            {
                //Création nouveau message sortant
                MessageOut = Client.CreateMessage();
                //Écrit le type de message à envoyer à partir de l'énumération
                MessageOut.Write((byte)PacketTypes.LOGIN);
                //Écrit le nom du joueur
                MessageOut.Write(NomJoueur);
                //Connecte le client au serveur 
                Client.Connect(HostIP, Port, MessageOut);

                Temps = DateTime.Now;
                Console.WriteLine("Connection du client envoyée à " + Temps);

                //Fonction attendant l'approbation de connection du serveur
                AttenteConnectionServeur();

                Console.WriteLine("Connection bien reçu du serveur à " + Temps);
                EstEnMarche = true;
            }

            //Doit être amélioré pour ajouter d'autre exceptions, mais cest un début
            //Probablement revoir la structure
            catch(NetworkNotAvailableException)
            {
                Console.WriteLine("La connection est invalide -> peut-être l'adresse est erronée?");
                Menu menu = new Menu(Game);
                Game.Components.Add(menu);
                menu.BoutonsLAN();
            }
            catch (NetException)
            {
                Console.WriteLine("Adresse éronnée");
                Menu menu = new Menu(Game);
                Game.Components.Add(menu);
                menu.BoutonsLAN();
            }

            catch(Exception)
            { 
                Console.WriteLine("Exception client");
                throw new Exception(); //Envoie de l'exception vers network manager
            }

        }

        //Attente du message de connection pour instancier les joueurs
        private void AttenteConnectionServeur()
        {
            //Détermine si le client peut démarer
            bool PeutPartir = false;

            //Loop tant que le client ne peut pas démarrer
            while (!PeutPartir)
            {
                //Court-circuite la fonction update du serveur étant donné qu'elle ne sera pas appelée 
                //tant que nous serons dans cette fonction 
                //Doit avoir une condition pour faire sur que le serveur n'est pas partie
                if(Serveur != null)
                    Serveur.UpdateServeur();

                //Regarde si un nouveau message est arrivé
                if ((MessageInc = Client.ReadMessage()) != null)
                {
                    //Dépend du type de message
                    switch (MessageInc.MessageType)
                    {
                        //Tous les mesages envoyés manuellement sont de type Data
                        case NetIncomingMessageType.Data:

                            //Lit le premier byte repsésentant le type de message dans l'énumération
                            if (MessageInc.ReadByte() == (byte)PacketTypes.WORLDSTATE)
                            {
                                //Reste à implanter quoi faire -> début ici 
                                //WorldStateUpdate();

                                //Après que tous les joueurs sont instanciés, on peut partir le jeu
                                PeutPartir = true;
                            }
                            break;
                            
                        case NetIncomingMessageType.StatusChanged:
                            if(MessageInc.ReadString() == "=Failed")
                                throw new NetworkNotAvailableException("La connection a échouée");
                            break;

                        default:
                            //ne devrait pas arriver, envoie un message d'erreur
                            string messageRecu = MessageInc.ReadString();
                            Console.WriteLine( messageRecu + " Message reçu non géré");
                            break;
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Si l'intervalle de temps est passé
            if ((Temps + IntervalleRafraichissement) < DateTime.Now)
            {
                GérerÉvénementEtEnvoyerAuServeur();

                //Regarde si le serveur a envoyé un message
                RegarderNouveauMessageServeur();

                //Update le temps
                Temps = DateTime.Now;
            }
            base.Update(gameTime);
        }

        //Update le monde
        void WorldStateUpdate()
        {
            //Console.WriteLine("WorldState Update");

            //On vide la liste des joueurs contenant les informations
            if (ListeJoueurs != null)
                ListeJoueurs.Clear();

            // Declare count
            int NbDeJoueurs = 0;

            //On lit le nombre de joueurs envoyé dans le message
            NbDeJoueurs = MessageInc.ReadInt32();

            //On recrée les joueurs présents
            for (int i = 0; i < NbDeJoueurs; i++)
            {
                JoueurMultijoueur j = new JoueurMultijoueur(this.Game, MessageInc.SenderConnection,this);

                //On lit toutes les propriétés du joueur
                MessageInc.ReadAllProperties(j);

                ListeJoueurs.Add(j);
            }
        }

        /// //Regarde s'il y a un nouveau message
        private void RegarderNouveauMessageServeur()
        {
            //Tant qu'il y a un nouveau message
            while ((MessageInc = Client.ReadMessage()) != null)
            {
                if (MessageInc.MessageType == NetIncomingMessageType.Data)
                {
                    //Lit le type d'information
                    byte byteEnum = MessageInc.ReadByte();

                    if (byteEnum == (byte)PacketTypes.WORLDSTATE)
                    {
                        //Reste à implanter quoi faire
                        WorldStateUpdate();
                    }

                    if(byteEnum == (byte)PacketTypes.STARTGAME_INFO)
                    {
                        Console.WriteLine("STARTGAME_INFO recue _ Client");
                        if (EstMaster == false)
                        {
                            RecevoirInfoPartieToClient_Joining(MessageInc.ReadBytes((int)MessageInc.LengthBytes - 1));
                            Console.WriteLine("STARTGAME_INFO gérée");
                            Serveur.PartieEnCours.ActiverPartieSlave();
                            Console.WriteLine("Partie démarrée");
                        }
                    }
                }

            }
        }

        private void GérerÉvénementEtEnvoyerAuServeur()
        {
            //Crée un Message en string contenant les actions
            string message = "";
            //message = GérerAction();

            //Si un action à été effectuée
            if (message != null)
            {
                MessageOut = Client.CreateMessage();

                MessageOut.Write(message);

                // Send it to server
                Client.SendMessage(MessageOut, NetDeliveryMethod.ReliableOrdered);
            }
        }

        public void EnvoyerMessageServeur(PacketTypes typeInfo, string messageToSend)
        {
            if (messageToSend != null)
            {
                MessageOut = Client.CreateMessage();

                MessageOut.Write(messageToSend);

                // Send it to server
                Client.SendMessage(MessageOut, NetDeliveryMethod.ReliableOrdered);
            }
        }

        public void EnvoyerMessageServeur(PacketTypes typeInfo, byte[] messageToSend)
        {
            if (messageToSend != null)
            {
                MessageOut = Client.CreateMessage();

                MessageOut.Write((byte)typeInfo);
                MessageOut.Write(messageToSend);

                // Send it to server
                Client.SendMessage(MessageOut, NetDeliveryMethod.ReliableOrdered);

                Console.WriteLine(MessageOut.ToString());
            }
        }

        public void EnvoyerInfoPartieToServeur_StartGame(Mode1v1LAN partieToSend)
        {
            Console.WriteLine("Essaie Sérialisation");
            try
            {
                InfoMode1v1LAN infoMode1v1LAN = new InfoMode1v1LAN((JoueurMultijoueur)partieToSend.JoueurPrincipal, (JoueurMultijoueur)partieToSend.JoueurSecondaire, partieToSend.gestionnairePartie, partieToSend.EstPartieActive, partieToSend.EnvironnementPartie, partieToSend.Serveur);
                byte[] infoPartie = Serialiseur.ObjToByteArray(infoMode1v1LAN);
                EnvoyerMessageServeur(PacketTypes.STARTGAME_INFO, infoPartie);
            }

            catch (Exception e)
            {
                Console.WriteLine("Erreur dans l'envoie des informations de début de partie au serveur");
                Console.WriteLine(e.ToString());
            }

        }

        public void RecevoirInfoPartieToClient_Joining(byte[] infoPartie)
        {
            Console.WriteLine("Essai Désérialisation");
            try
            {
                //Problème ici, incapable de démarrer une partie. Probablement a cause que les classe serializable s'instancie pas comme il le faut
                InfoMode1v1LAN infoMode1v1LAN = Serialiseur.ByteArrayToObj<InfoMode1v1LAN>(infoPartie);
                Serveur = new NetworkServer(Game, infoMode1v1LAN.InfoServer.NomJeu, infoMode1v1LAN.InfoServer.Port, infoMode1v1LAN.InfoServer.TempsServeurMaster);
                Serveur.PartieEnCours = new Mode1v1LAN(this.Game, infoMode1v1LAN.InfoJoueurPrincipal, infoMode1v1LAN.InfoJoueurSecondaire, infoMode1v1LAN.EstPartieActive, infoMode1v1LAN.InfoGestionnaireEnvironnement, Serveur);
                Game.Components.Add(Serveur.PartieEnCours);
            }

            catch (Exception e)
            {
                Console.WriteLine("Erreur dans la réception et/ou la désérialisation de l'objet");
                Console.WriteLine(e.ToString());
            }
        }
    }
}
