﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtelierXNA
{
    public class CaméraJoueur : Caméra, IActivable
    {
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        const float VITESSE_INITIALE_ROTATION = 0.15f;
        const float DELTA_LACET = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_TANGAGE = MathHelper.Pi / 180; // 1 degré à la fois
        const float RAYON_COLLISION = 1f;
        const float TEMPS_LANCER = 2.71f;
        const float TEMPS_TOURNER = MathHelper.Pi + 2 * TEMPS_LANCER;
        const float MAX_MOUV_HOR = 0.15f;
        const float MIN_MOUV_HOR = -0.15f;
        const float MIN_MOUV_VERT = -MathHelper.Pi / 6;
        const float MAX_MOUV_VERT = 0.5f;

        Vector3 MouvCamLancerJoueur1 = new Vector3(0, 0.005f, 0.007f);
        Vector3 MouvCamLancerJoueur2 = new Vector3(0, 0.005f, -0.007f);
        Vector3 PositionLancerJoueur1 = new Vector3(0, 1.5f, -1.0f);
        Vector3 PositionLancerJoueur2 = new Vector3(0, 1.5f, 1.0f);


        Vector3 Direction { get; set; }
        Vector3 Latéral { get; set; }
        float VitesseRotationDroite { get; set; }
        float VitesseRotationGauche { get; set; }

        float VitesseRotationHaut { get; set; }
        float VitesseRotationBas { get; set; }

        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        InputManager GestionInput { get; set; }

        public bool EstMouvCamActif { get; set; }
        bool RotationAntiHoraire { get; set; }
        public float TempsTotal { get; set; }
        GestionEnvironnement gestionEnviro { get; set; }
        ATH ath { get; set; }


        public CaméraJoueur(Game jeu, Vector3 positionCaméra, Vector3 cible, Vector3 orientation, float intervalleMAJ)
            : base(jeu)
        {
            IntervalleMAJ = intervalleMAJ;
            CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
            CréerPointDeVue(positionCaméra, cible, orientation);
        }

        public override void Initialize()
        {
            gestionEnviro = (Game.Components.ToList().Find(item => item is GestionEnvironnement) as GestionEnvironnement);

            Cible = new Vector3(0, 1f, 0);

            TempsTotal = 0;
            EstMouvCamActif = false;
            RotationAntiHoraire = true;

            VitesseRotationDroite = VITESSE_INITIALE_ROTATION;
            VitesseRotationGauche = VITESSE_INITIALE_ROTATION;
            VitesseRotationBas = VITESSE_INITIALE_ROTATION;
            VitesseRotationHaut = VITESSE_INITIALE_ROTATION;
            TempsÉcouléDepuisMAJ = 0;
            base.Initialize();
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
        }

        protected override void CréerPointDeVue()
        {
            Direction = Vector3.Normalize(Direction);
            OrientationVerticale = Vector3.Normalize(OrientationVerticale);
            Latéral = Vector3.Normalize(Latéral);
            Vue = Matrix.CreateLookAt(Position, Position + Direction, OrientationVerticale);
            GénérerFrustum();
        }

        protected override void CréerPointDeVue(Vector3 position, Vector3 cible, Vector3 orientation)
        {
            Position = position;
            OrientationVerticale = orientation;
            Direction = cible - Position;
            Latéral = Vector3.Cross(Direction, OrientationVerticale);
            OrientationVerticale = -Vector3.Cross(Direction, Latéral);
            CréerPointDeVue();
        }

        public override void Update(GameTime gameTime)
        {
            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                TempsTotal += TempsÉcouléDepuisMAJ;
                GérerRotation();
                CréerPointDeVue();
                MouvJoueur();
                TempsÉcouléDepuisMAJ = 0;
            }
            base.Update(gameTime);
        }

        private int GérerTouche(Keys touche)
        {
            return GestionInput.EstEnfoncée(touche) ? 1 : 0;
        }


        private void GérerRotation()
        {
            if (GestionInput.EstEnfoncée(Keys.Right) || GestionInput.EstEnfoncée(Keys.Left))
            {
                GérerLacet();
            }
            if (GestionInput.EstEnfoncée(Keys.Up) || GestionInput.EstEnfoncée(Keys.Down))
            {
                GérerTangage();
            }
        }
        private void GérerTangage()
        {
            Matrix matriceTangage;
            if (GestionInput.EstEnfoncée(Keys.Up))
            {
                matriceTangage = Matrix.CreateFromAxisAngle(Latéral, DELTA_TANGAGE * VitesseRotationHaut);
            }
            else
            {
                matriceTangage = Matrix.CreateFromAxisAngle(Latéral, -DELTA_TANGAGE * VitesseRotationBas);
            }
            if (Position.Z > 0)//lorsque c'est joueur principal
            {
                if (Vue.Forward.Y <= MIN_MOUV_VERT)
                {
                    VitesseRotationHaut = 0f; //bloque mouvement vertical
                }
                else if (Vue.Forward.Y >= MAX_MOUV_VERT)
                {
                    VitesseRotationBas = 0f; //bloque mouvement vertical
                }
                else
                {
                    VitesseRotationBas = VITESSE_INITIALE_ROTATION;
                    VitesseRotationHaut = VITESSE_INITIALE_ROTATION;
                }
            }
            else
            {

                if (Vue.Forward.Y >= -MIN_MOUV_VERT)
                {
                    VitesseRotationHaut = 0f; //bloque mouvement vertical
                }
                else if (Vue.Forward.Y <= -MAX_MOUV_VERT)
                {
                    VitesseRotationBas = 0f; //bloque mouvement vertical
                }
                else
                {
                    VitesseRotationBas = VITESSE_INITIALE_ROTATION;
                    VitesseRotationHaut = VITESSE_INITIALE_ROTATION;
                }
            }
            Direction = Vector3.Transform(Direction, matriceTangage);
            Direction = Vector3.Normalize(Direction);
            OrientationVerticale = Vector3.Up;
        }
        private void GérerLacet()
        {
            Matrix matriceLacet;
            if (GestionInput.EstEnfoncée(Keys.Right))
            {
                matriceLacet = Matrix.CreateFromAxisAngle(OrientationVerticale, -DELTA_LACET * VitesseRotationDroite);
            }
            else
            {
                matriceLacet = Matrix.CreateFromAxisAngle(OrientationVerticale, DELTA_LACET * VitesseRotationGauche);
            }
            if (Position.Z > 0)//lorsque c'est joueur principal
            {
                if (Vue.Forward.X <= MIN_MOUV_HOR)
                {
                    VitesseRotationDroite = 0f;//bloque mouvement horizontal
                }
                else if (Vue.Forward.X >= MAX_MOUV_HOR)
                {
                    VitesseRotationGauche = 0f;//bloque mouvement horizontal
                }
                else
                {
                    VitesseRotationGauche = VITESSE_INITIALE_ROTATION;
                    VitesseRotationDroite = VITESSE_INITIALE_ROTATION;
                }
            }
            else
            {
                if (Vue.Forward.X >= -MIN_MOUV_HOR)
                {
                    VitesseRotationDroite = 0f;//bloque mouvement horizontal
                }
                else if (Vue.Forward.X <= -MAX_MOUV_HOR)
                {
                    VitesseRotationGauche = 0f;//bloque mouvement horizontal
                }
                else
                {
                    VitesseRotationGauche = VITESSE_INITIALE_ROTATION;
                    VitesseRotationDroite = VITESSE_INITIALE_ROTATION;
                }
            }
            Direction = Vector3.Transform(Direction, matriceLacet);
            Direction = Vector3.Normalize(Direction);
            Latéral = Vector3.Cross(Direction, OrientationVerticale);
            Latéral = Vector3.Normalize(Latéral);
        }

        void MouvJoueur()
        {
            ath = Game.Components.ToList().Find(item => item is ATH) as ATH;

            if (EstMouvCamActif && TempsTotal <= TEMPS_LANCER)//mouvement de reculons avant le lancer
            {
                if (Position.Z > 0)//lorsque c'est joueur principal
                {
                    EffectuerMouvLancerCam(MouvCamLancerJoueur1);
                }
                else
                {
                    EffectuerMouvLancerCam(MouvCamLancerJoueur2);
                    RotationAntiHoraire = false;
                }
            }
            if (EstMouvCamActif && TempsTotal >= 2 * TEMPS_LANCER && TempsTotal <= TEMPS_TOURNER)//mouvement changer de côté
            {
                ath.BoutonLancer.EstActif = false;
                if (RotationAntiHoraire)
                {
                    EffectuerMouvTournerCam(1);
                }
                else
                {
                    EffectuerMouvTournerCam(-1);
                }
            }
            if (EstMouvCamActif && TempsTotal >= TEMPS_TOURNER)//mouvement pour revenir au point de vue pour lancer
            {
                if (Position.Z < 0)
                {
                    EffectuerMouvLancerCam(-MouvCamLancerJoueur2);
                }
                else
                {
                    EffectuerMouvLancerCam(-MouvCamLancerJoueur1);
                }
            }
            if (TempsTotal >= TEMPS_TOURNER + TEMPS_LANCER && EstMouvCamActif)
            {
                EstMouvCamActif = false;
                RotationAntiHoraire = true;
                if (gestionEnviro.TypeDePartie != TypePartie.LAN)
                {
                    ath.BoutonLancer.EstActif = true;
                    ath.EstTourJoueurPrincipal = !ath.EstTourJoueurPrincipal;
                }
                else
                {
                    ath.EstTourJoueurPrincipal = !ath.EstTourJoueurPrincipal;
                    ath.BoutonLancer.EstActif = ath.EstTourJoueurPrincipal;
                }
                if (Position.Z < 0)
                {
                    Déplacer(PositionLancerJoueur1, Cible, Vector3.Up);//pour remettre le point de vue exactement à la même place
                }
                else
                {
                    Déplacer(PositionLancerJoueur2, Cible, Vector3.Up);//pour remettre le point de vue exactement à la même place
                }
            }
        }

        void EffectuerMouvLancerCam(Vector3 mouvement)
        {
            Position += mouvement;
            Déplacer(Position, Cible, Vector3.Up);
        }
        void EffectuerMouvTournerCam(int bord1OuMoins1)
        {
            float rayon = (float)Math.Sqrt(Position.Z * Position.Z + Position.X * Position.X);
            Déplacer(new Vector3(bord1OuMoins1 * rayon * (float)Math.Sin(TempsTotal - 2 * TEMPS_LANCER), Position.Y, bord1OuMoins1 * rayon * (float)Math.Cos(TempsTotal - 2 * TEMPS_LANCER)), Cible, Vector3.Up);
        }

        public void ModifierActivation()
        {
            if ((Game.Components.ToList().Find(item => item is GestionEnvironnement) as GestionEnvironnement).TypeDePartie != TypePartie.LAN)
                Enabled = !Enabled;
        }
    }
}
