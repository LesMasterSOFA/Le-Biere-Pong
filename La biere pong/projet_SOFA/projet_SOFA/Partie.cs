﻿using System;
using Microsoft.Xna.Framework;


namespace AtelierXNA
{
    enum ÉtatPartie
    {
        EnCours, Gagnée, Perdue
    }
    public abstract class Partie : Microsoft.Xna.Framework.DrawableGameComponent, IActivable
    {
        protected bool EstTourJoueur { get; set; }
        protected bool EstTourAdversaire { get { return !EstTourJoueur; } }
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
            Random générateurRand = new Random();
            gestionnairePartie = new GestionPartie(Game);
            EstPartieActive = false;
            EstTourJoueur = Convert.ToBoolean(générateurRand.Next(0, 1));
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
