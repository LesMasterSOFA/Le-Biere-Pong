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
    public abstract class Caméra : Microsoft.Xna.Framework.GameComponent
    {
        protected const float OUVERTURE_OBJECTIF = MathHelper.PiOver4;
        protected const float DISTANCE_PLAN_RAPPROCHÉ = 0.01f;
        protected const float DISTANCE_PLAN_ÉLOIGNÉ = 100000f;

        public Matrix Vue { get; protected set; }
        public Matrix Projection { get; protected set; }
        public BoundingFrustum Frustum { get; protected set; }
        public Vector3 Position { get; protected set; }
        public Vector3 Cible { get; protected set; }
        public Vector3 OrientationVerticale { get; protected set; }
        protected float AngleOuvertureObjectif { get; set; }
        protected float AspectRatio { get; set; }
        protected float DistancePlanRapproché { get; set; }
        protected float DistancePlanÉloigné { get; set; }

        public Caméra(Game jeu)
            : base(jeu)
        {
        }

        protected virtual void CréerPointDeVue()
        {
            Vue = Matrix.CreateLookAt(Position, Cible, OrientationVerticale);
        }

        protected virtual void CréerPointDeVue(Vector3 position, Vector3 cible)
        {
            Position = position;
            Cible = cible;
            OrientationVerticale = Vector3.Up;
            CréerPointDeVue();
        }

        protected virtual void CréerPointDeVue(Vector3 position, Vector3 cible, Vector3 orientationVerticale)
        {
            Position = position;
            Cible = cible;
            OrientationVerticale = orientationVerticale;
            CréerPointDeVue();
        }

        private void CréerProjection()
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(AngleOuvertureObjectif, AspectRatio, DistancePlanRapproché, DistancePlanÉloigné);
        }

        protected virtual void CréerVolumeDeVisualisation(float angleOuvertureObjectif, float distancePlanRapproché, float distancePlanÉloigné)
        {
            AngleOuvertureObjectif = angleOuvertureObjectif;
            AspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
            DistancePlanRapproché = distancePlanRapproché;
            DistancePlanÉloigné = distancePlanÉloigné;
            CréerProjection();
        }

        protected virtual void CréerVolumeDeVisualisation(float angleOuvertureObjectif, float aspectRatio, float distancePlanRapproché, float distancePlanÉloigné)
        {
            AngleOuvertureObjectif = angleOuvertureObjectif;
            AspectRatio = aspectRatio;
            DistancePlanRapproché = distancePlanRapproché;
            DistancePlanÉloigné = distancePlanÉloigné;
            CréerProjection();
        }

        protected void GénérerFrustum()
        {
            Frustum = new BoundingFrustum(Vue * Projection);
        }

        public virtual void Déplacer(Vector3 position, Vector3 cible, Vector3 orientationVerticale)
        {
            CréerPointDeVue(position, cible, orientationVerticale);
        }
    }
}
