using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{

   abstract class EnvironnementDeBase : Microsoft.Xna.Framework.DrawableGameComponent
   {
       #region Constantes et propri�t�s

      const int DIMENSION_TERRAIN = 7;
      const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
      const float DIAM�TRE_VERRE = 0.09225f;
      const float RAYON_VERRE = DIAM�TRE_VERRE / 2;
      const float HAUTEUR_VERRE = 0.1199f;
      Vector2 �tenduePlanMur = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN - 4);
      Vector2 �tenduePlanPlafond = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN);
      Vector2 charpentePlan = new Vector2(4, 3);
      Vector3 DIMENSION_BONHOMMME = new Vector3(0.50f, 1.705f, 0.367f);

      protected Vector3 DimensionTable { get; set; }
      protected float DistanceVerre { get; set; }
      float Temps�coul�DepuisMAJ { get; set; }
      float TempsTotal { get; set; }
      //String repr�sentant les noms des murs (Changeant selon l'environnement)
      protected string NomGauche { get; set; }
      protected string NomDroite { get; set; }
      protected string NomPlafond { get; set; }
      protected string NomPlancher { get; set; }
      protected string NomAvant { get; set; }
      protected string NomArri�re { get; set; }
      //
      PlanTextur� Gauche { get; set; }
      PlanTextur� Droite { get; set; }
      PlanTextur� Plafond { get; set; }
      PlanTextur� Plancher { get; set; }
      PlanTextur� Avant { get; set; }
      PlanTextur� Arri�re { get; set; }
      //
      InputManager GestionClavier { get; set; }
      BallePhysique Balle { get; set; }
      List<ObjetDeBase> ListeBiereJoueur { get; set; }
      List<ObjetDeBase> ListeBiereAdv { get; set; }
      Personnage personnagePrincipal { get; set; }
      public List<VerreJoueurPrincipal> VerresJoueur { get; private set; }
      public List<VerreAdversaire> VerresAdversaire { get; private set; }
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
      AI Ai { get; set; }
      TypePartie TypeDePartie { get; set; }
      ATH ath { get; set; }
       #endregion

      public EnvironnementDeBase(Game game, GestionEnvironnement gestionEnv, string personnageJoueurPrincipalModel, string personnageJoueurPrincipalTexture,
                                string personnageJoueurSecondaireModel, string personnageJoueurSecondaireTexture, TypePartie typePartie)
         : base(game)
      {
         PersonnageJoueurPrincipalModel = personnageJoueurPrincipalModel;
         PersonnageJoueurPrincipalTexture = personnageJoueurPrincipalTexture;
         PersonnageJoueurSecondaireModel = personnageJoueurSecondaireModel;
         PersonnageJoueurSecondaireTexture = personnageJoueurSecondaireTexture;
         gestionEnviro = gestionEnv;
         TypeDePartie = typePartie;
      }

      #region Initialisation d'environnement

      public override void Initialize()
      {
         GestionClavier = Game.Services.GetService(typeof(InputManager)) as InputManager;
         Ai = new AI(ModeDifficult�.Difficile);
         ActiverLancer = true;
         ActiverInfo = true;

         RotationInitialePersonnagePrincipal = new Vector3(-MathHelper.PiOver2, 0, 0);
         PositionInitialePersonnagePrincipal = new Vector3(0.182f, 0, -DimensionTable.Z / 2 - 0.1f - 0.546f);
         RotationInitialePersonnageSecondaire = new Vector3(-MathHelper.PiOver2, MathHelper.Pi, 0);
         PositionInitialePersonnageSecondaire = new Vector3(-0.182f, 0, DimensionTable.Z / 2 + 0.1f + 0.546f);

         PositionIniBalle = new Vector3(0, 1.4f, DimensionTable.Z / 2 + 0.1f);
         PositionIniBalleAdv = new Vector3(0, 1.4f, -DimensionTable.Z / 2 - 0.1f);
         Balle = new BallePhysique(Game, "balle", "couleur_Balle", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 1.4f, 1.7f));


         //Initialise les murs (plans textur�s) avec la texture n�cessaire
         Gauche = new PlanTextur�(Game, 1f, new Vector3(0, MathHelper.PiOver2, 0), new Vector3((float)-DIMENSION_TERRAIN / 2, ((float)DIMENSION_TERRAIN - 4) / 2, 0), �tenduePlanMur, charpentePlan, NomGauche, INTERVALLE_MAJ_STANDARD);
         Droite = new PlanTextur�(Game, 1f, new Vector3(0, -MathHelper.PiOver2, 0), new Vector3((float)DIMENSION_TERRAIN / 2, ((float)DIMENSION_TERRAIN - 4) / 2, 0), �tenduePlanMur, charpentePlan, NomDroite, INTERVALLE_MAJ_STANDARD);
         Plafond = new PlanTextur�(Game, 1f, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0, DIMENSION_TERRAIN - 4, 0), �tenduePlanPlafond, charpentePlan, NomPlafond, INTERVALLE_MAJ_STANDARD);
         Plancher = new PlanTextur�(Game, 1f, new Vector3(-MathHelper.PiOver2, 0, 0), new Vector3(0, 0, 0), �tenduePlanPlafond, charpentePlan, NomPlancher, INTERVALLE_MAJ_STANDARD);
         Avant = new PlanTextur�(Game, 1f, Vector3.Zero, new Vector3(0, (float)(DIMENSION_TERRAIN - 4) / 2, (float)-DIMENSION_TERRAIN / 2), �tenduePlanMur, charpentePlan, NomAvant, INTERVALLE_MAJ_STANDARD);
         Arri�re = new PlanTextur�(Game, 1f, new Vector3(0, -MathHelper.Pi, 0), new Vector3(0, (float)(DIMENSION_TERRAIN - 4) / 2, (float)DIMENSION_TERRAIN / 2), �tenduePlanMur, charpentePlan, NomArri�re, INTERVALLE_MAJ_STANDARD);
         //Ajoutes les murs aux composantes
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

      protected void InitialiserMurs(string[] tabMurs)
      {
          NomGauche = tabMurs[0];
          NomDroite = tabMurs[1];
          NomPlafond = tabMurs[2];
          NomPlancher = tabMurs[3];
          NomAvant = tabMurs[4];
          NomArri�re = tabMurs[5];
      }
      //Fixe les positions des verres
      void FixerLesPositions()
      {
         //Verres du joueur
         ListePositionVerres.Add(new Vector3(0, DimensionTable.Y, DistanceVerre));
         ListePositionVerres.Add(new Vector3(DIAM�TRE_VERRE, DimensionTable.Y, DistanceVerre));
         ListePositionVerres.Add(new Vector3(-DIAM�TRE_VERRE, DimensionTable.Y, DistanceVerre));
         ListePositionVerres.Add(new Vector3(DIAM�TRE_VERRE / 2, DimensionTable.Y, DistanceVerre - DIAM�TRE_VERRE * (float)Math.Sin(Math.PI / 3)));
         ListePositionVerres.Add(new Vector3(-DIAM�TRE_VERRE / 2, DimensionTable.Y, DistanceVerre - DIAM�TRE_VERRE * (float)Math.Sin(Math.PI / 3)));
         ListePositionVerres.Add(new Vector3(0, DimensionTable.Y, DistanceVerre - 2 * DIAM�TRE_VERRE * (float)Math.Sin(Math.PI / 3)));
         //Verres de l'adversaire
         ListePositionVerresAdv.Add(new Vector3(0, DimensionTable.Y, -DistanceVerre)); 
         ListePositionVerresAdv.Add(new Vector3(DIAM�TRE_VERRE, DimensionTable.Y, -DistanceVerre));
         ListePositionVerresAdv.Add(new Vector3(-DIAM�TRE_VERRE, DimensionTable.Y, -DistanceVerre));
         ListePositionVerresAdv.Add(new Vector3(DIAM�TRE_VERRE / 2, DimensionTable.Y, -DistanceVerre + DIAM�TRE_VERRE * (float)Math.Sin(Math.PI / 3)));
         ListePositionVerresAdv.Add(new Vector3(-DIAM�TRE_VERRE / 2, DimensionTable.Y, -DistanceVerre + DIAM�TRE_VERRE * (float)Math.Sin(Math.PI / 3))); 
         ListePositionVerresAdv.Add(new Vector3(0, DimensionTable.Y, -DistanceVerre + 2 * DIAM�TRE_VERRE * (float)Math.Sin(Math.PI / 3)));
      }

      //Ajoute la bi�re dans les verres (la bi�re �tant un mod�le avec une texture)
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
      void Cr�erLesVerres()
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
      //Fonction abstract pour initialiser les mod�les qui seront initialiser dans l'environnement m�me puisqu'ils varient selon l'environnement.
      abstract protected void InitialiserMod�les();
      #endregion

      //Gestion d'�v�nements regroup�es, chaque fonction repr�sentant un mode de jeu particulier
      #region Gestion d'�v�nements

      public override void Update(GameTime gameTime)
      {
         float Temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
         Temps�coul�DepuisMAJ += Temps�coul�;
         if (Temps�coul�DepuisMAJ >= INTERVALLE_MAJ_STANDARD)
         {
            TempsTotal += Temps�coul�DepuisMAJ;

            ath = Game.Components.ToList().Find(item => item is ATH) as ATH;

            Effectuer�v�nementLocal();
            Effectuer�v�nementHistoire();

            if (Game.Components.Where(net => net is NetworkClient).Count() == 1)
            {
               NetworkClient client = Game.Components.ToList().Find(net => net is NetworkClient)as NetworkClient;
               Effectuer�v�nementLAN(client);
            }
            Temps�coul�DepuisMAJ = 0;
            base.Update(gameTime);
         }
      }

      void Effectuer�v�nementHistoire()
      {
         if (TypeDePartie == TypePartie.Histoire)
         {
            if (!ath.EstTourJoueurPrincipal && ath.BoutonLancer.EstActif)
            {
               ath.BoutonLancer.EstActif = false;
               float[] tab = Ai.G�rerAI();
               Game.Components.Add(new AffichageInfoLancer(Game, tab[0], tab[1], tab[2]));
               Joueur joueur = new Joueur(Game); List<Personnage> ListePerso = new List<Personnage>();
               foreach (Personnage perso in Game.Components.Where(person => person is Personnage))
               {
                  ListePerso.Add(perso);
               }
               if (gestionEnviro.Cam�raJeu.Position.Z > 0)
               {
                  joueur.ChangerAnimation(TypeActionPersonnage.Lancer, ListePerso.Find(peros => peros.Position.Z > 0));
               }
               else
               {
                  joueur.ChangerAnimation(TypeActionPersonnage.Lancer, ListePerso.Find(peros => peros.Position.Z < 0));
               }
               gestionEnviro.Cam�raJeu.TempsTotal = 0;
               gestionEnviro.Cam�raJeu.EstMouvCamActif = true;
            }
         }
      }

      void Effectuer�v�nementLAN(NetworkClient client)
      {
          if (client.EstMessageRe�uLancerBalle)
          {
              client.EstMessageRe�uLancerBalle = false;
              float[] tableauInfoLancer = client.InfoBalle;
              Console.WriteLine(tableauInfoLancer[0]);
              if (Game.Components.Where(affinfo => affinfo is AffichageInfoLancer).Count() != 0)
              {
                  Game.Components.Remove(Game.Components.ToList().Find(item => item is AffichageInfoLancer));
              }
              Game.Components.Add(new AffichageInfoLancer(Game, tableauInfoLancer[0], tableauInfoLancer[1], tableauInfoLancer[2]));
              Joueur joueur = new Joueur(Game);
              List<Personnage> ListePerso = new List<Personnage>();
              foreach (Personnage perso in Game.Components.Where(person => person is Personnage))
              {
                  ListePerso.Add(perso);
              }
              if (gestionEnviro.Cam�raJeu.Position.Z > 0)
              {
                  joueur.ChangerAnimation(TypeActionPersonnage.Lancer, ListePerso.Find(peros => peros.Position.Z > 0));
              }
              else
              {
                  joueur.ChangerAnimation(TypeActionPersonnage.Lancer, ListePerso.Find(peros => peros.Position.Z < 0));
              }
              gestionEnviro.Cam�raJeu.TempsTotal = 0;
              gestionEnviro.Cam�raJeu.EstMouvCamActif = true;
          }
      }

      void Effectuer�v�nementLocal()
      {
         if (Game.Components.Where(info => info is AffichageInfoLancer).Count() == 1)
         {
            if (ActiverLancer)
            {
               TempsTotal = 0;
               ActiverLancer = false;
               infoLancer = Game.Components.ToList().Find(item => item is AffichageInfoLancer) as AffichageInfoLancer;
               #region //Pour r�seau
               
               if (Game.Components.Where(net => net is NetworkClient).Count() == 1 && ath.BoutonLancer.EstActif)
               {
                   NetworkClient client = Game.Components.ToList().Find(item => item is NetworkClient) as NetworkClient;
                   client.EnvoyerInfoLancerBalle(infoLancer.Force, infoLancer.InfoAngleHor, infoLancer.InfoAngleVert);
               }
               ath.BoutonLancer.EstActif =false;
               #endregion
            }
            else if (ActiverInfo && TempsTotal >= 2.5f)
            {
               ActiverInfo = false;
               if (gestionEnviro.Cam�raJeu.Position.Z > 0)
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
               Game.Components.Remove(Game.Components.ToList().Find(item=>item is AffichageInfoLancer));
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
                 if (Balle.RebondSurTable && ListeBiereAdv.Count >= 1)
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
                 Game.Components.Remove(ListeBiereJoueur[Balle.Index�Retirer]);
                 Game.Components.Remove(VerresJoueur[Balle.Index�Retirer]);
                 ListeBiereJoueur.RemoveAt(Balle.Index�Retirer);
                 VerresJoueur.RemoveAt(Balle.Index�Retirer);
                 ListePositionVerres.RemoveAt(Balle.Index�Retirer);
                 if (Balle.RebondSurTable && ListeBiereJoueur.Count >= 1)
                 {
                     Game.Components.Remove(ListeBiereJoueur[0]);
                     Game.Components.Remove(VerresJoueur[0]);
                     ListeBiereJoueur.RemoveAt(0);
                     VerresJoueur.RemoveAt(0);
                     ListePositionVerres.RemoveAt(0);
                 }
             }

             #region changer animation pour boire
             Joueur joueur = new Joueur(Game); 
             List<Personnage> ListePerso = new List<Personnage>();
             foreach (Personnage perso in Game.Components.Where(person => person is Personnage))
             {
                 ListePerso.Add(perso);
             }
             if (gestionEnviro.Cam�raJeu.Position.Z > 0)
             {
                 joueur.ChangerAnimation(TypeActionPersonnage.Boire, ListePerso.Find(peros => peros.Position.Z < 0));
             }
             else
             {
                 joueur.ChangerAnimation(TypeActionPersonnage.Boire, ListePerso.Find(peros => peros.Position.Z > 0));
             }
             #endregion

             Game.Components.Remove(Balle);
             Balle = new BallePhysique(Game, "balle", "couleur_Balle", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 1.4f, 1.7f));

             ActiverLancer = true;
             ActiverInfo = true;
         }
      }
      #endregion

   }
}
