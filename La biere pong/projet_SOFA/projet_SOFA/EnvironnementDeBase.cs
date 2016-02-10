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
       const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
       const int DIMENSION_TERRAIN = 300;
       Vector2 étenduePlan = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN);
       Vector2 charpentePlan = new Vector2(4, 3);
       PlanTexturé Gauche { get; set; }
       PlanTexturé Droite { get; set; }
       PlanTexturé Plafond { get; set; }
       PlanTexturé Plancher { get; set; }
       PlanTexturé Avant { get; set; }
       PlanTexturé Arrière { get; set; }
       string NomGauche { get; set; }
       string NomDroite { get; set; }
       string NomPlanfond { get; set; }
       string NomPlancher { get; set; }
       string NomAvant { get; set; }
       string NomArrière { get; set; }

       public EnvironnementDeBase(Game game, string nomGauche, string nomDroite, string nomDessus, string nomDessous, string nomAvant, string nomArrière)
         : base(game)
      
       {
           NomGauche = nomGauche;
           NomDroite = nomDroite;
           NomPlanfond = nomDessus;
           NomPlancher = nomDessous;
           NomAvant = nomAvant;
           NomArrière = nomArrière;
       }

       public override void Initialize()
       {
           Gauche = new PlanTexturé(Game, 1f, new Vector3(0, MathHelper.PiOver2, 0), new Vector3(-DIMENSION_TERRAIN / 2, DIMENSION_TERRAIN / 2, 0), étenduePlan, charpentePlan, NomGauche, INTERVALLE_MAJ_STANDARD);
           Droite = new PlanTexturé(Game, 1f, new Vector3(0, -MathHelper.PiOver2, 0), new Vector3(DIMENSION_TERRAIN / 2, DIMENSION_TERRAIN / 2, 0), étenduePlan, charpentePlan, NomDroite, INTERVALLE_MAJ_STANDARD);
           Plafond = new PlanTexturé(Game, 1f, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0, DIMENSION_TERRAIN, 0), étenduePlan, charpentePlan, NomPlanfond, INTERVALLE_MAJ_STANDARD);
           Plancher = new PlanTexturé(Game, 1f, new Vector3(-MathHelper.PiOver2, 0, 0), new Vector3(0, 0, 0), étenduePlan, charpentePlan, NomPlancher, INTERVALLE_MAJ_STANDARD);
           Avant = new PlanTexturé(Game, 1f, Vector3.Zero, new Vector3(0, DIMENSION_TERRAIN / 2, -DIMENSION_TERRAIN / 2), étenduePlan, charpentePlan, NomAvant, INTERVALLE_MAJ_STANDARD);
           Arrière = new PlanTexturé(Game, 1f, new Vector3(0, -MathHelper.Pi, 0), new Vector3(0, DIMENSION_TERRAIN / 2, DIMENSION_TERRAIN / 2), étenduePlan, charpentePlan, NomArrière, INTERVALLE_MAJ_STANDARD);

           Game.Components.Add(Gauche);
           Game.Components.Add(Droite);
           Game.Components.Add(Plafond);
           Game.Components.Add(Plancher);
           Game.Components.Add(Avant);
           Game.Components.Add(Arrière);

           base.Initialize();
       }

       public override void Update(GameTime gameTime)
       {
           base.Update(gameTime);
       }
   }
}
