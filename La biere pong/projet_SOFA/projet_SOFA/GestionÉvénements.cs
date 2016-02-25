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

        static void VérifierLancerAdversaire(Game game, VerreJoueurPrincipal verreCible, bool estBalleDansVerre, bool estBalleRebond, bool estTrickShot)
        {
            randGen = new Random();
            foreach (VerreJoueurPrincipal verre in game.Components)
            {
                ListeVerresJoueur.Add(verre);
            }
            EnleverVerres<VerreJoueurPrincipal>(ListeVerresJoueur,game, verreCible, estBalleDansVerre, estBalleRebond, estTrickShot);
        }
        static void EnleverVerres<T>(List<T> listeDeVerres,Game jeu, T verreCible, bool estBalleDansVerre, bool estBalleRebond, bool estTrickShot)
        {
            if (estBalleDansVerre)
            {
                listeDeVerres.Remove(verreCible);
                if (estBalleRebond)
                {
                    listeDeVerres.Remove(listeDeVerres[randGen.Next(listeDeVerres.Count)]);
                }
                if (estTrickShot)
                {
                    listeDeVerres.Remove(listeDeVerres[randGen.Next(listeDeVerres.Count)]);
                }
            }
            foreach (T verre in listeDeVerres)
            {
                if (jeu.Components.Contains(verre) && !listeDeVerres.Contains(verre))
                {
                    jeu.Components.Remove(verre);
                }
            }
        }
    }
}
