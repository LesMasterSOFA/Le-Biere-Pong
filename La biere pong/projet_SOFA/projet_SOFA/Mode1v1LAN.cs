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
        BoutonDeCommande BoutonJouer { get; set; }
        ATH ath { get; set; }
        
        public Mode1v1LAN(Game game)
            : base(game)
        {
            MenuSélectionPersonnage();
        }

        public override void Initialize()
        {
            EnvironnementPartie = new GestionEnvironnement(this.Game, Environnements.Garage);
            ath = new ATH(Game);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        void ActiverEnvironnement()
        {
            if (EstPartieActive)
            {
                Game.Components.Add(EnvironnementPartie);
                Game.Components.Add(ath);
            }
        }

        void MenuSélectionPersonnage()
        {
            BoutonJouer = new BoutonDeCommande(Game, "jouer", "Impact20", "BoutonBleu", "BoutonBleuPale", new Vector2(100, 100), true, Activerpartie);
            Game.Components.Add(BoutonJouer);
        }

        void Activerpartie()
        {
            ModifierActivation();
            ActiverEnvironnement();
            Game.Components.Remove(BoutonJouer);
        }

    }
}
