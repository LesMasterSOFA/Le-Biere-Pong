﻿using System;
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
        Vector3 PositionPersonnage { get; set; }
        
        public Personnage(Game game, Vector3 positionPersonnage)
            : base(game)
        {
            PositionPersonnage = positionPersonnage;
        }
        public override void Initialize()
        {
            PersonnagePrincipal = new ObjetDeBase(Game, "superBoy", "superBoyTex","Shader", 1, new Vector3(0, 0,-MathHelper.PiOver2), PositionPersonnage);
            Game.Components.Add(PersonnagePrincipal);
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
