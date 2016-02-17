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
using Microsoft.Xna.Framework.Net;


namespace AtelierXNA
{
    //Énumération contentant les différents types de packets,
    //pouvant être ensuite converti en byte ce qui permet de déterminer ce qu'il faut faire avec tel ou tel packet
    public enum PacketTypes { LOGIN, MOVE, WORLDSTATE }

    public class NetworkManager : Microsoft.Xna.Framework.GameComponent
    {
        List<NetworkClient> ListeClients = new List<NetworkClient>();
        NetworkServer Serveur;
        const string NOM_JEU = "BEERPONG";
        const int PORT = 5011;


        public NetworkManager(Game game)
            : base(game)
        {
            Serveur = new NetworkServer(Game, NOM_JEU, PORT);
            ListeClients.Add(new NetworkClient(Game, NOM_JEU, PORT, "Joueur1"));
        }

        public override void Initialize()
        {

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
    }
}
