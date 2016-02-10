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
    class EnvironnementGarage : EnvironnementDeBase
    {
        public EnvironnementGarage(Game game, string nomGauche, string nomDroite, string nomDessus, string nomDessous, string nomAvant, string nomArrière)
         : base(game, nomGauche, nomDroite, nomDessus, nomDessous, nomAvant, nomArrière)
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
