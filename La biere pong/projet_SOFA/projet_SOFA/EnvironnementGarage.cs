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

   class EnvironnementGarage : EnrivonnementDeBase
   {
      ObjetDeBase Etabli { get; set; }
      ObjetDeBase Urinoir { get; set; }
      ObjetDeBase Table { get; set; }

      public EnvironnementGarage(Game game, GestionEnvironnement gestionEnv, string nomGauche, string nomDroite, string nomPlafond, string nomPlancher,
                                string nomAvant, string nomArrière, string personnageJoueurPrincipalModel, string personnageJoueurPrincipalTexture,
                                string personnageJoueurSecondaireModel, string personnageJoueurSecondaireTexture, Vector3 dimensionTable, float distanceVerre)
         : base(game, gestionEnv, nomGauche, nomDroite, nomPlafond, nomPlancher, nomAvant, nomArrière, personnageJoueurPrincipalModel, personnageJoueurPrincipalTexture,
                personnageJoueurSecondaireModel, personnageJoueurSecondaireTexture, dimensionTable, distanceVerre)
      { }

      public override void Initialize()
      {
         InitialiserModèles();
         base.Initialize();
      }

      void InitialiserModèles()
      {
         Table = new ObjetDeBase(Game, "table_plastique", "table_plastique", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
         Etabli = new ObjetDeBase(Game, "etabli", "etabli", "Shader", 1, new Vector3(0, MathHelper.Pi, 0), new Vector3(2.75f, -0.35f, 2.5f));
         Urinoir = new ObjetDeBase(Game, "urinoir", "urinoir", "Shader", 1, new Vector3(0, MathHelper.PiOver2, -MathHelper.PiOver2), new Vector3(-3.5f, 0.5f, 0));
         Game.Components.Add(Table);
         Game.Components.Add(Etabli);
         Game.Components.Add(Urinoir);
      }
   }
}
