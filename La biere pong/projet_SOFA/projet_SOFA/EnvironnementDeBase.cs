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
   public class EnvironnementDeBase : Microsoft.Xna.Framework.GameComponent
   {
      float intervalleMAJ;
      Vector2 charpente;
      Vector2 étendue;
      float homothétieInitiale;

      public EnvironnementDeBase(Game game, Vector3 rotationInitiale, Vector3 positionInitiale, string nomTexture)
         : base(game)
      {

      }

      public override void Initialize()
      {
         base.Initialize();
      }

      public override void Update(GameTime gameTime)
      {
         base.Update(gameTime);
      }
   }
}
