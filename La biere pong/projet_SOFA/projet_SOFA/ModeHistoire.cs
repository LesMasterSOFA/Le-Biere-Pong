using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;


namespace AtelierXNA
{

    public class ModeHistoire : PartieSolo
    {
        const int MARGE_BOUTONS = 60;
        BoutonDeCommande BoutonTest { get; set; }
        Vector2 PositionTest { get; set; }
        BoutonDeCommande BoutonTest2 { get; set; }
        Vector2 PositionTest2 { get; set; }
        int ChangerNiveau { get; set; }
        EnrivonnementDeBase Environnement { get; set; }
        Menu Menu { get; set; }
        public ModeHistoire(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            ChangerNiveau = 0;
            EnvironnementPartie = new GestionEnvironnement(Game, Environnements.Garage, SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex.ToString(), SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex.ToString(), TypePartie.Histoire);
            ModifierActivation();
            if (EstPartieActive)
            {
                Game.Components.Add(EnvironnementPartie);
            }
            Game.Components.Remove(this);
            Game.Components.Add(new GestionPartie(Game));
            MediaPlayer.Stop();
            //PositionTest = new Vector2(MARGE_BOUTONS + 100, Game.Window.ClientBounds.Height - MARGE_BOUTONS + 20);
            //BoutonTest = new BoutonDeCommande(Game, "Next Level", "Impact20", "BoutonBleu", "BoutonBleuPale", PositionTest, true, SecondNiveau);
            //PositionTest2 = new Vector2(MARGE_BOUTONS + 100, Game.Window.ClientBounds.Height - MARGE_BOUTONS + 20);
            //BoutonTest2 = new BoutonDeCommande(Game, "Next Level", "Impact20", "BoutonBleu", "BoutonBleuPale", PositionTest, true, TroisièmeNiveau);
            //Game.Components.Add(BoutonTest);
            Game.Components.Add(new ATH(Game, JoueurPrincipal));
            base.Initialize();
        }
        void ChangerDeNiveau()
        {
           if (Game.Components.Where(item => item is EnrivonnementDeBase).Count() == 1)
           {
              foreach (EnrivonnementDeBase env in Game.Components.Where(item => item is EnrivonnementDeBase))
              {
                 Environnement = env;
              }
           }
           if (Environnement.VerresJoueur.Count == 0 || Environnement.VerresAdversaire.Count == 0)
           {
              ChangerNiveau += 1;
              if (ChangerNiveau == 1)
              {
                 SecondNiveau();
                 
              }
              else if (ChangerNiveau == 2)
              {
                 TroisièmeNiveau();
              }
              else if (ChangerNiveau == 3)
              {
                 RetourAuMenu();
              }

           }
        }
        void TroisièmeNiveau()
        {
            for(int i = 3; i<Game.Components.Count;i++)
            {
                Game.Components.RemoveAt(i);
                i--;
            }
            Game.Services.RemoveService(typeof(Caméra));
            EnvironnementPartie = new GestionEnvironnement(Game, Environnements.SalleManger, SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex.ToString(), SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex.ToString(), TypePartie.Histoire);
            Game.Components.Add(EnvironnementPartie);
            //Game.Components.Add(BoutonTest2);
            Game.Components.Add(new ATH(Game, JoueurPrincipal));
        }
        void SecondNiveau()
        {
            for (int i = 3; i < Game.Components.Count; i++)
            {
                Game.Components.RemoveAt(i);
                i--;
            }
            Game.Components.Remove(ath);
            Game.Services.RemoveService(typeof(Caméra));
            EnvironnementPartie = new GestionEnvironnement(Game, Environnements.SousSol, SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex.ToString(), SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex.ToString(), TypePartie.Histoire);
            Game.Components.Add(EnvironnementPartie);
            Game.Components.Add(new ATH(Game, JoueurPrincipal));
            
        }
        void RetourAuMenu()
        {
           for (int i = 3; i < Game.Components.Count; i++)
           {
              Game.Components.RemoveAt(i);
              i--;
           }
           Game.Components.Remove(ath);
           Game.Services.RemoveService(typeof(Caméra));
           Menu = new Menu(Game);
           Game.Components.Add(Menu);
        }

        #region Fonction Saoul
        //Fonction inutilisable, doit savoir tour a qui (gestion événement)

        void SaoulEnCrissGarage()
        {
           EnvironnementGarage Garage;
           if (Game.Components.Where(item => item is EnvironnementGarage).Count() == 1)
           {
              foreach (EnvironnementGarage GaragePT in Game.Components.Where(item => item is EnvironnementGarage))
              {
                 Garage = GaragePT;
                 Garage.VerresJoueur.Count();

                 if (EstTourJoueur)
                 {
                    while (EstTourJoueur)
                    {
                       EnvironnementPartie.CaméraJeu.Déplacer(EnvironnementPartie.CaméraJeu.Position, new Vector3(EnvironnementPartie.CaméraJeu.Cible.X + (float)Math.Sin(60 * (6 - 3)), EnvironnementPartie.CaméraJeu.Cible.Y, EnvironnementPartie.CaméraJeu.Cible.Z), EnvironnementPartie.CaméraJeu.OrientationVerticale);
                       EnvironnementPartie.CaméraJeu.Déplacer(EnvironnementPartie.CaméraJeu.Position, new Vector3(EnvironnementPartie.CaméraJeu.Cible.X, EnvironnementPartie.CaméraJeu.Cible.Y + (float)Math.Cos(60 * (6 - 3)), EnvironnementPartie.CaméraJeu.Cible.Z), EnvironnementPartie.CaméraJeu.OrientationVerticale);
                    }
                 }
                 if (EstTourAdversaire)
                 {
                    while (EstTourAdversaire)
                    {
                       EnvironnementPartie.CaméraJeu.Déplacer(EnvironnementPartie.CaméraJeu.Position, new Vector3(EnvironnementPartie.CaméraJeu.Cible.X + (float)Math.Sin(60 * (6 - 3)), EnvironnementPartie.CaméraJeu.Cible.Y, EnvironnementPartie.CaméraJeu.Cible.Z), EnvironnementPartie.CaméraJeu.OrientationVerticale);
                       EnvironnementPartie.CaméraJeu.Déplacer(EnvironnementPartie.CaméraJeu.Position, new Vector3(EnvironnementPartie.CaméraJeu.Cible.X, EnvironnementPartie.CaméraJeu.Cible.Y + (float)Math.Cos(60 * (6 - 3)), EnvironnementPartie.CaméraJeu.Cible.Z), EnvironnementPartie.CaméraJeu.OrientationVerticale);
                    }
                 }

              }

           }

        }
        void SaoulEnCrissSalleManger()
        {
           EnvironnementSalleManger SalleManger;
           if (Game.Components.Where(item => item is EnvironnementSalleManger).Count() == 1)
           {
              foreach (EnvironnementSalleManger SalleMangerPT in Game.Components.Where(item => item is EnvironnementSalleManger))
              {
                 SalleManger = SalleMangerPT;
                 SalleManger.VerresJoueur.Count();

                 if (EstTourJoueur)
                 {
                    while (EstTourJoueur)
                    {
                       EnvironnementPartie.CaméraJeu.Déplacer(EnvironnementPartie.CaméraJeu.Position, new Vector3(EnvironnementPartie.CaméraJeu.Cible.X + (float)Math.Sin(60 * (6 - 3)), EnvironnementPartie.CaméraJeu.Cible.Y, EnvironnementPartie.CaméraJeu.Cible.Z), EnvironnementPartie.CaméraJeu.OrientationVerticale);
                       EnvironnementPartie.CaméraJeu.Déplacer(EnvironnementPartie.CaméraJeu.Position, new Vector3(EnvironnementPartie.CaméraJeu.Cible.X, EnvironnementPartie.CaméraJeu.Cible.Y + (float)Math.Cos(60 * (6 - 3)), EnvironnementPartie.CaméraJeu.Cible.Z), EnvironnementPartie.CaméraJeu.OrientationVerticale);
                    }
                 }
                 if (EstTourAdversaire)
                 {
                    while (EstTourAdversaire)
                    {
                       EnvironnementPartie.CaméraJeu.Déplacer(EnvironnementPartie.CaméraJeu.Position, new Vector3(EnvironnementPartie.CaméraJeu.Cible.X + (float)Math.Sin(60 * (6 - 3)), EnvironnementPartie.CaméraJeu.Cible.Y, EnvironnementPartie.CaméraJeu.Cible.Z), EnvironnementPartie.CaméraJeu.OrientationVerticale);
                       EnvironnementPartie.CaméraJeu.Déplacer(EnvironnementPartie.CaméraJeu.Position, new Vector3(EnvironnementPartie.CaméraJeu.Cible.X, EnvironnementPartie.CaméraJeu.Cible.Y + (float)Math.Cos(60 * (6 - 3)), EnvironnementPartie.CaméraJeu.Cible.Z), EnvironnementPartie.CaméraJeu.OrientationVerticale);
                    }
                 }
              }
           }
        }
        void SaoulEnCrissSousSol()
        {
           EnvironnementSousSol SousSol;
           if (Game.Components.Where(item => item is EnvironnementSousSol).Count() == 1)
           {
              foreach (EnvironnementSousSol SousSolPT in Game.Components.Where(item => item is EnvironnementSousSol))
              {
                 SousSol = SousSolPT;
                 SousSol.VerresJoueur.Count();

                 if (EstTourJoueur)
                 {
                    while (EstTourJoueur)
                    {
                       EnvironnementPartie.CaméraJeu.Déplacer(EnvironnementPartie.CaméraJeu.Position, new Vector3(EnvironnementPartie.CaméraJeu.Cible.X + (float)Math.Sin(60 * (6 - 3)), EnvironnementPartie.CaméraJeu.Cible.Y, EnvironnementPartie.CaméraJeu.Cible.Z), EnvironnementPartie.CaméraJeu.OrientationVerticale);
                       EnvironnementPartie.CaméraJeu.Déplacer(EnvironnementPartie.CaméraJeu.Position, new Vector3(EnvironnementPartie.CaméraJeu.Cible.X, EnvironnementPartie.CaméraJeu.Cible.Y + (float)Math.Cos(60 * (6 - 3)), EnvironnementPartie.CaméraJeu.Cible.Z), EnvironnementPartie.CaméraJeu.OrientationVerticale);
                    }
                 }
                 if (EstTourAdversaire)
                 {
                    while (EstTourAdversaire)
                    {
                       EnvironnementPartie.CaméraJeu.Déplacer(EnvironnementPartie.CaméraJeu.Position, new Vector3(EnvironnementPartie.CaméraJeu.Cible.X + (float)Math.Sin(60 * (6 - 3)), EnvironnementPartie.CaméraJeu.Cible.Y, EnvironnementPartie.CaméraJeu.Cible.Z), EnvironnementPartie.CaméraJeu.OrientationVerticale);
                       EnvironnementPartie.CaméraJeu.Déplacer(EnvironnementPartie.CaméraJeu.Position, new Vector3(EnvironnementPartie.CaméraJeu.Cible.X, EnvironnementPartie.CaméraJeu.Cible.Y + (float)Math.Cos(60 * (6 - 3)), EnvironnementPartie.CaméraJeu.Cible.Z), EnvironnementPartie.CaméraJeu.OrientationVerticale);
                    }
                 }

              }

           }

        }
        #endregion


        public override void Update(GameTime gameTime)
        {
            ChangerDeNiveau();
            base.Update(gameTime);
        }
    }
}
