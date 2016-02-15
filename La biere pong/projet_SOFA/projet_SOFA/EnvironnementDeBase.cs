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
       const int DIMENSION_TERRAIN = 7;
       Vector2 �tenduePlanMur = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN-4);
       Vector2 �tenduePlanPlafond = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN);
       Vector2 charpentePlan = new Vector2(4, 3);
       PlanTextur� Gauche { get; set; }
       PlanTextur� Droite { get; set; }
       PlanTextur� Plafond { get; set; }
       PlanTextur� Plancher { get; set; }
       PlanTextur� Avant { get; set; }
       PlanTextur� Arri�re { get; set; }
       string NomGauche { get; set; }
       string NomDroite { get; set; }
       string NomPlanfond { get; set; }
       string NomPlancher { get; set; }
       string NomAvant { get; set; }
       string NomArri�re { get; set; }

       public EnvironnementDeBase(Game game, string nomGauche, string nomDroite, string nomDessus, string nomDessous, string nomAvant, string nomArri�re)
         : base(game)
      
       {
           NomGauche = nomGauche;
           NomDroite = nomDroite;
           NomPlanfond = nomDessus;
           NomPlancher = nomDessous;
           NomAvant = nomAvant;
           NomArri�re = nomArri�re;
       }

       public override void Initialize()
       {

           Gauche = new PlanTextur�(Game, 1f, new Vector3(0, MathHelper.PiOver2, 0), new Vector3((float)-DIMENSION_TERRAIN / 2, ((float)DIMENSION_TERRAIN - 4) / 2, 0), �tenduePlanMur, charpentePlan, "BriquesGrises_COLOR", INTERVALLE_MAJ_STANDARD);
           Droite = new PlanTextur�(Game, 1f, new Vector3(0, -MathHelper.PiOver2, 0), new Vector3((float)DIMENSION_TERRAIN / 2, ((float)DIMENSION_TERRAIN-4) / 2, 0), �tenduePlanMur, charpentePlan, "BriquesGrises_COLOR", INTERVALLE_MAJ_STANDARD);
           Plafond = new PlanTextur�(Game, 1f, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0, DIMENSION_TERRAIN-4, 0), �tenduePlanPlafond, charpentePlan, "BriquesGrises_COLOR", INTERVALLE_MAJ_STANDARD);
           Plancher = new PlanTextur�(Game, 1f, new Vector3(-MathHelper.PiOver2, 0, 0), new Vector3(0, 0, 0), �tenduePlanPlafond, charpentePlan, NomPlancher, INTERVALLE_MAJ_STANDARD);
           Avant = new PlanTextur�(Game, 1f, Vector3.Zero, new Vector3(0, (float)(DIMENSION_TERRAIN-4) / 2, (float)-DIMENSION_TERRAIN / 2), �tenduePlanMur, charpentePlan, "BriquesGrises_COLOR", INTERVALLE_MAJ_STANDARD);
           Arri�re = new PlanTextur�(Game, 1f, new Vector3(0, -MathHelper.Pi, 0), new Vector3(0, (float)(DIMENSION_TERRAIN-4) / 2, (float)DIMENSION_TERRAIN / 2), �tenduePlanMur, charpentePlan, "BriquesGrises_COLOR", INTERVALLE_MAJ_STANDARD);

           

           Game.Components.Add(Gauche);
           Game.Components.Add(Droite);
           Game.Components.Add(Plafond);
           Game.Components.Add(Plancher);
           Game.Components.Add(Avant);
           Game.Components.Add(Arri�re);

           base.Initialize();
       }
       public override void Update(GameTime gameTime)
       {
           base.Update(gameTime);
       }
   }
}
