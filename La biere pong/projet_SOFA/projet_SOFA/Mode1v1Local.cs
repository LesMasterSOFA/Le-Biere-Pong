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

    public class Mode1v1Local : PartieMultijoueur
    {
        Viewport mainViewport; //Écran totale
        Viewport leftViewport; //moitié écran gauche
        Viewport rightViewport; //moitié écran droite

        public Mode1v1Local(Game game)
            : base(game)
        {

        }


        public override void Initialize()
        {
            // Initialize the values for each viewport -> utile pour multijoueur
            mainViewport = this.Game.GraphicsDevice.Viewport;
            leftViewport = mainViewport;
            rightViewport = mainViewport;
            leftViewport.Width = leftViewport.Width / 2;
            rightViewport.Width = rightViewport.Width / 2;
            rightViewport.X = leftViewport.Width + 1;

            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }
    }
}
