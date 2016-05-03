using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AtelierXNA
{
   public enum TypeActionPersonnage
   {
      Rien, Boire, Lancer, ApresLancer, ApresBoire
   }


   public class Joueur : Microsoft.Xna.Framework.GameComponent, IActivable
   {
      public Personnage Avatar { get; protected set; }
      public string GamerTag { get; protected set; }
      public Texture2D ImageJoueur { get; protected set; }
      public GestionPartie GestionnaireDeLaPartie { get; protected set; }
      public InputManager GestionnaireInput { get; protected set; }
      public bool EstActif { get; protected set; }
      public Viewport ÉcranDeJeu { get; protected set; } //Défini s'il est sur l'écran totale ou une partie
      public ATH ath { get; protected set; }

      public Joueur(Game game, Personnage avatar, Texture2D imageJoueur, GestionPartie gestionnaireDeLaPartie, Viewport écranDeJeu, string gamerTag)
         : base(game)
      {
         Avatar = avatar;
         ImageJoueur = imageJoueur;
         GestionnaireDeLaPartie = gestionnaireDeLaPartie;
         ÉcranDeJeu = écranDeJeu;
         GamerTag = gamerTag;
         EstActif = true; //active le joueur
      }

      public Joueur(Game game, GestionPartie gestionnairePartie, Viewport écranDeJeu)
         : base(game)
      {
         GestionnaireDeLaPartie = gestionnairePartie;
         ÉcranDeJeu = écranDeJeu;
      }
      //Temporaire
      public Joueur(Game game)
         : base(game)
      {
      }

      public override void Initialize()
      {
         GestionnaireInput = new InputManager(this.Game);
         //ath = new ATH(Game, this);
         //Game.Components.Add(ath);
         base.Initialize();
      }

      public override void Update(GameTime gameTime)
      {

         base.Update(gameTime);
      }

      public void ModifierActivation()
      {
         EstActif = !EstActif;
      }
      public void ChangerAnimation(TypeActionPersonnage typeAnimation, Personnage personnage)
      {
         string action = "";

         switch (typeAnimation)
         {
            case TypeActionPersonnage.Rien:
               action = "superBoy";
               break;
            case TypeActionPersonnage.Boire:
               action = "superBoyBoire";
               break;
            case TypeActionPersonnage.Lancer:
               action = "superBoyLancer";
               break;
            case TypeActionPersonnage.ApresLancer:
               action = "superBoyApresLancer";
               break;
            case TypeActionPersonnage.ApresBoire:
               action = "superBoyApresBoire";
               break;

         }
         Game.Components.Remove(personnage);
         Game.Components.Insert(14, new Personnage(Game, action, personnage.NomTexture, personnage.NomEffet, personnage.Échelle, personnage.Rotation, personnage.Position));
      }
   }
}
