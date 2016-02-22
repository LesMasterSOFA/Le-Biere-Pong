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
       
        ObjetDeBase Table { get; set; }
        ObjetDeBase Balle { get; set; }
        public Caméra CaméraJeu { get; set; }
        string NomEnvironnement { get; set; }
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

        public override void Initialize() //j'ai changé les échelles des modeles pour quils soient tous a 1f, maintenant, la position est en metres.
        /* Dimensions de la balle: rayon de 2 cm
         * Centre de la balle : centre de la sphere
         * Dimensions de la table: X = 76cm 
         *                         Y = 74cm
         *                         Z = 183cm (pas sur)
         * Centre de la table : a terre, au milieu
         * Dimension du verre : rayon de 9.225cm (dans le haut du verre)
         *                      hauteur : 11.99cm
         * Centre du verre : a terre, au centre du cercle
         * Dimension du monsieur : X = 79.399cm
         *                         Y = 1.705m
         *                         Z = 36.761cm
         * Centre du monsieur : a terre, au milieu
         */
        {
            
            //Instanciation et ajout dans components de caméra
            Vector3 positionCaméra = new Vector3(0, 1.25f, 2f);
            Vector3 cibleCaméra = new Vector3(0, 1f, 0);
            CaméraJeu = new CaméraSubjective(Game, positionCaméra, cibleCaméra, Vector3.Up, INTERVALLE_MAJ_STANDARD);
            Game.Components.Add(CaméraJeu);
            Game.Services.AddService(typeof(Caméra), CaméraJeu);
            InstancierEnvironnement();

            //Instanciation objets
            Table = new ObjetDeBase(Game, "table_plastique", "table_plastique", 1, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            Balle = new ObjetDeBase(Game, "balle","couleur_Balle", 1, new Vector3(0, 0, 0), new Vector3(0, 0.74f+0.02f, 0));
            
            personnagePrincipal = new Personnage(this.Game);

            VerresJoueur = new List<ObjetDeBase>();
            VerreJoueur1 = new ObjetDeBase(Game, "verre", "verre_tex", 1f, Vector3.Zero, new Vector3(0, 0.74f, 0.8f));
            VerreJoueur2 = new ObjetDeBase(Game, "verre", "verre_tex", 1f, Vector3.Zero, new Vector3(0.09225f, 0.74f, 0.8f));
            VerreJoueur3 = new ObjetDeBase(Game, "verre", "verre_tex", 1f, Vector3.Zero, new Vector3(-0.09225f, 0.74f, 0.8f));
            VerreJoueur4 = new ObjetDeBase(Game, "verre", "verre_tex", 1f, Vector3.Zero, new Vector3(0.09225f / 2, 0.74f, 0.8f - 0.09225f * (float)Math.Sin(Math.PI / 3)));
            VerreJoueur5 = new ObjetDeBase(Game, "verre", "verre_tex", 1f, Vector3.Zero, new Vector3(-0.09225f / 2, 0.74f, 0.8f - 0.09225f* (float)Math.Sin(Math.PI / 3)));
            VerreJoueur6 = new ObjetDeBase(Game, "verre", "verre_tex", 1f, Vector3.Zero, new Vector3(0, 0.74f, 0.8f-2*0.09225f* (float)Math.Sin(Math.PI / 3)));
            VerresJoueur.Add(VerreJoueur1);
            VerresJoueur.Add(VerreJoueur2);
            VerresJoueur.Add(VerreJoueur3);
            VerresJoueur.Add(VerreJoueur4);
            VerresJoueur.Add(VerreJoueur5);
            VerresJoueur.Add(VerreJoueur6);

            VerresAdversaire = new List<ObjetDeBase>();
            VerreAdversaire1 = new ObjetDeBase(Game, "verre", "verre_tex", 1f, Vector3.Zero, new Vector3(0, 0.74f, -0.8f));
            VerreAdversaire2 = new ObjetDeBase(Game, "verre", "verre_tex", 1f, Vector3.Zero, new Vector3(0.09225f, 0.74f, -0.8f));
            VerreAdversaire3 = new ObjetDeBase(Game, "verre", "verre_tex", 1f, Vector3.Zero, new Vector3(-0.09225f, 0.74f, -0.8f));
            VerreAdversaire4 = new ObjetDeBase(Game, "verre", "verre_tex", 1f, Vector3.Zero, new Vector3(0.09225f / 2, 0.74f, -0.8f + 0.09225f * (float)Math.Sin(Math.PI / 3)));
            VerreAdversaire5 = new ObjetDeBase(Game, "verre", "verre_tex", 1f, Vector3.Zero, new Vector3(-0.09225f / 2, 0.74f, -0.8f + 0.09225f * (float)Math.Sin(Math.PI / 3)));
            VerreAdversaire6 = new ObjetDeBase(Game, "verre", "verre_tex", 1f, Vector3.Zero, new Vector3(0, 0.74f, -0.8f + 2 * 0.09225f * (float)Math.Sin(Math.PI / 3)));
            VerresAdversaire.Add(VerreAdversaire1);
            VerresAdversaire.Add(VerreAdversaire2);
            VerresAdversaire.Add(VerreAdversaire3);
            VerresAdversaire.Add(VerreAdversaire4);
            VerresAdversaire.Add(VerreAdversaire5);
            VerresAdversaire.Add(VerreAdversaire6);


            
            //Ajout des objets dans la liste de Components
            Game.Components.Add(Table);
            Game.Components.Add(Balle);
            Game.Components.Add(personnagePrincipal);
            AjouterVerresJoueur();//Les ajouter dans les Game.Components
            AjouterVerresAdversaire();//Les ajouter dans les Game.Components
            base.Initialize();
        }
        void AjouterVerresJoueur()
        {
            foreach (ObjetDeBase verre in VerresJoueur)
            {
                Game.Components.Add(verre);
            }
        }
        void AjouterVerresAdversaire()
        {
            foreach (ObjetDeBase verre in VerresAdversaire)
            {
                Game.Components.Add(verre);
            }
        }

        //Cette fonction envoi les textures des différents murs aux environnements, pour les modèles propre à l'environnement ils sont instanciés directement dans ce dernier.
        void InstancierEnvironnement()
        {
            switch (NomEnvironnement)
            {
                case "Garage":
                    EnvironnementGarage Garage = new EnvironnementGarage(Game, "GaucheGarage", "DroiteGarage", "PlafondGarage", "PlancherGaragee", "AvantGarage", "ArriereGarage");
                    Game.Components.Add(Garage);
                    break;
                default:
                    throw new Exception();
            }
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
