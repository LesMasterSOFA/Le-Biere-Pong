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
    public class Mode1v1LAN : PartieMultijoueur
    {
        public ATH ath { get; private set; }
        public NetworkServer Serveur { get; private set; }
        public List<JoueurMultijoueur> ListeJoueurs { get; private set; }
        public Environnements Environnement { get; private set; }
        public NetworkManager GestionNetwork { get; set; }
        Menu MenuSelectionEnvironnement { get; set; }

  
        public Mode1v1LAN(Game game, NetworkServer serveur, NetworkManager gestionNetwork)
            : base(game)
        {
            Serveur = serveur;
            ListeJoueurs = new List<JoueurMultijoueur>();
            GestionNetwork = gestionNetwork;
            MenuSélectionPersonnage();
        }

        //Constructeur sérialiseur
        public Mode1v1LAN( Game game, InfoJoueurMultijoueur infoJoueurPrincipal, InfoJoueurMultijoueur infoJoueurSecondaire, 
            bool estPartieActive, InfoGestionEnvironnement infoEnvironnementPartie, NetworkServer infoServeur)
            : base(game)
        {
            //Reste à créer des constructeur pour ces champs
            JoueurPrincipal = new JoueurMultijoueur(this.Game,infoJoueurPrincipal);
            if(infoJoueurSecondaire != null)
                JoueurSecondaire = new JoueurMultijoueur(this.Game, infoJoueurSecondaire);
            EstPartieActive = estPartieActive;
            Environnement = infoEnvironnementPartie.NomEnvironnement;
            Serveur = infoServeur;
        }
        
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if(Serveur.ListeJoueurs.Count == 2 && MenuSelectionEnvironnement.BoutonJouer.EstActif != true)
            {
                MenuSelectionEnvironnement.BoutonJouer.EstActif = true;
            }
            base.Update(gameTime);
        }

        void ActiverEnvironnement()
        {
            if (EstPartieActive)
            {
                EnvironnementPartie = new GestionEnvironnement(this.Game, Environnement);
                ath = new ATH(Game);
                Game.Components.Add(EnvironnementPartie);
                Game.Components.Add(ath);
            }
        }

        void MenuSélectionPersonnage()
        {
            MenuSelectionEnvironnement = new Menu(this.Game);
            Game.Components.Add(MenuSelectionEnvironnement);
            MenuSelectionEnvironnement.BoutonsSelectionEnvironnementLAN(this);

            //JoueurPrincipal = ...;
            //JoueurSecondaire = ...;


            //Temporaire en attendant que le menu n'est pas créé
            if (Serveur.ListeJoueurs.Count >= 1 && Serveur.ListeJoueurs[0] != null)
                JoueurPrincipal = new JoueurMultijoueur(this.Game, Serveur.ListeJoueurs[0].IP, GestionNetwork.MasterClient);
                //JoueurPrincipal = new JoueurMultijoueur(this.Game, )
            if (Serveur.ListeJoueurs.Count >= 2 && Serveur.ListeJoueurs[1] != null) 
                JoueurSecondaire = new JoueurMultijoueur(this.Game, Serveur.ListeJoueurs[1].IP, GestionNetwork.SlaveClient);

        }

        public void InitialiserGarage()
        {
            Environnement = Environnements.Garage;
        }

        public void InitialiserSalleManger()
        {
            Environnement = Environnements.SalleManger;
        }

        public void InitialiserSousSol()
        {
            Environnement = Environnements.SousSol;
        }

        public void ActiverPartieMaster()
        {
            if (Serveur.ListeJoueurs.Count == 2)
            {
                Game.Components.Remove(MenuSelectionEnvironnement.BoutonJouer);
                ModifierActivation();
                ActiverEnvironnement();
                JoueurPrincipal.Client.EnvoyerInfoPartieToServeur_StartGame(this);
            }
        }

        public void ActiverPartieSlave()
        {
            ModifierActivation();
            ActiverEnvironnement();
        }

    }



    [Serializable]
    class InfoMode1v1LAN
    {
        public bool EstPartieActive { get; private set; }
        public InfoJoueurMultijoueur InfoJoueurPrincipal { get; private set; }
        public InfoJoueurMultijoueur InfoJoueurSecondaire { get; private set; }
        public InfoGestionPartie InfoGestionnairePartie { get; private set; }
        public InfoGestionEnvironnement InfoGestionnaireEnvironnement { get; private set; }
        public InfoNetworkServer InfoServer { get; private set; }

        public InfoMode1v1LAN(JoueurMultijoueur joueurPrincipal,JoueurMultijoueur joueurSecondaire, GestionPartie gestionnairePartie, 
            bool estPartieActive, GestionEnvironnement environnementPartie, 
            NetworkServer serveur)
        {
            if (joueurPrincipal != null)
            {
                InfoJoueurPrincipal = new InfoJoueurMultijoueur(joueurPrincipal.Avatar, joueurPrincipal.GamerTag,
                    joueurPrincipal.ImageJoueur, joueurPrincipal.GestionnaireDeLaPartie,
                    joueurPrincipal.EstActif, joueurPrincipal.IP);
            }
            else
                Console.WriteLine("Joueur Principal null");

            if (joueurSecondaire != null)
            {
                InfoJoueurSecondaire = new InfoJoueurMultijoueur(joueurSecondaire.Avatar, joueurSecondaire.GamerTag,
                    joueurSecondaire.ImageJoueur, joueurSecondaire.GestionnaireDeLaPartie,
                    joueurSecondaire.EstActif, joueurSecondaire.IP);
            }
            else
                Console.WriteLine("Joueur Secondaire null");

            InfoGestionnairePartie = new InfoGestionPartie();

            EstPartieActive = estPartieActive;

            InfoGestionnaireEnvironnement = new InfoGestionEnvironnement(environnementPartie.NomEnvironnement);

            InfoServer = new InfoNetworkServer(serveur.Port, serveur.NomJeu, serveur.Temps);
        }
    }
}
