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
    public class GestionEnvironnement : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        PlanTexturé Gauche { get; set; }
        PlanTexturé Droite { get; set; }
        PlanTexturé Dessus { get; set; }
        PlanTexturé Dessous { get; set; }
        PlanTexturé Avant { get; set; }
        PlanTexturé Arrière { get; set; }
        ObjetDeBase Table { get; set; }
        ObjetDeBase Balle { get; set; }
        public Caméra CaméraJeu { get; set; }
        string NomEnvironnement { get; set; }
        EnvironnementDeBase THEenvironnement { get; set; }
        Personnage personnagePrincipal { get; set; }

        public GestionEnvironnement(Game game, string nomEnvironnement)
            : base(game)
        {
            NomEnvironnement = nomEnvironnement;
        }

        public override void Initialize()
        {

            //Instanciation et ajout dans components de caméra
            Vector3 positionCaméra = new Vector3(0, 90, 65);
            Vector3 cibleCaméra = new Vector3(0, 100, 0);
            CaméraJeu = new CaméraSubjective(Game, positionCaméra, cibleCaméra, Vector3.Up, INTERVALLE_MAJ_STANDARD);
            Game.Components.Add(CaméraJeu);
            Game.Services.AddService(typeof(Caméra), CaméraJeu);

            //Instanciation objets
            Table = new ObjetDeBase(Game, "tablebois2", "tablebois", 0.25f, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            Balle = new ObjetDeBase(Game, "balle", "blanc", 0.1f, new Vector3(0, 0, 0), new Vector3(0, 100, 0));
            personnagePrincipal = new Personnage(this.Game);
            
            //Ajout des objets dans la liste de Components
            Game.Components.Add(Table);
            Game.Components.Add(Balle);
            Game.Components.Add(personnagePrincipal);

            InstancierEnvironnement();
            base.Initialize();
        }

        void InstancierEnvironnement()
        {
            switch (NomEnvironnement)
            {
                case "condo":
                    THEenvironnement = new EnvironnementGarage(Game, "gauche", "droite", "plafond", "plancher", "avant", "arriere");
                    break;
                default:
                    throw new Exception();
            }
            Game.Components.Add(THEenvironnement);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
