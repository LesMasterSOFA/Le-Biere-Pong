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

   class EnrivonnementDeBase : Microsoft.Xna.Framework.DrawableGameComponent
   {
      const int DIMENSION_TERRAIN = 7;
      Vector2 �tenduePlanMur = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN - 4);
      Vector2 �tenduePlanPlafond = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN);
      Vector2 charpentePlan = new Vector2(4, 3);
      const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
      Vector3 DIMENSION_BONHOMMME = new Vector3(0.50f, 1.705f, 0.367f);
      const float DIAM�TRE_VERRE = 0.09225f;
      const float RAYON_VERRE = DIAM�TRE_VERRE / 2;
      const float HAUTEUR_VERRE = 0.1199f;
      //
      Vector3 DimensionTable { get; set; }
      float DistanceVerre { get; set; }
      //
      string NomGauche { get; set; }
      string NomDroite { get; set; }
      string NomPlafond { get; set; }
      string NomPlancher { get; set; }
      string NomAvant { get; set; }
      string NomArri�re { get; set; }
      float Temps�coul�DepuisMAJ { get; set; }
      float TempsTotal { get; set; }
      InputManager GestionClavier { get; set; }
      PlanTextur� Gauche { get; set; }
      PlanTextur� Droite { get; set; }
      PlanTextur� Plafond { get; set; }
      PlanTextur� Plancher { get; set; }
      PlanTextur� Avant { get; set; }
      PlanTextur� Arri�re { get; set; }
      BallePhysique Balle { get; set; }
      List<ObjetDeBase> ListeBiereJoueur { get; set; }
      List<ObjetDeBase> ListeBiereAdv { get; set; }
      Personnage personnagePrincipal { get; set; }
      List<VerreJoueurPrincipal> VerresJoueur { get; set; }
      List<VerreAdversaire> VerresAdversaire { get; set; }
      List<Vector3> ListePositionVerres { get; set; }
      List<Vector3> ListePositionVerresAdv { get; set; }
      BoundingBox BoundingTable { get; set; }
      BoundingBox BoundingBonhommePrincipal { get; set; }
      BoundingBox BoundingBonhommeSecondaire { get; set; }

      //Pour personnages
      Vector3 RotationInitialePersonnagePrincipal { get; set; }
      Vector3 PositionInitialePersonnagePrincipal { get; set; }
      Vector3 RotationInitialePersonnageSecondaire { get; set; }
      Vector3 PositionInitialePersonnageSecondaire { get; set; }
      public string PersonnageJoueurPrincipalModel { get; private set; }
      public string PersonnageJoueurPrincipalTexture { get; private set; }
      public string PersonnageJoueurSecondaireModel { get; private set; }
      public string PersonnageJoueurSecondaireTexture { get; private set; }
      Personnage PersonnagePrincipal { get; set; }
      Personnage PersonnageSecondaire { get; set; }
      GestionEnvironnement gestionEnviro { get; set; }
      Vector3 PositionIniBalle { get; set; }
      Vector3 PositionIniBalleAdv { get; set; }
      AffichageInfoLancer infoLancer { get; set; }
      bool ActiverLancer { get; set; }
      bool ActiverInfo { get; set; }
      Random RandGen { get; set; }
      AI Ai { get; set; }

      public EnrivonnementDeBase(Game game, GestionEnvironnement gestionEnv, string nomGauche, string nomDroite, string nomPlafond, string nomPlancher,
                                string nomAvant, string nomArri�re, string personnageJoueurPrincipalModel, string personnageJoueurPrincipalTexture,
                                string personnageJoueurSecondaireModel, string personnageJoueurSecondaireTexture, Vector3 dimensionTable, float distanceVerre)
         : base(game)
      {
         NomGauche = nomGauche;
         NomDroite = nomDroite;
         NomPlafond = nomPlafond;
         NomPlancher = nomPlancher;
         NomAvant = nomAvant;
         NomArri�re = nomArri�re;
         PersonnageJoueurPrincipalModel = personnageJoueurPrincipalModel;
         PersonnageJoueurPrincipalTexture = personnageJoueurPrincipalTexture;
         PersonnageJoueurSecondaireModel = personnageJoueurSecondaireModel;
         PersonnageJoueurSecondaireTexture = personnageJoueurSecondaireTexture;
         gestionEnviro = gestionEnv;
         DimensionTable = dimensionTable;
         DistanceVerre = distanceVerre;
      }
      public override void Initialize()
      {
         Ai = new AI(ModeDifficult�.Difficile);
         RandGen = new Random();
         ActiverLancer = true;
         ActiverInfo = true;
         RotationInitialePersonnagePrincipal = new Vector3(-MathHelper.PiOver2, 0, 0);
         PositionInitialePersonnagePrincipal = new Vector3(0.182f, 0, -DimensionTable.Z / 2 - 0.1f - 0.546f);
         RotationInitialePersonnageSecondaire = new Vector3(-MathHelper.PiOver2, MathHelper.Pi, 0);
         PositionInitialePersonnageSecondaire = new Vector3(-0.182f, 0, DimensionTable.Z / 2 + 0.1f + 0.546f);

         PositionIniBalle = new Vector3(0, 1.4f, DimensionTable.Z / 2 + 0.1f);
         PositionIniBalleAdv = new Vector3(0, 1.4f, -DimensionTable.Z / 2 - 0.1f);
         Balle = new BallePhysique(Game, "balle", "couleur_Balle", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 1.4f, 1.7f));
         GestionClavier = Game.Services.GetService(typeof(InputManager)) as InputManager;

         Gauche = new PlanTextur�(Game, 1f, new Vector3(0, MathHelper.PiOver2, 0), new Vector3((float)-DIMENSION_TERRAIN / 2, ((float)DIMENSION_TERRAIN - 4) / 2, 0), �tenduePlanMur, charpentePlan, NomGauche, INTERVALLE_MAJ_STANDARD);
         Droite = new PlanTextur�(Game, 1f, new Vector3(0, -MathHelper.PiOver2, 0), new Vector3((float)DIMENSION_TERRAIN / 2, ((float)DIMENSION_TERRAIN - 4) / 2, 0), �tenduePlanMur, charpentePlan, NomDroite, INTERVALLE_MAJ_STANDARD);
         Plafond = new PlanTextur�(Game, 1f, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0, DIMENSION_TERRAIN - 4, 0), �tenduePlanPlafond, charpentePlan, NomPlafond, INTERVALLE_MAJ_STANDARD);
         Plancher = new PlanTextur�(Game, 1f, new Vector3(-MathHelper.PiOver2, 0, 0), new Vector3(0, 0, 0), �tenduePlanPlafond, charpentePlan, NomPlancher, INTERVALLE_MAJ_STANDARD);
         Avant = new PlanTextur�(Game, 1f, Vector3.Zero, new Vector3(0, (float)(DIMENSION_TERRAIN - 4) / 2, (float)-DIMENSION_TERRAIN / 2), �tenduePlanMur, charpentePlan, NomAvant, INTERVALLE_MAJ_STANDARD);
         Arri�re = new PlanTextur�(Game, 1f, new Vector3(0, -MathHelper.Pi, 0), new Vector3(0, (float)(DIMENSION_TERRAIN - 4) / 2, (float)DIMENSION_TERRAIN / 2), �tenduePlanMur, charpentePlan, NomArri�re, INTERVALLE_MAJ_STANDARD);

         Game.Components.Add(Gauche);
         Game.Components.Add(Droite);
         Game.Components.Add(Plafond);
         Game.Components.Add(Plancher);
         Game.Components.Add(Avant);
         Game.Components.Add(Arri�re);

         ListePositionVerres = new List<Vector3>();
         ListePositionVerresAdv = new List<Vector3>();
         FixerLesPositions();
         AjouterBiere();

         BoundingTable = new BoundingBox(new Vector3(-DimensionTable.X / 2, DimensionTable.Y - 0.1f, -DimensionTable.Z / 2), new Vector3(DimensionTable.X / 2, DimensionTable.Y, DimensionTable.Z / 2));
         BoundingBonhommePrincipal = new BoundingBox(new Vector3(PositionInitialePersonnagePrincipal.X - DIMENSION_BONHOMMME.X / 2, 0, PositionInitialePersonnagePrincipal.Z - DIMENSION_BONHOMMME.Z), new Vector3(PositionInitialePersonnagePrincipal.X + DIMENSION_BONHOMMME.X / 2, DIMENSION_BONHOMMME.Y, PositionInitialePersonnagePrincipal.Z));
         BoundingBonhommeSecondaire = new BoundingBox(new Vector3(PositionInitialePersonnageSecondaire.X - DIMENSION_BONHOMMME.X / 2, 0, PositionInitialePersonnageSecondaire.Z - DIMENSION_BONHOMMME.Z), new Vector3(PositionInitialePersonnageSecondaire.X + DIMENSION_BONHOMMME.X / 2, DIMENSION_BONHOMMME.Y, PositionInitialePersonnageSecondaire.Z));

         Cr�erLesVerres();

         //Pour personnages
         PersonnagePrincipal = new Personnage(Game, PersonnageJoueurPrincipalModel, PersonnageJoueurPrincipalTexture, "Shader", 1, RotationInitialePersonnagePrincipal, PositionInitialePersonnagePrincipal);
         PersonnageSecondaire = new Personnage(Game, PersonnageJoueurSecondaireModel, PersonnageJoueurSecondaireTexture, "Shader", 1, RotationInitialePersonnageSecondaire, PositionInitialePersonnageSecondaire);

         //Ajout des objets dans la liste de Components
         Game.Components.Add(PersonnagePrincipal);
         Game.Components.Add(PersonnageSecondaire);

         InitialiserMod�les();
         base.Initialize();
      }
      void FixerLesPositions()
      {
         ListePositionVerres.Add(new Vector3(0, DimensionTable.Y, DistanceVerre)); ListePositionVerres.Add(new Vector3(DIAM�TRE_VERRE, DimensionTable.Y, DistanceVerre));
         ListePositionVerres.Add(new Vector3(-DIAM�TRE_VERRE, DimensionTable.Y, DistanceVerre)); ListePositionVerres.Add(new Vector3(DIAM�TRE_VERRE / 2, DimensionTable.Y, DistanceVerre - DIAM�TRE_VERRE * (float)Math.Sin(Math.PI / 3)));
         ListePositionVerres.Add(new Vector3(-DIAM�TRE_VERRE / 2, DimensionTable.Y, DistanceVerre - DIAM�TRE_VERRE * (float)Math.Sin(Math.PI / 3))); ListePositionVerres.Add(new Vector3(0, DimensionTable.Y, DistanceVerre - 2 * DIAM�TRE_VERRE * (float)Math.Sin(Math.PI / 3)));

         ListePositionVerresAdv.Add(new Vector3(0, DimensionTable.Y, -DistanceVerre)); ListePositionVerresAdv.Add(new Vector3(DIAM�TRE_VERRE, DimensionTable.Y, -DistanceVerre));
         ListePositionVerresAdv.Add(new Vector3(-DIAM�TRE_VERRE, DimensionTable.Y, -DistanceVerre)); ListePositionVerresAdv.Add(new Vector3(DIAM�TRE_VERRE / 2, DimensionTable.Y, -DistanceVerre + DIAM�TRE_VERRE * (float)Math.Sin(Math.PI / 3)));
         ListePositionVerresAdv.Add(new Vector3(-DIAM�TRE_VERRE / 2, DimensionTable.Y, -DistanceVerre + DIAM�TRE_VERRE * (float)Math.Sin(Math.PI / 3))); ListePositionVerresAdv.Add(new Vector3(0, DimensionTable.Y, -DistanceVerre + 2 * DIAM�TRE_VERRE * (float)Math.Sin(Math.PI / 3)));
      }
      void AjouterBiere()
      {
         ListeBiereJoueur = new List<ObjetDeBase>();
         for (int i = 0; i < 6; ++i)
         {
            ListeBiereJoueur.Add(new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(ListePositionVerres[i].X, ListePositionVerres[i].Y + 0.07f, ListePositionVerres[i].Z)));
         }
         foreach (ObjetDeBase biere in ListeBiereJoueur)
         {
            Game.Components.Add(biere);
         }

         ListeBiereAdv = new List<ObjetDeBase>();
         for (int i = 0; i < 6; ++i)
         {
            ListeBiereAdv.Add(new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(ListePositionVerresAdv[i].X, ListePositionVerresAdv[i].Y + 0.07f, ListePositionVerresAdv[i].Z)));
         }
         foreach (ObjetDeBase biere in ListeBiereAdv)
         {
            Game.Components.Add(biere);
         }
      }

      void Cr�erLesVerres()
      {
         VerresJoueur = new List<VerreJoueurPrincipal>();
         for (int i = 0; i < 6; ++i)
         {
            VerresJoueur.Add(new VerreJoueurPrincipal(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerres[i]));
         }
         foreach (VerreJoueurPrincipal verre in VerresJoueur)
         {
            Game.Components.Add(verre);
         }

         VerresAdversaire = new List<VerreAdversaire>();
         for (int i = 0; i < 6; ++i)
         {
            VerresAdversaire.Add(new VerreAdversaire(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerresAdv[i]));
         }
         foreach (VerreAdversaire verre in VerresAdversaire)
         {
            Game.Components.Add(verre);
         }
      }

      public override void Update(GameTime gameTime)
      {
         float Temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
         Temps�coul�DepuisMAJ += Temps�coul�;
         if (Temps�coul�DepuisMAJ >= INTERVALLE_MAJ_STANDARD)
         {
            TempsTotal += Temps�coul�DepuisMAJ;
            Effectuer�v�nement();
            Temps�coul�DepuisMAJ = 0;
         }
         base.Update(gameTime);
      }

      void Effectuer�v�nement()
      {
         if (Game.Components.Where(info => info is AffichageInfoLancer).Count() == 1)
         {
            if (ActiverLancer)
            {
               TempsTotal = 0;
               ActiverLancer = false;
            }
            else if (ActiverInfo && TempsTotal >= 2.5f)
            {
               ActiverInfo = false;
               foreach (AffichageInfoLancer info in Game.Components.Where(info => info is AffichageInfoLancer))
               {
                  infoLancer = info;
               }

               if (gestionEnviro.Cam�raJeu.Position.Z > 0)
               {
                  Balle = new BallePhysique(Game, "balle", "couleur_Balle", "Shader", 1, new Vector3(0, 0, 0), PositionIniBalle,
                                           -(4f * infoLancer.Force) / 100f - 0.5f, (float)MathHelper.ToRadians(infoLancer.InfoAngleHor),
                                           (float)MathHelper.ToRadians(infoLancer.InfoAngleVert), BoundingTable, BoundingBonhommePrincipal,
                                           ListePositionVerresAdv, RAYON_VERRE, HAUTEUR_VERRE, DimensionTable.Y, DIMENSION_TERRAIN, true, INTERVALLE_MAJ_STANDARD);
               }
               else
               {
                  //float[] tableau = Ai.G�rerAI();
                  Balle = new BallePhysique(Game, "balle", "couleur_Balle", "Shader", 1, new Vector3(0, 0, 0), PositionIniBalleAdv,
                                           (4f * infoLancer.Force) / 100f + 0.5f, (float)MathHelper.ToRadians(infoLancer.InfoAngleHor),
                                           (float)MathHelper.ToRadians(infoLancer.InfoAngleVert), BoundingTable, BoundingBonhommeSecondaire,
                                           ListePositionVerres, RAYON_VERRE, HAUTEUR_VERRE, DimensionTable.Y, DIMENSION_TERRAIN, false, INTERVALLE_MAJ_STANDARD);
               }
               Game.Components.Insert(13, Balle);
               Game.Components.Remove(Game.Components.Where(info => info is AffichageInfoLancer).ElementAt(0));
               int noDrawOrder = 0;
               foreach (DrawableGameComponent item in Game.Components.Where(x => x is DrawableGameComponent))
               {
                  item.DrawOrder = noDrawOrder++;
               }
            }
         }
         if (Balle.RetirerBalle)
         {
            Game.Components.Remove(Balle);
            Balle = new BallePhysique(Game, "balle", "couleur_Balle", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 1.4f, 1.7f));

            ActiverLancer = true;
            ActiverInfo = true;
         }
         if (Balle.EstDansVerre)
         {
            if (Balle.EstJoueurPrincipal)
            {
               Game.Components.Remove(ListeBiereAdv[Balle.Index�Retirer]);
               Game.Components.Remove(VerresAdversaire[Balle.Index�Retirer]);
               ListeBiereAdv.RemoveAt(Balle.Index�Retirer);
               VerresAdversaire.RemoveAt(Balle.Index�Retirer);
               ListePositionVerresAdv.RemoveAt(Balle.Index�Retirer);
               if (Balle.RebondSurTable)
               {
                  int indexDeuxieme = RandGen.Next(VerresAdversaire.Count);
                  Game.Components.Remove(ListeBiereAdv[indexDeuxieme]);
                  Game.Components.Remove(VerresAdversaire[indexDeuxieme]);
                  ListeBiereAdv.RemoveAt(indexDeuxieme);
                  VerresAdversaire.RemoveAt(indexDeuxieme);
                  ListePositionVerresAdv.RemoveAt(indexDeuxieme);
               }
            }
            else
            {
               Game.Components.Remove(ListeBiereJoueur[Balle.Index�Retirer]);
               Game.Components.Remove(VerresJoueur[Balle.Index�Retirer]);
               ListeBiereJoueur.RemoveAt(Balle.Index�Retirer);
               VerresJoueur.RemoveAt(Balle.Index�Retirer);
               ListePositionVerres.RemoveAt(Balle.Index�Retirer);
               if (Balle.RebondSurTable)
               {
                  int indexDeuxieme = RandGen.Next(VerresJoueur.Count);
                  Game.Components.Remove(ListeBiereJoueur[indexDeuxieme]);
                  Game.Components.Remove(VerresJoueur[indexDeuxieme]);
                  ListeBiereJoueur.RemoveAt(indexDeuxieme);
                  VerresJoueur.RemoveAt(indexDeuxieme);
                  ListePositionVerres.RemoveAt(indexDeuxieme);
               }
            }

            Game.Components.Remove(Balle);
            Balle = new BallePhysique(Game, "balle", "couleur_Balle", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 1.4f, 1.7f));

            ActiverLancer = true;
            ActiverInfo = true;
         }

         if (ListePositionVerresAdv.Count == 0 || ListePositionVerres.Count == 0)
         {
            //faire de quoi pour la fin de la partie
         }
      }

      public virtual void InitialiserMod�les()
      { }
   }
}
