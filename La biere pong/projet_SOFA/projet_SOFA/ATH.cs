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
        IndicateurForce indicateurForce { get; set; }
        RectangleColoré planPause { get; set; }
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
            BoutonLancer = new BoutonDeCommande(Game, "Lancer", "Impact20", "BoutonBleu", "BoutonBleuPale", PositionBoutonLancer, true, ActionLancer);
            BoutonPause = new BoutonDeCommande(Game, "Pause", "Impact20", "BoutonBleu", "BoutonBleuPale", PositionBoutonPause, true, MettreEnPause);

            Game.Components.Add(BoutonLancer);
            Game.Components.Add(BoutonPause);

            base.Initialize();
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
            Game.Components.Remove(planPause);
            Game.Components.Remove(BoutonRésumer);
            Game.Components.Remove(BoutonQuitter);

            foreach (IActivable composant in Game.Components.Where(composant => composant is IActivable))
            {
                composant.ModifierActivation();
            }

            PositionBoutonLancer = new Vector2(Game.Window.ClientBounds.Width - 60, Game.Window.ClientBounds.Height - 40);
            PositionBoutonPause = new Vector2(Game.Window.ClientBounds.Width - 60, 40);

            BoutonLancer = new BoutonDeCommande(Game, "Lancer", "Impact20", "BoutonBleu", "BoutonBleuPale", PositionBoutonLancer, true, ActionLancer);
            BoutonPause = new BoutonDeCommande(Game, "Pause", "Impact20", "BoutonBleu", "BoutonBleuPale", PositionBoutonPause, true, MettreEnPause);

            Game.Components.Add(BoutonLancer);
            Game.Components.Add(BoutonPause);
        }

        void ActionLancer()
        {
            Game.Components.Add(new IndicateurForce(Game));
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}