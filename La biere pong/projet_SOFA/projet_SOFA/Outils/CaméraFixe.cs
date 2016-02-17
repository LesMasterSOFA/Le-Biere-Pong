using Microsoft.Xna.Framework;

namespace AtelierXNA
{
    public class CaméraFixe : Caméra
    {
        public CaméraFixe(Game jeu, Vector3 positionCaméra, Vector3 cible, Vector3 orientation)
            : base(jeu)
        {
            this.CréerPointDeVue(positionCaméra, cible, orientation);
            CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
        }
    }
}
