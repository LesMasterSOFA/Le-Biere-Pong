//
// Auteur : Vincent Echelard
//
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
   public delegate void FonctionÉvénemtielle();

   public class BoutonDeCommande : Microsoft.Xna.Framework.DrawableGameComponent
   {
      const float MARGE_BOUTON = 0.2f; // 20%
      Color COULEUR_PAR_DÉFAUT = Color.Black;
      Color COULEUR_FOCUS = Color.White;
      Color COULEUR_INACTIF = Color.Gray;
      string Texte { get; set; }
      string NomFont { get; set; }
      string NomImageNormale { get; set; }
      string NomImageEnfoncée { get; set; }
      Vector2 Position { get; set; }
      Vector2 PositionChaîne { get; set; }
      Vector2 OrigineChaîne { get; set; }
      SpriteFont PoliceDeCaractères { get; set; }
      Texture2D ImageNormale { get; set; }
      Texture2D ImageEnfoncée { get; set; }
      Texture2D ImageBouton { get; set; }
      Rectangle RectangleDestination { get; set; }
      Color CouleurTexte { get; set; }
      SpriteBatch GestionSprites { get; set; }
      InputManager GestionInput { get; set; }
      RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }
      RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
      FonctionÉvénemtielle OnClick { get; set; }

      bool estActif;

      public bool EstActif
      {
         get { return estActif; }
         set
         {
            estActif = value;
            CouleurTexte = estActif ? COULEUR_PAR_DÉFAUT : COULEUR_INACTIF;
         }
      }


      public BoutonDeCommande(Game jeu, string texte, string nomFont, string nomImageNormale, string nomImageEnfoncée, 
                              Vector2 position, bool estActif, FonctionÉvénemtielle onClick)
         : base(jeu)
      {
         Texte = texte;
         NomFont = nomFont;
         NomImageNormale = nomImageNormale;
         NomImageEnfoncée = nomImageEnfoncée;
         Position = position;
         EstActif = estActif;
         OnClick = onClick;
      }

      protected override void LoadContent()
      {
         Vector2 dimensionChaîne;
         Vector2 dimension;
         GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
         GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
         GestionnaireDeFonts = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
         GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;

         PoliceDeCaractères = GestionnaireDeFonts.Find(NomFont);
         ImageNormale = GestionnaireDeTextures.Find(NomImageNormale);
         ImageEnfoncée = GestionnaireDeTextures.Find(NomImageEnfoncée);
         ImageBouton = ImageNormale;
         dimensionChaîne = PoliceDeCaractères.MeasureString(Texte);
         dimension = dimensionChaîne * (1 + MARGE_BOUTON);
         Position = Position - dimension / 2;
         PositionChaîne = new Vector2(Position.X + dimension.X / 2, Position.Y + dimension.Y / 2);
         OrigineChaîne = new Vector2(dimensionChaîne.X / 2, dimensionChaîne.Y / 2);
         RectangleDestination = new Rectangle((int)Position.X, (int)Position.Y, (int)dimension.X, (int)dimension.Y);
         base.LoadContent();
      }

      public override void Update(GameTime gameTime)
      {
         if (EstActif)
         {
            Point positionSouris = GestionInput.GetPositionSouris();
            if (RectangleDestination.Contains(positionSouris))
            {
               CouleurTexte = COULEUR_FOCUS;
               if (GestionInput.EstNouveauClicGauche())
               {
                  OnClick();
                  ImageBouton = ImageEnfoncée;
               }
               else
               {
                  if (!GestionInput.EstAncienClicGauche())
                  {
                     ImageBouton = ImageNormale;
                  }
               }
            }
            else
            {
               CouleurTexte = EstActif ? COULEUR_PAR_DÉFAUT : COULEUR_INACTIF;
               ImageBouton = ImageNormale;
            }
         }
         else
         {
            ImageBouton = ImageNormale;
         }
         base.Update(gameTime);
      }

      public override void Draw(GameTime gameTime)
      {
         GestionSprites.Begin();
         GestionSprites.Draw(ImageBouton, RectangleDestination, Color.White);
         GestionSprites.DrawString(PoliceDeCaractères, Texte, PositionChaîne, CouleurTexte, 0, OrigineChaîne, 1f, SpriteEffects.None, 1);
         GestionSprites.End();
         base.Draw(gameTime);
      }
   }
}

