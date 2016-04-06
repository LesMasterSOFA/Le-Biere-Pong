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
   public class BallePhysique : ObjetDeBase
   {
      const float INCERTITUDE = 0.0000001f;
      const float CONSTANTE_RESTITUTION_VERTICALE = 0.8f;
      const float CONSTANTE_RESTITUTION_BONHOMME = 0.2f;
      const float COEFFICIENT_FROTTEMENT = 0.85f;
      const float CHANGEMENT_DIRECTION = -1f;
      const float GRAVITÉ = 9.81f;
      const float RAYON_BALLE = 0.03f;
      BoundingSphere SphèreBalle { get; set; }
      float VitesseInitiale { get; set; }
      float VitesseEnX { get; set; }
      float VitesseEnY { get; set; }
      float VitesseEnZ { get; set; }
      float IntervalleMAJ { get; set; }
      float TempsÉcouléDepuisMAJ { get; set; }
      float TempsTotal { get; set; }
      Vector3 PositionInitiale { get; set; }
      float AngleHorizontal { get; set; }
      float AngleVertical { get; set; }
      BoundingBox BoundingTable { get; set; }
      BoundingBox BoundingBonhomme { get; set; }
      List<Vector3> ListePositionVerresAdv { get; set; }
      float RayonVerre { get; set; }
      float HauteurVerre { get; set; }
      float HauteurTable { get; set; }
      float RayonCollision { get; set; }
      float AngleLimiteVert { get; set; }
      public bool EnleverBalle { get; private set; }
      float Réduction { get; set; }
      int IndexCollision { get; set; }
      float DimensionTerrain { get; set; }

      public BallePhysique(Game jeu, string nomModèle, string nomTexture, string nomEffet, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale,
           float vitesseInitiale, float angleHorizontal, float angleVertical, BoundingBox boundingTable, BoundingBox boundingBonhomme, List<Vector3> listePositionVerresAdv, float rayonVerre, float hauteurVerre, float hauteurTable, float dimensionTerrain, float intervalleMAJ)
         : base(jeu, nomModèle, nomTexture, nomEffet, échelleInitiale, rotationInitiale, positionInitiale)
      {
         PositionInitiale = positionInitiale;
         Position = positionInitiale;
         VitesseInitiale = vitesseInitiale;
         AngleHorizontal = angleHorizontal;
         AngleVertical = angleVertical;
         BoundingTable = boundingTable;
         BoundingBonhomme = boundingBonhomme;
         ListePositionVerresAdv = listePositionVerresAdv;
         RayonVerre = rayonVerre;
         HauteurVerre = hauteurVerre;
         HauteurTable = hauteurTable;
         DimensionTerrain = dimensionTerrain;
         IntervalleMAJ = intervalleMAJ;
      }

      public override void Initialize()
      {
         Réduction = 1;
         EnleverBalle = false;
         AngleLimiteVert = (float)Math.Atan(RayonVerre / HauteurVerre);
         RayonCollision = (float)Math.Sqrt(Math.Pow(HauteurVerre, 2) + Math.Pow(RayonVerre, 2));
         VitesseEnX = VitesseInitiale * (float)Math.Cos(AngleVertical) * (float)Math.Sin(AngleHorizontal);
         VitesseEnY = VitesseInitiale * (float)Math.Sin(AngleVertical);
         VitesseEnZ = VitesseInitiale * (float)Math.Cos(AngleVertical) * (float)Math.Cos(AngleHorizontal);
         SphèreBalle = new BoundingSphere(Position, RAYON_BALLE);
         TempsÉcouléDepuisMAJ = 0;
         TempsTotal = TempsÉcouléDepuisMAJ;
         base.Initialize();
      }

      public override void Update(GameTime gameTime)
      {
         float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
         TempsÉcouléDepuisMAJ += TempsÉcoulé;
         if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
         {
            TempsTotal += TempsÉcouléDepuisMAJ;
            EffectuerDéplacement();
            Monde = GetMonde();
            TempsÉcouléDepuisMAJ = 0;
         }
         base.Update(gameTime);
      }
      void EffectuerDéplacement()
      {
         GérerCollisionVerres();
         GérerCollisionTable();
         GérerCollisionBonhomme();
         GérerCollisionPlancher();
         GérerCollisionMursFond();

         Position = new Vector3(PositionInitiale.X + VitesseEnX * TempsTotal,
                                PositionInitiale.Y + VitesseEnY * TempsTotal - GRAVITÉ * TempsTotal * TempsTotal / 2,
                                PositionInitiale.Z - VitesseEnZ * TempsTotal);
         SphèreBalle = new BoundingSphere(Position, RAYON_BALLE);
      }

      bool CollisionEntreDeuxVerres()
      {
         IndexCollision = -1;
         int cpt = 0;
         for (int i = 0; i < ListePositionVerresAdv.Count; ++i)
         {
            Vector3 vecteurDétectionAngle = Position - ListePositionVerresAdv[i];
            if (Vector3.Cross(vecteurDétectionAngle, Vector3.Up).Length() / Vector3.Up.Length() <= RayonVerre + RAYON_BALLE)
            {
               ++cpt;
               IndexCollision = i;
            }
         }

         return cpt > 1;
      }
      void GérerCollisionVerres()
      {
         if (Position.Y <= HauteurTable + HauteurVerre + RAYON_BALLE)
         {
            for (int i = 0; i < ListePositionVerresAdv.Count; ++i)
            {
               Vector3 vecteurDétectionAngle = Position - ListePositionVerresAdv[i];
               if (Vector3.Cross(vecteurDétectionAngle, Vector3.Up).Length() / Vector3.Up.Length() <= RayonVerre + RAYON_BALLE)
               {
                  if (CollisionEntreDeuxVerres() == true)
                  {
                      VitesseEnX = COEFFICIENT_FROTTEMENT * VitesseEnX;
                      VitesseEnZ = COEFFICIENT_FROTTEMENT * VitesseEnZ;
                     VitesseEnY = CONSTANTE_RESTITUTION_VERTICALE * Math.Abs(VitesseEnY - GRAVITÉ * TempsTotal);
                     PositionInitiale = new Vector3(Position.X, HauteurTable + HauteurVerre + RAYON_BALLE + INCERTITUDE, Position.Z);
                  }
                  else
                  {
                     float angleDirection = (float)Math.Acos(Vector3.Dot(vecteurDétectionAngle, Vector3.Up) / (vecteurDétectionAngle.Length() * Vector3.Up.Length()));

                     if (angleDirection <= AngleLimiteVert + 0.01f)
                     {
                        if (Math.Abs(angleDirection - AngleLimiteVert) <= 0.2f)
                        {
                            VitesseEnX = COEFFICIENT_FROTTEMENT * VitesseEnX;
                            VitesseEnZ = COEFFICIENT_FROTTEMENT * VitesseEnZ;
                           VitesseEnY = CONSTANTE_RESTITUTION_VERTICALE * Math.Abs(VitesseEnY - GRAVITÉ * TempsTotal);
                           PositionInitiale = new Vector3(Position.X, HauteurTable + HauteurVerre + RAYON_BALLE + INCERTITUDE, Position.Z);
                        }
                        else
                        {
                           VitesseEnX = 0;
                           VitesseEnZ = 0;
                           VitesseEnY = 0;
                           PositionInitiale = new Vector3(ListePositionVerresAdv[i].X, HauteurTable + HauteurVerre / 2, ListePositionVerresAdv[i].Z);
                        }
                     }
                     else
                     {
                        Vector2 replacerBalle = new Vector2(Position.X - ListePositionVerresAdv[i].X, Position.Z - ListePositionVerresAdv[i].Z);
                        replacerBalle = ((RayonVerre + RAYON_BALLE + INCERTITUDE) / replacerBalle.Length()) * replacerBalle;
                        VitesseEnX = COEFFICIENT_FROTTEMENT * VitesseEnX;
                        VitesseEnZ = COEFFICIENT_FROTTEMENT * CHANGEMENT_DIRECTION * VitesseEnZ;
                        VitesseEnY = CONSTANTE_RESTITUTION_VERTICALE * (VitesseEnY - GRAVITÉ * TempsTotal);
                        PositionInitiale = new Vector3(ListePositionVerresAdv[i].X + replacerBalle.X, Position.Y, ListePositionVerresAdv[i].Z + replacerBalle.Y);
                     }
                  }
                  TempsTotal = 0;
               }
            }
         }
      }

      void GérerCollisionTable()
      {
         if (SphèreBalle.Intersects(BoundingTable))
         {
            PositionInitiale = new Vector3(Position.X, HauteurTable + RAYON_BALLE + INCERTITUDE, Position.Z);
            VitesseEnY = CONSTANTE_RESTITUTION_VERTICALE * Math.Abs(VitesseEnY - GRAVITÉ * TempsTotal);
            VitesseEnX = COEFFICIENT_FROTTEMENT * VitesseEnX;
            VitesseEnZ = COEFFICIENT_FROTTEMENT * VitesseEnZ;
            TempsTotal = 0;
            if (Math.Abs(VitesseEnZ) < 0.1f)
            {
               Réduction += 0.04f;
               EnleverBalle = true;
               VitesseEnY /= Réduction;
            }
         }
      }

      void GérerCollisionBonhomme()
      {
          if (SphèreBalle.Intersects(BoundingBonhomme))
          {
              VitesseEnX = CONSTANTE_RESTITUTION_BONHOMME * VitesseEnX;
              VitesseEnZ = CHANGEMENT_DIRECTION * CONSTANTE_RESTITUTION_BONHOMME * VitesseEnZ;
              VitesseEnY = CONSTANTE_RESTITUTION_VERTICALE * (VitesseEnY - GRAVITÉ * TempsTotal);
              PositionInitiale = new Vector3(Position.X, Position.Y, Position.Z >= (BoundingBonhomme.Max.Z + BoundingBonhomme.Min.Z) / 2 ? BoundingBonhomme.Max.Z + RAYON_BALLE + INCERTITUDE : BoundingBonhomme.Min.Z - RAYON_BALLE - INCERTITUDE);
              TempsTotal = 0;
          }
      }

      void GérerCollisionPlancher()
      {
         if (Position.Y <= RAYON_BALLE)
         {
            PositionInitiale = new Vector3(Position.X, RAYON_BALLE + INCERTITUDE, Position.Z);
            VitesseEnY = CONSTANTE_RESTITUTION_VERTICALE * Math.Abs(VitesseEnY - GRAVITÉ * TempsTotal);
            VitesseEnX = COEFFICIENT_FROTTEMENT * VitesseEnX;
            VitesseEnZ = COEFFICIENT_FROTTEMENT * VitesseEnZ;
            TempsTotal = 0;
            if (Math.Abs(VitesseEnZ) < 0.1f)
            {
               Réduction += 0.04f;
               EnleverBalle = true;
               VitesseEnY /= Réduction;
            }
         }
      }

      void GérerCollisionMursFond()
      {
          if (Math.Abs(Position.Z) >= DimensionTerrain / 2 - RAYON_BALLE)
          {
              VitesseEnX = VitesseEnX;
              VitesseEnZ = CHANGEMENT_DIRECTION * VitesseEnZ;
              VitesseEnY = 1 * (VitesseEnY - GRAVITÉ * TempsTotal);
              PositionInitiale = new Vector3(Position.X, Position.Y, Position.Z > 0 ? DimensionTerrain / 2 - RAYON_BALLE - INCERTITUDE : -DimensionTerrain / 2 + RAYON_BALLE + INCERTITUDE);
              TempsTotal = 0;
          }
      }

      public override Matrix GetMonde()
      {
         Monde = Matrix.Identity;
         Monde *= Matrix.CreateScale(Échelle);
         Monde *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
         Monde *= Matrix.CreateTranslation(Position);
         return base.GetMonde();
      }
      public override void Draw(GameTime gameTime)
      {
         base.Draw(gameTime);
      }
   }
}
