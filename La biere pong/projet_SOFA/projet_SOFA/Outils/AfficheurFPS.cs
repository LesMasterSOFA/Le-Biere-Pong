using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AtelierXNA
{
   public class AfficheurFPS : Microsoft.Xna.Framework.GameComponent
   {
      float IntervalleMAJ { get; set; }
      float TempsÉcouléDepuisMAJ { get; set; }
      int CptFrames { get; set; }
      float ValFPS { get; set; }

      public AfficheurFPS(Game game, float intervalleMAJ)
         : base(game)
      {
         IntervalleMAJ = intervalleMAJ;
      }

      public override void Initialize()
      {
         TempsÉcouléDepuisMAJ = 0;
         ValFPS = 0;
         CptFrames = 0;
         base.Initialize();
      }


      public override void Update(GameTime gameTime)
      {
         float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
         ++CptFrames;
         TempsÉcouléDepuisMAJ += tempsÉcoulé;
         if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
         {
            float oldValFPS = ValFPS;
            ValFPS = CptFrames / TempsÉcouléDepuisMAJ;
            if (oldValFPS != ValFPS)
            {
               Game.Window.Title = ValFPS.ToString("0");
            }
            CptFrames = 0;
            TempsÉcouléDepuisMAJ = 0;
         }
         base.Update(gameTime);
      }

   }
}