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

        List<ObjetDeBase> VerresJoueur { get; set; }
        ObjetDeBase VerreJoueur1 { get; set; }
        ObjetDeBase VerreJoueur2 { get; set; }
        ObjetDeBase VerreJoueur3 { get; set; }
        ObjetDeBase VerreJoueur4 { get; set; }
        ObjetDeBase VerreJoueur5 { get; set; }
        ObjetDeBase VerreJoueur6 { get; set; }

        List<ObjetDeBase> VerresAdversaire { get; set; }
        ObjetDeBase VerreAdversaire1 { get; set; }
        ObjetDeBase VerreAdversaire2 { get; set; }
        ObjetDeBase VerreAdversaire3 { get; set; }
        ObjetDeBase VerreAdversaire4 { get; set; }
        ObjetDeBase VerreAdversaire5 { get; set; }
        ObjetDeBase VerreAdversaire6 { get; set; }


        public GestionEnvironnement(Game game, string nomEnvironnement)
            : base(game)
        {
            NomEnvironnement = nomEnvironnement;
        }

        public override void Initialize()
        {
            //Instanciation et ajout dans components de caméra
            Vector3 positionCaméra = new Vector3(0, 90, 100);
            Vector3 cibleCaméra = new Vector3(0, 70, 0);
            CaméraJeu = new CaméraSubjective(Game, positionCaméra, cibleCaméra, Vector3.Up, INTERVALLE_MAJ_STANDARD);
            Game.Components.Add(CaméraJeu);
            Game.Services.AddService(typeof(Caméra), CaméraJeu);

            //Instanciation objets
            Table = new ObjetDeBase(Game, "table_plastique", "table_plastique", 40, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            Balle = new ObjetDeBase(Game, "balle","couleur_Balle", 1f, new Vector3(0, 0, 0), new Vector3(0, 100, 0));
            personnagePrincipal = new Personnage(this.Game);

            VerresJoueur = new List<ObjetDeBase>();
            VerreJoueur1 = new ObjetDeBase(Game, "verre", "verre_tex", 1f, Vector3.Zero, new Vector3(0, 75, 0));
            VerresJoueur.Add(VerreJoueur1);
            AjouterVerres();//Les ajouter dans les Game.Components
            
            
            //Ajout des objets dans la liste de Components
            Game.Components.Add(Table);
            Game.Components.Add(Balle);
            Game.Components.Add(personnagePrincipal);

            InstancierEnvironnement();
            base.Initialize();
        }
        void AjouterVerres()
        {
            foreach (ObjetDeBase verre in VerresJoueur)
            {
                Game.Components.Add(verre);
            }
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
