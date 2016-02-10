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
    public class GestionEnvironnement : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        const int DIMENSION_TERRAIN = 500;
        Vector2 étenduePlan = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN);
        Vector2 charpentePlan = new Vector2(4, 3);
        PlanTexturé Gauche { get; set; }
        PlanTexturé Droite { get; set; }
        PlanTexturé Dessus { get; set; }
        PlanTexturé Dessous { get; set; }
        PlanTexturé Avant { get; set; }
        PlanTexturé Arrière { get; set; }
        ObjetDeBase Table { get; set; }
        ObjetDeBase Balle { get; set; }

        public GestionEnvironnement(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            Table = new ObjetDeBase(Game, "tablebois2", "tablebois", 0.25f, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            Balle = new ObjetDeBase(Game, "balle", "blanc", 0.1f, new Vector3(0, 0, 0), new Vector3(0, 100, 0));
            Game.Components.Add(Table);
            Game.Components.Add(Balle);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Gauche = new PlanTexturé(Game, 1f, new Vector3(0, MathHelper.PiOver2, 0), new Vector3(-DIMENSION_TERRAIN / 2, DIMENSION_TERRAIN / 2, 0), étenduePlan, charpentePlan, "BeerPong", INTERVALLE_MAJ_STANDARD);
            Droite = new PlanTexturé(Game, 1f, new Vector3(0, -MathHelper.PiOver2, 0), new Vector3(DIMENSION_TERRAIN / 2, DIMENSION_TERRAIN / 2, 0), étenduePlan, charpentePlan, "BeerPong", INTERVALLE_MAJ_STANDARD);
            Avant = new PlanTexturé(Game, 1f, Vector3.Zero, new Vector3(0, DIMENSION_TERRAIN / 2, -DIMENSION_TERRAIN / 2), étenduePlan, charpentePlan, "BeerPong", INTERVALLE_MAJ_STANDARD);
            Arrière = new PlanTexturé(Game, 1f, new Vector3(0, -MathHelper.Pi, 0), new Vector3(0, DIMENSION_TERRAIN / 2, DIMENSION_TERRAIN / 2), étenduePlan, charpentePlan, "BeerPong", INTERVALLE_MAJ_STANDARD);
            Dessus = new PlanTexturé(Game, 1f, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0, DIMENSION_TERRAIN, 0), étenduePlan, charpentePlan, "BeerPong", INTERVALLE_MAJ_STANDARD);
            Dessous = new PlanTexturé(Game, 1f, new Vector3(-MathHelper.PiOver2, 0, 0), new Vector3(0, 0, 0), étenduePlan, charpentePlan, "BeerPong", INTERVALLE_MAJ_STANDARD);
            
            Game.Components.Add(Gauche);
            Game.Components.Add(Droite);
            Game.Components.Add(Avant);
            Game.Components.Add(Arrière);
            Game.Components.Add(Dessus);
            Game.Components.Add(Dessous);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
