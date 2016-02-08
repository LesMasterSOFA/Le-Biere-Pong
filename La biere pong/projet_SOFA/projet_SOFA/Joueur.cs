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
        Personnage Avatar { get; set; } 
        string GamerTag { get; set; } //nom du joueur
        Texture2D ImageJoueur { get; set; }
        //List<GameComponent> ListeComponentsJoueurPartie { get; set; } //liste des objets du jeu //GestionPartie s'en occupe ? 
        GestionPartie GestionnaireDeLaPartie { get; set; }
        InputManager GestionnaireInput { get; set; }
        string IP { get; set; }
        bool EstActif { get; set; }

        //Constructeur normal
        public Joueur(Game game, Personnage avatar, Texture2D imageJoueur, GestionPartie gestionnaireDeLaPartie, string ip, string gamerTag)
            : base(game)
        {
            Avatar = avatar;
            ImageJoueur = imageJoueur;
            //ListeComponentsJoueurPartie = listeComponentsJoueurPartie;
            GestionnaireDeLaPartie = gestionnaireDeLaPartie;
            IP = ip;
            GamerTag = gamerTag;

        }

        //Temporaire en attendant que personnage soit créé
        public Joueur(Game game, GestionPartie gestionnaireDeLaPartie, string ip, string gamerTag)
            : base(game)
        {
            //ListeComponentsJoueurPartie = listeComponentsJoueurPartie;
            GestionnaireDeLaPartie = gestionnaireDeLaPartie;
            IP = ip;
            GamerTag = gamerTag;
        }


        public override void Initialize()
        {
            GestionnaireInput = new InputManager(this.Game);

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
