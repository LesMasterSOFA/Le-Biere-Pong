using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AtelierXNA
{
    public class JoueurMultijoueur : Joueur
    { 
        public NetConnection IP { get; private set; }
        public NetworkClient Client { get; private set; }
        

        public JoueurMultijoueur(Game game, NetConnection ip, NetworkClient client)
            :base(game)
        {
            Client = client;
            IP = ip;
        }
        public JoueurMultijoueur(Game game, NetConnection ip)
            : base(game)
        {
            IP = ip;
        }
        //Constructeur sérialiseur
        public JoueurMultijoueur(Game game, InfoJoueurMultijoueur infoJoueurMultijoueur):base(game)
        {
            GamerTag = infoJoueurMultijoueur.Gamertag;
            GestionnaireDeLaPartie = new GestionPartie(this.Game, infoJoueurMultijoueur.InfoGestionnairePartie);
            EstActif = infoJoueurMultijoueur.EstActif;
            Client = (NetworkClient)Game.Components[4]; // indice du client slave
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
            Gamertag = gamertag;
            if(imageJoueur != null)
                ImageJoueur = imageJoueur.Name;
            InfoGestionnairePartie = new InfoGestionPartie();
            EstActif = estActif;
            IP = ip.ToString();
        }
    }
}
