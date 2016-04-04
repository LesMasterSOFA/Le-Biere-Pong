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
        public NetworkClient Client { get; private set; }
        
        //Constructeur normal
        public JoueurMultijoueur(Game game, Personnage avatar, Texture2D imageJoueur, GestionPartie gestionnaireDeLaPartie, Viewport écranDeJeu, NetConnection ip, string gamerTag, NetworkClient client)
            : base(game, avatar, imageJoueur, gestionnaireDeLaPartie, écranDeJeu, gamerTag)
        {
            IP = ip;
            Client = client;
        }

        //Temporaire
        public JoueurMultijoueur(Game game, NetConnection ip, NetworkClient client)
            :base(game)
        {
            Client = client;
            IP = ip;
        }
        //Temporaire
        public JoueurMultijoueur(Game game, NetConnection ip)
            : base(game)
        {
            IP = ip;
        }
        //Constructeur sérialiseur
        public JoueurMultijoueur(Game game, InfoJoueurMultijoueur infoJoueurMultijoueur):base(game)
        {
                //Avatar = new Personnage(this.Game, infoJoueurMultijoueur.InfoAvatar);
                GamerTag = infoJoueurMultijoueur.Gamertag;
                //ImageJoueur = new RessourcesManager<Texture2D>(this.Game, "Texture").Find(infoJoueurMultijoueur.ImageJoueur);
                GestionnaireDeLaPartie = new GestionPartie(this.Game, infoJoueurMultijoueur.InfoGestionnairePartie);
                EstActif = infoJoueurMultijoueur.EstActif;
                //IP = new NetConnection();
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
        public InfoPersonnage InfoAvatar { get; private set; }
        public string Gamertag { get; private set; }
        public string ImageJoueur { get; private set; }
        public InfoGestionPartie InfoGestionnairePartie { get; private set; }
        public bool EstActif { get; private set; }
        public string IP { get; private set; }

        public InfoJoueurMultijoueur(Personnage avatar, string gamertag, Texture2D imageJoueur, GestionPartie gestionnairePartie, bool estActif, NetConnection ip)
        {
            //InfoAvatar = new InfoPersonnage(avatar.NomModèle, avatar.NomTexture, avatar.NomEffet, avatar.Échelle, avatar.Rotation, avatar.Position);
            Gamertag = gamertag;
            if(imageJoueur != null)
            ImageJoueur = imageJoueur.Name;
            InfoGestionnairePartie = new InfoGestionPartie();
            EstActif = estActif;
            IP = ip.ToString();
        }
    }
}
