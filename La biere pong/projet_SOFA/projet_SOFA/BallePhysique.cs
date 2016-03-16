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
      const float CONSTANTE_RESTITUTION_TABLE = 0.9f;
      const float CONSTANTE_RESTITUTION_VERS = 0.5f;
      const float GRAVITÉ = 9.81f;
      const float RAYON = 0.02f;
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

      public BallePhysique(Game jeu, string nomModèle, string nomTexture,string nomEffet, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale,
           float vitesseInitiale, float angleHorizontal, float angleVertical, BoundingBox boundingTable, List<Vector3> listePositionVerresAdv, float rayonVerre, float hauteurVerre, float intervalleMAJ)
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
         IntervalleMAJ = intervalleMAJ;
      }

      public override void Initialize()
      {
         CréerListeSphère();
         VitesseEnX = VitesseInitiale * (float)Math.Cos(AngleVertical) * (float)Math.Sin(AngleHorizontal);
         VitesseEnY = VitesseInitiale * (float)Math.Sin(AngleVertical);
         VitesseEnZ = VitesseInitiale * (float)Math.Cos(AngleVertical) * (float)Math.Cos(AngleHorizontal);
         SphèreBalle = new BoundingSphere(Position, RAYON);
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
            ListeSphèreCollision.Add(new BoundingSphere(ListePositionVerresAdv[i] + new Vector3(0, HauteurVerre, 0), RayonVerre));
         }
      }
      void EffectuerDéplacement()
      {
         for (int i = 0; i < ListeSphèreCollision.Count; ++i)
         {
            if (SphèreBalle.Intersects(ListeSphèreCollision[i]))
            {
               Position = Position;
               i = ListeSphèreCollision.Count;
            }
         }
         if (SphèreBalle.Intersects(BoundingTable))
         {
            PositionInitiale = Position;
            VitesseEnY = CONSTANTE_RESTITUTION_TABLE * Math.Abs(VitesseEnY - GRAVITÉ * TempsTotal);
            TempsTotal = 0.01f;
         }
         Position = new Vector3(PositionInitiale.X + VitesseEnX * TempsTotal,
                                   PositionInitiale.Y + VitesseEnY * TempsTotal - GRAVITÉ * TempsTotal * TempsTotal / 2,
                                   PositionInitiale.Z - VitesseEnZ * TempsTotal);
         SphèreBalle = new BoundingSphere(Position, 0.02f);
         
      }

      public override Matrix GetMonde()
      {
          Monde = Matrix.Identity;
          Monde *= Matrix.CreateScale(Échelle);
          Monde *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
          Monde *= Matrix.CreateTranslation(Position);
          return base.GetMonde();
      }
   }
}
