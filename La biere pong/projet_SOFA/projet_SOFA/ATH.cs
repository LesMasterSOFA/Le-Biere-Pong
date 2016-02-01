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
    public class ATH : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Vector2 PositionBoutonLancer { get; set; }
        Vector2 PositionBoutonPause { get; set; }
        BoutonDeCommande BoutonPause { get; set; }
        BoutonDeCommande BoutonLancer { get; set; }
        BoutonDeCommande BoutonRésumer { get; set; }
        BoutonDeCommande BoutonQuitter { get; set; }
        PlanColoré planPause { get; set; }
        string NombreDePoints { get; set; }
        int LargeurÉcran { get; set; }
        int HauteurÉcran { get; set; }

        public ATH(Game game)
            : base(game){}
        public override void Initialize()
        {
            LargeurÉcran = Game.Window.ClientBounds.Width;
            HauteurÉcran = Game.Window.ClientBounds.Height;

            PositionBoutonLancer = new Vector2(Game.Window.ClientBounds.Width - 60, Game.Window.ClientBounds.Height - 40);
            PositionBoutonPause = new Vector2(Game.Window.ClientBounds.Width - 60, 40);
            BoutonLancer = new BoutonDeCommande(Game, "Lancer", "Arial20", "BoutonBleu", "BoutonBleuPale", PositionBoutonLancer, false, ActionLancer);
            BoutonPause = new BoutonDeCommande(Game, "Pause", "Arial20", "BoutonBleu", "BoutonBleuPale", PositionBoutonPause, true, MettreEnPause);

            Game.Components.Add(BoutonLancer);
            Game.Components.Add(BoutonPause);

            base.Initialize();
        }
        public void MettreEnPause()
        {
            Game.Components.Remove(BoutonLancer);
            Game.Components.Remove(BoutonPause);

            Vector2 Position1 = new Vector2(Game.Window.ClientBounds.Width/2,Game.Window.ClientBounds.Height/2 - 30);
            Vector2 Position2 = new Vector2(Game.Window.ClientBounds.Width/2,Game.Window.ClientBounds.Height/2 + 30);
            planPause = new PlanColoré(Game, 1f, new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector2(Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height), new Vector2(1, 1), new Color(0,0,0,175), 1f);
            BoutonRésumer = new BoutonDeCommande(Game, "Résumer", "Arial20", "BoutonBleu", "BoutonBleuPale", Position1, true, MettreEnPlay);
            BoutonQuitter = new BoutonDeCommande(Game, "Quitter", "Arial20", "BoutonBleu", "BoutonBleuPale", Position2, true, Game.Exit);

            Game.Components.Add(planPause);
            Game.Components.Add(BoutonRésumer);
            Game.Components.Add(BoutonQuitter);
        }
        public void MettreEnPlay()
        {
            Game.Components.Remove(planPause);
            Game.Components.Remove(BoutonRésumer);
            Game.Components.Remove(BoutonQuitter);

            PositionBoutonLancer = new Vector2(Game.Window.ClientBounds.Width - 60, Game.Window.ClientBounds.Height - 40);
            PositionBoutonPause = new Vector2(Game.Window.ClientBounds.Width - 60, 40);

            BoutonLancer = new BoutonDeCommande(Game, "Lancer", "Arial20", "BoutonBleu", "BoutonBleuPale", PositionBoutonLancer, false, null);
            BoutonPause = new BoutonDeCommande(Game, "Pause", "Arial20", "BoutonBleu", "BoutonBleuPale", PositionBoutonPause, true, MettreEnPause);

            Game.Components.Add(BoutonLancer);
            Game.Components.Add(BoutonPause);
        }
        void ActionLancer()
        {
            Vector2 positionIndicateurForce = new Vector2(30, HauteurÉcran - 60);
            Vector2 grandeurIndicateurForce = new Vector2(100, 50);

            IndicateurForce indicateurForce = new IndicateurForce(this.Game, positionIndicateurForce, grandeurIndicateurForce);
            Game.Components.Add(indicateurForce);
        }
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
    }
}
