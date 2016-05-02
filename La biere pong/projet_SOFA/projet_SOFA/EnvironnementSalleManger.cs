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

   class EnvironnementSalleManger : EnrivonnementDeBase
   {
      ObjetDeBase Chaise1 { get; set; }
      ObjetDeBase Chaise2 { get; set; }
      ObjetDeBase Chaise3 { get; set; }
      ObjetDeBase Chaise4 { get; set; }
      ObjetDeBase Chaise5 { get; set; }
      ObjetDeBase Chaise6 { get; set; }
      ObjetDeBase Urinoir { get; set; }
      ObjetDeBase Table { get; set; }
      public EnvironnementSalleManger(Game game, GestionEnvironnement gestionEnv, string nomGauche, string nomDroite, string nomPlafond, string nomPlancher,
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
      public override void InitialiserModèles()
      {
         Table = new ObjetDeBase(Game, "tablesallemanger", "tex_table_salle", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
         Chaise1 = new ObjetDeBase(Game, "Chaise", "UVChaiseFait", "Shader", 1, new Vector3(0, 0, 0), new Vector3(3.1f, 0, 1));
         Chaise2 = new ObjetDeBase(Game, "Chaise", "UVChaiseFait", "Shader", 1, new Vector3(0, 0, 0), new Vector3(3.1f, 0, -1));
         Chaise3 = new ObjetDeBase(Game, "Chaise", "UVChaiseFait", "Shader", 1, new Vector3(0, 0, 0), new Vector3(3.1f, 0, 0));
         Chaise4 = new ObjetDeBase(Game, "Chaise", "UVChaiseFait", "Shader", 1, new Vector3(0, MathHelper.Pi, 0), new Vector3(-3.1f, 0, 2));
         Chaise5 = new ObjetDeBase(Game, "Chaise", "UVChaiseFait", "Shader", 1, new Vector3(0, MathHelper.Pi, 0), new Vector3(-3.1f, 0, -2));
         Chaise6 = new ObjetDeBase(Game, "Chaise", "UVChaiseFait", "Shader", 1, new Vector3(0, MathHelper.Pi, 0), new Vector3(-3.1f, 0, -3));
         Game.Components.Add(Table);
         Game.Components.Add(Chaise1);
         Game.Components.Add(Chaise2);
         Game.Components.Add(Chaise3);
         Game.Components.Add(Chaise4);
         Game.Components.Add(Chaise5);
         Game.Components.Add(Chaise6);

      }
   }
}