﻿using System;
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
    public class CaméraJoueur : Caméra
    {
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        const float VITESSE_INITIALE_ROTATION = 0.1f;
        const float DELTA_LACET = MathHelper.Pi / 180; // 1 degré à la fois
        const float DELTA_TANGAGE = MathHelper.Pi / 180; // 1 degré à la fois
        const float RAYON_COLLISION = 1f;


        Vector3 Direction { get; set; }
        Vector3 Latéral { get; set; }
        float VitesseRotationDroite { get; set; }
        float VitesseRotationGauche { get; set; }

        float VitesseRotationHaut { get; set; }
        float VitesseRotationBas { get; set; }

        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        InputManager GestionInput { get; set; }

   

        public CaméraJoueur(Game jeu, Vector3 positionCaméra, Vector3 cible, Vector3 orientation, float intervalleMAJ)
            : base(jeu)
        {
            IntervalleMAJ = intervalleMAJ;
            CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
            CréerPointDeVue(positionCaméra, cible, orientation);
        }

        public override void Initialize()
        {
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
                GérerRotation();
                CréerPointDeVue();
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



            if (Vue.Forward.Y <= 0.1f)
            {
                VitesseRotationHaut = 0f;
            }
            else if (Vue.Forward.Y >= 0.3f)
            {
                VitesseRotationBas = 0f;
            }
            else
            {
                VitesseRotationBas = VITESSE_INITIALE_ROTATION;
                VitesseRotationHaut = VITESSE_INITIALE_ROTATION;
            }
            Direction = Vector3.Transform(Direction, matriceTangage);
            Direction = Vector3.Normalize(Direction);
            OrientationVerticale = Vector3.Transform(OrientationVerticale, matriceTangage);
            OrientationVerticale = Vector3.Normalize(OrientationVerticale);
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



            if (Vue.Forward.X <= -0.085f)
            {
                VitesseRotationDroite = 0f;
            }
            else if(Vue.Forward.X >= 0.085f)
            {
                VitesseRotationGauche = 0f;
            }
            else
            {
                VitesseRotationGauche = VITESSE_INITIALE_ROTATION;
                VitesseRotationDroite = VITESSE_INITIALE_ROTATION;
            }
            Direction = Vector3.Transform(Direction, matriceLacet);
            Direction = Vector3.Normalize(Direction);
            Latéral = Vector3.Cross(Direction, OrientationVerticale);
            Latéral = Vector3.Normalize(Latéral);
        }
    }
}
