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
    public class ATH : Microsoft.Xna.Framework.DrawableGameComponent
    {
        List<BoutonDeCommande> listeBoutons { get; set; }
        IndicateurForce indicateurForce { get; set; }
        Vector2 PositionBoutonLancer { get; set; }
        Vector2 PositionBoutonPause { get; set; }
        BoutonDeCommande BoutonPause { get; set; }
        public BoutonDeCommande BoutonLancer { get; set; }
        BoutonDeCommande BoutonRésumer { get; set; }
        BoutonDeCommande BoutonQuitter { get; set; }
        RectangleColoré planPause { get; set; }
        string NombreDePoints { get; set; }
        int LargeurÉcran { get; set; }
        int HauteurÉcran { get; set; }

        public Joueur JoueurCourant { get; private set; }

        public ATH(Game game, Joueur joueurCourant)
            : base(game){}
        public override void Initialize()
        {
            listeBoutons = new List<BoutonDeCommande>();

            indicateurForce = new IndicateurForce(Game, JoueurCourant);

            LargeurÉcran = Game.Window.ClientBounds.Width;
            HauteurÉcran = Game.Window.ClientBounds.Height;

            InitialiserBoutons();

            base.Initialize();
        }
        public void InitialiserBoutons()
        {
            PositionBoutonLancer = new Vector2(Game.Window.ClientBounds.Width - 60, Game.Window.ClientBounds.Height - 40);
            PositionBoutonPause = new Vector2(Game.Window.ClientBounds.Width - 60, 40);
            BoutonLancer = new BoutonDeCommande(Game, "Lancer", "Impact20", "BoutonBleu", "BoutonBleuPale", PositionBoutonLancer, true, ActionLancer);
            BoutonLancer.Enabled = true;////
            BoutonPause = new BoutonDeCommande(Game, "Pause", "Impact20", "BoutonBleu", "BoutonBleuPale", PositionBoutonPause, true, MettreEnPause);

            Game.Components.Add(BoutonLancer);
            Game.Components.Add(BoutonPause);
        }
        public void MettreEnPause()
        {
            foreach (IActivable composant in Game.Components.Where(composant => composant is IActivable))
            {
                composant.ModifierActivation();
            }

            Vector2 Position1 = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2 - 30);
            Vector2 Position2 = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2 + 30);
            planPause = new RectangleColoré(Game);
            BoutonRésumer = new BoutonDeCommande(Game, "Résumer", "Impact20", "BoutonBleu", "BoutonBleuPale", Position1, true, MettreEnPlay);
            BoutonQuitter = new BoutonDeCommande(Game, "Quitter", "Impact20", "BoutonBleu", "BoutonBleuPale", Position2, true, Game.Exit);

            
            Game.Components.Add(planPause);
            Game.Components.Add(BoutonRésumer);
            Game.Components.Add(BoutonQuitter); 
            
        }

        public void MettreEnPlay()
        {
            Game.Components.Remove(BoutonQuitter);
            Game.Components.Remove(BoutonRésumer);
            Game.Components.Remove(planPause);

            foreach (IActivable composant in Game.Components.Where(composant => composant is IActivable))
            {
                composant.ModifierActivation();
            }

            //PositionBoutonLancer = new Vector2(Game.Window.ClientBounds.Width - 60, Game.Window.ClientBounds.Height - 40);
            //PositionBoutonPause = new Vector2(Game.Window.ClientBounds.Width - 60, 40);

            //BoutonLancer = new BoutonDeCommande(Game, "Lancer", "Impact20", "BoutonBleu", "BoutonBleuPale", PositionBoutonLancer, true, ActionLancer);
            //BoutonPause = new BoutonDeCommande(Game, "Pause", "Impact20", "BoutonBleu", "BoutonBleuPale", PositionBoutonPause, true, MettreEnPause);

            //listeBoutons.Add(BoutonLancer);
            //listeBoutons.Add(BoutonPause);
            //AjouterNouveauxBoutons();
        }

        void ActionLancer()
        {
            if (Game.Components.Contains(indicateurForce))
            {
                Game.Components.Remove(indicateurForce);
            }
            Game.Components.Add(indicateurForce);
            BoutonLancer.Enabled = true;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        void AjouterNouveauxBoutons()
        {
            foreach (BoutonDeCommande btn in listeBoutons)
            {
                Game.Components.Add(btn);
            }

        }
        void EnleverBoutonsExistants()
        {
            if (listeBoutons.Count != 0)
            {
                foreach (BoutonDeCommande btn in listeBoutons)
                {
                    Game.Components.Remove(btn);
                }
                listeBoutons.Clear();
            }
        }
    }
}