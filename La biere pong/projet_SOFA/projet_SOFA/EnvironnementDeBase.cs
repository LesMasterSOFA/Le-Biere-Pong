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
      Vector2 étenduePlanMur = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN - 4);
      Vector2 étenduePlanPlafond = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN);
      Vector2 charpentePlan = new Vector2(4, 3);
      const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
      Vector3 DIMENSION_BONHOMMME = new Vector3(0.50f, 1.705f, 0.367f);
      const float DIAMÈTRE_VERRE = 0.09225f;
      const float RAYON_VERRE = DIAMÈTRE_VERRE / 2;
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
      string NomArrière { get; set; }
      float TempsÉcouléDepuisMAJ { get; set; }
      float TempsTotal { get; set; }
      InputManager GestionClavier { get; set; }
      PlanTexturé Gauche { get; set; }
      PlanTexturé Droite { get; set; }
      PlanTexturé Plafond { get; set; }
      PlanTexturé Plancher { get; set; }
      PlanTexturé Avant { get; set; }
      PlanTexturé Arrière { get; set; }
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
      TypePartie TypeDePartie { get; set; }
      ATH ath { get; set; }

      public EnrivonnementDeBase(Game game, GestionEnvironnement gestionEnv, string nomGauche, string nomDroite, string nomPlafond, string nomPlancher,
                                string nomAvant, string nomArrière, string personnageJoueurPrincipalModel, string personnageJoueurPrincipalTexture,
                                string personnageJoueurSecondaireModel, string personnageJoueurSecondaireTexture, Vector3 dimensionTable, float distanceVerre, TypePartie typePartie)
         : base(game)
      {
         NomGauche = nomGauche;
         NomDroite = nomDroite;
         NomPlafond = nomPlafond;
         NomPlancher = nomPlancher;
         NomAvant = nomAvant;
         NomArrière = nomArrière;
         PersonnageJoueurPrincipalModel = personnageJoueurPrincipalModel;
         PersonnageJoueurPrincipalTexture = personnageJoueurPrincipalTexture;
         PersonnageJoueurSecondaireModel = personnageJoueurSecondaireModel;
         PersonnageJoueurSecondaireTexture = personnageJoueurSecondaireTexture;
         gestionEnviro = gestionEnv;
         DimensionTable = dimensionTable;
         DistanceVerre = distanceVerre;
         TypeDePartie = typePartie;
      }
      public override void Initialize()
      {
         Ai = new AI(ModeDifficulté.Difficile);
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

         ListePositionVerres = new List<Vector3>();
         ListePositionVerresAdv = new List<Vector3>();
         FixerLesPositions();
         AjouterBiere();

         BoundingTable = new BoundingBox(new Vector3(-DimensionTable.X / 2, DimensionTable.Y - 0.1f, -DimensionTable.Z / 2), new Vector3(DimensionTable.X / 2, DimensionTable.Y, DimensionTable.Z / 2));
         BoundingBonhommePrincipal = new BoundingBox(new Vector3(PositionInitialePersonnagePrincipal.X - DIMENSION_BONHOMMME.X / 2, 0, PositionInitialePersonnagePrincipal.Z - DIMENSION_BONHOMMME.Z), new Vector3(PositionInitialePersonnagePrincipal.X + DIMENSION_BONHOMMME.X / 2, DIMENSION_BONHOMMME.Y, PositionInitialePersonnagePrincipal.Z));
         BoundingBonhommeSecondaire = new BoundingBox(new Vector3(PositionInitialePersonnageSecondaire.X - DIMENSION_BONHOMMME.X / 2, 0, PositionInitialePersonnageSecondaire.Z - DIMENSION_BONHOMMME.Z), new Vector3(PositionInitialePersonnageSecondaire.X + DIMENSION_BONHOMMME.X / 2, DIMENSION_BONHOMMME.Y, PositionInitialePersonnageSecondaire.Z));

         CréerLesVerres();

         //Pour personnages
         PersonnagePrincipal = new Personnage(Game, PersonnageJoueurPrincipalModel, PersonnageJoueurPrincipalTexture, "Shader", 1, RotationInitialePersonnagePrincipal, PositionInitialePersonnagePrincipal);
         PersonnageSecondaire = new Personnage(Game, PersonnageJoueurSecondaireModel, PersonnageJoueurSecondaireTexture, "Shader", 1, RotationInitialePersonnageSecondaire, PositionInitialePersonnageSecondaire);

         //Ajout des objets dans la liste de Components
         Game.Components.Add(PersonnagePrincipal);
         Game.Components.Add(PersonnageSecondaire);

         InitialiserModèles();
         base.Initialize();
      }
      void FixerLesPositions()
      {
         ListePositionVerres.Add(new Vector3(0, DimensionTable.Y, DistanceVerre)); ListePositionVerres.Add(new Vector3(DIAMÈTRE_VERRE, DimensionTable.Y, DistanceVerre));
         ListePositionVerres.Add(new Vector3(-DIAMÈTRE_VERRE, DimensionTable.Y, DistanceVerre)); ListePositionVerres.Add(new Vector3(DIAMÈTRE_VERRE / 2, DimensionTable.Y, DistanceVerre - DIAMÈTRE_VERRE * (float)Math.Sin(Math.PI / 3)));
         ListePositionVerres.Add(new Vector3(-DIAMÈTRE_VERRE / 2, DimensionTable.Y, DistanceVerre - DIAMÈTRE_VERRE * (float)Math.Sin(Math.PI / 3))); ListePositionVerres.Add(new Vector3(0, DimensionTable.Y, DistanceVerre - 2 * DIAMÈTRE_VERRE * (float)Math.Sin(Math.PI / 3)));

         ListePositionVerresAdv.Add(new Vector3(0, DimensionTable.Y, -DistanceVerre)); ListePositionVerresAdv.Add(new Vector3(DIAMÈTRE_VERRE, DimensionTable.Y, -DistanceVerre));
         ListePositionVerresAdv.Add(new Vector3(-DIAMÈTRE_VERRE, DimensionTable.Y, -DistanceVerre)); ListePositionVerresAdv.Add(new Vector3(DIAMÈTRE_VERRE / 2, DimensionTable.Y, -DistanceVerre + DIAMÈTRE_VERRE * (float)Math.Sin(Math.PI / 3)));
         ListePositionVerresAdv.Add(new Vector3(-DIAMÈTRE_VERRE / 2, DimensionTable.Y, -DistanceVerre + DIAMÈTRE_VERRE * (float)Math.Sin(Math.PI / 3))); ListePositionVerresAdv.Add(new Vector3(0, DimensionTable.Y, -DistanceVerre + 2 * DIAMÈTRE_VERRE * (float)Math.Sin(Math.PI / 3)));
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

      void CréerLesVerres()
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
         float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
         TempsÉcouléDepuisMAJ += TempsÉcoulé;
         if (TempsÉcouléDepuisMAJ >= INTERVALLE_MAJ_STANDARD)
         {
            TempsTotal += TempsÉcouléDepuisMAJ;
            EffectuerÉvénementLocal();
            if (Game.Components.Where(net => net is NetworkClient).Count() == 1)
            {
                NetworkClient client = (NetworkClient)Game.Components.Where(net => net is NetworkClient).ElementAt(0);
                EffectuerÉvénementLAN(client);
            }
            TempsÉcouléDepuisMAJ = 0;
         }
         base.Update(gameTime);
      }

      void EffectuerÉvénementLAN(NetworkClient client)
      {
          if (client.EstMessageReçuLancerBalle)
          {
              client.EstMessageReçuLancerBalle = false;
              float[] tableauInfoLancer = client.InfoBalle;
              Console.WriteLine(tableauInfoLancer[0]);
              if (Game.Components.Where(affinfo => affinfo is AffichageInfoLancer).Count() != 0)
              {
                  Game.Components.Remove(Game.Components.Where(affinfo => affinfo is AffichageInfoLancer).ElementAt(0));
              }
              Game.Components.Add(new AffichageInfoLancer(Game, tableauInfoLancer[0], tableauInfoLancer[1], tableauInfoLancer[2]));
              Joueur joueur = new Joueur(Game);
              List<Personnage> ListePerso = new List<Personnage>();
              foreach (Personnage perso in Game.Components.Where(person => person is Personnage))
              {
                  ListePerso.Add(perso);
              }
              if (gestionEnviro.CaméraJeu.Position.Z > 0)
              {
                  joueur.ChangerAnimation(TypeActionPersonnage.Lancer, ListePerso.Find(peros => peros.Position.Z > 0));
              }
              else
              {
                  joueur.ChangerAnimation(TypeActionPersonnage.Lancer, ListePerso.Find(peros => peros.Position.Z < 0));
              }
              gestionEnviro.CaméraJeu.TempsTotal = 0;
              gestionEnviro.CaméraJeu.EstMouvCamActif = true;
          }
      }
      void EffectuerÉvénementLocal()
      {
         if (Game.Components.Where(info => info is AffichageInfoLancer).Count() == 1)
         {
            if (ActiverLancer)
            {
               TempsTotal = 0;
               ActiverLancer = false;
               foreach (AffichageInfoLancer info in Game.Components.Where(info => info is AffichageInfoLancer))
               {
                  infoLancer = info;
               }
                #region //pour réseau
                foreach(ATH hud in Game.Components.Where(hud => hud is ATH))
                {
                    ath = hud;
                }
               if (Game.Components.Where(net => net is NetworkClient).Count() == 1 && ath.BoutonLancer.EstActif)
               {
                   NetworkClient client = (NetworkClient)Game.Components.Where(net => net is NetworkClient).ElementAt(0);
                   client.EnvoyerInfoLancerBalle(infoLancer.Force, infoLancer.InfoAngleHor, infoLancer.InfoAngleVert);
               }
               ath.BoutonLancer.EstActif =false;
               #endregion
            }
            else if (ActiverInfo && TempsTotal >= 2.5f)
            {
               ActiverInfo = false;
               if (gestionEnviro.CaméraJeu.Position.Z > 0)
               {
                  Balle = new BallePhysique(Game, "balle", "couleur_Balle", "Shader", 1, new Vector3(0, 0, 0), PositionIniBalle,
                                           -(4f * infoLancer.Force) / 100f - 0.5f, (float)MathHelper.ToRadians(infoLancer.InfoAngleHor),
                                           (float)MathHelper.ToRadians(infoLancer.InfoAngleVert), BoundingTable, BoundingBonhommePrincipal,
                                           ListePositionVerresAdv, RAYON_VERRE, HAUTEUR_VERRE, DimensionTable.Y, DIMENSION_TERRAIN, true, INTERVALLE_MAJ_STANDARD);
               }
               else
               {
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
                 Game.Components.Remove(ListeBiereAdv[Balle.IndexÀRetirer]);
                 Game.Components.Remove(VerresAdversaire[Balle.IndexÀRetirer]);
                 ListeBiereAdv.RemoveAt(Balle.IndexÀRetirer);
                 VerresAdversaire.RemoveAt(Balle.IndexÀRetirer);
                 ListePositionVerresAdv.RemoveAt(Balle.IndexÀRetirer);
                 if (Balle.RebondSurTable && ListeBiereAdv.Count > 1)
                 {
                     Game.Components.Remove(ListeBiereAdv[0]);
                     Game.Components.Remove(VerresAdversaire[0]);
                     ListeBiereAdv.RemoveAt(0);
                     VerresAdversaire.RemoveAt(0);
                     ListePositionVerresAdv.RemoveAt(0);
                 }
             }
             else
             {
                 Game.Components.Remove(ListeBiereJoueur[Balle.IndexÀRetirer]);
                 Game.Components.Remove(VerresJoueur[Balle.IndexÀRetirer]);
                 ListeBiereJoueur.RemoveAt(Balle.IndexÀRetirer);
                 VerresJoueur.RemoveAt(Balle.IndexÀRetirer);
                 ListePositionVerres.RemoveAt(Balle.IndexÀRetirer);
                 if (Balle.RebondSurTable && ListeBiereJoueur.Count > 1)
                 {
                     Game.Components.Remove(ListeBiereJoueur[0]);
                     Game.Components.Remove(VerresJoueur[0]);
                     ListeBiereJoueur.RemoveAt(0);
                     VerresJoueur.RemoveAt(0);
                     ListePositionVerres.RemoveAt(0);
                 }
             }

             Game.Components.Remove(Balle);
             Balle = new BallePhysique(Game, "balle", "couleur_Balle", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 1.4f, 1.7f));

             ActiverLancer = true;
             ActiverInfo = true;
         }
      }

      public virtual void InitialiserModèles()
      { }
   }
}
