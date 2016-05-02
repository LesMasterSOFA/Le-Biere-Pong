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
    public class AffichageInfoLancer : DrawableGameComponent
    {
        Caméra CaméraJeu { get; set; }

        RessourcesManager<SpriteFont> GestionFonts { get; set; }
        SpriteBatch GestionSprites { get; set; }

        public float InfoAngleHor { get; set; }
        public float InfoAngleVert { get; set; }
        public float Force { get; set; }
        ATH ath { get; set; }

        public AffichageInfoLancer(Game game, float force)
            : base(game)
        {
            Force = force;
        }
        public AffichageInfoLancer(Game game, float force, float angleH, float angleV)
            : base(game)
        {
            Force = force;
            InfoAngleHor = angleH;
            InfoAngleVert = angleV;
        }

        public override void Initialize()
        {
            CaméraJeu = Game.Services.GetService(typeof(Caméra)) as Caméra;

            foreach (ATH hud in Game.Components.Where(item => item is ATH))
            {
                ath = hud;
            }
            if (ath.EstTourJoueurPrincipal)
            {
                if (CaméraJeu.Position.Z > 0)
                {
                    InfoAngleHor = (float)MathHelper.ToDegrees((float)Math.Tan(CaméraJeu.Vue.Forward.X / CaméraJeu.Vue.Forward.Z));
                }
                else
                {
                    InfoAngleHor = (float)MathHelper.ToDegrees((float)Math.Tan(-CaméraJeu.Vue.Forward.X / CaméraJeu.Vue.Forward.Z));
                }
                InfoAngleVert = (float)MathHelper.ToDegrees((float)Math.Tan(CaméraJeu.Vue.Forward.Y / CaméraJeu.Vue.Forward.Z)) + 31.3f;
            }
            //else 
            //{
            //    InfoAngleHor = 0;
            //    InfoAngleVert = 0;
            //}
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
            GestionSprites.DrawString(GestionFonts.Find("Impact20"), "Force : " + ((int)Force).ToString() + "%", Vector2.Zero, Color.Black);
            GestionSprites.DrawString(GestionFonts.Find("Impact20"), "Angle horizontal : " + Math.Abs(InfoAngleHor).ToString("0.00") + "°", new Vector2(0, 50), Color.Black);
            GestionSprites.DrawString(GestionFonts.Find("Impact20"), "Angle vertical : " + InfoAngleVert.ToString("0.00") + "°", new Vector2(0, 100), Color.Black);
            GestionSprites.End();
            base.Draw(gameTime);
        }
    }
}
