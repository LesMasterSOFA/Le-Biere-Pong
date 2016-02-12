using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtelierXNA
{
    public abstract class PrimitiveDeBaseAnimée : PrimitiveDeBase
    {
        private float lacet;
        private float tangage;
        private float roulis;

        private float Homothétie { get; set; }
        private Vector3 Position { get; set; }
        private float IntervalleMAJ { get; set; }
        protected InputManager GestionInput { get; private set; }
        private float TempsÉcouléDepuisMAJ { get; set; }
        private float IncrémentAngleRotation { get; set; }
        private bool Lacet { get; set; }
        private bool Tangage { get; set; }
        private bool Roulis { get; set; }
        protected bool MondeÀRecalculer { get; set; }
        protected float AngleLacet
        {
            get
            {
                if (Lacet)
                {
                    lacet += IncrémentAngleRotation;
                    double num = (double)MathHelper.WrapAngle(lacet);
                }
                return this.lacet;
            }
            set
            {
                lacet = value;
            }
        }

        protected float AngleTangage
        {
            get
            {
                if (Tangage)
                {
                    tangage += IncrémentAngleRotation;
                    double num = (double)MathHelper.WrapAngle(tangage);
                }
                return tangage;
            }
            set
            {
                tangage = value;
            }
        }

        protected float AngleRoulis
        {
            get
            {
                if (Roulis)
                {
                    this.roulis += IncrémentAngleRotation;
                    double num = (double)MathHelper.WrapAngle(roulis);
                }
                return roulis;
            }
            set
            {
                roulis = value;
            }
        }

        protected PrimitiveDeBaseAnimée(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, float intervalleMAJ)
            : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale)
        {
            this.IntervalleMAJ = intervalleMAJ;
        }

        public override void Initialize()
        {
            Homothétie = HomothétieInitiale;
            GérerRotation();
            Position = PositionInitiale;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            IncrémentAngleRotation = (float)(MathHelper.Pi * IntervalleMAJ / 2);
            TempsÉcouléDepuisMAJ = 0;
            base.Initialize();
        }

        protected override void CalculerMatriceMonde()
        {
            Monde = Matrix.Identity * Matrix.CreateScale(Homothétie) * Matrix.CreateFromYawPitchRoll(AngleLacet, AngleTangage, AngleRoulis) * Matrix.CreateTranslation(Position);
        }

        public override void Update(GameTime gameTime)
        {
            GérerClavier();
            TempsÉcouléDepuisMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                EffectuerMiseÀJour();
                TempsÉcouléDepuisMAJ -= IntervalleMAJ;
            }
            base.Update(gameTime);
        }

        protected virtual void EffectuerMiseÀJour()
        {
            if (MondeÀRecalculer)
            {
                CalculerMatriceMonde();
                MondeÀRecalculer = false;
            }
        }

        private void GérerRotation()
        {
            AngleLacet = RotationInitiale.Y;
            AngleTangage = RotationInitiale.X;
            AngleRoulis = RotationInitiale.Z;
        }

        protected virtual void GérerClavier()
        {
            if (GestionInput.EstEnfoncée(Keys.LeftControl) || GestionInput.EstEnfoncée(Keys.RightControl))
            {
                if (GestionInput.EstNouvelleTouche(Keys.Space))
                {
                    GérerRotation();
                    MondeÀRecalculer = true;
                }
                if (GestionInput.EstNouvelleTouche(Keys.D1) || GestionInput.EstNouvelleTouche(Keys.NumPad1))
                    Lacet = !Lacet;
                if (GestionInput.EstNouvelleTouche(Keys.D2) || GestionInput.EstNouvelleTouche(Keys.NumPad2))
                    Tangage = !Tangage;
                if (GestionInput.EstNouvelleTouche(Keys.D3) || GestionInput.EstNouvelleTouche(Keys.NumPad3))
                    Roulis = !Roulis;
            }
            MondeÀRecalculer = MondeÀRecalculer || Lacet || Tangage || Roulis;
        }
    }
}
