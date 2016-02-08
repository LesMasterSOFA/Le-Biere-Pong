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

    public class IndicateurForce : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Vector2 PositionFond { get; set; }
        Rectangle GrandeurFond { get; set; }
        Texture2D ImageFondIndicateurForce { get; set; }
        RessourcesManager<Texture2D> GestionTextures { get; set; }
        SpriteBatch GestionSprites { get; set; }
        Texture2D ImageBarreIndication { get; set; }
        Vector2 PositionBarreIndication { get; set; }
        Rectangle GrandeurBarre { get; set; }

        public IndicateurForce(Game game, Vector2 position, Vector2 grandeur)
            : base(game)
        {
            PositionFond = position;
            GrandeurFond = new Rectangle(0, 0, (int)grandeur.X, (int)grandeur.Y);
        }

        public override void Initialize()
        {
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            GestionTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            ImageFondIndicateurForce = GestionTextures.Find("FondIndicateurForce");
            ImageBarreIndication = GestionTextures.Find("BarreIndicationForce");

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            PositionBarreIndication = new Vector2(100,100);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Begin();
            GestionSprites.Draw(ImageFondIndicateurForce, PositionFond, GrandeurFond, Color.White);
            GestionSprites.Draw(ImageBarreIndication, PositionBarreIndication, GrandeurBarre, Color.White);
            GestionSprites.End();
            base.Draw(gameTime);
        }


    }
}
