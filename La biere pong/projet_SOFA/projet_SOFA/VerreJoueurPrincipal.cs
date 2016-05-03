using Microsoft.Xna.Framework;


namespace AtelierXNA
{
    public class VerreJoueurPrincipal : ObjetDeBase
    {
        public VerreJoueurPrincipal(Game game, string nomModèle, string nomTexture, string nomEffet, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale)
            : base(game, nomModèle, nomTexture, nomEffet, échelleInitiale, rotationInitiale, positionInitiale)
        {
        }
    }
}
