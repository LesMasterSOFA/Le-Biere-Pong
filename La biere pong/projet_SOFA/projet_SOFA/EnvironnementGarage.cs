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

    class EnvironnementGarage : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        const int DIMENSION_TERRAIN = 7;
        Vector2 étenduePlanMur = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN - 4);
        Vector2 étenduePlanPlafond = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN);
        Vector2 charpentePlan = new Vector2(4, 3);
        string NomGauche { get; set; }
        string NomDroite { get; set; }
        string NomPlafond { get; set; }
        string NomPlancher { get; set; }
        string NomAvant { get; set; }
        string NomArrière { get; set; }
        PlanTexturé Gauche { get; set; }
        PlanTexturé Droite { get; set; }
        PlanTexturé Plafond { get; set; }
        PlanTexturé Plancher { get; set; }
        PlanTexturé Avant { get; set; }
        PlanTexturé Arrière { get; set; }
        ObjetDeBase Etabli { get; set; }
        ObjetDeBase Urinoir { get; set; }
        public EnvironnementGarage(Game game, string nomGauche, string nomDroite, string nomPlafond, string nomPlancher, string nomAvant, string nomArrière)
            : base(game)
       {
           NomGauche = nomGauche;
           NomDroite = nomDroite;
           NomPlafond = nomPlafond;
           NomPlancher = nomPlancher;
           NomAvant = nomAvant;
           NomArrière = nomArrière;

       }

       public override void Initialize()
       {
           
           Gauche = new PlanTexturé(Game, 1f, new Vector3(0, MathHelper.PiOver2, 0), new Vector3((float)-DIMENSION_TERRAIN / 2, ((float)DIMENSION_TERRAIN - 4) / 2, 0), étenduePlanMur, charpentePlan, NomGauche, INTERVALLE_MAJ_STANDARD);
           Droite = new PlanTexturé(Game, 1f, new Vector3(0, -MathHelper.PiOver2, 0), new Vector3((float)DIMENSION_TERRAIN / 2, ((float)DIMENSION_TERRAIN - 4) / 2, 0), étenduePlanMur, charpentePlan, NomDroite, INTERVALLE_MAJ_STANDARD);
           Plafond = new PlanTexturé(Game, 1f, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0, DIMENSION_TERRAIN - 4, 0), étenduePlanPlafond, charpentePlan, NomPlafond, INTERVALLE_MAJ_STANDARD);
           Plancher = new PlanTexturé(Game, 1f, new Vector3(-MathHelper.PiOver2, 0, 0), new Vector3(0, 0, 0), étenduePlanPlafond, charpentePlan, NomPlancher, INTERVALLE_MAJ_STANDARD);
           Avant = new PlanTexturé(Game, 1f, Vector3.Zero, new Vector3(0, (float)(DIMENSION_TERRAIN - 4) / 2, (float)-DIMENSION_TERRAIN / 2), étenduePlanMur, charpentePlan, NomAvant, INTERVALLE_MAJ_STANDARD);
           Arrière = new PlanTexturé(Game, 1f, new Vector3(0, -MathHelper.Pi, 0), new Vector3(0, (float)(DIMENSION_TERRAIN - 4) / 2, (float)DIMENSION_TERRAIN / 2), étenduePlanMur, charpentePlan, NomArrière, INTERVALLE_MAJ_STANDARD);
           Game.Components.Add(Gauche);
           Game.Components.Add(Droite);
           Game.Components.Add(Plafond);
           Game.Components.Add(Plancher);
           Game.Components.Add(Avant);
           Game.Components.Add(Arrière);
           InitialiserModèles();
           base.Initialize();
       }

       public override void Update(GameTime gameTime)
       {
           base.Update(gameTime);
       }
       void InitialiserModèles()
       {
           Etabli = new ObjetDeBase(Game, "etabli", "etabli", 1, new Vector3(0, MathHelper.Pi, 0), new Vector3(2.75f, -0.35f, 2.5f));
           Urinoir = new ObjetDeBase(Game, "urinoir", "urinoir", 1, new Vector3(0, MathHelper.PiOver2, -MathHelper.PiOver2), new Vector3(-3.5f, 0.5f, 0));
           Game.Components.Add(Etabli);
           Game.Components.Add(Urinoir);
           
       }
    }
}
