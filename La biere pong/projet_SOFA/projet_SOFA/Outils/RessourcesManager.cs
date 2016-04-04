using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
   class RessourcesManager<T>
   {
      Game Jeu { get; set; }
      string RépertoireRessources { get; set; }
      List<RessourceDeBase<T>> ListeRessources { get; set; }

      public RessourcesManager(Game jeu, string répertoireRessources)
      {
         Jeu = jeu;
         RépertoireRessources = répertoireRessources;
         ListeRessources = new List<RessourceDeBase<T>>();
      }

      public void Add(string nom, T ressourceÀAjouter)
      {
         RessourceDeBase<T> RessourceÀAjouter = new RessourceDeBase<T>(nom, ressourceÀAjouter);
         if (!ListeRessources.Contains(RessourceÀAjouter))
         {
            ListeRessources.Add(RessourceÀAjouter);
         }
      }

      void Add(RessourceDeBase<T> ressourceÀAjouter)
      {
         ressourceÀAjouter.Load();
         ListeRessources.Add(ressourceÀAjouter);
      }

      public T Find(string nomRessource)
      {
         const int RESSOURCE_PAS_TROUVÉE = -1;
         RessourceDeBase<T> ressourceÀRechercher = new RessourceDeBase<T>(Jeu.Content, RépertoireRessources, nomRessource);
         int indexRessource = ListeRessources.IndexOf(ressourceÀRechercher);
         if (indexRessource == RESSOURCE_PAS_TROUVÉE)
         {
            Add(ressourceÀRechercher);
            indexRessource = ListeRessources.Count - 1;
         }
         return ListeRessources[indexRessource].Ressource;
      }
   }
}
