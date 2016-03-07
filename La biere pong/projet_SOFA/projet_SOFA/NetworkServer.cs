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

        string NomJeu { get; set; }
        int Port { get; set; }
        DateTime Temps { get; set; }
        TimeSpan IntervalleRafraichissement { get; set; }
        NetIncomingMessage MessageInc { get; set; }
        List<Joueur> ListeJoueurs { get; set; }


        public NetworkServer(Game jeu, string nomJeu, int port):base(jeu)
        {
            NomJeu = nomJeu;
            Port = port;
            Create(NomJeu, Port);
            IntervalleRafraichissement = new TimeSpan(0, 0, 0, 0, 30); //30 ms
            Console.WriteLine("Waiting for new connections and updateing world state to current ones");
            
            ListeJoueurs = new List<Joueur>();
        }

        void Create(string nomJeu, int port)
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
                            Console.WriteLine("Incoming LOGIN");

                            //Ajout du client
                            MessageInc.SenderConnection.Approve();

                            //Initialisation de joueur
                            //Reste à ajouter le joueur dans le WorldState
                            var joueur = new Joueur(Game, MessageInc.SenderConnection);


                            // Création d'un message pouvant être envoyé
                            NetOutgoingMessage MessageSortant = Serveur.CreateMessage();

                            // Tout d'abord on dit quel sorte de message on envoir en Byte
                            //Envoie d'un message renvoyant l'état du monde
                            MessageSortant.Write((byte)PacketTypes.WORLDSTATE);

                            // Ensuite on écrit l'information à être envoyée
                            //Doit être modifié, probablemement un int servant à dire combien de joueurs il y a
                            MessageSortant.Write(ListeJoueurs.Count);

                            //Passe sur tous les joueurs dans le jeu
                            foreach (Joueur j in ListeJoueurs)
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
                        break;


                    //Représente tout les messages envoyés manuellement par le client
                    case NetIncomingMessageType.Data:

                        // Détermination tu type d'information
                        if (MessageInc.ReadByte() == (byte)PacketTypes.MOVE)
                        {
                            //On regarde qui a envoyé le message
                            foreach (Joueur j in ListeJoueurs)
                            {
                                if (j.IP == MessageInc.SenderConnection)
                                {
                                    //On gère le message tout dépendant de ce qu'il faut faire( switch/case)
                                }
                                EnvoieNouveauMessage();
                            }
                        }
                        break;

                    //S'il y a un message parmi: NetConnectionStatus.Connected, NetConnectionStatus.Connecting ,
                    //NetConnectionStatus.Disconnected, NetConnectionStatus.Disconnecting, NetConnectionStatus.None

                    case NetIncomingMessageType.StatusChanged:

                        Console.WriteLine(MessageInc.SenderConnection.ToString() + " status changed. " + (NetConnectionStatus)MessageInc.SenderConnection.Status);

                        //Il doit y avoir cette instruction pour enlever le joueur déconnecté
                        if (MessageInc.SenderConnection.Status == NetConnectionStatus.Disconnected || MessageInc.SenderConnection.Status == NetConnectionStatus.Disconnecting)
                        {
                            //On trouve le joueur déconnecté et on l'enlève
                            foreach (Joueur j in ListeJoueurs)
                            {
                                if (j.IP == MessageInc.SenderConnection)
                                {
                                    ListeJoueurs.Remove(j);
                                }
                            }
                        }
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
                    EnvoieNouveauMessage();
                }
                //Update le temps
                Temps = DateTime.Now;
            }
        }

        public override void Update(GameTime gameTime)
        {
            UpdateServeur();
        }

            void EnvoieNouveauMessage()
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
                   foreach (Joueur j in ListeJoueurs)
                    {
                       // Écrit tous les propriété des objet 
                        MessageSortant.WriteAllProperties(j);
                    }

                    //// Le message contient: le type d'information, le nombre de joueurs et l'information 

                    //Envoie du message à toutes les connections dans l'ordre qu'il a été envoyé
                    Serveur.SendMessage(MessageSortant, Serveur.Connections, NetDeliveryMethod.ReliableOrdered, 0);
            }
        }
    }
