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
using Lidgren.Network;


namespace AtelierXNA
{
    public class Joueur : Microsoft.Xna.Framework.GameComponent, IActivable
    {
        Personnage Avatar { get; set; } 
        string GamerTag { get; set; } //nom du joueur
        public Texture2D ImageJoueur { get; set; } //Devrait être private set
        //List<GameComponent> ListeComponentsJoueurPartie { get; set; } //liste des objets du jeu //GestionPartie s'en occupe ? 
        GestionPartie GestionnaireDeLaPartie { get; set; }
        InputManager GestionnaireInput { get; set; }
        public NetConnection IP { get; private set; } 
        bool EstActif { get; set; }
        Viewport ÉcranDeJeu; //Défini s'il est sur l'écran totale ou une partie
   
        //Constructeur normal
        public Joueur(Game game, Personnage avatar, Texture2D imageJoueur, GestionPartie gestionnaireDeLaPartie, Viewport écranDeJeu, NetConnection ip, string gamerTag)
            : base(game)
        {
            Avatar = avatar;
            ImageJoueur = imageJoueur;
            //ListeComponentsJoueurPartie = listeComponentsJoueurPartie;
            GestionnaireDeLaPartie = gestionnaireDeLaPartie;
            ÉcranDeJeu = écranDeJeu;
            IP = ip;
            GamerTag = gamerTag;
            EstActif = true; //active le joueur

        }

        //Temporaire en attendant que personnage soit créé
        public Joueur(Game game, GestionPartie gestionnaireDeLaPartie, Viewport écranDeJeu, NetConnection ip, string gamerTag)
            : base(game)
        {
            //ListeComponentsJoueurPartie = listeComponentsJoueurPartie;
            GestionnaireDeLaPartie = gestionnaireDeLaPartie;
            ÉcranDeJeu = écranDeJeu;
            IP = ip;
            GamerTag = gamerTag;
            EstActif = true; //active le joueur
        }

        //Joueur de base sans nom ni ip
        public Joueur(Game game, GestionPartie gestionnaireDeLaPartie, Viewport écranDeJeu)
            : base(game)
        {
            //ListeComponentsJoueurPartie = listeComponentsJoueurPartie;
            GestionnaireDeLaPartie = gestionnaireDeLaPartie;
            ÉcranDeJeu = écranDeJeu;
            EstActif = true; //active le joueur
        }

        //Temporaire
        public Joueur(Game game, NetConnection ip)
            :base(game)
        {
            IP = ip;
        }

        public override void Initialize()
        {
            GestionnaireInput = Game.Services.GetService(typeof(InputManager))as InputManager;
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
