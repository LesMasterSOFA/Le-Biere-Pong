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
        BoutonDeCommande BoutonPause { get; set; }
        BoutonDeCommande BoutonLancer { get; set; }
        string NombreDePoints { get; set; }
        int LargeurÉcran { get; set; }
        int HauteurÉcran { get; set; }
        

       
        public ATH(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            LargeurÉcran = Game.Window.ClientBounds.Width;
            HauteurÉcran = Game.Window.ClientBounds.Height;

            Vector2 positionBoutonLancer = new Vector2(LargeurÉcran - 60, HauteurÉcran - 40);
            Vector2 positionBoutonPause = new Vector2(LargeurÉcran - 60, 40);
            BoutonLancer = new BoutonDeCommande(Game, "Lancer", "Arial20", "BoutonBleu", "BoutonBleuPale", positionBoutonLancer, true, ActionLancer);
            BoutonPause = new BoutonDeCommande(Game, "Pause", "Arial20", "BoutonBleu", "BoutonBleuPale", positionBoutonPause, true, null);
            Game.Components.Add(BoutonLancer);
            Game.Components.Add(BoutonPause);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        void ActionLancer()
        {
            Vector2 positionIndicateurForce = new Vector2(30,HauteurÉcran - 60);
            Vector2 grandeurIndicateurForce= new Vector2(100, 50);

            IndicateurForce indicateurForce = new IndicateurForce(this.Game, positionIndicateurForce, grandeurIndicateurForce);
            Game.Components.Add(indicateurForce);
        }
    }
}
