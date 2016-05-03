using System;
using Microsoft.Xna.Framework;


namespace AtelierXNA
{
    public enum Environnements { Garage, SalleManger, SousSol }
    public enum SuperboyPersonnage { superBoy, superBoyTex, superBoyTex2 }
    public enum TypePartie { Pratique, LAN, Histoire, Local }
    public class GestionEnvironnement : Microsoft.Xna.Framework.GameComponent
    {
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;

        public CaméraJoueur CaméraJeu { get; set; }

        public Environnements NomEnvironnement { get; private set; }

        public string PersonnageJoueurPrincipalModel { get; private set; }
        public string PersonnageJoueurPrincipalTexture { get; private set; }
        public string PersonnageJoueurSecondaireModel { get; private set; }
        public string PersonnageJoueurSecondaireTexture { get; private set; }
        public TypePartie TypeDePartie { get; set; }

        public GestionEnvironnement(Game game, Environnements nomEnvironnement, string personnageJoueurPrincipalModel, string personnageJoueurPrincipalTexture, string personnageJoueurSecondaireModel, string personnageJoueurSecondaireTexture, TypePartie typePartie)
            : base(game)
        {
            NomEnvironnement = nomEnvironnement;
            PersonnageJoueurPrincipalModel = personnageJoueurPrincipalModel;
            PersonnageJoueurPrincipalTexture = personnageJoueurPrincipalTexture;
            PersonnageJoueurSecondaireModel = personnageJoueurSecondaireModel;
            PersonnageJoueurSecondaireTexture = personnageJoueurSecondaireTexture;
            TypeDePartie = typePartie;
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
            Vector3 positionCaméra = new Vector3(0, 1.5f, 1.0f);
            Vector3 cibleCaméra = new Vector3(0, 1f, 0);

            CaméraJeu = new CaméraJoueur(Game, positionCaméra, cibleCaméra, Vector3.Up, INTERVALLE_MAJ_STANDARD);
            //CaméraJeu = new CaméraSubjective(Game, positionCaméra, cibleCaméra, Vector3.Up, INTERVALLE_MAJ_STANDARD);

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
                    EnvironnementGarage Garage = new EnvironnementGarage(Game, this, PersonnageJoueurPrincipalModel, PersonnageJoueurPrincipalTexture, PersonnageJoueurSecondaireModel, PersonnageJoueurSecondaireTexture, TypeDePartie);
                    Game.Components.Add(Garage);
                    break;
                case Environnements.SalleManger:
                    EnvironnementSalleManger SalleManger = new EnvironnementSalleManger(Game, this, PersonnageJoueurPrincipalModel, PersonnageJoueurPrincipalTexture, PersonnageJoueurSecondaireModel, PersonnageJoueurSecondaireTexture, TypeDePartie);
                    Game.Components.Add(SalleManger);
                    break;
                case Environnements.SousSol:
                    EnvironnementSousSol SousSol = new EnvironnementSousSol(Game, this, PersonnageJoueurPrincipalModel, PersonnageJoueurPrincipalTexture, PersonnageJoueurSecondaireModel, PersonnageJoueurSecondaireTexture, TypeDePartie);
                    Game.Components.Add(SousSol);
                    break;
                default:
                    throw new Exception();
            }
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

