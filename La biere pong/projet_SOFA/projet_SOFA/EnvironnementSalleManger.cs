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

    class EnvironnementSalleManger : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const float RAYON_VERRE_HAUT = 0.09225f;
        const float HAUTEUR_VERRE = 0.1199f;
        const float DIMENSION_TABLE_Y = 0.847f;
        const float DIMENSION_TABLE_X = 0.90f;
        const float DIMENSION_TABLE_Z = 2.20f;
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        const int DIMENSION_TERRAIN = 7;
        Vector2 étenduePlanMur = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN - 4);
        Vector2 étenduePlanPlafond = new Vector2(DIMENSION_TERRAIN, DIMENSION_TERRAIN);
        Vector2 charpentePlan = new Vector2(4, 3);
        string NomGauche { get; set; }
        string NomDroite { get; set; }
        string NomPlafond { get; set; }
        string NomPlancher { get; set; }
        string NomAvant { get; set; }
        string NomArrière { get; set; }
        InputManager GestionClavier { get; set; }
        PlanTexturé Gauche { get; set; }
        PlanTexturé Droite { get; set; }
        PlanTexturé Plafond { get; set; }
        PlanTexturé Plancher { get; set; }
        PlanTexturé Avant { get; set; }
        PlanTexturé Arrière { get; set; }
        ObjetDeBase Table { get; set; }
        ObjetDeBase Balle { get; set; }
        ObjetDeBase Chaise1 { get; set; }
        ObjetDeBase Chaise2 { get; set; }
        ObjetDeBase Chaise3 { get; set; }
        ObjetDeBase Chaise4 { get; set; }
        ObjetDeBase Chaise5 { get; set; }
        ObjetDeBase Chaise6 { get; set; }
        ObjetDeBase Urinoir { get; set; }
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
        SoundEffect Tadah { get; set; }
        SoundEffect Wow { get; set; }

        //Pour personnages
        Vector3 RotationInitialePersonnagePrincipal = new Vector3(-MathHelper.PiOver2, 0, 0);
        Vector3 PositionInitialePersonnagePrincipal = new Vector3(0.182f, 0, -1.3f);
        Vector3 RotationInitialePersonnageSecondaire = new Vector3(-MathHelper.PiOver2, MathHelper.Pi, 0);
        Vector3 PositionInitialePersonnageSecondaire = new Vector3(-0.182f, 0, 1.3f);
        public string PersonnageJoueurPrincipalModel { get; private set; }
        public string PersonnageJoueurPrincipalTexture { get; private set; }
        public string PersonnageJoueurSecondaireModel { get; private set; }
        public string PersonnageJoueurSecondaireTexture { get; private set; }
        Personnage PersonnagePrincipal { get; set; }
        Personnage PersonnageSecondaire { get; set; }


        public EnvironnementSalleManger(Game game, string nomGauche, string nomDroite, string nomPlafond, string nomPlancher, string nomAvant, string nomArrière,
            string personnageJoueurPrincipalModel, string personnageJoueurPrincipalTexture, string personnageJoueurSecondaireModel, string personnageJoueurSecondaireTexture)
            :base(game)
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
         * Dimensions de la table: X = 90cm 
         *                         Y = 84.148cm
         *                         Z = 220cm
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

            Table = new ObjetDeBase(Game, "tablesallemanger", "tex_table_salle", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            BoundingBox boundingTable = new BoundingBox(new Vector3(-DIMENSION_TABLE_Y / 2, 0, -DIMENSION_TABLE_Z / 2), new Vector3(DIMENSION_TABLE_Y / 2, 0.755f, DIMENSION_TABLE_Z / 2));
            //Balle = new BallePhysique(Game, "balle", "couleur_Balle", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 1f, 1.7f), 4.5f, 0, MathHelper.PiOver4, boundingTable, ListePositionVerresAdv, RAYON_VERRE_HAUT, HAUTEUR_VERRE, INTERVALLE_MAJ_STANDARD);

            CréerLesVerres();

            //Pour personnages
            PersonnagePrincipal = new Personnage(Game, PersonnageJoueurPrincipalModel, PersonnageJoueurPrincipalTexture, "Shader", 1, RotationInitialePersonnagePrincipal, PositionInitialePersonnagePrincipal);
            PersonnageSecondaire = new Personnage(Game, PersonnageJoueurSecondaireModel, PersonnageJoueurSecondaireTexture, "Shader", 1, RotationInitialePersonnageSecondaire, PositionInitialePersonnageSecondaire);


            //Ajout des objets dans la liste de Components
            Game.Components.Add(Balle);
            Game.Components.Add(Table);

            Game.Components.Add(PersonnagePrincipal);
            Game.Components.Add(PersonnageSecondaire);

            AjouterVerresJoueur();//Les ajouter dans les Game.Components
            AjouterVerresAdversaire();//Les ajouter dans les Game.Components
            InitialiserModèles();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            Wow = Game.Content.Load<SoundEffect>("Sounds\\WOW");
            Tadah = Game.Content.Load<SoundEffect>("Sounds\\Tadah");
        }

        void FixerLesPositions()
        {
            ListePositionVerres.Add(new Vector3(0, DIMENSION_TABLE_Y, 1f)); ListePositionVerres.Add(new Vector3(0.09225f, DIMENSION_TABLE_Y, 1f));
            ListePositionVerres.Add(new Vector3(-0.09225f, DIMENSION_TABLE_Y, 1f)); ListePositionVerres.Add(new Vector3(0.09225f / 2, DIMENSION_TABLE_Y, 1f - 0.09225f * (float)Math.Sin(Math.PI / 3)));
            ListePositionVerres.Add(new Vector3(-0.09225f / 2, DIMENSION_TABLE_Y, 1f - 0.09225f * (float)Math.Sin(Math.PI / 3))); ListePositionVerres.Add(new Vector3(0, DIMENSION_TABLE_Y, 1f - 2 * 0.09225f * (float)Math.Sin(Math.PI / 3)));

            ListePositionVerresAdv.Add(new Vector3(0, DIMENSION_TABLE_Y, -1f)); ListePositionVerresAdv.Add(new Vector3(0.09225f, DIMENSION_TABLE_Y, -1f));
            ListePositionVerresAdv.Add(new Vector3(-0.09225f, DIMENSION_TABLE_Y, -1f)); ListePositionVerresAdv.Add(new Vector3(0.09225f / 2, DIMENSION_TABLE_Y, -1f + 0.09225f * (float)Math.Sin(Math.PI / 3)));
            ListePositionVerresAdv.Add(new Vector3(-0.09225f / 2, DIMENSION_TABLE_Y, -1f + 0.09225f * (float)Math.Sin(Math.PI / 3))); ListePositionVerresAdv.Add(new Vector3(0, DIMENSION_TABLE_Y, -1f + 2 * 0.09225f * (float)Math.Sin(Math.PI / 3)));
        }
        void AjouterBiere()
        {
            ListeBiereJoueur = new List<ObjetDeBase>();
            BiereJoueur1 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0, 0.91f, 1f));
            BiereJoueur2 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0.09225f, 0.91f, 1f));
            BiereJoueur3 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(-0.09225f, 0.91f, 1f));
            BiereJoueur4 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0.09225f / 2, 0.91f, 1f - 0.09225f * (float)Math.Sin(Math.PI / 3)));
            BiereJoueur5 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(-0.09225f / 2, 0.91f, 1f - 0.09225f * (float)Math.Sin(Math.PI / 3)));
            BiereJoueur6 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0, 0.91f, 1f - 2 * 0.09225f * (float)Math.Sin(Math.PI / 3)));
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

            BiereAdv1 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0, 0.91f, -1f));
            BiereAdv2 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0.09225f, 0.91f, -1f));
            BiereAdv3 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(-0.09225f, 0.91f, -1f));
            BiereAdv4 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0.09225f / 2, 0.91f, -1f + 0.09225f * (float)Math.Sin(Math.PI / 3)));
            BiereAdv5 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(-0.09225f / 2, 0.91f, -1f + 0.09225f * (float)Math.Sin(Math.PI / 3)));
            BiereAdv6 = new ObjetDeBase(Game, "Biere", "TextureBiere", "Shader", 1, new Vector3(MathHelper.PiOver2, 0, 0), new Vector3(0, 0.91f, -1f + 2 * 0.09225f * (float)Math.Sin(Math.PI / 3)));
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
            if (GestionClavier.EstNouvelleTouche(Keys.E))
            {
                Wow.Play();
                GestionÉvénements.EnleverVerres(VerresJoueur, Game, VerreJoueur1, true, true);
            }
            if(GestionClavier.EstNouvelleTouche(Keys.B))
            {
                Tadah.Play();
            }
            base.Update(gameTime);
        }
        void InitialiserModèles()
        {
            Chaise1 = new ObjetDeBase(Game, "Chaise", "UVChaiseFait", "Shader", 1, new Vector3(0, 0, 0), new Vector3(3.1f, 0, 1));
            Chaise2 = new ObjetDeBase(Game, "Chaise", "UVChaiseFait", "Shader", 1, new Vector3(0, 0, 0), new Vector3(3.1f, 0, -1));
            Chaise3 = new ObjetDeBase(Game, "Chaise", "UVChaiseFait", "Shader", 1, new Vector3(0, 0, 0), new Vector3(3.1f, 0, 0));
            Chaise4 = new ObjetDeBase(Game, "Chaise", "UVChaiseFait", "Shader", 1, new Vector3(0, MathHelper.Pi, 0), new Vector3(-3.1f, 0, 2));
            Chaise5 = new ObjetDeBase(Game, "Chaise", "UVChaiseFait", "Shader", 1, new Vector3(0, MathHelper.Pi, 0), new Vector3(-3.1f, 0, -2));
            Chaise6 = new ObjetDeBase(Game, "Chaise", "UVChaiseFait", "Shader", 1, new Vector3(0, MathHelper.Pi, 0), new Vector3(-3.1f, 0, -3));
            Game.Components.Add(Chaise1);
            Game.Components.Add(Chaise2);
            Game.Components.Add(Chaise3);
            Game.Components.Add(Chaise4);
            Game.Components.Add(Chaise5);
            Game.Components.Add(Chaise6);

        }
    }
}