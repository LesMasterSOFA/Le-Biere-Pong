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

    public class Joueur : Microsoft.Xna.Framework.GameComponent, IActivable
    {
        public Personnage Avatar { get; protected set; }
        public string GamerTag { get; protected set; }
        public Texture2D ImageJoueur { get; protected set; }
        public GestionPartie GestionnaireDeLaPartie { get; protected set; }
        public InputManager GestionnaireInput { get; protected set; }
        public bool EstActif { get; protected set; }
        public Viewport �cranDeJeu { get; protected set; } //D�fini s'il est sur l'�cran totale ou une partie
        public ATH ath { get; protected set; }
        
        public Joueur(Game game, Personnage avatar, Texture2D imageJoueur, GestionPartie gestionnaireDeLaPartie, Viewport �cranDeJeu, string gamerTag)
            :base(game)
        {
            Avatar = avatar;
            ImageJoueur = imageJoueur;
            GestionnaireDeLaPartie = gestionnaireDeLaPartie;
            �cranDeJeu = �cranDeJeu;
            GamerTag = gamerTag;
            EstActif = true; //active le joueur
        }

        public Joueur(Game game, GestionPartie gestionnairePartie, Viewport �cranDeJeu)
        :base(game)
        {
            GestionnaireDeLaPartie = gestionnairePartie;
            �cranDeJeu = �cranDeJeu;
        }
        //Temporaire
        public Joueur(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            GestionnaireInput = new InputManager(this.Game);
            ath = new ATH(Game);
            Game.Components.Add(ath);
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
    }
}
