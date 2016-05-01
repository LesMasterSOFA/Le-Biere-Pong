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
    public enum ModeDifficulté { Facile, Moyen, Difficile}
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class AI
    {
        const float VITESSE_FACILE = 280f;
        const float VITESSE_MOYEN = 250f;
        const float VITESSE_DIFFICILE = 370;

        const float ANGLE_HORIZONTAL_FACILE = 10;
        const float ANGLE_HORIZONTAL_MOYEN = 7;
        const float ANGLE_HORIZONTAL_DIFFICILE = 300;

        const float ANGLE_VERTICAL_FACILE = 50;
        const float ANGLE_VERTICAL_MOYEN = 40;
        const float ANGLE_VERTICAL_DIFFICILE = 30;

        public float Vitesse { get; set; }
        public float AngleHorizontal { get; set; }
        public float AngleVertical { get; set; }
        public int Cpt { get; set; }

        ModeDifficulté Difficulté { get; set; }

        public AI(ModeDifficulté difficulté)
        {
            Difficulté = difficulté;
            InstancerDifficulté(Difficulté);
        }

        void InstancerDifficulté(ModeDifficulté Mode)
        {
            switch (Mode)
            {

                case ModeDifficulté.Facile:
                    Vitesse = VITESSE_FACILE;
                    AngleHorizontal = ANGLE_HORIZONTAL_FACILE;
                    AngleVertical = ANGLE_VERTICAL_FACILE;
                    break;
                case ModeDifficulté.Moyen:
                    Vitesse = VITESSE_MOYEN;
                    AngleHorizontal = ANGLE_HORIZONTAL_MOYEN;
                    AngleVertical = ANGLE_VERTICAL_MOYEN;
                    break;
                case ModeDifficulté.Difficile:
                    Vitesse = VITESSE_DIFFICILE;
                    AngleHorizontal = ANGLE_HORIZONTAL_DIFFICILE;
                    AngleVertical = ANGLE_VERTICAL_DIFFICILE;
                    break;
                default:
                    throw new Exception();
            }
        }
        public float[] GérerAI()
        {
            float[] tableau = new float[3];

            bool estPositif = false;
            //Random randPositif = new Random();
            //int prob = randPositif.Next(100);
            //if (prob > 50)
            //    estPositif = true;
            if (Cpt % 2 == 0)
            {
               estPositif = true;
            }
            Cpt++;

            Random vitesseRand = new Random();
            tableau[0] = vitesseRand.Next(330, (int)Vitesse + 1);
            tableau[0] = tableau[0] / 100;
             

            Random angleHorizontalRand = new Random();
            tableau[1] = angleHorizontalRand.Next(0, (int)AngleHorizontal + 1);
            if (!estPositif)
                tableau[1] = -tableau[1];
            tableau[1] = tableau[1] / 100;
            if (Cpt % 3 == 0)
               tableau[1] = 0;


            Random angleVerticalRand = new Random();
            tableau[2] = angleVerticalRand.Next(30, (int)AngleVertical + 1);
            //if (!estPositif)
            //    tableau[2] = -tableau[2];
            return tableau;
        }
    }
}
