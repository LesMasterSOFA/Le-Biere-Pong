﻿using System;
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
    class NetworkClient : Microsoft.Xna.Framework.GameComponent
    {
        //Objet Client
        static NetClient Client;

        //Liste de joueur
        static List<Joueur> ListeJoueurs;

        // indique si le client roule
        public static bool EstEnMarche = false;

        DateTime Temps { get; set; }
        string NomJeu { get; set; }
        int Port { get; set; }
        string HostIP{ get; set; } //ip de l'host
        NetIncomingMessage MessageInc { get; set; } //message entrant
        NetOutgoingMessage MessageOut { get; set; } //message sortant
        string NomJoueur { get; set; }
        TimeSpan IntervalleRafraichissement { get; set; }

        public NetworkClient(Game jeu, string nomJeu, int port, string nomJoueur):base(jeu)
        {
            NomJeu = nomJeu;
            Port = port;
            NomJoueur = nomJoueur;
            Create(NomJeu, Port);
            Connect();
            ListeJoueurs = new List<Joueur>();
            IntervalleRafraichissement = new TimeSpan(0, 0, 0, 0, 30); //30 ms


        }

        void Create(string nomJeu, int port)
        {
            // Demande l'ip
            Console.WriteLine("Enter IP To Connect");
            HostIP = Console.ReadLine();

            //Crée la configuration du client -> doit avoir le même nom que le serveur
            NetPeerConfiguration Config = new NetPeerConfiguration(NomJeu);
            Config.Port = Port;
            Client = new NetClient(Config);
            Client.Start();
            Temps = DateTime.Now;
            Console.WriteLine("Client créé à " + Temps.ToString());
        }

        void Connect()
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
            Console.WriteLine("Connection du client envoyée à " +Temps);

            //Fonction attendant l'approbation de connection du serveur
            AttenteConnectionServeur();

            Console.WriteLine("Connection bien reçu du serveur à " + Temps);
            EstEnMarche = true;

        }

        //Attente du message de connection pour instancier les joueurs
        private void AttenteConnectionServeur()
        {
            //Détermine si le client peut démarer
            bool PeutPartir = false;
            
            //Loop tant que le client ne peut pas démarrer
            while (!PeutPartir)
            {
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
                                WorldStateUpdate();

                                //Après que tous les joueurs sont instanciés, on peut partir le jeu
                                PeutPartir = true;
                            }
                            break;

                        default:
                            //ne devrait pas arriver, envoie un message d'erreur
                            Console.WriteLine(MessageInc.ReadString() + " Message reçu non géré");
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
            Console.WriteLine("WorldState Update");

            //On vide la liste des joueurs contenant les informations
            ListeJoueurs.Clear();

            // Declare count
            int NbDeJoueurs = 0;

            //On lit le nombre de joueurs envoyé dans le message
            NbDeJoueurs = MessageInc.ReadInt32();

            //On recrée les joueurs présents
            for (int i = 0; i < NbDeJoueurs; i++)
            {
                Joueur j = new Joueur(this.Game, MessageInc.SenderConnection);

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
                    if (MessageInc.ReadByte() == (byte)PacketTypes.WORLDSTATE)
                    {
                        WorldStateUpdate();
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
    }
}