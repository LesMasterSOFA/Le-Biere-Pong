using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
   class PlanColoré : Plan
   {
      VertexPositionColor[] Sommets { get; set; }
      Color Couleur { get; set; }

      public PlanColoré(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, Vector2 charpente, Color couleur, float intervalleMAJ)
         : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, étendue, charpente, intervalleMAJ)
      {
         Couleur = couleur;
      }

      protected override void InitialiserParamètresEffetDeBase()
      {
         EffetDeBase.VertexColorEnabled = true;
      }

      protected override void InitialiserSommets()
      {
         int NoSommet = 0;
         for (int cptY = 0; cptY < PtsSommets.GetLength(1)-1; cptY++)
         {
            for (int cptX = 0; cptX < PtsSommets.GetLength(0); cptX++)
            {
               Sommets[NoSommet++] = new VertexPositionColor(PtsSommets[cptX, cptY], Couleur);
               Sommets[NoSommet++] = new VertexPositionColor(PtsSommets[cptX, cptY + 1], Couleur);
            }
         }
      }
      protected override void AllouerTableauSommets()
      {
         Sommets = new VertexPositionColor[NbSommets];
      }
      protected override void DessinerTriangleStrip(int noStrip)
      {
         GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Sommets, noStrip, NbTrianglesParStrip);
      }

   }
}
