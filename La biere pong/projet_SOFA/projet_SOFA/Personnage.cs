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
    public class Personnage : Microsoft.Xna.Framework.DrawableGameComponent
    {
        ObjetDeBase PersonnageAllo { get; set; }
        public Personnage(Game game)
            : base(game)
        {
        }
        public override void Initialize()
        {
            PersonnageAllo = new ObjetDeBase(Game, "allo", 1f, new Vector3(0, 0,-MathHelper.PiOver2), Vector3.Zero);
            Game.Components.Add(PersonnageAllo);
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
