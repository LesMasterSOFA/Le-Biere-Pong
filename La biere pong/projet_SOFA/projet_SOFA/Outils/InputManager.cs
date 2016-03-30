using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace AtelierXNA
{
   public class InputManager : Microsoft.Xna.Framework.GameComponent
   {
      Keys[] AnciennesTouches { get; set; }
      Keys[] NouvellesTouches { get; set; }
      MouseState ÉtatSouris { get; set; }
      MouseState AncienneÉtatSouris { get; set; }
      MouseState NouvelleÉtatSouris { get; set; }
      KeyboardState ÉtatClavier { get; set; }

      public InputManager(Game game)
         : base(game)
      { }

      public override void Initialize()
      {
         AnciennesTouches = new Keys[0];
         NouvellesTouches = new Keys[0];
         ÉtatSouris = Mouse.GetState();
         base.Initialize();
      }

      public override void Update(GameTime gameTime)
      {
         AnciennesTouches = NouvellesTouches;
         ÉtatClavier = Keyboard.GetState();
         NouvellesTouches = ÉtatClavier.GetPressedKeys();
         AncienneÉtatSouris = NouvelleÉtatSouris;
         ÉtatSouris = Mouse.GetState();
         NouvelleÉtatSouris = ÉtatSouris;

         base.Update(gameTime);
      }

      public bool EstClavierActivé
      {
         get { return NouvellesTouches.Length > 0; }
      }

      public bool EstEnfoncée(Keys touche)
      {
         return ÉtatClavier.IsKeyDown(touche);
      }

      public bool EstNouvelleTouche(Keys touche)
      {
         int NbTouches = AnciennesTouches.Length;
         bool EstNouvelleTouche = ÉtatClavier.IsKeyDown(touche);
         int i = 0;
         while (i < NbTouches && EstNouvelleTouche)
         {
            EstNouvelleTouche = AnciennesTouches[i] != touche;
            ++i;
         }
         return EstNouvelleTouche;
      }

      public bool EstSourisActive
      {
         get { return NouvelleÉtatSouris != AncienneÉtatSouris; }
      }
      public bool EstAncienClicDroit()
      {
         return (NouvelleÉtatSouris.RightButton == ButtonState.Pressed && AncienneÉtatSouris.RightButton == ButtonState.Pressed);
      }
      public bool  EstAncienClicGauche()
      {
         return (AncienneÉtatSouris.LeftButton == ButtonState.Pressed && NouvelleÉtatSouris.LeftButton == ButtonState.Pressed);
      }
      public bool EstNouveauClicDroit()
      {
         return (AncienneÉtatSouris.RightButton == ButtonState.Released && NouvelleÉtatSouris.RightButton == ButtonState.Pressed);
      }
      public bool EstNouveauClicGauche()
      {
         return (AncienneÉtatSouris.LeftButton == ButtonState.Released && NouvelleÉtatSouris.LeftButton == ButtonState.Pressed);
      }
      public Point GetPositionSouris()
      {
         return new Point(ÉtatSouris.X, ÉtatSouris.Y);
      }
   }
}