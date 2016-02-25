﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AtelierXNA
{
   class RessourceDeBase<T> : IEquatable<RessourceDeBase<T>>
   {
      public string Nom { get; private set; }
      public T Ressource { get; private set; }
      ContentManager Content { get; set; }
      string Répertoire { get; set; }

      // Ce constructeur est appelé lorsque l'on construit un objet TextureDeBase
      // à partir d'une image déjà présente en mémoire.
      public RessourceDeBase(string nom, T ressource)
      {
         Nom = nom;
         Content = null;
         Répertoire = "";
         Ressource = ressource;
      }

      // Ce constructeur est appelé lorsque l'on construit un objet TextureDeBase
      // à partir du nom d'une image qui sera éventuellement chargé en mémoire.
      public RessourceDeBase(ContentManager content, string répertoire, string nom)
      {
         Nom = nom;
         Content = content;
         Répertoire = répertoire;
         Ressource = default(T);
      }

      public void Load()
      {
         if (Ressource == null)
         {
            string NomComplet = Répertoire + "/" + Nom;
            Ressource = Content.Load<T>(NomComplet);
         }
      }

      #region IEquatable<RessourceDeBase<T>> Membres

      public bool Equals(RessourceDeBase<T> other)
      {
         return Nom == other.Nom;
      }

      #endregion
   }
}
