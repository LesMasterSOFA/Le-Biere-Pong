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

    public class IndicateurForce : Microsoft.Xna.Framework.DrawableGameComponent,IActivable
    {
        Vector2 Résolution { get; set; }
        int Force { get; set; }
        Vector2 PositionFond { get; set; }
        Rectangle GrandeurFond { get; set; }
        Texture2D ImageFondIndicateurForce { get; set; }
        RessourcesManager<Texture2D> GestionTextures { get; set; }
        RessourcesManager<SpriteFont> GestionFont { get; set; }
        SpriteBatch GestionSprites { get; set; }
        InputManager GestionInput { get; set; }
        Texture2D BarreIndicatrice { get; set; }
        Vector2 PositionBarreIndication { get; set; }
        float PositionMilieu { get; set; }
        Vector2 AnciennePositionBarre { get; set; }
        bool estActifBarre;
        Rectangle GrandeurBarre { get; set; }
        float vitesse;
        public float VitesseBarre
        {
            get
            {
                return vitesse;
            }
            private set
            {
                vitesse = ModifierVitesseBarre(value);
            }
        }

        public IndicateurForce(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            Résolution = new Vector2(Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);

            PositionFond = new Vector2(Résolution.X / 26.667f, Résolution.Y - Résolution.Y / 6.8571f);
            int x=(int)(Résolution.X / 4.0404f);
            GrandeurFond = new Rectangle((int)PositionFond.X, (int)PositionFond.Y,x ,x/4);

            PositionMilieu = PositionFond.X + GrandeurFond.Width / 2;
            Force = 0;
            estActifBarre = true;
            VitesseBarre = GrandeurFond.X/6000f;
            GrandeurBarre = new Rectangle(0, 0, 5, GrandeurFond.Height);
            AnciennePositionBarre = new Vector2(0, 0);
            PositionBarreIndication = new Vector2(PositionFond.X + GrandeurFond.Width / 2, PositionFond.Y);
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            GestionFont = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GestionTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            BarreIndicatrice = GestionTextures.Find("BarreIndicationForce");
            ImageFondIndicateurForce = GestionTextures.Find("FondIndicateurForce");
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if(estActifBarre)
            {
                AnciennePositionBarre = PositionBarreIndication;
                PositionBarreIndication = new Vector2(AnciennePositionBarre.X + VitesseBarre*GrandeurFond.X, AnciennePositionBarre.Y);
                if (PositionBarreIndication.X + GrandeurBarre.Width >= PositionFond.X + GrandeurFond.Width || PositionBarreIndication.X <= PositionFond.X)
                {
                    VitesseBarre = -VitesseBarre;
                }
                if (GestionInput.EstEnfoncée(Keys.Space))
                {
                    estActifBarre = false;
                    Force = DéterminerForce(PositionBarreIndication.X);
                    //PositionBarreIndication = new Vector2(PositionMilieu, AnciennePositionBarre.Y);
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Begin();
            GestionSprites.Draw(ImageFondIndicateurForce, GrandeurFond, Color.White);
            GestionSprites.Draw(BarreIndicatrice,PositionBarreIndication, GrandeurBarre, Color.Black);
            GestionSprites.DrawString(GestionFont.Find("Impact20"), "Force : "+Force.ToString(), Vector2.Zero, Color.Black);
            GestionSprites.End();
            base.Draw(gameTime);
        }
        float ModifierVitesseBarre(float nvVitesse)
        {
            return nvVitesse;
        }
        int DéterminerForce(float postionEnX)
        {
            float différence = PositionMilieu-postionEnX;
            différence = Math.Abs(différence);
            float taux = (PositionMilieu-différence)/PositionMilieu * 100;
            return (int)taux;
        }
        public void ModifierActivation()
        {
            estActifBarre = !estActifBarre;
        }

    }
}
