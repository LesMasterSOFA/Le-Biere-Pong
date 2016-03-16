﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;


namespace AtelierXNA
{
    //Énumération contentant les différents types de packets,
    //pouvant être ensuite converti en byte ce qui permet de déterminer ce qu'il faut faire avec tel ou tel packet
    public enum PacketTypes { LOGIN, MOVE, WORLDSTATE, STARTGAME_INFO, MESSAGE_NULL }

    public class NetworkManager : Microsoft.Xna.Framework.GameComponent
    {
        public NetworkServer Serveur { get; private set; }
        const string NOM_JEU = "BEERPONG";
        const int PORT = 5011;
        public Mode1v1LAN Partie { get; private set; }
        public NetworkClient MasterClient { get; private set; } //Client local dirigeant la partie
        public NetworkClient SlaveClient { get; private set; } //Client extérieur se greffant à la partie

        public NetworkManager(Game game)
            : base(game)
        {
            ConsoleWindow.ShowConsoleWindow();
        }

        void CréerServeur()
        {
            Serveur = new NetworkServer(Game, NOM_JEU, PORT, this);
            Game.Components.Add(Serveur);
        }

        void CréerSlaveClient()
        {
            SlaveClient = new NetworkClient(Game, NOM_JEU, PORT, "Joueur1", Serveur);
            Game.Components.Add(SlaveClient);
        }

        void CréerSlaveClient(string nomJoueur)
        {
            SlaveClient = new NetworkClient(Game, NOM_JEU, PORT, nomJoueur, Serveur);
            Game.Components.Add(SlaveClient);
        }

        void CréerMasterClient()
        {
            MasterClient = new NetworkClient(Game, NOM_JEU,"localhost", PORT, "Joueur1", Serveur);
            Game.Components.Add(MasterClient);
        }

        void CréerMasterClient(string nomJoueur)
        {
            MasterClient = new NetworkClient(Game, NOM_JEU, "localhost", PORT, nomJoueur, Serveur);
            Game.Components.Add(MasterClient);
        }

        public void RejoindrePartie(string nomJoueur)
        {
            try
            {
                CréerSlaveClient(nomJoueur);
                //RecevoirInfoPartieToClient_Joining();
            }
            //Doit ajouter d'autre exception et leur traitement
            catch(Exception e)
            {
                Console.WriteLine("Problème lors du rejoignement de partie");
                Console.WriteLine(e.ToString());
                Menu menu = new Menu(Game);
                Game.Components.Add(menu);
                menu.BoutonsLAN();
            }
        }

        public void HébergerPartie()
        {
            try
            {
                CréerServeur();
                CréerMasterClient();
                Partie = new Mode1v1LAN(Game, Serveur, this);
                Game.Components.Add(Partie);
            }
            //Doit ajouter d'autre exception et leur traitement
            catch(Exception e)
            {
                Console.WriteLine("Problème dans l'hébergement de la partie");
                Console.WriteLine(e.ToString());
                Menu menu = new Menu(Game);
                Game.Components.Add(menu);
                menu.BoutonsLAN();
            }
        }

        public void RecevoirInfoPartieToClient_Joining(byte[] infoPartie)
        {
            Console.WriteLine("Essaie Désérialisation");
            try
            {
                InfoMode1v1LAN infoMode1v1LAN = Serialiseur.ByteArrayToObj<InfoMode1v1LAN>(infoPartie);
                Partie = new Mode1v1LAN(this.Game, infoMode1v1LAN.InfoJoueurPrincipal, infoMode1v1LAN.InfoJoueurSecondaire, infoMode1v1LAN.EstPartieActive, infoMode1v1LAN.InfoGestionnaireEnvironnement, infoMode1v1LAN.InfoServer);
            }

            catch (Exception e)
            {
                Console.WriteLine("Erreur dans la réception et/ou la désérialisation de l'objet");
                Console.WriteLine(e.ToString());
            }
        }
        
        public void EnvoyerInfoPartieToServeur_StartGame()
        {
            Console.WriteLine("Essaie Sérialisation");
            try
            {
                InfoMode1v1LAN infoMode1v1LAN = new InfoMode1v1LAN((JoueurMultijoueur)Partie.JoueurPrincipal, Partie.JoueurSecondaire, Partie.GestionnairePartie, Partie.EstPartieActive, Partie.EnvironnementPartie, Partie.Serveur);
                byte[] infoPartie = Serialiseur.ObjToByteArray(infoMode1v1LAN);
                MasterClient.EnvoyerMessageServeur(PacketTypes.STARTGAME_INFO ,infoPartie);
            }

            catch(Exception e)
            {
                Console.WriteLine("Erreur dans l'envoie des informations de début de partie au serveur");
                Console.WriteLine(e.ToString());
            }

        }
    }
}
