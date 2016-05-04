using System;
using Microsoft.Xna.Framework;


namespace AtelierXNA
{
    //Énumération contentant les différents types de packets,
    //pouvant être ensuite converti en byte ce qui permet de déterminer ce qu'il faut faire avec tel ou tel packet
    public enum PacketTypes { LOGIN, WORLDSTATE, STARTGAME_INFO, MESSAGE_NULL, ANIMATION, POSITION_BALLE, EST_TOUR_JOUEUR_PRINCIPAL_INFO, VERRE_À_ENLEVER, LANCER_BALLE_INFO }

    public class NetworkManager : Microsoft.Xna.Framework.GameComponent
    {
        public NetworkServer Serveur { get; private set; }
        public const string NOM_JEU = "BEERPONG";
        public const int PORT = 25565;
        public NetworkClient MasterClient { get; private set; } //Client local dirigeant la partie
        public NetworkClient SlaveClient { get; private set; } //Client extérieur se greffant à la partie
        public Mode1v1LAN PartieEnCours { get; private set; }

        public NetworkManager(Game game)
            : base(game)
        {
            //sert au Debug
            //ConsoleWindow.ShowConsoleWindow();
        }

        void CréerServeur()
        {
            Serveur = new NetworkServer(Game, NOM_JEU, PORT);
            Game.Components.Add(Serveur);
        }

        void CréerSlaveClient()
        {
            SlaveClient = new NetworkClient(Game, NOM_JEU, PORT, "Joueur1", Serveur,false);
            Game.Components.Add(SlaveClient);
        }

        void CréerSlaveClient(string nomJoueur)
        {
            SlaveClient = new NetworkClient(Game, NOM_JEU, PORT, nomJoueur, Serveur,false);
            Game.Components.Add(SlaveClient);
        }

        void CréerMasterClient()
        {
            MasterClient = new NetworkClient(Game, NOM_JEU,"localhost", PORT, "Joueur1", Serveur,true);
            Game.Components.Add(MasterClient);
        }

        void CréerMasterClient(string nomJoueur)
        {
            MasterClient = new NetworkClient(Game, NOM_JEU, "localhost", PORT, nomJoueur, Serveur,true);
            Game.Components.Add(MasterClient);
        }

        public void RejoindrePartie(string nomJoueur)
        {
            try
            {
                CréerSlaveClient(nomJoueur);
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
                PartieEnCours = new Mode1v1LAN(Game, Serveur, this);
                Game.Components.Add(PartieEnCours);
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

    }
}
