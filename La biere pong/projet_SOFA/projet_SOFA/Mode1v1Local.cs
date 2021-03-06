﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;



namespace AtelierXNA
{

    public class Mode1v1Local : PartieSolo
    {
        SpriteBatch GestionSprites { get; set; }
        RessourcesManager<Texture2D> gestionnaireTexture { get; set; }
        RessourcesManager<SpriteFont> gestionnaireFont { get; set; }
        Texture2D ImageFondÉcran { get; set; }
        Texture2D ImageMenuGarage { get; set; }
        Texture2D ImageMenuSalleManger { get; set; }
        Texture2D ImageMenuSousSol { get; set; }
        Texture2D BoutonBleu { get; set; }
        BoutonDeCommande BoutonGarage { get; set; }
        BoutonDeCommande BoutonSalleManger { get; set; }
        BoutonDeCommande BoutonSousSol { get; set; }
        Rectangle RectangleFondÉcran { get; set; }
        Rectangle RectangleGarage { get; set; }
        Rectangle RectangleSalleManger { get; set; }
        Rectangle RectangleSousSol { get; set; }
        public Mode1v1Local(Game game)
            : base(game)
        {
            Random PileOuFace = new Random();
            EstTourJoueur = Convert.ToBoolean(PileOuFace.Next(0, 1));
        }

        public override void Initialize()
        {
            RectangleFondÉcran = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height + 15);
            RectangleGarage = new Rectangle(Game.Window.ClientBounds.Width / 7, 150, Game.Window.ClientBounds.Width / 5, 233);
            RectangleSalleManger = new Rectangle(3 * Game.Window.ClientBounds.Width / 7, 150, Game.Window.ClientBounds.Width / 5, 233);
            RectangleSousSol = new Rectangle(5 * Game.Window.ClientBounds.Width / 7, 150, Game.Window.ClientBounds.Width / 5, 233);
            ath = new ATH(Game,new Joueur(Game)); // reste à avoir une liste de joueur et selectionner l'ath selon le joueur
            base.Initialize();
            MenuSélectionEnvironnement();

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
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        void MenuSélectionEnvironnement()
        {
            BoutonGarage = new BoutonDeCommande(Game, "Garage", "Impact20", "BoutonBleu", "BoutonBleuPale", new Vector2(17 * Game.Window.ClientBounds.Width / 70, 100), true, InitialiserGarage);
            BoutonSalleManger = new BoutonDeCommande(Game, "Salle à manger", "Impact20", "BoutonBleu", "BoutonBleuPale", new Vector2(37 * Game.Window.ClientBounds.Width / 70, 100), true, InitialiserSalle);
            BoutonSousSol = new BoutonDeCommande(Game, "Sous-sol", "Impact20", "BoutonBleu", "BoutonBleuPale", new Vector2(57 * Game.Window.ClientBounds.Width / 70, 100), true, InitialiserSousSol);
            Game.Components.Add(BoutonGarage);
            Game.Components.Add(BoutonSalleManger);
            Game.Components.Add(BoutonSousSol);
        }

        void InitialiserGarage()
        {
            EnvironnementPartie = new GestionEnvironnement(Game, Environnements.Garage, SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex2.ToString(), SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex.ToString(),TypePartie.Local);
            ModifierActivation();
            if (EstPartieActive)
            {
                Game.Components.Add(EnvironnementPartie);
                Game.Components.Add(ath);
            }
            Game.Components.Remove(BoutonGarage);
            Game.Components.Remove(BoutonSalleManger);
            Game.Components.Remove(BoutonSousSol);
            Game.Components.Remove(this);
            Game.Components.Add(new GestionPartie(Game));
            MediaPlayer.Stop();
        }

        void InitialiserSalle()
        {
            EnvironnementPartie = new GestionEnvironnement(Game, Environnements.SalleManger, SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex2.ToString(), SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex.ToString(),TypePartie.Local);
            ModifierActivation();
            if (EstPartieActive)
            {
                Game.Components.Add(EnvironnementPartie);
                Game.Components.Add(ath);
            }
            Game.Components.Remove(BoutonGarage);
            Game.Components.Remove(BoutonSalleManger);
            Game.Components.Remove(BoutonSousSol);
            Game.Components.Remove(this);
            Game.Components.Add(new GestionPartie(Game));
            MediaPlayer.Stop();
        }

        void InitialiserSousSol()
        {
            EnvironnementPartie = new GestionEnvironnement(Game, Environnements.SousSol, SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex2.ToString(), SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex.ToString(),TypePartie.Local);
            ModifierActivation();
            if (EstPartieActive)
            {
                Game.Components.Add(EnvironnementPartie);
                Game.Components.Add(ath);
            }
            Game.Components.Remove(BoutonGarage);
            Game.Components.Remove(BoutonSalleManger);
            Game.Components.Remove(BoutonSousSol);
            Game.Components.Remove(this);
            Game.Components.Add(new GestionPartie(Game));
            MediaPlayer.Stop();
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
            GestionSprites.Draw(ImageFondÉcran, RectangleFondÉcran, Color.White);
            GestionSprites.Draw(ImageMenuGarage, RectangleGarage, Color.White);
            GestionSprites.Draw(ImageMenuSalleManger, RectangleSalleManger, Color.White);
            GestionSprites.Draw(ImageMenuSousSol, RectangleSousSol, Color.White);
            GestionSprites.End();
            base.Draw(gameTime);
        }


    }
}
