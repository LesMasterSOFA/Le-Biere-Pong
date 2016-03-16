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
        public Mode1v1LAN(Game game)
            : base(game)
        {

        }


        public override void Initialize()
        {
            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void GestionInputGameplay(Joueur joueur, GameTime gameTime)
        {
            UpdateInput(gameTime, joueur);

            joueur.Update(gameTime);

        }

        //s'occupe de la gestion des inputs
        void UpdateInput(GameTime gameTime, Joueur joueur)
        {
            joueur.Update(gameTime);
        }
    }
}
