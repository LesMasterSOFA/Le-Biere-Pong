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
   class ObjetCollision : ObjetDeBase
   {
      public BoundingSphere[] SphèreCollison { get; private set; }
      public ObjetCollision(Game jeu, string nomModèle, string nomTexture, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale)
         : base(jeu,nomModèle,nomTexture,échelleInitiale,rotationInitiale,positionInitiale)
      { }

      public override void Initialize()
      {
         base.Initialize();
         SphèreCollison = DonnerSphèreCollison();
      }
      BoundingSphere[] DonnerSphèreCollison()
      {
         SphèreCollison = new BoundingSphere[Modèle.Meshes.Count];
         for (int i = 0; i < Modèle.Meshes.Count; i++)
         {
            SphèreCollison[i] = Modèle.Meshes[i].BoundingSphere;
            SphèreCollison[i].Center += Position;
         }

         return SphèreCollison;
      }
   }
}
