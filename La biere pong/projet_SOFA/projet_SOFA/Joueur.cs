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
    public enum TypeActionPersonnage
    {
        Rien, Boire, Lancer, ApresLancer, ApresBoire
    }


    public class Joueur : Microsoft.Xna.Framework.GameComponent, IActivable
    {
        public Personnage Avatar { get; protected set; }
        public string GamerTag { get; protected set; }
        public Texture2D ImageJoueur { get; protected set; }
        public GestionPartie GestionnaireDeLaPartie { get; protected set; }
        public InputManager GestionnaireInput { get; protected set; }
        public bool EstActif { get; protected set; }
        public Viewport ÉcranDeJeu { get; protected set; } //Défini s'il est sur l'écran totale ou une partie
        public ATH ath { get; protected set; }
        
        public Joueur(Game game, Personnage avatar, Texture2D imageJoueur, GestionPartie gestionnaireDeLaPartie, Viewport écranDeJeu, string gamerTag)
            :base(game)
        {
            Avatar = avatar;
            ImageJoueur = imageJoueur;
            GestionnaireDeLaPartie = gestionnaireDeLaPartie;
            ÉcranDeJeu = écranDeJeu;
            GamerTag = gamerTag;
            EstActif = true; //active le joueur
        }

        public Joueur(Game game, GestionPartie gestionnairePartie, Viewport écranDeJeu)
        :base(game)
        {
            GestionnaireDeLaPartie = gestionnairePartie;
            ÉcranDeJeu = écranDeJeu;
        }
        //Temporaire
        public Joueur(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            GestionnaireInput = new InputManager(this.Game);
            //ath = new ATH(Game, this);
            //Game.Components.Add(ath);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public void ModifierActivation()
        {
            EstActif = !EstActif;
        }
        public void ChangerAnimation(TypeActionPersonnage typeAnimation, Joueur joueur)
        {
            Vector3 positionJoueur = new Vector3();
            Vector3 rotationJoueur = new Vector3();
            Vector3 positionAdv = new Vector3();
            Vector3 rotationAdv = new Vector3();

            string action = "";
            List<Personnage> liste = new List<Personnage>();
            foreach (Personnage perso in Game.Components.Where(perso => perso is Personnage))
            {
                liste.Add(perso);
            }

            foreach (Personnage perso in liste)
            {
                positionJoueur = liste[0].Position;
                rotationJoueur = liste[0].Rotation;
                if (liste.Count == 2)
                {
                    positionAdv = liste[1].Position;
                    rotationAdv = liste[1].Rotation;
                }

                Game.Components.Remove(perso);
            }

            switch (typeAnimation)
            {
                case TypeActionPersonnage.Rien:
                    action = "superBoy";
                    break;
                case TypeActionPersonnage.Boire:
                    action = "superBoyBoire";
                    break;
                case TypeActionPersonnage.Lancer:
                    action = "superBoyLancer";
                    break;
                case TypeActionPersonnage.ApresLancer:
                    action = "superBoyApresLancer";
                    break;
                case TypeActionPersonnage.ApresBoire:
                    action = "superBoyApresBoire";
                    break;

            }
            if (this != joueur)
            {
                Game.Components.Insert(17, new Personnage(Game, action, "superBoyTex", "Shader", 1, rotationJoueur, positionJoueur));//principal
                Game.Components.Insert(17, new Personnage(Game, "superBoy", "superBoyTex", "Shader", 1, rotationAdv, positionAdv));//secondaire
            }

            else
            {
                Game.Components.Insert(17, new Personnage(Game, action, "superBoyTex", "Shader", 1, rotationAdv, positionAdv));//secodaire
                Game.Components.Insert(17, new Personnage(Game, "superBoy", "superBoyTex", "Shader", 1, rotationJoueur, positionJoueur));//principal
            }
                
        }
    }
}
