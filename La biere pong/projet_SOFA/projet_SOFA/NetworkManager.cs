using System;
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
    public enum PacketTypes { LOGIN, MOVE, WORLDSTATE }

    public class NetworkManager : Microsoft.Xna.Framework.GameComponent
    {
        NetworkServer Serveur;
        const string NOM_JEU = "BEERPONG";
        const int PORT = 5011;
        Mode1v1LAN Partie { get; set; }


        public NetworkManager(Game game)
            : base(game)
        {
            ConsoleWindow.ShowConsoleWindow();
        }

        void CréerServeur()
        {
            Serveur = new NetworkServer(Game, NOM_JEU, PORT);
            Game.Components.Add(Serveur);
        }

        void CréerClient()
        {
            NetworkClient client = new NetworkClient(Game, NOM_JEU, PORT, "Joueur1", Serveur);
            Game.Components.Add(client);
        }

        void CréerClient(string nomJoueur)
        {
            NetworkClient client = new NetworkClient(Game, NOM_JEU, PORT, nomJoueur, Serveur);
            Game.Components.Add(client);
        }

        void CréerClientLocal()
        {
            NetworkClient client = new NetworkClient(Game, NOM_JEU,"localhost", PORT, "Joueur1", Serveur);
            Game.Components.Add(client);
        }

        void CréerClientLocal(string nomJoueur)
        {
            NetworkClient client = new NetworkClient(Game, NOM_JEU,"localhost", PORT, nomJoueur, Serveur);
            Game.Components.Add(client);
        }

        public void RejoindrePartie(string nomJoueur)
        {
            try
            {
                CréerClient(nomJoueur);
                RecevoirInfoPartieToClient_Joining();
            }
            //Doit ajouter d'autre exception et leur traitement
            catch(Exception)
            {

            }
        }

        public void HébergerPartie()
        {
            try
            {
                CréerServeur();
                CréerClientLocal();
                Partie = new Mode1v1LAN(Game, Serveur, this);
                Game.Components.Add(Partie);
            }
            //Doit ajouter d'autre exception et leur traitement
            catch(Exception e)
            {
                Console.WriteLine("Problème dans l'hébergement de la partie");
                Console.WriteLine(e.ToString());
            }
        }

        public void RecevoirInfoPartieToClient_Joining()
        {

        }
        
        public void EnvoyerInfoPartieToServeur_StartGame()
        {
            //Serveur.ListeJoueurs.Find(j => j.GamerTag == "Joueur1");
            Console.WriteLine("Essaie Sérialisation");
            try
            {
                InfoMode1v1LAN infoMode1v1LAN = new InfoMode1v1LAN((JoueurMultijoueur)Partie.JoueurPrincipal, Partie.JoueurSecondaire, Partie.GestionnairePartie, Partie.EstPartieActive, Partie.EnvironnementPartie, Partie.Serveur);
                byte[] infoPartie = Serialiseur.ObjToByteArray(infoMode1v1LAN);
            }

            catch(Exception e)
            {
                Console.WriteLine("Erreur dans l'envoie des informations de début de partie au serveur");
                Console.WriteLine(e.ToString());
            }

        }
    }
}
