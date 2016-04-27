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

    class EnvironnementSousSol : Microsoft.Xna.Framework.DrawableGameComponent
    {
       const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
       Vector3 DIMENSION_TABLE = new Vector3(0.76f, 0.74f, 1.8f);
       Vector3 DIMENSION_BONHOMMME = new Vector3(0.50f, 1.705f, 0.367f);
       const float DIAMÈTRE_VERRE = 0.09225f;
       const float RAYON_VERRE = DIAMÈTRE_VERRE / 2;
       const float HAUTEUR_VERRE = 0.1199f;
       const int DIMENSION_TERRAIN = 7;
       const float DISTANCE_VERRES = 0.8f;
        Vector2 étenduePlanMur = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN - 4);
        Vector2 étenduePlanPlafond = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN);
        Vector2 charpentePlan = new Vector2(4, 3);
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
        ObjetDeBase Table { get; set; }
        BallePhysique Balle { get; set; }
        Personnage personnagePrincipal { get; set; }
        List<VerreJoueurPrincipal> VerresJoueur { get; set; }
        VerreJoueurPrincipal VerreJoueur1 { get; set; }
        VerreJoueurPrincipal VerreJoueur2 { get; set; }
        VerreJoueurPrincipal VerreJoueur3 { get; set; }
        VerreJoueurPrincipal VerreJoueur4 { get; set; }
        VerreJoueurPrincipal VerreJoueur5 { get; set; }
        VerreJoueurPrincipal VerreJoueur6 { get; set; }
        List<VerreAdversaire> VerresAdversaire { get; set; }
        VerreAdversaire VerreAdversaire1 { get; set; }
        VerreAdversaire VerreAdversaire2 { get; set; }
        VerreAdversaire VerreAdversaire3 { get; set; }
        VerreAdversaire VerreAdversaire4 { get; set; }
        VerreAdversaire VerreAdversaire5 { get; set; }
        VerreAdversaire VerreAdversaire6 { get; set; }
        List<Vector3> ListePositionVerres { get; set; }
        List<Vector3> ListePositionVerresAdv { get; set; }
        ObjetDeBase BiereJoueur1 { get; set; }
        ObjetDeBase BiereJoueur2 { get; set; }
        ObjetDeBase BiereJoueur3 { get; set; }
        ObjetDeBase BiereJoueur4 { get; set; }
        ObjetDeBase BiereJoueur5 { get; set; }
        ObjetDeBase BiereJoueur6 { get; set; }
        ObjetDeBase BiereAdv1 { get; set; }
        ObjetDeBase BiereAdv2 { get; set; }
        ObjetDeBase BiereAdv3 { get; set; }
        ObjetDeBase BiereAdv4 { get; set; }
        ObjetDeBase BiereAdv5 { get; set; }
        ObjetDeBase BiereAdv6 { get; set; }
        List<ObjetDeBase> ListeBiereJoueur { get; set; }
        List<ObjetDeBase> ListeBiereAdv { get; set; }
        BoundingBox BoundingTable { get; set; }
        BoundingBox BoundingBonhommePrincipal { get; set; }
        BoundingBox BoundingBonhommeSecondaire { get; set; }

        //Pour personnages
        Vector3 RotationInitialePersonnagePrincipal = new Vector3(-MathHelper.PiOver2, 0, 0);
        Vector3 PositionInitialePersonnagePrincipal = new Vector3(0.182f, 0, -1);
        Vector3 RotationInitialePersonnageSecondaire = new Vector3(-MathHelper.PiOver2, MathHelper.Pi, 0);
        Vector3 PositionInitialePersonnageSecondaire = new Vector3(-0.182f, 0, 1);
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


        public EnvironnementSousSol(Game game, GestionEnvironnement gestionEnv, string nomGauche, string nomDroite, string nomPlafond, string nomPlancher, string nomAvant, string nomArrière,
          string personnageJoueurPrincipalModel, string personnageJoueurPrincipalTexture, string personnageJoueurSecondaireModel, string personnageJoueurSecondaireTexture)
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

       }
        //j'ai changé les échelles des modeles pour quils soient tous a 1f, maintenant, la position est en metres.
        /* Dimensions de la balle: rayon de 2 cm
         * Centre de la balle : centre de la sphere
         * Dimensions de la table: X = 76cm 
         *                         Y = 74cm
         *                         Z = 183cm (pas sur)
         * Centre de la table : a terre, au milieu
         * Dimension du verre : rayon de 9.225cm (dans le haut du verre)
         *                      hauteur : 11.99cm
         * Centre du verre : a terre, au centre du cercle
         * Dimension du monsieur : X = 79.399cm
         *                         Y = 1.705m
         *                         Z = 36.761cm
         * Centre du monsieur : a terre, au milieu
         */
        public override void Initialize()
        {
           ActiverLancer = true;
           ActiverInfo = true;
           RotationInitialePersonnagePrincipal = new Vector3(-MathHelper.PiOver2, 0, 0);
           PositionInitialePersonnagePrincipal = new Vector3(0.182f, 0, -DIMENSION_TABLE.Z / 2 - 0.1f - 0.546f);
           RotationInitialePersonnageSecondaire = new Vector3(-MathHelper.PiOver2, MathHelper.Pi, 0);
           PositionInitialePersonnageSecondaire = new Vector3(-0.182f, 0, DIMENSION_TABLE.Z / 2 + 0.1f + 0.546f);

           PositionIniBalle = new Vector3(0, 1.4f, DIMENSION_TABLE.Z / 2 + 0.1f);
           PositionIniBalleAdv = new Vector3(0, 1.4f, -DIMENSION_TABLE.Z / 2 - 0.1f);
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

           Table = new ObjetDeBase(Game, "table_plastique", "table_plastique", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
           BoundingTable = new BoundingBox(new Vector3(-DIMENSION_TABLE.X / 2, DIMENSION_TABLE.Y - 0.1f, -DIMENSION_TABLE.Z / 2), new Vector3(DIMENSION_TABLE.X / 2, DIMENSION_TABLE.Y, DIMENSION_TABLE.Z / 2));
           BoundingBonhommePrincipal = new BoundingBox(new Vector3(PositionInitialePersonnagePrincipal.X - DIMENSION_BONHOMMME.X / 2, 0, PositionInitialePersonnagePrincipal.Z - DIMENSION_BONHOMMME.Z), new Vector3(PositionInitialePersonnagePrincipal.X + DIMENSION_BONHOMMME.X / 2, DIMENSION_BONHOMMME.Y, PositionInitialePersonnagePrincipal.Z));
           BoundingBonhommeSecondaire = new BoundingBox(new Vector3(PositionInitialePersonnageSecondaire.X - DIMENSION_BONHOMMME.X / 2, 0, PositionInitialePersonnageSecondaire.Z - DIMENSION_BONHOMMME.Z), new Vector3(PositionInitialePersonnageSecondaire.X + DIMENSION_BONHOMMME.X / 2, DIMENSION_BONHOMMME.Y, PositionInitialePersonnageSecondaire.Z));

           CréerLesVerres();

           //Pour personnages
           PersonnagePrincipal = new Personnage(Game, PersonnageJoueurPrincipalModel, PersonnageJoueurPrincipalTexture, "Shader", 1, RotationInitialePersonnagePrincipal, PositionInitialePersonnagePrincipal);
           PersonnageSecondaire = new Personnage(Game, PersonnageJoueurSecondaireModel, PersonnageJoueurSecondaireTexture, "Shader", 1, RotationInitialePersonnageSecondaire, PositionInitialePersonnageSecondaire);

           //Ajout des objets dans la liste de Components
           Game.Components.Add(Table);
           Game.Components.Add(PersonnageSecondaire);
           Game.Components.Add(PersonnagePrincipal);

           AjouterVerresJoueur();//Les ajouter dans les Game.Components
           AjouterVerresAdversaire();//Les ajouter dans les Game.Components
           base.Initialize();
        }
        void FixerLesPositions()
        {
           ListePositionVerres.Add(new Vector3(0, DIMENSION_TABLE.Y, DISTANCE_VERRES)); ListePositionVerres.Add(new Vector3(DIAMÈTRE_VERRE, DIMENSION_TABLE.Y, DISTANCE_VERRES));
           ListePositionVerres.Add(new Vector3(-DIAMÈTRE_VERRE, DIMENSION_TABLE.Y, DISTANCE_VERRES)); ListePositionVerres.Add(new Vector3(DIAMÈTRE_VERRE / 2, DIMENSION_TABLE.Y, DISTANCE_VERRES - DIAMÈTRE_VERRE * (float)Math.Sin(Math.PI / 3)));
           ListePositionVerres.Add(new Vector3(-DIAMÈTRE_VERRE / 2, DIMENSION_TABLE.Y, DISTANCE_VERRES - DIAMÈTRE_VERRE * (float)Math.Sin(Math.PI / 3))); ListePositionVerres.Add(new Vector3(0, DIMENSION_TABLE.Y, DISTANCE_VERRES - 2 * DIAMÈTRE_VERRE * (float)Math.Sin(Math.PI / 3)));

           ListePositionVerresAdv.Add(new Vector3(0, DIMENSION_TABLE.Y, -DISTANCE_VERRES)); ListePositionVerresAdv.Add(new Vector3(DIAMÈTRE_VERRE, DIMENSION_TABLE.Y, -DISTANCE_VERRES));
           ListePositionVerresAdv.Add(new Vector3(-DIAMÈTRE_VERRE, DIMENSION_TABLE.Y, -DISTANCE_VERRES)); ListePositionVerresAdv.Add(new Vector3(DIAMÈTRE_VERRE / 2, DIMENSION_TABLE.Y, -DISTANCE_VERRES + DIAMÈTRE_VERRE * (float)Math.Sin(Math.PI / 3)));
           ListePositionVerresAdv.Add(new Vector3(-DIAMÈTRE_VERRE / 2, DIMENSION_TABLE.Y, -DISTANCE_VERRES + DIAMÈTRE_VERRE * (float)Math.Sin(Math.PI / 3))); ListePositionVerresAdv.Add(new Vector3(0, DIMENSION_TABLE.Y, -DISTANCE_VERRES + 2 * DIAMÈTRE_VERRE * (float)Math.Sin(Math.PI / 3)));
        }
        void AjouterBiere()
        {
           ListeBiereJoueur = new List<ObjetDeBase>();
           BiereJoueur1 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0.09225f, 0.81f, 0.8f));
           BiereJoueur2 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0, 0.81f, 0.8f));
           BiereJoueur3 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(-0.09225f, 0.81f, 0.8f));
           BiereJoueur4 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0.09225f / 2, 0.81f, 0.8f - 0.09225f * (float)Math.Sin(Math.PI / 3)));
           BiereJoueur5 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(-0.09225f / 2, 0.81f, 0.8f - 0.09225f * (float)Math.Sin(Math.PI / 3)));
           BiereJoueur6 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0, 0.81f, 0.8f - 2 * 0.09225f * (float)Math.Sin(Math.PI / 3)));
           ListeBiereJoueur.Add(BiereJoueur1);
           ListeBiereJoueur.Add(BiereJoueur2);
           ListeBiereJoueur.Add(BiereJoueur3);
           ListeBiereJoueur.Add(BiereJoueur4);
           ListeBiereJoueur.Add(BiereJoueur5);
           ListeBiereJoueur.Add(BiereJoueur6);

           foreach (ObjetDeBase biere in ListeBiereJoueur)
           {
              Game.Components.Add(biere);
           }
           ListeBiereAdv = new List<ObjetDeBase>();

           BiereAdv1 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0.09225f, 0.81f, -0.8f));
           BiereAdv2 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0, 0.81f, -0.8f));
           BiereAdv3 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(-0.09225f, 0.81f, -0.8f));
           BiereAdv4 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0.09225f / 2, 0.81f, -0.8f + 0.09225f * (float)Math.Sin(Math.PI / 3)));
           BiereAdv5 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(-0.09225f / 2, 0.81f, -0.8f + 0.09225f * (float)Math.Sin(Math.PI / 3)));
           BiereAdv6 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0, 0.81f, -0.8f + 2 * 0.09225f * (float)Math.Sin(Math.PI / 3)));
           ListeBiereAdv.Add(BiereAdv1);
           ListeBiereAdv.Add(BiereAdv2);
           ListeBiereAdv.Add(BiereAdv3);
           ListeBiereAdv.Add(BiereAdv4);
           ListeBiereAdv.Add(BiereAdv5);
           ListeBiereAdv.Add(BiereAdv6);
           foreach (ObjetDeBase biere in ListeBiereAdv)
           {
              Game.Components.Add(biere);
           }
        }

        void CréerLesVerres()
        {
           VerresJoueur = new List<VerreJoueurPrincipal>();
           VerreJoueur1 = new VerreJoueurPrincipal(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerres[0]);
           VerreJoueur2 = new VerreJoueurPrincipal(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerres[1]);
           VerreJoueur3 = new VerreJoueurPrincipal(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerres[2]);
           VerreJoueur4 = new VerreJoueurPrincipal(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerres[3]);
           VerreJoueur5 = new VerreJoueurPrincipal(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerres[4]);
           VerreJoueur6 = new VerreJoueurPrincipal(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerres[5]);
           VerresJoueur.Add(VerreJoueur1);
           VerresJoueur.Add(VerreJoueur2);
           VerresJoueur.Add(VerreJoueur3);
           VerresJoueur.Add(VerreJoueur4);
           VerresJoueur.Add(VerreJoueur5);
           VerresJoueur.Add(VerreJoueur6);

           VerresAdversaire = new List<VerreAdversaire>();
           VerreAdversaire1 = new VerreAdversaire(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerresAdv[0]);
           VerreAdversaire2 = new VerreAdversaire(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerresAdv[1]);
           VerreAdversaire3 = new VerreAdversaire(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerresAdv[2]);
           VerreAdversaire4 = new VerreAdversaire(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerresAdv[3]);
           VerreAdversaire5 = new VerreAdversaire(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerresAdv[4]);
           VerreAdversaire6 = new VerreAdversaire(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerresAdv[5]);
           VerresAdversaire.Add(VerreAdversaire1);
           VerresAdversaire.Add(VerreAdversaire2);
           VerresAdversaire.Add(VerreAdversaire3);
           VerresAdversaire.Add(VerreAdversaire4);
           VerresAdversaire.Add(VerreAdversaire5);
           VerresAdversaire.Add(VerreAdversaire6);
        }
        void AjouterVerresJoueur()
        {
           foreach (VerreJoueurPrincipal verre in VerresJoueur)
           {
              Game.Components.Add(verre);
           }
        }
        void AjouterVerresAdversaire()
        {
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
              EffectuerÉvènement();
              TempsÉcouléDepuisMAJ = 0;
           }
           base.Update(gameTime);
        }

        void EffectuerÉvènement()
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

                 if (gestionEnviro.CaméraJeu.Position.Z > 0)
                 {
                    Balle = new BallePhysique(Game, "balle", "couleur_Balle", "Shader", 1, new Vector3(0, 0, 0), PositionIniBalle,
                                             -(4f * infoLancer.Force) / 100f - 0.5f, (float)MathHelper.ToRadians(infoLancer.InfoAngleHor),
                                             (float)MathHelper.ToRadians(infoLancer.InfoAngleVert), BoundingTable, BoundingBonhommePrincipal,
                                             ListePositionVerresAdv, RAYON_VERRE, HAUTEUR_VERRE, DIMENSION_TABLE.Y, DIMENSION_TERRAIN, true, INTERVALLE_MAJ_STANDARD);
                 }
                 else
                 {
                    Balle = new BallePhysique(Game, "balle", "couleur_Balle", "Shader", 1, new Vector3(0, 0, 0), PositionIniBalleAdv,
                                             (4f * infoLancer.Force) / 100f + 0.5f, (float)MathHelper.ToRadians(infoLancer.InfoAngleHor),
                                             (float)MathHelper.ToRadians(infoLancer.InfoAngleVert), BoundingTable, BoundingBonhommeSecondaire,
                                             ListePositionVerres, RAYON_VERRE, HAUTEUR_VERRE, DIMENSION_TABLE.Y, DIMENSION_TERRAIN, false, INTERVALLE_MAJ_STANDARD);
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
                 Game.Components.Remove(VerresAdversaire[Balle.IndexÀRetirer]);
                 ListePositionVerresAdv.Remove(ListePositionVerresAdv[Balle.IndexÀRetirer]);
                 if (Balle.RebondSurTable)
                 {
                    Game.Components.Remove(VerresAdversaire[Balle.IndexÀRetirer == 0 ? 1 : 0]);
                    ListePositionVerresAdv.Remove(ListePositionVerresAdv[Balle.IndexÀRetirer == 0 ? 1 : 0]);
                 }
              }
              else
              {
                 Game.Components.Remove(VerresJoueur[Balle.IndexÀRetirer]);
                 ListePositionVerresAdv.Remove(ListePositionVerres[Balle.IndexÀRetirer]);
                 if (Balle.RebondSurTable)
                 {
                    Game.Components.Remove(VerresJoueur[Balle.IndexÀRetirer == 0 ? 1 : 0]);
                    ListePositionVerresAdv.Remove(ListePositionVerres[Balle.IndexÀRetirer == 0 ? 1 : 0]);
                 }
              }

              Game.Components.Remove(Balle);
              Balle = new BallePhysique(Game, "balle", "couleur_Balle", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 1.4f, 1.7f));

              ActiverLancer = true;
              ActiverInfo = true;
           }
        }
    }
}
