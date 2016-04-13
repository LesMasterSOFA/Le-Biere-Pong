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
        static GestionÉvénements()
        {
        }
        
        //J'ai fait des fonctions mais je savais pas ou les appeler alors il n'y a pas de references


        //public static void GérerMode1v1Local(Game jeu, bool estTourJoueur, GestionEnvironnement environnement, ATH ath,
        //    List<VerreAdversaire> listeVerresAdv, List<VerreJoueurPrincipal> listeVerresJoueur)
        //{
        //    const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        //    bool estPartieActive = true;
        //    estTourJoueur = !estTourJoueur;

        //    if (estTourJoueur)
        //    {
        //        //Caméra sur joueur
        //        Vector3 positionCaméra = new Vector3(0, 1.5f, 2f);
        //        Vector3 cibleCaméra = new Vector3(0, 1f, 0);
        //        environnement.CaméraJeu = new CaméraJoueur(jeu, positionCaméra, cibleCaméra, Vector3.Up, INTERVALLE_MAJ_STANDARD);

        //        // Activer lancer
        //        ath.BoutonLancer.Enabled = true;

        //        if (estLancerTerminé)
        //        {
        //            //Vérifier si lancer est bon, comment?
        //            VerreAdversaire leVerre = listeVerresAdv[0]; // trouver quel est leVerre qui est rentré.
        //            bool estBalleDansVerre = VérifierRésultatLancer();
        //            bool estBalleRebond = false; //Frank va faire une fonction pour déterminer si c'est un rebond.
        //            EnleverVerres(listeVerresAdv, jeu, leVerre, estBalleDansVerre, estBalleRebond);

        //            //Vérifier si la partie est finie
        //            VérifierNombreDeVerres(listeVerresAdv);
        //        }
        //    }
        //    else
        //    {
        //        //Caméra sur adversaire
        //        Vector3 positionCaméra = new Vector3(0, 1.5f, -2f);
        //        Vector3 cibleCaméra = new Vector3(0, 1f, 0);
        //        environnement.CaméraJeu = new CaméraJoueur(jeu, positionCaméra, cibleCaméra, Vector3.Up, INTERVALLE_MAJ_STANDARD);

        //        // Activer lancer
        //        ath.BoutonLancer.Enabled = true;

        //        if (estLancerTerminé)
        //        {
        //            //Vérifier si lancer est bon, comment?
        //            VerreJoueurPrincipal leVerre = listeVerresJoueur[0]; // trouver quel est leVerre qui est rentré.
        //            bool estBalleDansVerre = VérifierRésultatLancer();
        //            bool estBalleRebond = false; //Frank va faire une fonction pour déterminer si c'est un rebond.
        //            EnleverVerres(listeVerresJoueur, jeu, leVerre, estBalleDansVerre, estBalleRebond);

        //            //Vérifier si la partie est finie
        //            VérifierNombreDeVerres(listeVerresJoueur);
        //        }

        //    }

        //    TerminerPartie();
        //}





        public static void EnleverVerres<T>(List<T> listeDeVerres, Game jeu, T verreCible, bool estBalleDansVerre, bool estBalleRebond)
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
                //if (estTrickShot)
                //{
                //    T verreRand1 = listeDeVerres[randGen.Next(listeDeVerres.Count)];
                //    GameComponent verreRandComponent = verreRand1 as GameComponent;
                //    listeDeVerres.Remove(verreRand1);
                //    jeu.Components.Remove(verreRandComponent);
                //}
            }
        }
      
    }
}
