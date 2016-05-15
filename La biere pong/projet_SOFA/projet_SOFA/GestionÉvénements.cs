using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;


namespace AtelierXNA
{
    public class GestionÉvénements : Microsoft.Xna.Framework.GameComponent
    {
        const int DIMENSION_TERRAIN = 7;
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        const float DIAMÈTRE_VERRE = 0.09225f;
        const float RAYON_VERRE = DIAMÈTRE_VERRE / 2;
        const float HAUTEUR_VERRE = 0.1199f;

        BoundingBox BoundingTable { get; set; }
        BoundingBox BoundingBonhommePrincipal { get; set; }
        BoundingBox BoundingBonhommeSecondaire { get; set; }
        Vector3 PositionIniBalle { get; set; }
        Vector3 PositionIniBalleAdv { get; set; }
        protected Vector3 DimensionTable { get; set; }
        BallePhysique Balle { get; set; }

        List<Vector3> ListePositionVerres { get; set; }
        List<Vector3> ListePositionVerresAdv { get; set; }
        List<ObjetDeBase> ListeBiereJoueur { get; set; }
        List<ObjetDeBase> ListeBiereAdv { get; set; }
        List<VerreJoueurPrincipal> VerresJoueur { get; set; }
        List<VerreAdversaire> VerresAdversaire { get; set; }

        AffichageInfoLancer infoLancer { get; set; }
        bool ActiverLancer { get; set; }
        bool ActiverInfo { get; set; }
        AI Ai { get; set; }
        TypePartie TypeDePartie { get; set; }
        ATH ath { get; set; }

        float TempsÉcouléDepuisMAJ { get; set; }
        float TempsTotal { get; set; }

        GestionEnvironnement gestionEnviro { get; set; }

        public GestionÉvénements(Game game, List<Vector3> listeVerreJ, List<Vector3> listeVerreA, List<ObjetDeBase> listeBiereJ, 
            List<ObjetDeBase> listeBiereA,List<VerreJoueurPrincipal> verresJ,List<VerreAdversaire> verresA,Vector3 dimensionTable,
            BoundingBox boundingTable,BoundingBox boundingJ,BoundingBox boundingA)
            : base(game)
        {
            ListePositionVerres = listeVerreJ;
            ListePositionVerresAdv = listeVerreA;
            ListeBiereJoueur = listeBiereJ;
            ListeBiereAdv = listeBiereA;
            VerresJoueur = verresJ;
            VerresAdversaire = verresA;
            DimensionTable = dimensionTable;
            BoundingTable = boundingTable;
            BoundingBonhommePrincipal = boundingJ;
            BoundingBonhommeSecondaire = boundingA;
        }
        public override void Initialize()
        {
            Ai = new AI(ModeDifficulté.Difficile);
            ActiverLancer = true;
            ActiverInfo = true;

            PositionIniBalle = new Vector3(0, 1.4f, DimensionTable.Z / 2 + 0.1f);
            PositionIniBalleAdv = new Vector3(0, 1.4f, -DimensionTable.Z / 2 - 0.1f);
            Balle = new BallePhysique(Game, "balle", "couleur_Balle", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 1.4f, 1.7f));

            gestionEnviro = Game.Components.ToList().Find(item => item is GestionEnvironnement) as GestionEnvironnement;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= INTERVALLE_MAJ_STANDARD)
            {
                TempsTotal += TempsÉcouléDepuisMAJ;
                EffectuerÉvénementLocal();
                EffectuerÉvénementHistoire();

                ath = Game.Components.ToList().Find(item => item is ATH) as ATH;

                if (Game.Components.Where(net => net is NetworkClient).Count() == 1)
                {
                    NetworkClient client = Game.Components.ToList().Find(net => net is NetworkClient) as NetworkClient;
                    EffectuerÉvénementLAN(client);
                }
                TempsÉcouléDepuisMAJ = 0;
                base.Update(gameTime);
            }
        }

        void EffectuerÉvénementHistoire()
        {
            if (TypeDePartie == TypePartie.Histoire)
            {
                if (!ath.EstTourJoueurPrincipal && ath.BoutonLancer.EstActif)
                {
                    ath.BoutonLancer.EstActif = false;
                    float[] tab = Ai.GérerAI();

                    Game.Components.Add(new AffichageInfoLancer(Game, tab[0], tab[1], tab[2]));//selon les infos de l'AI

                    ChangerAnimationPersonnage(TypeActionPersonnage.Lancer);

                    gestionEnviro.CaméraJeu.TempsTotal = 0;
                    gestionEnviro.CaméraJeu.EstMouvCamActif = true;
                }
            }
        }

        void EffectuerÉvénementLAN(NetworkClient client)
        {
            if (client.EstMessageReçuLancerBalle)
            {
                client.EstMessageReçuLancerBalle = false;
                float[] tableauInfoLancer = client.InfoBalle;
                Console.WriteLine(tableauInfoLancer[0]);

                Game.Components.Add(new AffichageInfoLancer(Game, tableauInfoLancer[0], tableauInfoLancer[1], tableauInfoLancer[2]));//selon infos reçues

                ChangerAnimationPersonnage(TypeActionPersonnage.Lancer);

                gestionEnviro.CaméraJeu.TempsTotal = 0;
                gestionEnviro.CaméraJeu.EstMouvCamActif = true;
            }
        }

        void EffectuerÉvénementLocal()
        {
            if (Game.Components.Where(info => info is AffichageInfoLancer).Count() == 1) //S'il existe un AffichageInfoLancer dans Game.Components
            {
                if (ActiverLancer)
                {
                    TempsTotal = 0;
                    ActiverLancer = false;
                    infoLancer = Game.Components.ToList().Find(item => item is AffichageInfoLancer) as AffichageInfoLancer;
                    EnvoyerInfoClient();//Pour Réseau
                    ath.BoutonLancer.EstActif = false;
                }
                else if (ActiverInfo && TempsTotal >= 2.5f)
                {
                    ActiverInfo = false;
                    LancerBalle();
                    Game.Components.Remove(Game.Components.ToList().Find(item => item is AffichageInfoLancer));
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
                EnleverVerres();
                ChangerAnimationPersonnage(TypeActionPersonnage.Boire);

                Game.Components.Remove(Balle);
                Balle = new BallePhysique(Game, "balle", "couleur_Balle", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 1.4f, 1.7f));

                ActiverLancer = true;
                ActiverInfo = true;
            }
        }

        #region fonctions pour update
        void EnvoyerInfoClient()
        {
            if (Game.Components.Where(net => net is NetworkClient).Count() == 1 && ath.BoutonLancer.EstActif)
            {
                NetworkClient client = Game.Components.ToList().Find(item => item is NetworkClient) as NetworkClient;
                client.EnvoyerInfoLancerBalle(infoLancer.Force, infoLancer.InfoAngleHor, infoLancer.InfoAngleVert);
            }
        }
        void LancerBalle()
        {
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
        }
        void EnleverVerres()
        {
            if (Balle.EstJoueurPrincipal)
            {
                Game.Components.Remove(ListeBiereAdv[Balle.IndexÀRetirer]);
                Game.Components.Remove(VerresAdversaire[Balle.IndexÀRetirer]);
                ListeBiereAdv.RemoveAt(Balle.IndexÀRetirer);
                VerresAdversaire.RemoveAt(Balle.IndexÀRetirer);
                ListePositionVerresAdv.RemoveAt(Balle.IndexÀRetirer);
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
                Game.Components.Remove(ListeBiereJoueur[Balle.IndexÀRetirer]);
                Game.Components.Remove(VerresJoueur[Balle.IndexÀRetirer]);
                ListeBiereJoueur.RemoveAt(Balle.IndexÀRetirer);
                VerresJoueur.RemoveAt(Balle.IndexÀRetirer);
                ListePositionVerres.RemoveAt(Balle.IndexÀRetirer);
                if (Balle.RebondSurTable && ListeBiereJoueur.Count >= 1)
                {
                    Game.Components.Remove(ListeBiereJoueur[0]);
                    Game.Components.Remove(VerresJoueur[0]);
                    ListeBiereJoueur.RemoveAt(0);
                    VerresJoueur.RemoveAt(0);
                    ListePositionVerres.RemoveAt(0);
                }
            }
        }
        void ChangerAnimationPersonnage(TypeActionPersonnage action)
        {
            Joueur joueur = new Joueur(Game);
            List<Personnage> ListePerso = new List<Personnage>();
            foreach (Personnage perso in Game.Components.Where(person => person is Personnage))
            {
                ListePerso.Add(perso);
            }
            if (gestionEnviro.CaméraJeu.Position.Z > 0)
            {
                joueur.ChangerAnimation(action, ListePerso.Find(peros => peros.Position.Z < 0));
            }
            else
            {
                joueur.ChangerAnimation(action, ListePerso.Find(peros => peros.Position.Z > 0));
            }
        }
        #endregion
    }
}
