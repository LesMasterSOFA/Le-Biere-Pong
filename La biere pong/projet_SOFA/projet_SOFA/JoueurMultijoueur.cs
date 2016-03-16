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
    public class JoueurMultijoueur : Joueur
    { 
        public NetConnection IP { get; private set; } 
        //Constructeur normal
        public JoueurMultijoueur(Game game, Personnage avatar, Texture2D imageJoueur, GestionPartie gestionnaireDeLaPartie, Viewport écranDeJeu, NetConnection ip, string gamerTag)
            : base(game, avatar, imageJoueur, gestionnaireDeLaPartie, écranDeJeu, gamerTag)
        {
            IP = ip;
        }

        //Temporaire
        public JoueurMultijoueur(Game game, NetConnection ip)
            :base(game)
        {
            IP = ip;
        }

        public override void Initialize()
        {
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
    }

    [Serializable]
    public class InfoJoueurMultijoueur
    {
        InfoPersonnage InfoAvatar { get; set; }
        string Gamertag { get; set; }
        string ImageJoueur { get; set; }
        InfoGestionPartie InfoGestionnairePartie { get; set; }
        bool EstActif { get; set; }
        string IP { get; set; }

        public InfoJoueurMultijoueur(Personnage avatar, string gamertag, Texture2D imageJoueur, GestionPartie gestionnairePartie, bool estActif, NetConnection ip)
        {
            InfoAvatar = new InfoPersonnage();
            Gamertag = gamertag;
            if(imageJoueur != null)
                ImageJoueur = imageJoueur.Name;
            InfoGestionnairePartie = new InfoGestionPartie();
            EstActif = estActif;
            IP = ip.ToString();
        }
    }
}
