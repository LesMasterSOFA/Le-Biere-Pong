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
    public abstract class Partie : Microsoft.Xna.Framework.GameComponent, IActivable
    {
        public Joueur JoueurPrincipal { get; protected set;}
        public GestionPartie GestionnairePartie { get; protected set; }
        public bool EstPartieActive { get; protected set; }
        public GestionEnvironnement EnvironnementPartie { get; protected set; }
        
        public Partie(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            GestionnairePartie = new GestionPartie(Game);
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
