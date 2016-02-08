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
    //public class Mode1v1LAN : PartieMultijoueur
    public class Mode1v1LAN : Microsoft.Xna.Framework.DrawableGameComponent
    {
        NetworkSession networkSession;
        AvailableNetworkSessionCollection availableSessions;
        int selectedSessionIndex;
        PacketReader packetReader = new PacketReader();
        PacketWriter packetWriter = new PacketWriter();
        Viewport mainViewport { get; set; }
        RessourcesManager<SpriteFont> gestionnaireFont { get; set; }
        SpriteBatch GestionSprites { get; set; }
        InputManager gestionInput { get; set; }
        GestionPartie gestionnaireDeLaPartie { get; set; }
        RessourcesManager<Texture2D> gestionnaireTexture { get; set; }


        public Mode1v1LAN(Game game)
            : base(game)
        {

        }


        public override void Initialize()
        {

            // Add Gamer Services
            Game.Components.Add(new GamerServicesComponent(this.Game));

            //Initialise les gestionnaires pour les menus
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            gestionnaireFont = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            gestionInput = new InputManager(this.Game);
            gestionnaireTexture = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;

            //Initialise le gestionnaire de la partie
            gestionnaireDeLaPartie = new GestionPartie(this.Game);

            


            // Respond to the SignedInGamer event
            SignedInGamer.SignedIn +=
                new EventHandler<SignedInEventArgs>(SignedInGamer_SignedIn);

            mainViewport = this.Game.GraphicsDevice.Viewport;

            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {

            //Passe sur tous les différents joueur qui sont logger
            foreach (SignedInGamer signedInGamer in SignedInGamer.SignedInGamers)
            {
                Joueur joueur = signedInGamer.Tag as Joueur; //Crée un joueur

                if (networkSession != null)
                {
                    // Handle the lobby input here...
                    if (networkSession.SessionState == NetworkSessionState.Lobby)
                        HandleLobbyInput();
                }

                else if (availableSessions != null)
                {
                    // Handle the available sessions input here..
                    HandleAvailableSessionsInput();
                }
                else
                {
                    HandleTitleScreenInput();
                }
            }
            base.Update(gameTime);
        }

        private void GestionInputGameplay(Joueur joueur, GameTime gameTime)
        {
            UpdateInput(gameTime, joueur);

            joueur.Update(gameTime);

            networkSession.Update();
        }

        //s'occupe de la gestion des inputs
        void UpdateInput(GameTime gameTime, Joueur joueur)
        {
            joueur.Update(gameTime);
        }

        //Crée un joueur quand un joueur se log
        void SignedInGamer_SignedIn(object sender, SignedInEventArgs e)
        {
            //Ajoute un joueur dans la liste de joueur logger
            e.Gamer.Tag = new Joueur(this.Game, gestionnaireDeLaPartie, mainViewport); //instancie un joueur sans nom ni ip
        }

        private void DrawGameplay(GameTime gameTime)
        {
            Joueur joueur;
            if (networkSession != null)
            {

                foreach (NetworkGamer networkGamer in networkSession.AllGamers)
                {
                    joueur = networkGamer.Tag as Joueur;

                    joueur.Draw();

                    //Probablement pas ici
                    //if (networkGamer.IsLocal)
                    //{
                    //    DrawPlayer(player, leftViewport);
                    //}
                    //else
                    //{
                    //    DrawPlayer(player, rightViewport);
                    //}
                }
            }
        }

        //Crée la session multijoueur
        void CreateSession()
        {
            networkSession = NetworkSession.Create(
                NetworkSessionType.SystemLink,
                1, 8, 2,
                null);

            networkSession.AllowHostMigration = true;
            networkSession.AllowJoinInProgress = true;

            HookSessionEvents();
        }

        //Évenement quand un joueur se joint
        private void HookSessionEvents()
        {
            networkSession.GamerJoined +=
                new EventHandler<GamerJoinedEventArgs>(
                    networkSession_GamerJoined);
        }

        //Création du joueur lorsqu'il joint la partie
        void networkSession_GamerJoined(object sender, GamerJoinedEventArgs e)
        {
            //Ne doit pas être ici... probablement dans partie multijoueur... je le laisse au cas ou
            //if (!e.Gamer.IsLocal)
            //{
            //    e.Gamer.Tag = new Player();
            //}
            //else
            //{
            //    e.Gamer.Tag = GetPlayer(e.Gamer.Gamertag);
            //}

            //Va chercher le joueur
            e.Gamer.Tag = GetPlayer(e.Gamer.Gamertag);
        }

        //Va chercher le joueur
        Joueur GetPlayer(String gamertag)
        {
            foreach (SignedInGamer signedInGamer in
                SignedInGamer.SignedInGamers)
            {
                if (signedInGamer.Gamertag == gamertag)
                {
                    return signedInGamer.Tag as Joueur;
                }
            }

            return new Joueur(this.Game, gestionnaireDeLaPartie, mainViewport); //Retourne un joueur sans nom ni ip
        }

        #region menu lan

        //Va probablement être modifié pour  dans menus
        //Va probablement devoir recevoir SignedInGamer si dans menus
        //***Reste à changer le home key on the keyboard et assigner un profil***
        void DessinerMenuAccueilLan()
        {
            this.Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            string message = "";

            if (SignedInGamer.SignedInGamers.Count == 0)
            {
                message = "No profile signed in!  \n" +
                    "Press the Home key on the keyboard \n";
            }

            else
            {
                message += "Press A to create a new session \n" +
                    "X to search for sessions \nB to quit \n \n";
            }

            GestionSprites.Begin();
            GestionSprites.DrawString(gestionnaireFont.Find("Arial20"), message, new Vector2(101, 101), Color.Black);
            GestionSprites.End();
        }

        //S'occupe de la gestion du menu d'accueil multijoueur
        protected void HandleTitleScreenInput()
        {
            if (gestionInput.EstClavierActivé)
            {
                if (gestionInput.EstEnfoncée(Keys.A))
                {
                    CreateSession();
                }
                else if (gestionInput.EstEnfoncée(Keys.X))
                {
                    availableSessions = NetworkSession.Find(
                        NetworkSessionType.SystemLink, 1, null);

                    selectedSessionIndex = 0;
                }

                else if (gestionInput.EstEnfoncée(Keys.B))
                {
                    //Devra être implémenter pour quitter ce menu
                    //Exit();
                }
            }
        }

        //Menu lobby multijoueur
        private void DrawLobby()
        {
            this.Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            GestionSprites.Begin();
            float y = 100;

            GestionSprites.DrawString(gestionnaireFont.Find("Arial20"), "Lobby (A=ready, B=leave)",
                new Vector2(101, y + 1), Color.Black);
            GestionSprites.DrawString(gestionnaireFont.Find("Arial20"), "Lobby (A=ready, B=leave)",
                new Vector2(101, y), Color.White);

            y += gestionnaireFont.Find("Arial20").LineSpacing * 2;

            foreach (NetworkGamer gamer in networkSession.AllGamers)
            {
                string text = gamer.Gamertag;

                Joueur joueur = gamer.Tag as Joueur;

                if (joueur.ImageJoueur == null)
                {
                    joueur.ImageJoueur = gestionnaireTexture.Find("imageGamerDeBase"); //Reste à mettre une image dans le répertoire textures
                }

                if (gamer.IsReady)
                    text += " - ready!";

                GestionSprites.Draw(joueur.ImageJoueur, new Vector2(100, y), Color.White);
                GestionSprites.DrawString(gestionnaireFont.Find("Arial20"), text, new Vector2(170, y), Color.White);

                y += gestionnaireFont.Find("Arial20").LineSpacing + 64;
            }
            GestionSprites.End();

        }

        //S'occupe de la gestion du menu du lobby multijoueur
        protected void HandleLobbyInput()
        {
            // Signal I'm ready to play!
            if (gestionInput.EstEnfoncée(Keys.A))
            {
                foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
                    gamer.IsReady = true;
            }

            if (gestionInput.EstEnfoncée(Keys.B))
            {
                networkSession = null;
                availableSessions = null;
            }

            // The host checks if everyone is ready, and moves 
            // to game play if true.
            if (networkSession.IsHost)
            {
                if (networkSession.IsEveryoneReady)
                    networkSession.StartGame();
            }

            // Pump the underlying session object.
            networkSession.Update();

        }

        //Menu sessions disponibles
        private void DrawAvailableSessions()
        {
            this.Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            GestionSprites.Begin();
            float y = 100;

            GestionSprites.DrawString(gestionnaireFont.Find("Arial20"),
                "Available sessions (A=join, B=back)",
                new Vector2(101, y + 1), Color.Black);
            GestionSprites.DrawString(gestionnaireFont.Find("Arial20"), "Available sessions (A=join, B=back)", new Vector2(100, y), Color.White);

            y += gestionnaireFont.Find("Arial20").LineSpacing * 2;

            int selectedSessionIndex = 0;

            for (
                int sessionIndex = 0;
                sessionIndex < availableSessions.Count;
                sessionIndex++)
            {
                Color color = Color.Black;

                if (sessionIndex == selectedSessionIndex)
                    color = Color.Yellow;

                GestionSprites.DrawString(gestionnaireFont.Find("Arial20"), availableSessions[sessionIndex].HostGamertag, new Vector2(100, y), color);

                y += gestionnaireFont.Find("Arial20").LineSpacing;
            }
            GestionSprites.End();
        }

        protected void HandleAvailableSessionsInput()
        {
            if (gestionInput.EstEnfoncée(Keys.A))
            {
                // Join the selected session.
                if (availableSessions.Count > 0)
                {
                    networkSession = NetworkSession.Join(
                        availableSessions[selectedSessionIndex]);
                    HookSessionEvents();

                    availableSessions.Dispose();
                    availableSessions = null;
                }
            }
            else if (gestionInput.EstEnfoncée(Keys.Up))
            {
                // Select the previous session from the list.
                if (selectedSessionIndex > 0)
                    selectedSessionIndex--;
            }
            else if (gestionInput.EstEnfoncée(Keys.Down))
            {
                // Select the next session from the list.
                if (selectedSessionIndex < availableSessions.Count - 1)
                    selectedSessionIndex++;
            }
            else if (gestionInput.EstEnfoncée(Keys.B))
            {
                // Go back to the title screen.
                availableSessions.Dispose();
                availableSessions = null;
            }

        }



        #endregion

        public override void Draw(GameTime gameTime)
        {
            if (networkSession != null)
            {
                //If the session is not null, we're either 
                //in the lobby or playing the game...
                // Draw the Lobby

                if (networkSession.SessionState == NetworkSessionState.Lobby)
                    DrawLobby();
            }
            else if (availableSessions != null)
            {
                // Show the available session...
                DrawAvailableSessions();
            }
            else
            {
                DessinerMenuAccueilLan();
            }

            //base.Draw(gameTime);
        }
    }
}
