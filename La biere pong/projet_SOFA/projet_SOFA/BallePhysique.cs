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
      const float CONSTANTE_RESTITUTION_TABLE = 0.8f;
      const float CONSTANTE_RESTITUTION_VERS = 0.5f;
      const float COEFFICIENT_FROTTEMENT = 0.75f;
      const float CHANGEMENT_DIRECTION = -1f;
      const float GRAVITÉ = 9.81f;
      const float RAYON_BALLE = 0.02f;
      BoundingSphere SphèreBalle { get; set; }
      float VitesseInitiale { get; set; }
      float VitesseEnX { get; set; }
      float VitesseEnY { get; set; }
      float VitesseEnZ { get; set; }
      float IntervalleMAJ { get; set; }
      float TempsÉcouléDepuisMAJ { get; set; }
      float TempsTotal { get; set; }
      Vector3 PositionInitiale { get; set; }
      Vector3[] PositionSommet { get; set; }
      Vector3 Déplacement { get; set; }
      float AngleHorizontal { get; set; }
      float AngleVertical { get; set; }
      BoundingBox BoundingTable { get; set; }
      List<Vector3> ListePositionVerresAdv { get; set; }
      List<BoundingSphere> ListeSphèreCollision { get; set; }
      float RayonVerre { get; set; }
      float HauteurVerre { get; set; }
      float HauteurTable { get; set; }
      float RayonCollision { get; set; }

      public BallePhysique(Game jeu, string nomModèle, string nomTexture,string nomEffet, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale,
           float vitesseInitiale, float angleHorizontal, float angleVertical, BoundingBox boundingTable, List<Vector3> listePositionVerresAdv, float rayonVerre, float hauteurVerre, float hauteurTable, float intervalleMAJ)
         : base(jeu,nomModèle,nomTexture,nomEffet,échelleInitiale,rotationInitiale,positionInitiale)
      {
         PositionInitiale = positionInitiale;
         Position = positionInitiale;
         VitesseInitiale = vitesseInitiale;
         AngleHorizontal = angleHorizontal;
         AngleVertical = angleVertical;
         BoundingTable = boundingTable;
         ListePositionVerresAdv = listePositionVerresAdv;
         RayonVerre = rayonVerre;
         HauteurVerre = hauteurVerre;
         HauteurTable = hauteurTable;
         IntervalleMAJ = intervalleMAJ;
      }

      public override void Initialize()
      {
         RayonCollision = (float)Math.Sqrt(Math.Pow(HauteurVerre / 2, 2) + Math.Pow(RayonVerre, 2));
         CréerListeSphère();
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
      void CréerListeSphère()
      {
         ListeSphèreCollision = new List<BoundingSphere>();
         for (int i = 0; i < ListePositionVerresAdv.Count; ++i)
         {
            ListeSphèreCollision.Add(new BoundingSphere(ListePositionVerresAdv[i] + new Vector3(0, HauteurVerre / 2, 0), RayonCollision));
         }
         //for (int i = 0; i < ListePositionVerresAdv.Count; ++i)
         //{
         //   ListeSphèreCollision.Add(new BoundingBox(new Vector3(ListePositionVerresAdv[i].X - RayonVerre, HauteurTable, ListePositionVerresAdv[i].Z - RayonVerre),
         //                                            new Vector3(ListePositionVerresAdv[i].X + RayonVerre, HauteurTable + HauteurVerre, ListePositionVerresAdv[i].Z + RayonVerre)));
         //}
      }
      void EffectuerDéplacement()
      {
         for (int i = 0; i < ListeSphèreCollision.Count; ++i)
         {
            if (SphèreBalle.Intersects(ListeSphèreCollision[i]))
            {
               Vector3 vecteurDirecteur = Position - ListeSphèreCollision[i].Center;
               vecteurDirecteur = (RayonCollision / vecteurDirecteur.Length()) * vecteurDirecteur;
               float angleDirection = (float)Math.Acos(Vector3.Dot(vecteurDirecteur, Vector3.Backward) / (vecteurDirecteur.Length() * Vector3.Backward.Length()));
               if (angleDirection <= MathHelper.PiOver4)
               {
                  VitesseEnX = CHANGEMENT_DIRECTION * CONSTANTE_RESTITUTION_VERS * VitesseEnX;
                  VitesseEnZ = CHANGEMENT_DIRECTION * CONSTANTE_RESTITUTION_VERS * VitesseEnZ;
               }
               else
               {
                  VitesseEnY = CONSTANTE_RESTITUTION_VERS * Math.Abs(VitesseEnY - GRAVITÉ * TempsTotal);
                  VitesseEnX = CONSTANTE_RESTITUTION_VERS * VitesseEnX;
                  VitesseEnZ = CONSTANTE_RESTITUTION_VERS * VitesseEnZ;
               }
               //PositionInitiale = new Vector3(Position.X, Position.Y, Position.Z);
               PositionInitiale = ListeSphèreCollision[i].Center + vecteurDirecteur;
               TempsTotal = IntervalleMAJ;
               i = ListeSphèreCollision.Count;
            }
         }
         if (SphèreBalle.Intersects(BoundingTable))
         {
            PositionInitiale = new Vector3(Position.X, HauteurTable + RAYON_BALLE, Position.Z);
            VitesseEnY = CONSTANTE_RESTITUTION_TABLE * Math.Abs(VitesseEnY - GRAVITÉ * TempsTotal);
            VitesseEnX = COEFFICIENT_FROTTEMENT * VitesseEnX;
            VitesseEnZ = COEFFICIENT_FROTTEMENT * VitesseEnZ;
            TempsTotal = IntervalleMAJ;
         }
         Position = new Vector3(PositionInitiale.X + VitesseEnX * TempsTotal,
                                PositionInitiale.Y + VitesseEnY * TempsTotal - GRAVITÉ * TempsTotal * TempsTotal / 2,
                                PositionInitiale.Z - VitesseEnZ * TempsTotal);
         SphèreBalle = new BoundingSphere(Position, RAYON_BALLE);
         
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
