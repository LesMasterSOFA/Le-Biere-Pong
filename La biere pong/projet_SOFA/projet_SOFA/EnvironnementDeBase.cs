using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{

   abstract class EnvironnementDeBase : Microsoft.Xna.Framework.DrawableGameComponent
   {
       #region Constantes et propriétés
      //Constantes
      const int DIMENSION_TERRAIN = 7;
      const float DIAMÈTRE_VERRE = 0.09225f;
      const float RAYON_VERRE = DIAMÈTRE_VERRE / 2;
      const float HAUTEUR_VERRE = 0.1199f;
      Vector2 étenduePlanMur = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN - 4);
      Vector2 étenduePlanPlafond = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN);
      Vector2 charpentePlan = new Vector2(4, 3);
      Vector3 DIMENSION_BONHOMMME = new Vector3(0.50f, 1.705f, 0.367f);
      const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
      //Liste de propriétés
      InputManager GestionClavier { get; set; }
      TypePartie TypeDePartie { get; set; }
      protected Vector3 DimensionTable { get; set; }
      protected float DistanceVerre { get; set; }
      //String représentant les noms des murs (Changeant selon l'environnement) et Plans texturés associés
      protected string NomGauche { get; set; }
      protected string NomDroite { get; set; }
      protected string NomPlafond { get; set; }
      protected string NomPlancher { get; set; }
      protected string NomAvant { get; set; }
      protected string NomArrière { get; set; }
      PlanTexturé Gauche { get; set; }
      PlanTexturé Droite { get; set; }
      PlanTexturé Plafond { get; set; }
      PlanTexturé Plancher { get; set; }
      PlanTexturé Avant { get; set; }
      PlanTexturé Arrière { get; set; }
      //Propriétés pour les listes de verres et leurs positions
      List<ObjetDeBase> ListeBiereJoueur { get; set; }
      List<ObjetDeBase> ListeBiereAdv { get; set; }
      public List<VerreJoueurPrincipal> VerresJoueur { get; set; }
      public List<VerreAdversaire> VerresAdversaire { get; set; }
      List<Vector3> ListePositionVerres { get; set; }
      List<Vector3> ListePositionVerresAdv { get; set; }
      BoundingBox BoundingTable { get; set; }
      //Pour personnages
      Personnage PersonnagePrincipal { get; set; }
      Personnage PersonnageSecondaire { get; set; }
      BoundingBox BoundingBonhommePrincipal { get; set; }
      BoundingBox BoundingBonhommeSecondaire { get; set; }
      Vector3 RotationInitialePersonnagePrincipal { get; set; }
      Vector3 PositionInitialePersonnagePrincipal { get; set; }
      Vector3 RotationInitialePersonnageSecondaire { get; set; }
      Vector3 PositionInitialePersonnageSecondaire { get; set; }
      public string PersonnageJoueurPrincipalModel { get; private set; }
      public string PersonnageJoueurPrincipalTexture { get; private set; }
      public string PersonnageJoueurSecondaireModel { get; private set; }
      public string PersonnageJoueurSecondaireTexture { get; private set; }
      
       #endregion

      public EnvironnementDeBase(Game game, GestionEnvironnement gestionEnv, string personnageJoueurPrincipalModel, string personnageJoueurPrincipalTexture,
                                string personnageJoueurSecondaireModel, string personnageJoueurSecondaireTexture, TypePartie typePartie)
         : base(game)
      {
         PersonnageJoueurPrincipalModel = personnageJoueurPrincipalModel;
         PersonnageJoueurPrincipalTexture = personnageJoueurPrincipalTexture;
         PersonnageJoueurSecondaireModel = personnageJoueurSecondaireModel;
         PersonnageJoueurSecondaireTexture = personnageJoueurSecondaireTexture;
         TypeDePartie = typePartie;
      }

      #region Initialisation d'environnement

      public override void Initialize()
      {
         GestionClavier = Game.Services.GetService(typeof(InputManager)) as InputManager;
         //Initialisation personnages
         RotationInitialePersonnagePrincipal = new Vector3(-MathHelper.PiOver2, 0, 0);
         PositionInitialePersonnagePrincipal = new Vector3(0.182f, 0, -DimensionTable.Z / 2 - 0.1f - 0.546f);
         RotationInitialePersonnageSecondaire = new Vector3(-MathHelper.PiOver2, MathHelper.Pi, 0);
         PositionInitialePersonnageSecondaire = new Vector3(-0.182f, 0, DimensionTable.Z / 2 + 0.1f + 0.546f);
         //Initialise les plans texturés avec les textures selon l'environnement
         InitialiserPlansTexturés();
         ListePositionVerres = new List<Vector3>();
         ListePositionVerresAdv = new List<Vector3>();
         FixerLesPositions();
         AjouterBiere();
         CréerLesVerres();

         //Bounding box pour les collisions
         BoundingTable = new BoundingBox(new Vector3(-DimensionTable.X / 2, DimensionTable.Y - 0.1f, -DimensionTable.Z / 2), new Vector3(DimensionTable.X / 2, DimensionTable.Y, DimensionTable.Z / 2));
         BoundingBonhommePrincipal = new BoundingBox(new Vector3(PositionInitialePersonnagePrincipal.X - DIMENSION_BONHOMMME.X / 2, 0, PositionInitialePersonnagePrincipal.Z - DIMENSION_BONHOMMME.Z), new Vector3(PositionInitialePersonnagePrincipal.X + DIMENSION_BONHOMMME.X / 2, DIMENSION_BONHOMMME.Y, PositionInitialePersonnagePrincipal.Z));
         BoundingBonhommeSecondaire = new BoundingBox(new Vector3(PositionInitialePersonnageSecondaire.X - DIMENSION_BONHOMMME.X / 2, 0, PositionInitialePersonnageSecondaire.Z - DIMENSION_BONHOMMME.Z), new Vector3(PositionInitialePersonnageSecondaire.X + DIMENSION_BONHOMMME.X / 2, DIMENSION_BONHOMMME.Y, PositionInitialePersonnageSecondaire.Z));
         //Ajout de la gestion d'événements
         Game.Components.Add(new GestionÉvénements(Game, ListePositionVerres, ListePositionVerresAdv, ListeBiereJoueur, ListeBiereAdv, VerresJoueur, VerresAdversaire, DimensionTable,BoundingTable,BoundingBonhommePrincipal,BoundingBonhommeSecondaire));
         //Pour personnages
         PersonnagePrincipal = new Personnage(Game, PersonnageJoueurPrincipalModel, PersonnageJoueurPrincipalTexture, "Shader", 1, RotationInitialePersonnagePrincipal, PositionInitialePersonnagePrincipal);
         PersonnageSecondaire = new Personnage(Game, PersonnageJoueurSecondaireModel, PersonnageJoueurSecondaireTexture, "Shader", 1, RotationInitialePersonnageSecondaire, PositionInitialePersonnageSecondaire);

         //Ajout des objets dans la liste de Components
         Game.Components.Add(PersonnagePrincipal);
         Game.Components.Add(PersonnageSecondaire);
         InitialiserModèles();
         base.Initialize();
      }

      protected void InitialiserMurs(string[] tabMurs)
      {
          NomGauche = tabMurs[0];
          NomDroite = tabMurs[1];
          NomPlafond = tabMurs[2];
          NomPlancher = tabMurs[3];
          NomAvant = tabMurs[4];
          NomArrière = tabMurs[5];
      }
      //Fixe les positions des verres
      void FixerLesPositions()
      {
         //Verres du joueur
         ListePositionVerres.Add(new Vector3(0, DimensionTable.Y, DistanceVerre));
         ListePositionVerres.Add(new Vector3(DIAMÈTRE_VERRE, DimensionTable.Y, DistanceVerre));
         ListePositionVerres.Add(new Vector3(-DIAMÈTRE_VERRE, DimensionTable.Y, DistanceVerre));
         ListePositionVerres.Add(new Vector3(DIAMÈTRE_VERRE / 2, DimensionTable.Y, DistanceVerre - DIAMÈTRE_VERRE * (float)Math.Sin(Math.PI / 3)));
         ListePositionVerres.Add(new Vector3(-DIAMÈTRE_VERRE / 2, DimensionTable.Y, DistanceVerre - DIAMÈTRE_VERRE * (float)Math.Sin(Math.PI / 3)));
         ListePositionVerres.Add(new Vector3(0, DimensionTable.Y, DistanceVerre - 2 * DIAMÈTRE_VERRE * (float)Math.Sin(Math.PI / 3)));
         //Verres de l'adversaire
         ListePositionVerresAdv.Add(new Vector3(0, DimensionTable.Y, -DistanceVerre)); 
         ListePositionVerresAdv.Add(new Vector3(DIAMÈTRE_VERRE, DimensionTable.Y, -DistanceVerre));
         ListePositionVerresAdv.Add(new Vector3(-DIAMÈTRE_VERRE, DimensionTable.Y, -DistanceVerre));
         ListePositionVerresAdv.Add(new Vector3(DIAMÈTRE_VERRE / 2, DimensionTable.Y, -DistanceVerre + DIAMÈTRE_VERRE * (float)Math.Sin(Math.PI / 3)));
         ListePositionVerresAdv.Add(new Vector3(-DIAMÈTRE_VERRE / 2, DimensionTable.Y, -DistanceVerre + DIAMÈTRE_VERRE * (float)Math.Sin(Math.PI / 3))); 
         ListePositionVerresAdv.Add(new Vector3(0, DimensionTable.Y, -DistanceVerre + 2 * DIAMÈTRE_VERRE * (float)Math.Sin(Math.PI / 3)));
      }
      //Initialise les murs (plans texturés) avec la texture nécessaire
      void InitialiserPlansTexturés()
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
      }
      //Ajoute la bière dans les verres (la bière étant un modèle avec une texture)
      void AjouterBiere()
      {
         ListeBiereJoueur = new List<ObjetDeBase>();
         for (int i = 0; i < ListePositionVerres.Count; ++i)
         {
            ListeBiereJoueur.Add(new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(ListePositionVerres[i].X, ListePositionVerres[i].Y + 0.07f, ListePositionVerres[i].Z)));
         }
         foreach (ObjetDeBase biere in ListeBiereJoueur)
         {
            Game.Components.Add(biere);
         }

         ListeBiereAdv = new List<ObjetDeBase>();
         for (int i = 0; i < ListePositionVerresAdv.Count; ++i)
         {
            ListeBiereAdv.Add(new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(ListePositionVerresAdv[i].X, ListePositionVerresAdv[i].Y + 0.07f, ListePositionVerresAdv[i].Z)));
         }
         foreach (ObjetDeBase biere in ListeBiereAdv)
         {
            Game.Components.Add(biere);
         }
      }

      //Ajoute les verres dans les composantes de jeu
      void CréerLesVerres()
      {
         VerresJoueur = new List<VerreJoueurPrincipal>();
         for (int i = 0; i < ListePositionVerres.Count; ++i)
         {
            VerresJoueur.Add(new VerreJoueurPrincipal(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerres[i]));
         }
         foreach (VerreJoueurPrincipal verre in VerresJoueur)
         {
            Game.Components.Add(verre);
         }

         VerresAdversaire = new List<VerreAdversaire>();
         for (int i = 0; i < ListePositionVerresAdv.Count; ++i)
         {
            VerresAdversaire.Add(new VerreAdversaire(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerresAdv[i]));
         }
         foreach (VerreAdversaire verre in VerresAdversaire)
         {
            Game.Components.Add(verre);
         }
      }
      //Fonction abstract pour initialiser les modèles qui seront initialiser dans l'environnement même puisqu'ils varient selon l'environnement.
      abstract protected void InitialiserModèles();
      #endregion
   }
}
