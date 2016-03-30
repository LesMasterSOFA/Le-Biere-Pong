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


namespace AtelierXNA
{

    public class ModePratique : PartieSolo
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
        public ModePratique(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            RectangleFondÉcran = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height+15);
            RectangleGarage = new Rectangle(300, 150, 266, 233);
            RectangleSalleManger = new Rectangle(750, 150, 266, 233);
            RectangleSousSol = new Rectangle(1200, 150, 266, 233);
            ath = new ATH(Game);
            base.Initialize();
            MenuSélectionPersonnage();
            
        }
        protected override void LoadContent()
        {
            base.LoadContent();
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            gestionnaireFont = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            gestionnaireTexture = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            ImageFondÉcran = gestionnaireTexture.Find("BeerPong");
            ImageMenuGarage = gestionnaireTexture.Find("MenuGarage");
            ImageMenuSalleManger = gestionnaireTexture.Find("MenuSalle");
            ImageMenuSousSol = gestionnaireTexture.Find("MenuSousSol");
            BoutonBleu = gestionnaireTexture.Find("BoutonBleu");
        }
        void ActiverEnvironnement()
        {
            if (EstPartieActive)
            {
                Game.Components.Add(EnvironnementPartie);
                Game.Components.Add(ath);
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        
        void MenuSélectionPersonnage()
        {
            BoutonGarage = new BoutonDeCommande(Game, "Garage", "Impact20", "BoutonBleu", "BoutonBleuPale", new Vector2(430, 100), true, InitialiserGarage);
            BoutonSalleManger = new BoutonDeCommande(Game, "Salle à manger", "Impact20", "BoutonBleu", "BoutonBleuPale", new Vector2(885,100), true, InitialiserSalle);
            BoutonSousSol = new BoutonDeCommande(Game, "Sous-sol", "Impact20", "BoutonBleu", "BoutonBleuPale", new Vector2(1340, 100), true, InitialiserSousSol);
            Game.Components.Add(BoutonGarage);
            Game.Components.Add(BoutonSalleManger);
            Game.Components.Add(BoutonSousSol);
        }
        void InitialiserGarage()
        {
            EnvironnementPartie = new GestionEnvironnement(Game, Environnements.Garage);
            ActiverPartie();
        }
        void InitialiserSalle()
        {
            EnvironnementPartie = new GestionEnvironnement(Game, Environnements.SalleManger);
            ActiverPartie();
        }
        void InitialiserSousSol()
        {
           EnvironnementPartie = new GestionEnvironnement(Game, Environnements.SousSol);
           ActiverPartie();
        }
        void ActiverPartie()
        {
            ModifierActivation();
            ActiverEnvironnement();
            Game.Components.Remove(BoutonGarage);
            Game.Components.Remove(BoutonSalleManger);
            Game.Components.Remove(BoutonSousSol);
            Game.Components.Remove(this);
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
