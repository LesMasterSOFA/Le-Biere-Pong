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
        ObjetDeBase PersonnagePrincipal { get; set; }
        
        public Personnage(Game game)
            : base(game)
        {
        }
        public override void Initialize()
        {
            //PersonnagePrincipal = new ObjetDeBase(Game, "batman hero110", "CHRNPCICOHER110_DIFFUSE", 20f, new Vector3(0, -MathHelper.PiOver2, 0), new Vector3(0, 0, -100));
            PersonnagePrincipal = new ObjetDeBase(Game, "untitled", "Tex_0173_0", 0.15f, new Vector3(0, 0, -MathHelper.PiOver2), new Vector3(0, 0, -100));
            Game.Components.Add(PersonnagePrincipal);
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
