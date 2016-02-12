using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
   public abstract class Plan : PrimitiveDeBaseAnimée
   {
      protected Vector3 Origine { get; private set; }  
      Vector2 Delta { get; set; } 
      protected Vector3[,] PtsSommets { get; private set; } 
      protected int NbColonnes { get; private set; }
      protected int NbRangées { get; private set; } 
      protected int NbTrianglesParStrip { get; private set; }
      protected BasicEffect EffetDeBase { get; private set; }
      Caméra CaméraJeu { get; set; }

      public Plan(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Vector2 étendue, Vector2 charpente, float intervalleMAJ)
         : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
      {
         NbColonnes = (int)charpente.X;
         NbRangées = (int)charpente.Y;
         Delta = new Vector2(étendue.X / NbColonnes, étendue.Y / NbRangées);
         Origine = new Vector3(-étendue.X / 2, -étendue.Y / 2, 0);
      }

      public override void Initialize()
      {
         CaméraJeu = Game.Services.GetService(typeof(Caméra)) as Caméra;
         NbTrianglesParStrip = 2 * NbColonnes;
         NbSommets = (NbTrianglesParStrip + 2) * NbRangées;
         PtsSommets = new Vector3[NbColonnes+1, NbRangées+1];
         CréerTableauSommets();
         CréerTableauPoints();
         base.Initialize();
      }

      protected abstract void CréerTableauSommets();

      protected override void LoadContent()
      {
         EffetDeBase = new BasicEffect(GraphicsDevice);
         InitialiserParamètresEffetDeBase();
         base.LoadContent();
      }

      protected abstract void InitialiserParamètresEffetDeBase();

      private void CréerTableauPoints()
      {
         for (int colonne = 0; colonne <= NbColonnes; colonne++)
         {
            for (int rangée = 0; rangée <= NbRangées; rangée++)
            {
               PtsSommets[colonne, rangée] = new Vector3(colonne * Delta.X + Origine.X, rangée * Delta.Y + Origine.Y, Origine.Z);
            }
         }
      }

      public override void Draw(GameTime gameTime)
      {
         EffetDeBase.World = GetMonde();
         EffetDeBase.View = CaméraJeu.Vue;
         EffetDeBase.Projection = CaméraJeu.Projection;
         foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
         {
            passeEffet.Apply();
            for (int i = 0; i < NbRangées; i++)
            {
                DessinerTriangleStrip(i);
            }
         }
         base.Draw(gameTime);
      }

      protected abstract void DessinerTriangleStrip(int noStrip);
   }
}
