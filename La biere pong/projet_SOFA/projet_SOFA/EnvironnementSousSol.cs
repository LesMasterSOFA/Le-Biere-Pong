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

   class EnvironnementSousSol : EnrivonnementDeBase
   {
      ObjetDeBase Table { get; set; }

      public EnvironnementSousSol(Game game, GestionEnvironnement gestionEnv, string nomGauche, string nomDroite, string nomPlafond, string nomPlancher,
                                string nomAvant, string nomArrière, string personnageJoueurPrincipalModel, string personnageJoueurPrincipalTexture,
                                string personnageJoueurSecondaireModel, string personnageJoueurSecondaireTexture, Vector3 dimensionTable, float distanceVerre,TypePartie typeDePartie)
         : base(game, gestionEnv, nomGauche, nomDroite, nomPlafond, nomPlancher, nomAvant, nomArrière, personnageJoueurPrincipalModel, personnageJoueurPrincipalTexture,
                personnageJoueurSecondaireModel, personnageJoueurSecondaireTexture, dimensionTable, distanceVerre, typeDePartie)
      { }
      public override void Initialize()
      {
         InitialiserModèles();
         base.Initialize();
      }
      public override void InitialiserModèles()
      {
         Table = new ObjetDeBase(Game, "table_plastique", "table_plastique", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
         Game.Components.Add(Table);
      }
   }
}
