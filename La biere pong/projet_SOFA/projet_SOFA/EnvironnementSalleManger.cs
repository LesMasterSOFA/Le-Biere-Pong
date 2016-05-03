using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{

   class EnvironnementSalleManger : EnrivonnementDeBase
   {
      ObjetDeBase Urinoir { get; set; }
      List<ObjetDeBase> ListeChaise { get; set; }
      ObjetDeBase Table { get; set; }

      public EnvironnementSalleManger(Game game, GestionEnvironnement gestionEnv, string personnageJoueurPrincipalModel, string personnageJoueurPrincipalTexture,
                                string personnageJoueurSecondaireModel, string personnageJoueurSecondaireTexture, TypePartie typeDePartie)
         : base(game, gestionEnv, personnageJoueurPrincipalModel, personnageJoueurPrincipalTexture,
                personnageJoueurSecondaireModel, personnageJoueurSecondaireTexture, typeDePartie)
      { }

      public override void Initialize()
      {
         InitialiserModèles();
         InitialiserMurs(new string[6]{"GaucheSallePetiteFoyer", "DroiteSallePlinthe", "PlafondSalle", "PlancherSalle", "AvantSallePlinthe", "ArriereSallePlinthe"});
         DimensionTable = new Vector3(0.9f, 0.847f, 2.2f);
         DistanceVerre = 1f;
         base.Initialize();
      }

      protected override void InitialiserModèles()
      {
         Table = new ObjetDeBase(Game, "tablesallemanger", "tex_table_salle", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
         ListeChaise = new List<ObjetDeBase>();
         ListeChaise.Add(new ObjetDeBase(Game, "Chaise", "UVChaiseFait", "Shader", 1, new Vector3(0, 0, 0), new Vector3(3.1f, 0, 1)));
         ListeChaise.Add(new ObjetDeBase(Game, "Chaise", "UVChaiseFait", "Shader", 1, new Vector3(0, 0, 0), new Vector3(3.1f, 0, -1)));
         ListeChaise.Add(new ObjetDeBase(Game, "Chaise", "UVChaiseFait", "Shader", 1, new Vector3(0, 0, 0), new Vector3(3.1f, 0, 0)));
         ListeChaise.Add(new ObjetDeBase(Game, "Chaise", "UVChaiseFait", "Shader", 1, new Vector3(0, MathHelper.Pi, 0), new Vector3(-3.1f, 0, 2)));
         ListeChaise.Add(new ObjetDeBase(Game, "Chaise", "UVChaiseFait", "Shader", 1, new Vector3(0, MathHelper.Pi, 0), new Vector3(-3.1f, 0, -2)));
         ListeChaise.Add(new ObjetDeBase(Game, "Chaise", "UVChaiseFait", "Shader", 1, new Vector3(0, MathHelper.Pi, 0), new Vector3(-3.1f, 0, -3)));

         Game.Components.Add(Table);
         foreach(ObjetDeBase c in ListeChaise)
         {
             Game.Components.Add(c);
         }
      }
   }
}