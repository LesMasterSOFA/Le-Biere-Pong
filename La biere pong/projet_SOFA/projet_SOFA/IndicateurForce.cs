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

    public class IndicateurForce : DrawableGameComponent, IActivable
    {
        Joueur JoueurCourant { get; set; }
        AffichageInfoLancer affInfo { get; set; }
        Vector2 R�solution { get; set; }
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
        public float VitesseBarre { get; set; }
        Cam�raJoueur cam { get; set; }
        BallePhysique balle { get; set; }
        List<Personnage> ListePerso { get; set; }
        ATH ath { get; set; }

        public IndicateurForce(Game game, Joueur joueur)
            : base(game)
        {
            JoueurCourant = joueur;
        }

        public override void Initialize()
        {
            ListePerso = new List<Personnage>();
            foreach (Cam�raJoueur camera in Game.Components.Where(item => item is Cam�ra))
            {
                cam = camera;
            }

            foreach (Personnage perso in Game.Components.Where(item => item is Personnage))
            {
                ListePerso.Add(perso);
            }
            foreach (ATH hud in Game.Components.Where(hud => hud is ATH))
            {
                ath = hud;
            }

            estActifBarre = true;

            R�solution = new Vector2(Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            int x = (int)(R�solution.X / 4.0404f);

            PositionFond = new Vector2(R�solution.X / 26.667f, R�solution.Y - R�solution.Y / 6.8571f);
            GrandeurFond = new Rectangle((int)PositionFond.X, (int)PositionFond.Y, x, x / 4);
            PositionMilieu = PositionFond.X + GrandeurFond.Width / 2;
            VitesseBarre = GrandeurFond.X / 1000f;

            GrandeurBarre = new Rectangle(0, 0, 5, GrandeurFond.Height);
            AnciennePositionBarre = new Vector2(0, 0);
            PositionBarreIndication = new Vector2(PositionFond.X + GrandeurFond.Width / 2, PositionFond.Y);

            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            GestionFont = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GestionTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            BarreIndicatrice = GestionTextures.Find("BarreIndicationForce");
            ImageFondIndicateurForce = GestionTextures.Find("FondIndicateurForce");

            JoueurCourant = new Joueur(Game);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (estActifBarre)
            {
                AnciennePositionBarre = PositionBarreIndication;
                PositionBarreIndication = new Vector2(AnciennePositionBarre.X + VitesseBarre * GrandeurFond.X, AnciennePositionBarre.Y);
                if (PositionBarreIndication.X + GrandeurBarre.Width >= PositionFond.X + GrandeurFond.Width || PositionBarreIndication.X <= PositionFond.X)
                {
                    VitesseBarre = -VitesseBarre;
                }
                if (GestionInput.EstNouvelleTouche(Keys.Space))
                {
                    if (Game.Components.Contains(affInfo))
                    {
                        Game.Components.Remove(affInfo);
                    }
                    estActifBarre = false;
                    affInfo = new AffichageInfoLancer(Game, D�terminerForce(PositionBarreIndication.X));
                    if (cam.Position.Z > 0)
                    {
                        JoueurCourant.ChangerAnimation(TypeActionPersonnage.Lancer, ListePerso.Find(item => item.Position.Z > 0));
                    }
                    else
                    {
                        JoueurCourant.ChangerAnimation(TypeActionPersonnage.Lancer, ListePerso.Find(perso => perso.Position.Z < 0));
                    }
                    Game.Components.Add(affInfo);
                    Game.Components.Remove(this);

                    cam.TempsTotal = 0;
                    cam.EstMouvCamActif = true;
                    ath.BoutonLancer.EstActif = !ath.BoutonLancer.EstActif;
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Begin();
            GestionSprites.Draw(ImageFondIndicateurForce, GrandeurFond, Color.White);
            GestionSprites.Draw(BarreIndicatrice, PositionBarreIndication, GrandeurBarre, Color.Black);
            GestionSprites.End();
            base.Draw(gameTime);
        }
        float D�terminerForce(float postionEnX)
        {
            float diff�rence = Math.Abs(PositionMilieu - postionEnX);
            float taux = (PositionMilieu - diff�rence) / PositionMilieu * 100;
            return taux;
        }
        public void ModifierActivation()
        {
            estActifBarre = false;
        }

    }
}
