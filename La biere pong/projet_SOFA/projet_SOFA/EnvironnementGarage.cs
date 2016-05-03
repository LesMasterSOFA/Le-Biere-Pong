using Microsoft.Xna.Framework;

namespace AtelierXNA
{

   class EnvironnementGarage : EnrivonnementDeBase
   {
      ObjetDeBase Etabli { get; set; }
      ObjetDeBase Urinoir { get; set; }
      ObjetDeBase Table { get; set; }

      public EnvironnementGarage(Game game, GestionEnvironnement gestionEnv, string personnageJoueurPrincipalModel, string personnageJoueurPrincipalTexture,
                                string personnageJoueurSecondaireModel, string personnageJoueurSecondaireTexture, TypePartie typeDePartie)
         : base(game, gestionEnv, personnageJoueurPrincipalModel, personnageJoueurPrincipalTexture,
                personnageJoueurSecondaireModel, personnageJoueurSecondaireTexture, typeDePartie)
      { }

      public override void Initialize()
      {
         InitialiserModèles();
         InitialiserMurs(new string[6] {"GaucheGarage","DroiteGarage","PlafondGarage","PlancherGarage","AvantGarage","ArriereGarage"});
         DimensionTable = new Vector3(0.76f, 0.74f, 1.8f);
         DistanceVerre = 0.8f;
         base.Initialize();
      }

      protected override void InitialiserModèles()
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
