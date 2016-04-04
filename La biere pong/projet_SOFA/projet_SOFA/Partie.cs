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
    enum ÉtatPartie
    {
        EnCours, Gagnée, Perdue
    }
    public abstract class Partie : Microsoft.Xna.Framework.DrawableGameComponent, IActivable
    {
        ÉtatPartie étatDePartie { get; set; }
        public GestionPartie gestionnairePartie { get; set; }
        public bool EstPartieActive { get; set; }
        public GestionEnvironnement EnvironnementPartie { get; set; }
        public Partie(Game game)
            : base(game)
        {
            étatDePartie = ÉtatPartie.EnCours;
        }

        public override void Initialize()
        {
            gestionnairePartie = new GestionPartie(Game);
            EstPartieActive = false;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void ModifierActivation()
        {
            EstPartieActive = !EstPartieActive;
        }
    }
}
