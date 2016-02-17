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
    public class RectangleColoré : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Rectangle rectangle { get; set; }
        SpriteBatch GestionSprites { get; set; }
        RessourcesManager<Texture2D> GestionTextures { get; set; }
        Texture2D textureNoire { get; set; }
        public RectangleColoré(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            GestionTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;

            rectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);

            base.Initialize();
        }
        protected override void LoadContent()
        {
            base.LoadContent();
            textureNoire = GestionTextures.Find("noir");
        }
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Begin();
            GestionSprites.Draw(textureNoire, rectangle,new Color(0,0,0,175));
            GestionSprites.End();
            base.Draw(gameTime);
        }
    }
}
