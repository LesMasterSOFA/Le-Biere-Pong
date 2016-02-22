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
        protected GestionEnvironnement gestionnaireEnvironnement { get; set; }
        protected Joueur JoueurPrincipal { get; set;}
        string Environnement { get; set; }
        protected GestionPartie gestionnairePartie { get; set; }
        protected bool EstPartieActive { get; set; }
        public Partie(Game game)
            : base(game)
        {
        }

        void DéterminerEnvironnement(string environnement)
        {
            Environnement = environnement;
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
