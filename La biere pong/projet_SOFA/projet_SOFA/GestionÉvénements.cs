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
    static public class GestionÉvénements
    {
        static Random randGen { get; set; }
        static List<VerreJoueurPrincipal> ListeVerresJoueur { get; set; }
        static List<VerreAdversaire> ListeVerresAdversaire { get; set; }
        static GestionÉvénements()
        {
        }

        //J'ai fait des fonctions mais je savais pas ou les appeler alors il n'y a pas de references

        public static void EnleverVerres<T>(List<T> listeDeVerres,Game jeu, T verreCible, bool estBalleDansVerre, bool estBalleRebond, bool estTrickShot)
        {
            GameComponent verreComponent = verreCible as GameComponent;
            randGen = new Random();
            if (estBalleDansVerre)
            {
                listeDeVerres.Remove(verreCible);
                jeu.Components.Remove(verreComponent);
                if (estBalleRebond)
                {
                    T verreRand1 = listeDeVerres[randGen.Next(listeDeVerres.Count)];
                    GameComponent verreRandComponent = verreRand1 as GameComponent;
                    listeDeVerres.Remove(verreRand1);
                    jeu.Components.Remove(verreRandComponent);
                }
                if (estTrickShot)
                {
                    T verreRand1 = listeDeVerres[randGen.Next(listeDeVerres.Count)];
                    GameComponent verreRandComponent = verreRand1 as GameComponent;
                    listeDeVerres.Remove(verreRand1);
                    jeu.Components.Remove(verreRandComponent);
                }
            }
        }
    }
}
