using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AtelierXNA
{
    public class Mode1v1LAN : PartieMultijoueur
    {
        BoutonDeCommande BoutonJouer { get; set; }
        public ATH ath { get; private set; }
        public NetworkServer Serveur { get; private set; }
        public List<JoueurMultijoueur> ListeJoueurs { get; private set; }
        public Environnements Environnement { get; private set; }
        public NetworkManager GestionNetwork { get; private set; }

        BoutonDeCommande BoutonGarage { get; set; }
        BoutonDeCommande BoutonSalleManger { get; set; }
        BoutonDeCommande BoutonSousSol { get; set; }
        Rectangle RectangleFondÉcran { get; set; }
        Rectangle RectangleGarage { get; set; }
        Rectangle RectangleSalleManger { get; set; }
        Rectangle RectangleSousSol { get; set; }
        SpriteBatch GestionSprites { get; set; }
        RessourcesManager<Texture2D> gestionnaireTexture { get; set; }
        RessourcesManager<SpriteFont> gestionnaireFont { get; set; }
        Texture2D ImageFondÉcran { get; set; }
        Texture2D ImageMenuGarage { get; set; }
        Texture2D ImageMenuSalleManger { get; set; }
        Texture2D ImageMenuSousSol { get; set; }
        Texture2D BoutonBleu { get; set; }
        bool MenuActif { get; set; }
        InputManager GestionInput { get; set; }
        List<Personnage> ListePerso { get; set; }
        
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
            JoueurPrincipal = new JoueurMultijoueur(this.Game,infoJoueurPrincipal);
            if(infoJoueurSecondaire != null)
                JoueurSecondaire = new JoueurMultijoueur(this.Game, infoJoueurSecondaire);
            EstPartieActive = estPartieActive;
            Environnement = infoEnvironnementPartie.NomEnvironnement;
            Serveur = infoServeur;
        }

        protected override void LoadContent()
        {

            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            gestionnaireFont = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            gestionnaireTexture = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            ImageFondÉcran = gestionnaireTexture.Find("BeerPong");
            ImageMenuGarage = gestionnaireTexture.Find("MenuGarage");
            ImageMenuSalleManger = gestionnaireTexture.Find("MenuSalle");
            ImageMenuSousSol = gestionnaireTexture.Find("MenuSousSol");
            BoutonBleu = gestionnaireTexture.Find("BoutonBleu");
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //Pour bouton jouer menu selection environnement
            if(Serveur.ListeJoueurs.Count == 2 && BoutonJouer.EstActif != true)
            {
                BoutonJouer.EstActif = true;
            }

            #region tests réseau
            //if (EstPartieActive)
            //{  
                ////Pour tester l'animation
                //if(GestionInput.EstNouvelleTouche(Keys.B))
                //{
                //   JoueurPrincipal.ChangerAnimation(TypeActionPersonnage.Boire, ListePerso.Find(perso => perso.Position.Z > 0));
                //   JoueurPrincipal.Client.EnvoyerInfoAnimationJoueur(false, TypeActionPersonnage.Boire);
                //}
                ////Pour tester envoie vector3 position balle
                //if(GestionInput.EstNouvelleTouche(Keys.X))
                //{
                //    var p = new Vector3(4,5,6);
                //    JoueurPrincipal.Client.EnvoyerInfoPositionBalle(p);
                //}
                ////pour tester envoie est tour joueur principal
                //if(GestionInput.EstNouvelleTouche(Keys.C))
                //{
                //    bool b = true;
                //    JoueurPrincipal.Client.EnvoyerInfoEstTourJoueurPrincipal(b);
                //}
                ////pour tester envoie verre à enlever
                //if(GestionInput.EstNouvelleTouche(Keys.V))
                //{
                //    JoueurPrincipal.Client.EnvoyerInfoVerreÀEnlever(true, 3);
                //}
                ////pour tester envoie lancer balle
                //if(GestionInput.EstNouvelleTouche(Keys.N))
                //{
                //    JoueurPrincipal.Client.EnvoyerInfoLancerBalle(7f, 8f, 9f);
                //}
            //}
            #endregion

            base.Update(gameTime);
        }

        void ActiverEnvironnement()
        {
            if (EstPartieActive)
            {
                EnvironnementPartie = new GestionEnvironnement(this.Game, Environnement, SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex2.ToString(), SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex.ToString(),TypePartie.LAN);
                EnleverMenuSelectionEnvironnement();
                Game.Components.Add(EnvironnementPartie);

                ListePerso = new List<Personnage>();
                foreach (Personnage perso in Game.Components.Where(item => item is Personnage))
                {
                   ListePerso.Add(perso);
                }
            }
        }

        void MenuSélectionPersonnage()
        {
            RectangleFondÉcran = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height + 15);
            RectangleGarage = new Rectangle(Game.Window.ClientBounds.Width / 7, 150, Game.Window.ClientBounds.Width / 5, 233);
            RectangleSalleManger = new Rectangle(3 * Game.Window.ClientBounds.Width / 7, 150, Game.Window.ClientBounds.Width / 5, 233);
            RectangleSousSol = new Rectangle(5 * Game.Window.ClientBounds.Width / 7, 150, Game.Window.ClientBounds.Width / 5, 233);
            BoutonJouer = new BoutonDeCommande(Game, "jouer", "Impact20", "BoutonBleu", "BoutonBleuPale", new Vector2(100, 100), false, ActiverPartieMaster);
            BoutonGarage = new BoutonDeCommande(Game, "Garage", "Impact20", "BoutonBleu", "BoutonBleuPale", new Vector2(17 * Game.Window.ClientBounds.Width / 70, 100), true, InitialiserGarage);
            BoutonSalleManger = new BoutonDeCommande(Game, "Salle à manger", "Impact20", "BoutonBleu", "BoutonBleuPale", new Vector2(37 * Game.Window.ClientBounds.Width / 70, 100), true, InitialiserSalleManger);
            BoutonSousSol = new BoutonDeCommande(Game, "Sous-sol", "Impact20", "BoutonBleu", "BoutonBleuPale", new Vector2(57 * Game.Window.ClientBounds.Width / 70, 100), true, InitialiserSousSol);

            MenuActif = true;
            Game.Components.Add(BoutonJouer);
            Game.Components.Add(BoutonGarage);
            Game.Components.Add(BoutonSalleManger);
            Game.Components.Add(BoutonSousSol);
        }

        void InitialiserGarage()
        {
            Environnement = Environnements.Garage;
        }

        void InitialiserSalleManger()
        {
            Environnement = Environnements.SalleManger;
        }

        void InitialiserSousSol()
        {
            Environnement = Environnements.SousSol;
        }

        void ActiverPartieMaster()
        {
            if (Serveur.ListeJoueurs.Count == 2)
            {
                if (Serveur.ListeJoueurs.Count >= 1 && Serveur.ListeJoueurs[0] != null)
                    JoueurPrincipal = new JoueurMultijoueur(this.Game, Serveur.ListeJoueurs[0].IP, GestionNetwork.MasterClient);
                if (Serveur.ListeJoueurs.Count >= 2 && Serveur.ListeJoueurs[1] != null)
                    JoueurSecondaire = new JoueurMultijoueur(this.Game, Serveur.ListeJoueurs[1].IP, GestionNetwork.SlaveClient);

                ModifierActivation();
                ActiverEnvironnement();
                ath = new ATH(Game, JoueurPrincipal);
                Game.Components.Add(ath);
                JoueurPrincipal.Client.EnvoyerInfoPartieToServeur_StartGame(this);
            }
        }

        public void ActiverPartieSlave()
        {
            ModifierActivation();
            ActiverEnvironnement();
            ath = new ATH(Game,JoueurSecondaire);
            Game.Components.Add(ath);
        }

        void EnleverMenuSelectionEnvironnement()
        {
            MenuActif = false;
            Game.Components.Remove(BoutonJouer);
            Game.Components.Remove(BoutonGarage);
            Game.Components.Remove(BoutonSalleManger);
            Game.Components.Remove(BoutonSousSol);
            Game.Components.Add(new Afficheur3D(Game));
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Begin();
            int noDrawOrder = 0;
            foreach (GameComponent item in Game.Components)
            {
                if (item is DrawableGameComponent)
                {
                    ((DrawableGameComponent)item).DrawOrder = noDrawOrder++;
                }
            }

            DrawMenuEnivronnement(gameTime, MenuActif);
            base.Draw(gameTime);
            GestionSprites.End();
        }

        void DrawMenuEnivronnement(GameTime gameTime, bool MenuActif)
        {
            if (MenuActif)
            {
                //L'image de fond d'écran se dessine par dessus les boutons
                //GestionSprites.Draw(ImageFondÉcran, RectangleFondÉcran, Color.White);
                GestionSprites.Draw(ImageMenuGarage, RectangleGarage, Color.White);
                GestionSprites.Draw(ImageMenuSalleManger, RectangleSalleManger, Color.White);
                GestionSprites.Draw(ImageMenuSousSol, RectangleSousSol, Color.White);
            }
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
