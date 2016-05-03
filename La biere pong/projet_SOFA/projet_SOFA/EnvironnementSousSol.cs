using Microsoft.Xna.Framework;

namespace AtelierXNA
{

   class EnvironnementSousSol : EnrivonnementDeBase
   {
      ObjetDeBase Table { get; set; }

      public EnvironnementSousSol(Game game, GestionEnvironnement gestionEnv, string personnageJoueurPrincipalModel, string personnageJoueurPrincipalTexture,
                                string personnageJoueurSecondaireModel, string personnageJoueurSecondaireTexture,TypePartie typeDePartie)
         : base(game, gestionEnv, personnageJoueurPrincipalModel, personnageJoueurPrincipalTexture,
                personnageJoueurSecondaireModel, personnageJoueurSecondaireTexture, typeDePartie)
      { }

      public override void Initialize()
      {
         InitialiserModèles();
         InitialiserMurs(new string[6] {"GaucheSousSol", "DroiteSousSol", "PlafondSousSol", "PlancherSousSol", "AvantSousSol", "ArriereSousSol"});
         DimensionTable = new Vector3(0.76f, 0.74f, 1.8f);
         DistanceVerre = 0.8f;
         base.Initialize();
      }

      protected override void InitialiserModèles()
      {
         Table = new ObjetDeBase(Game, "table_plastique", "table_plastique", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
         Game.Components.Add(Table);
      }
   }
}
