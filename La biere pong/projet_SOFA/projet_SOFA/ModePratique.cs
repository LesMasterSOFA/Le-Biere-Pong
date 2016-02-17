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

    public class ModePratique : PartieSolo
    {
        ATH ath { get; set; }
        GestionEnvironnement EnvironnementPartie { get; set; }
        BoutonDeCommande BoutonJouer { get; set; }
        public ModePratique(Game game)
            : base(game)
        {
            MenuSélectionPersonnage();
        }

        public override void Initialize()
        {
            EnvironnementPartie = new GestionEnvironnement(this.Game,"Garage");
            ath = new ATH(Game);
            base.Initialize();
        }

        void ActiverEnvironnement()
        {
            if (EstPartieActive)
            {
                Game.Components.Add(EnvironnementPartie);
                Game.Components.Add(ath);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        void MenuSélectionPersonnage()
        {
            
            BoutonJouer = new BoutonDeCommande(Game, "jouer", "Impact20", "BoutonBleu", "BoutonBleuPale", new Vector2(100,100), true, Activerpartie);
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
