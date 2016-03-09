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
   public enum Environnements { Garage } //À ajouter les environnement dedans

   public class GestionEnvironnement : Microsoft.Xna.Framework.GameComponent
   {
      const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
      const float DIMENSION_TABLE_X = 0.76f;
      const float DIMENSION_TABLE_Y = 0.74f;
      const float DIMENSION_TABLE_Z = 1.83f;
      const float RAYON_VERRE_HAUT = 0.09225f;
      const float HAUTEUR_VERRE = 0.1199f;
      InputManager GestionClavier { get; set; }
      ObjetDeBase Table { get; set; }
      ObjetDeBase Balle { get; set; }
      public Caméra CaméraJeu { get; set; }
      Environnements NomEnvironnement { get; set; }
      Personnage personnagePrincipal { get; set; }
      List<VerreJoueurPrincipal> VerresJoueur { get; set; }
      VerreJoueurPrincipal VerreJoueur1 { get; set; }
      VerreJoueurPrincipal VerreJoueur2 { get; set; }
      VerreJoueurPrincipal VerreJoueur3 { get; set; }
      VerreJoueurPrincipal VerreJoueur4 { get; set; }
      VerreJoueurPrincipal VerreJoueur5 { get; set; }
      VerreJoueurPrincipal VerreJoueur6 { get; set; }
      List<VerreAdversaire> VerresAdversaire { get; set; }
      VerreAdversaire VerreAdversaire1 { get; set; }
      VerreAdversaire VerreAdversaire2 { get; set; }
      VerreAdversaire VerreAdversaire3 { get; set; }
      VerreAdversaire VerreAdversaire4 { get; set; }
      VerreAdversaire VerreAdversaire5 { get; set; }
      VerreAdversaire VerreAdversaire6 { get; set; }
      List<Vector3> ListePositionVerres { get; set; }
      List<Vector3> ListePositionVerresAdv { get; set; }


      public GestionEnvironnement(Game game, Environnements nomEnvironnement)
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
         GestionClavier = Game.Services.GetService(typeof(InputManager)) as InputManager;

         ListePositionVerres = new List<Vector3>();
         ListePositionVerresAdv = new List<Vector3>();
         FixerLesPositions();

         //Instanciation et ajout dans components de caméra
         Vector3 positionCaméra = new Vector3(0, 1.5f, 2f);
         Vector3 cibleCaméra = new Vector3(0, 1f, 0);
         CaméraJeu = new CaméraSubjective(Game, positionCaméra, cibleCaméra, Vector3.Up, INTERVALLE_MAJ_STANDARD);
         Game.Components.Add(CaméraJeu);
         Game.Services.AddService(typeof(Caméra), CaméraJeu);
         InstancierEnvironnement();

         //Instanciation objets

         personnagePrincipal = new Personnage(this.Game);

         Table = new ObjetDeBase(Game, "table_plastique", "table_plastique", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
         BoundingBox boundingTable = new BoundingBox(new Vector3(-DIMENSION_TABLE_X / 2, 0, -DIMENSION_TABLE_Z / 2), new Vector3(DIMENSION_TABLE_X / 2, 0.755f, DIMENSION_TABLE_Z / 2));
         Balle = new BallePhysique(Game, "balle", "couleur_Balle", "Shader", 1, new Vector3(0, 0, 0), new Vector3(0, 1f, 1.7f), 4.5f, 0, MathHelper.PiOver4, boundingTable, ListePositionVerresAdv, RAYON_VERRE_HAUT, HAUTEUR_VERRE, INTERVALLE_MAJ_STANDARD);

         personnagePrincipal = new Personnage(this.Game);

         CréerLesVerres();

         //Ajout des objets dans la liste de Components
         Game.Components.Add(Balle);
         Game.Components.Add(Table);
         Game.Components.Add(personnagePrincipal);
         AjouterVerresJoueur();//Les ajouter dans les Game.Components
         AjouterVerresAdversaire();//Les ajouter dans les Game.Components
      }
      void FixerLesPositions()
      {
         ListePositionVerres.Add(new Vector3(0, 0.74f, 0.8f)); ListePositionVerres.Add(new Vector3(0.09225f, 0.74f, 0.8f));
         ListePositionVerres.Add(new Vector3(-0.09225f, 0.74f, 0.8f)); ListePositionVerres.Add(new Vector3(0.09225f / 2, 0.74f, 0.8f - 0.09225f * (float)Math.Sin(Math.PI / 3)));
         ListePositionVerres.Add(new Vector3(-0.09225f / 2, 0.74f, 0.8f - 0.09225f * (float)Math.Sin(Math.PI / 3))); ListePositionVerres.Add(new Vector3(0, 0.74f, 0.8f - 2 * 0.09225f * (float)Math.Sin(Math.PI / 3)));

         ListePositionVerresAdv.Add(new Vector3(0, 0.74f, -0.8f)); ListePositionVerresAdv.Add(new Vector3(0.09225f, 0.74f, -0.8f));
         ListePositionVerresAdv.Add(new Vector3(-0.09225f, 0.74f, -0.8f)); ListePositionVerresAdv.Add(new Vector3(0.09225f / 2, 0.74f, -0.8f + 0.09225f * (float)Math.Sin(Math.PI / 3)));
         ListePositionVerresAdv.Add(new Vector3(-0.09225f / 2, 0.74f, -0.8f + 0.09225f * (float)Math.Sin(Math.PI / 3))); ListePositionVerresAdv.Add(new Vector3(0, 0.74f, -0.8f + 2 * 0.09225f * (float)Math.Sin(Math.PI / 3)));
      }

      void CréerLesVerres()
      {
         VerresJoueur = new List<VerreJoueurPrincipal>();
         VerreJoueur1 = new VerreJoueurPrincipal(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerres[0]);
         VerreJoueur2 = new VerreJoueurPrincipal(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerres[1]);
         VerreJoueur3 = new VerreJoueurPrincipal(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerres[2]);
         VerreJoueur4 = new VerreJoueurPrincipal(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerres[3]);
         VerreJoueur5 = new VerreJoueurPrincipal(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerres[4]);
         VerreJoueur6 = new VerreJoueurPrincipal(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerres[5]);
         VerresJoueur.Add(VerreJoueur1);
         VerresJoueur.Add(VerreJoueur2);
         VerresJoueur.Add(VerreJoueur3);
         VerresJoueur.Add(VerreJoueur4);
         VerresJoueur.Add(VerreJoueur5);
         VerresJoueur.Add(VerreJoueur6);

         VerresAdversaire = new List<VerreAdversaire>();
         VerreAdversaire1 = new VerreAdversaire(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerresAdv[0]);
         VerreAdversaire2 = new VerreAdversaire(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerresAdv[1]);
         VerreAdversaire3 = new VerreAdversaire(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerresAdv[2]);
         VerreAdversaire4 = new VerreAdversaire(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerresAdv[3]);
         VerreAdversaire5 = new VerreAdversaire(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerresAdv[4]);
         VerreAdversaire6 = new VerreAdversaire(Game, "verre", "verre_tex", "Shader", 1f, Vector3.Zero, ListePositionVerresAdv[5]);
         VerresAdversaire.Add(VerreAdversaire1);
         VerresAdversaire.Add(VerreAdversaire2);
         VerresAdversaire.Add(VerreAdversaire3);
         VerresAdversaire.Add(VerreAdversaire4);
         VerresAdversaire.Add(VerreAdversaire5);
         VerresAdversaire.Add(VerreAdversaire6);

      }
      void AjouterVerresJoueur()
      {
         foreach (VerreJoueurPrincipal verre in VerresJoueur)
         {
            Game.Components.Add(verre);
         }
      }
      void AjouterVerresAdversaire()
      {
         foreach (VerreAdversaire verre in VerresAdversaire)
         {
            Game.Components.Add(verre);
         }
      }

      //Cette fonction envoi les textures des différents murs aux environnements, pour les modèles propre à l'environnement ils sont instanciés directement dans ce dernier.
      void InstancierEnvironnement()
      {
         switch (NomEnvironnement)
         {
            case Environnements.Garage:
               EnvironnementGarage Garage = new EnvironnementGarage(Game, "GaucheGarage", "DroiteGarage", "PlafondGarage", "PlancherGaragee", "AvantGarage", "ArriereGarage");
               Game.Components.Add(Garage);
               break;
            default:
               throw new Exception();
         }

      }
      public override void Update(GameTime gameTime)
      {
         //pour essai
         if (GestionClavier.EstNouvelleTouche(Keys.E))
         {
            GestionÉvénements.EnleverVerres(VerresJoueur, Game, VerreJoueur1, true, true, false);
         }
         base.Update(gameTime);
      }
   }
}

