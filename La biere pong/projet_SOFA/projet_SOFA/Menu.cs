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


namespace AtelierXNA
{
    public class Menu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const int MARGE_BOUTONS = 60;
        string TexteTitre { get; set; }
        Rectangle RectangleFondÉcran { get; set; }
        Texture2D ImageFondÉcran { get; set; }
        Texture2D ImageBouton { get; set; }
        RessourcesManager<Texture2D> gestionnaireTexture { get; set; }
        RessourcesManager<SpriteFont> gestionnaireFont { get; set; }
        SpriteBatch GestionSprites { get; set; }
        Vector2 PositionCentre { get; set; }
        Vector2 PositionBack { get; set; }
        List<BoutonDeCommande> ListeBoutonsCommandeMenu { get; set; }
        public BoutonDeCommande BoutonJouer { get; set; }
        BoutonDeCommande BoutonSolo { get; set; }
        BoutonDeCommande BoutonPratique { get; set; }
        BoutonDeCommande BoutonHistoire { get; set; }
        BoutonDeCommande BoutonMultijoueur { get; set; }
        BoutonDeCommande Bouton1v1Local { get; set; }
        BoutonDeCommande Bouton1v1LAN { get; set; }
        BoutonDeCommande BoutonHéberger { get; set; }
        BoutonDeCommande BoutonRejoindre { get; set; }
        BoutonDeCommande BoutonBack { get; set; }
        BoutonDeCommande BoutonAfficherConsole { get; set; }
        BoutonDeCommande BoutonEnleverConsole { get; set; }
        BoutonDeCommande BoutonEffacerConsole { get; set; }
        BoutonDeCommande BoutonGarage { get; set; }
        BoutonDeCommande BoutonSalleManger { get; set; }
        BoutonDeCommande BoutonSousSol { get; set; }
        Rectangle RectangleGarage { get; set; }
        Rectangle RectangleSalleManger { get; set; }
        Rectangle RectangleSousSol { get; set; }
        Texture2D ImageMenuGarage { get; set; }
        Texture2D ImageMenuSalleManger { get; set; }
        Texture2D ImageMenuSousSol { get; set; }
        SoundEffect ChansonMenu { get; set; }

        public Menu(Game game)
            :base(game)
        {
        }

        public override void Initialize()
        {
            ListeBoutonsCommandeMenu = new List<BoutonDeCommande>();
            RectangleFondÉcran = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            PositionCentre = new Vector2(RectangleFondÉcran.X + RectangleFondÉcran.Width / 2f, RectangleFondÉcran.Y + RectangleFondÉcran.Height / 2f);
            PositionBack = new Vector2(Game.Window.ClientBounds.Width - MARGE_BOUTONS - 100, Game.Window.ClientBounds.Height - MARGE_BOUTONS + 20);
            base.Initialize();
            InitialiserMenu();
        }
        void InitialiserMenu()
        {
            ChansonMenu.Play();
            TexteTitre = "LE BIERE PONG";
            EnleverBoutonsExistants();
            BoutonJouer = new BoutonDeCommande(Game, "Jouer", "Impact20", "BoutonBleu", "BoutonBleuPale", PositionCentre, true, BoutonsJouer);
            ListeBoutonsCommandeMenu.Add(BoutonJouer);
            AjouterNouveauxBoutons();
        }
        void BoutonsJouer()
        {
            TexteTitre = "JOUER";
            EnleverBoutonsExistants();
            BoutonBack = new BoutonDeCommande(Game, "Back", "Impact20", "BoutonBackBleu", "BoutonBackBleuPale", PositionBack, true, InitialiserMenu);
            BoutonSolo = new BoutonDeCommande(Game, "Solo", "Impact20", "BoutonBleu", "BoutonBleuPale", PositionCentre, true, BoutonsSolo);
            BoutonMultijoueur = new BoutonDeCommande(Game, "Multijoueur", "Impact20", "BoutonBleu", "BoutonBleuPale",
                                                     new Vector2(PositionCentre.X, PositionCentre.Y + MARGE_BOUTONS), true, BoutonsMultijoueur);
            ListeBoutonsCommandeMenu.Add(BoutonBack);
            ListeBoutonsCommandeMenu.Add(BoutonSolo);
            ListeBoutonsCommandeMenu.Add(BoutonMultijoueur);
            AjouterNouveauxBoutons();
        }
        void BoutonsSolo()
        {
            TexteTitre = "SOLO";
            EnleverBoutonsExistants();
            BoutonBack = new BoutonDeCommande(Game, "Back", "Impact20", "BoutonBackBleu", "BoutonBackBleuPale", PositionBack, true, BoutonsJouer);
            BoutonHistoire = new BoutonDeCommande(Game, "Histoire", "Impact20", "BoutonBleu", "BoutonBleuPale", PositionCentre, true, PartirModeHistoire);//fct événementielle -> Partir histoire
            BoutonPratique = new BoutonDeCommande(Game, "Pratique", "Impact20", "BoutonBleu", "BoutonBleuPale",
                                                  new Vector2(PositionCentre.X, PositionCentre.Y+MARGE_BOUTONS), true, PartirModePratique);//fct événementielle -> Partir pratique
            ListeBoutonsCommandeMenu.Add(BoutonBack);
            ListeBoutonsCommandeMenu.Add(BoutonHistoire);
            ListeBoutonsCommandeMenu.Add(BoutonPratique);
            AjouterNouveauxBoutons();
        }
        void BoutonsMultijoueur()
        {
            TexteTitre = "MULTIJOUEUR";
            EnleverBoutonsExistants();
            BoutonBack = new BoutonDeCommande(Game, "Back", "Impact20", "BoutonBackBleu", "BoutonBackBleuPale", PositionBack, true, BoutonsJouer);
            Bouton1v1Local = new BoutonDeCommande(Game, "Local", "Impact20", "BoutonBleu", "BoutonBleuPale", PositionCentre, true, PartirMode1v1Local);//fct événementielle -> Partir 1v1 local
            Bouton1v1LAN = new BoutonDeCommande(Game, "LAN", "Impact20", "BoutonBleu", "BoutonBleuPale",
                                                  new Vector2(PositionCentre.X, PositionCentre.Y + MARGE_BOUTONS), true, BoutonsLAN);
            ListeBoutonsCommandeMenu.Add(BoutonBack);
            ListeBoutonsCommandeMenu.Add(Bouton1v1Local);
            ListeBoutonsCommandeMenu.Add(Bouton1v1LAN);
            AjouterNouveauxBoutons();
        }

        public void BoutonsLAN()
        {
            TexteTitre = "LAN";
            EnleverBoutonsExistants();
            BoutonBack = new BoutonDeCommande(Game, "Back", "Impact20", "BoutonBackBleu", "BoutonBackBleuPale", PositionBack, true, BoutonsMultijoueur);
            BoutonHéberger = new BoutonDeCommande(Game, "Héberger", "Impact20", "BoutonBleu", "BoutonBleuPale", PositionCentre, true, HébergerMode1v1LAN);//fct événementielle -> Partir host
            BoutonRejoindre = new BoutonDeCommande(Game, "Rejoindre", "Impact20", "BoutonBleu", "BoutonBleuPale",
                                                  new Vector2(PositionCentre.X, PositionCentre.Y + MARGE_BOUTONS), true, RejoindreMode1v1LAN);//fct événementielle -> Partir join
            BoutonAfficherConsole = new BoutonDeCommande(Game, "Afficher Console", "Impact20", "BoutonBleu", "BoutonBleuPale",
                                                  new Vector2(PositionBack.X, PositionCentre.Y + MARGE_BOUTONS), true, AfficherConsole);
            BoutonEnleverConsole = new BoutonDeCommande(Game, "Enlever Console", "Impact20", "BoutonBleu", "BoutonBleuPale",
                                                  new Vector2(PositionBack.X, PositionCentre.Y + 2*MARGE_BOUTONS), true, ConsoleWindow.HideConsoleWindow);
            BoutonEffacerConsole = new BoutonDeCommande(Game, "Effacer Console", "Impact20", "BoutonBleu", "BoutonBleuPale",
                                                  new Vector2(PositionBack.X, PositionCentre.Y + 3 * MARGE_BOUTONS), true, EffacerConsole);
            ListeBoutonsCommandeMenu.Add(BoutonBack);
            ListeBoutonsCommandeMenu.Add(BoutonHéberger);
            ListeBoutonsCommandeMenu.Add(BoutonRejoindre);
            ListeBoutonsCommandeMenu.Add(BoutonAfficherConsole);
            ListeBoutonsCommandeMenu.Add(BoutonEnleverConsole);
            ListeBoutonsCommandeMenu.Add(BoutonEffacerConsole);
            AjouterNouveauxBoutons();
        }

        public void BoutonsSelectionEnvironnementLAN(Mode1v1LAN partie)
        {
            Initialize();
            TexteTitre = "SÉLECTION D'ENVIRONNEMENT";
            EnleverBoutonsExistants();
            RectangleGarage = new Rectangle(Game.Window.ClientBounds.Width / 7, 150, Game.Window.ClientBounds.Width / 5, 233);
            RectangleSalleManger = new Rectangle(3 * Game.Window.ClientBounds.Width / 7, 150, Game.Window.ClientBounds.Width / 5, 233);
            RectangleSousSol = new Rectangle(5 * Game.Window.ClientBounds.Width / 7, 150, Game.Window.ClientBounds.Width / 5, 233);
            BoutonJouer = new BoutonDeCommande(Game, "jouer", "Impact20", "BoutonBleu", "BoutonBleuPale", new Vector2(100, 100), false, partie.ActiverPartieMaster);
            BoutonGarage = new BoutonDeCommande(Game, "Garage", "Impact20", "BoutonBleu", "BoutonBleuPale", new Vector2(17 * Game.Window.ClientBounds.Width / 70, 100), true, partie.InitialiserGarage);
            BoutonSalleManger = new BoutonDeCommande(Game, "Salle à manger", "Impact20", "BoutonBleu", "BoutonBleuPale", new Vector2(37 * Game.Window.ClientBounds.Width / 70, 100), true, partie.InitialiserSalleManger);
            BoutonSousSol = new BoutonDeCommande(Game, "Sous-sol", "Impact20", "BoutonBleu", "BoutonBleuPale", new Vector2(57 * Game.Window.ClientBounds.Width / 70, 100), true, partie.InitialiserSousSol);
            BoutonBack = new BoutonDeCommande(Game, "Back", "Impact20", "BoutonBackBleu", "BoutonBackBleuPale", PositionBack, true, BoutonsJouer);
            
            ListeBoutonsCommandeMenu.Add(BoutonBack);
            ListeBoutonsCommandeMenu.Add(BoutonSousSol);
            ListeBoutonsCommandeMenu.Add(BoutonSalleManger);
            ListeBoutonsCommandeMenu.Add(BoutonGarage);
            ListeBoutonsCommandeMenu.Add(BoutonJouer);

            AjouterNouveauxBoutons();
        }

        void EffacerConsole()
        {
            ConsoleWindow.HideConsoleWindow();
            Console.Clear();
            ConsoleWindow.ShowConsoleWindow();
        }

        void AfficherConsole()
        {
            ConsoleWindow.HideConsoleWindow();
            ConsoleWindow.ShowConsoleWindow();
            ConsoleWindow.PutInForeGround();
        }

        void PartirModeHistoire()
        {
            Game.Components.Remove(this);
            EnleverBoutonsExistants();
            Game.Components.Add(new ModeHistoire(Game));
        }

        void PartirModePratique()
        {
            Game.Components.Remove(this);
            EnleverBoutonsExistants();
            Game.Components.Add(new ModePratique(Game));
        }

        void PartirMode1v1Local()
        {
            Game.Components.Remove(this);
            EnleverBoutonsExistants();
            Game.Components.Add(new Mode1v1Local(Game));
        }

        void HébergerMode1v1LAN()
        {
            Game.Components.Remove(this);
            EnleverBoutonsExistants();
            NetworkManager gestionReseau = new NetworkManager(this.Game);
            Game.Components.Add(gestionReseau);
            gestionReseau.HébergerPartie();
        }

        void RejoindreMode1v1LAN()
        {
            Game.Components.Remove(this);
            EnleverBoutonsExistants();
            NetworkManager gestionReseau = new NetworkManager(this.Game);
            Game.Components.Add(gestionReseau);
            gestionReseau.RejoindrePartie("Joueur2");
        }

        void AjouterNouveauxBoutons()
        {
            foreach (BoutonDeCommande btn in ListeBoutonsCommandeMenu)
            {
                Game.Components.Add(btn);
            }
        }
        void EnleverBoutonsExistants()
        {
            if (ListeBoutonsCommandeMenu != null)
            {
                if (ListeBoutonsCommandeMenu.Count != 0)
                {
                    foreach (BoutonDeCommande btn in ListeBoutonsCommandeMenu)
                    {
                        Game.Components.Remove(btn);
                    }
                    ListeBoutonsCommandeMenu.Clear();
                }
            }
        }
        protected override void LoadContent()
        {
            base.LoadContent();
            ChansonMenu = Game.Content.Load<SoundEffect>("Sounds\\Menu");
            SoundEffectInstance instanceChansonMenu = ChansonMenu.CreateInstance();
            instanceChansonMenu.IsLooped = true;
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            gestionnaireFont = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            gestionnaireTexture = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            ImageFondÉcran = gestionnaireTexture.Find("BeerPong");
            ImageBouton = gestionnaireTexture.Find("BoutonBleu");
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Begin();
            GestionSprites.Draw(ImageFondÉcran, RectangleFondÉcran, Color.White);
            GestionSprites.DrawString(gestionnaireFont.Find("Impact40"), TexteTitre, new Vector2(MARGE_BOUTONS, MARGE_BOUTONS), Color.Black);
            GestionSprites.End();
            base.Draw(gameTime);
        }
    }
}
