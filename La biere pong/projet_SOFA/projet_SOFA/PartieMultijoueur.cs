using Microsoft.Xna.Framework;


namespace AtelierXNA
{
    public class PartieMultijoueur : Partie
    {
        public JoueurMultijoueur JoueurPrincipal { get; set; }
        public JoueurMultijoueur JoueurSecondaire { get; protected set; }

        public PartieMultijoueur(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
