using Microsoft.Xna.Framework;


namespace AtelierXNA
{
    public class PartieSolo : Partie
    {
        protected ATH ath { get; set; }
        public Joueur JoueurPrincipal { get; set; }

        public PartieSolo(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            JoueurPrincipal = new Joueur(Game, base.gestionnairePartie, Game.GraphicsDevice.Viewport);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
