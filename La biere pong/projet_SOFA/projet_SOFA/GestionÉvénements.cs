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
        static Game Jeu { get; set; }
        static Random randGen { get; set; }
        static Verre VerreCible { get; set; }
        static bool EstBalleDansVerre { get; set; }
        static bool EstBalleRebond { get; set; }
        static bool EstTrickShot { get; set; }
        static List<Verre> ListeVerres { get; set; }
        static public GestionÉvénements(Game game)
            : base(game)
        {
            Jeu = game;
            VerreCible = null;
            EstBalleDansVerre = false;
            EstBalleRebond = false;
            EstTrickShot = false;
            ListeVerres = new List<Verre>();
            foreach (Verre verre in Jeu.Components)
            {
                ListeVerres.Add(verre);
            }
        }
        static public GestionÉvénements(Game game,Verre verreCible, bool estBalleRebond,bool estTrickShot)
            : base(game)
        {
            Jeu = game;
            VerreCible = verreCible;
            EstBalleDansVerre = true;
            EstBalleRebond = estBalleRebond;
            EstTrickShot = estTrickShot
            ListeVerres = new List<Verre>();
            foreach(Verre verre in Jeu.Components)
            {
                ListeVerres.Add(verre);
            }
        }

        //J'ai fait des fonctions mais je savais pas ou les appeler alors il n'y a pas de references

        static void VérifierLancer()
        {
            if (EstBalleDansVerre)
            {
                ListeVerres.Remove(VerreCible);
                if (EstBalleRebond)
                {
                    ListeVerres.Remove(ListeVerres[randGen.Next(ListeVerres.Count)]);
                }
            }
        }
    }
}
