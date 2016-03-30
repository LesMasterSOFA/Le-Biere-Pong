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
    public class PartieSolo : Partie
    {
        protected ATH ath { get; set; }
        public Joueur JoueurPrincipal { get; set; }

        public PartieSolo(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            JoueurPrincipal = new Joueur(Game, base.gestionnairePartie, Game.GraphicsDevice.Viewport);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
