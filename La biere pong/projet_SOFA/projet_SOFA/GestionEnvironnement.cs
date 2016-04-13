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
   public enum Environnements { Garage, SalleManger, SousSol } //À ajouter les environnement dedans
   public enum SuperboyPersonnage {superBoy, superBoyTex } //Contient les informations pour le personnage Superboy

   public class GestionEnvironnement : Microsoft.Xna.Framework.GameComponent
   {
      const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
      
      public Caméra CaméraJeu { get; set; }

      public Environnements NomEnvironnement { get; private set; }

      public string PersonnageJoueurPrincipalModel { get; private set; }
      public string PersonnageJoueurPrincipalTexture { get; private set; }
      public string PersonnageJoueurSecondaireModel { get; private set; }
      public string PersonnageJoueurSecondaireTexture { get; private set; }

      public GestionEnvironnement(Game game, Environnements nomEnvironnement, string personnageJoueurPrincipalModel, string personnageJoueurPrincipalTexture, string personnageJoueurSecondaireModel, string personnageJoueurSecondaireTexture)
          : base(game)
      {
          NomEnvironnement = nomEnvironnement;
          PersonnageJoueurPrincipalModel = personnageJoueurPrincipalModel;
          PersonnageJoueurPrincipalTexture = personnageJoueurPrincipalTexture;
          PersonnageJoueurSecondaireModel = personnageJoueurSecondaireModel;
          PersonnageJoueurSecondaireTexture = personnageJoueurSecondaireTexture;
      }

      //Constructeur Sérialiseur -> reste à ajouter modif personnages ***
      public GestionEnvironnement(Game game, InfoGestionEnvironnement infoGestionEnvironnement)
          : base(game)
      {
          NomEnvironnement = infoGestionEnvironnement.NomEnvironnement;
      }

      public override void Initialize() 
      {
         //Instanciation et ajout dans components de caméra
         Vector3 positionCaméra = new Vector3(0, 1.5f, 2f);
         Vector3 cibleCaméra = new Vector3(0, 1f, 0);
         CaméraJeu = new CaméraSubjective(Game, positionCaméra, cibleCaméra, Vector3.Up, INTERVALLE_MAJ_STANDARD);
         Game.Components.Add(CaméraJeu);
         Game.Services.AddService(typeof(Caméra), CaméraJeu);
         InstancierEnvironnement();
      }

      //Cette fonction envoi les textures des différents murs aux environnements, pour les modèles propre à l'environnement ils sont instanciés directement dans ce dernier.
      void InstancierEnvironnement()
      {
              switch (NomEnvironnement)
              {

                  case Environnements.Garage:
                      EnvironnementGarage Garage = new EnvironnementGarage(Game, "DroiteGarage", "GaucheGarage", "PlafondGarage", "PlancherGarage", "AvantGarage", "ArriereGarage", SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex.ToString(), SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex.ToString());
                      Game.Components.Add(Garage);
                      break;
                  case Environnements.SalleManger:
                      EnvironnementSalleManger SalleManger = new EnvironnementSalleManger(Game, "GaucheSallePetiteFoyer", "DroiteSallePlinthe", "PlafondSalle", "PlancherSalle", "AvantSallePlinthe", "ArriereSallePlinthe", SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex.ToString(), SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex.ToString());
                      Game.Components.Add(SalleManger);
                      break;
                  case Environnements.SousSol:
                      EnvironnementSousSol SousSol = new EnvironnementSousSol(Game, "GaucheSousSol", "DroiteSousSol", "PlafondSousSol", "PlancherSousSol", "AvantSousSol", "ArriereSousSol", SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex.ToString(), SuperboyPersonnage.superBoy.ToString(), SuperboyPersonnage.superBoyTex.ToString());
                      Game.Components.Add(SousSol);
                      break;
                  default:
                      throw new Exception();
              }
      }

      public override void Update(GameTime gameTime)
      {
          //pour essai
          //if (GestionClavier.EstNouvelleTouche(Keys.E))
          //{
          //   GestionÉvénements.EnleverVerres(VerresJoueur, Game, VerreJoueur1, true, true);
          //}
          base.Update(gameTime);
      }
    }

    [Serializable]
    public class InfoGestionEnvironnement
    {
        public Environnements NomEnvironnement { get; private set; }
        public InfoGestionEnvironnement(Environnements nomEnvironnement)
        {
            NomEnvironnement = nomEnvironnement;
        }
    }
}

