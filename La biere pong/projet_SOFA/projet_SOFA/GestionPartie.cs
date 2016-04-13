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
    public class GestionPartie : Microsoft.Xna.Framework.GameComponent
    {
        Random GénérateurRandom { get; set; }
        bool EstTourJoueur { get; set; }
        bool EstTourAdversaire { get { return !EstTourJoueur; } }
        public GestionPartie(Game game)
            : base(game)
        {

        }

        //Constructeur sérialiseur
        public GestionPartie(Game game, InfoGestionPartie infoGestionPartie)
            :base(game)
        {

        }


        public override void Initialize()
        {
            GénérateurRandom = new Random();
            EstTourJoueur = Convert.ToBoolean(GénérateurRandom.Next(0, 1));
            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }

    [Serializable]
    public class InfoGestionPartie
    {
        public InfoGestionPartie()
        {

        }
    }
}
