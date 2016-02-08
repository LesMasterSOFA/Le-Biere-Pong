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
    public class Mode1v1LAN : PartieMultijoueur
    {
        NetworkSession networkSession;
        AvailableNetworkSessionCollection availableSessions;
        int selectedSessionIndex;
        PacketReader packetReader = new PacketReader();
        PacketWriter packetWriter = new PacketWriter();
        Viewport mainViewport { get; set; }

        public Mode1v1LAN(Game game)
            : base(game)
        {

        }


        public override void Initialize()
        {

            // Add Gamer Services
            Game.Components.Add(new GamerServicesComponent(this.Game));

            // Respond to the SignedInGamer event
            SignedInGamer.SignedIn +=
                new EventHandler<SignedInEventArgs>(SignedInGamer_SignedIn);

            mainViewport = this.Game.GraphicsDevice.Viewport;

            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            //GestionInputGameplay(joueur, gameTime); manque un joueur
            base.Update(gameTime);
        }

        private void GestionInputGameplay(Joueur joueur, GameTime gameTime)
        {
            UpdateInput(joueur);

            joueur.Update(gameTime);

            networkSession.Update();
        }

        //s'occupe de la gestion des inputs
        void UpdateInput(Joueur joueur)
        {

        }
        //Crée un joueur quand un joueur se log
        void SignedInGamer_SignedIn(object sender, SignedInEventArgs e)
        {
            //Ajoute un joueur dans la liste de joueur logger
            e.Gamer.Tag = new Joueur(this.Game, new GestionPartie(this.Game), mainViewport); //instancie un joueur sans nom ni ip
        }
    }
}
