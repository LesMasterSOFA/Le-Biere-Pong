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
    public class AffichageInfoLancer : ATH
    {
        Cam�ra Cam�raJeu { get; set; }

        RessourcesManager<SpriteFont> GestionFonts { get; set; }
        SpriteBatch GestionSprites { get; set; }

        int InfoAngle { get; set; }
        int Force { get; set; }

        public AffichageInfoLancer(Game game,int force)
            : base(game)
        {
            Force = force;
        }

        public override void Initialize()
        {
            Cam�raJeu = Game.Services.GetService(typeof(Cam�ra)) as Cam�ra;
            
            if (Cam�raJeu.Vue.Forward.X != 0 &&Cam�raJeu.Vue.Forward.Z != 0)
            {
                InfoAngle = (int)MathHelper.ToDegrees((float)Math.Tan(Cam�raJeu.Vue.Forward.X / Cam�raJeu.Vue.Forward.Z));
            }
            else 
            {
                InfoAngle = 0;
            }

            GestionFonts = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Begin();
            GestionSprites.DrawString(GestionFonts.Find("Impact20"), "Force : " + Force.ToString(), Vector2.Zero, Color.Black);
            GestionSprites.DrawString(GestionFonts.Find("Impact20"), "Angle : " + InfoAngle.ToString(), new Vector2(0,50), Color.Black);
            GestionSprites.End();
            base.Draw(gameTime);
        }
    }
}
