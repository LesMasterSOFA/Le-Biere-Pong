﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;


namespace AtelierXNA
{
    public class Personnage : ObjetDeBase, IActivable
    {
        AnimationPlayer animationPlayer { get; set; }
        Joueur joueur { get; set; }

        TimeSpan time = new TimeSpan(); 

        RessourcesManager<Model> GestionnaireDeModèles { get; set; }

        public Personnage(Game game, string nomModèle, string nomTexture, string nomEffet, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale)
            : base(game, nomModèle, nomTexture, nomEffet, échelleInitiale, rotationInitiale, positionInitiale)
        {
        }

        //Constructeur Sérialiseur
        public Personnage(Game game, InfoPersonnage infoPersonnage)
            :base(game, infoPersonnage.NomModèle, infoPersonnage.NomTexture, infoPersonnage.NomEffet, infoPersonnage.Échelle, infoPersonnage.Rotation, infoPersonnage.Position)
        {
        }
        public override void Initialize()
        {
            joueur = new Joueur(Game);
            base.Initialize();

            // Look up our custom skinning information.
            SkinningData skinningData = Modèle.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            // Create an animation player, and start decoding an animation clip.
            animationPlayer = new AnimationPlayer(skinningData);

            AnimationClip animationLancer = skinningData.AnimationClips["Action"];
            //AnimationClip animationBoire = skinningData.AnimationClips["DrinkAction"];
            animationPlayer.StartClip(animationLancer);
        }

        public override void Update(GameTime gameTime)
        {
            animationPlayer.Update(gameTime.ElapsedGameTime, true, Monde);
            time += gameTime.ElapsedGameTime;
            if (time >= animationPlayer.CurrentClip.Duration)
            {
                switch (NomModèle)
                {
                    case "superBoyLancer":
                        joueur.ChangerAnimation(TypeActionPersonnage.ApresLancer, this);
                        break;
                    case "superBoyBoire":
                        joueur.ChangerAnimation(TypeActionPersonnage.ApresBoire, this);
                        break;
                }
            }
            if (time.Seconds >= animationPlayer.CurrentClip.Duration.Seconds + 1 && NomModèle != "superBoy")
            {
                joueur.ChangerAnimation(TypeActionPersonnage.Rien, this);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            int noDrawOrder = 0;
            foreach (GameComponent item in Game.Components)
            {
                if (item is DrawableGameComponent)
                {
                    ((DrawableGameComponent)item).DrawOrder = noDrawOrder++;
                }
            }
            Matrix[] bones = animationPlayer.GetSkinTransforms();
            foreach (ModelMesh mesh in Modèle.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);

                    effect.View = CaméraJeu.Vue;
                    effect.Projection = CaméraJeu.Projection;

                    effect.EnableDefaultLighting();

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;

                    effect.Texture = TextureModèle;
                }

                mesh.Draw();
            }
        }
        public void ModifierActivation()
        {
            if ((Game.Components.ToList().Find(item => item is GestionEnvironnement) as GestionEnvironnement).TypeDePartie != TypePartie.LAN)
                Enabled = !Enabled;
        }
    }


    [Serializable]
    public class InfoPersonnage
    {
        public string NomModèle { get; private set; }
        public string NomTexture { get; private set; }
        public string NomEffet { get; private set; }
        public float Échelle { get; private set; }
        public Vector3 Rotation { get; private set; }
        public Vector3 Position { get; private set; }

        public InfoPersonnage(string nomModèle, string nomTexture, string nomEffet, float échelle, Vector3 rotation, Vector3 position)
        {
            NomModèle = nomModèle;
            NomTexture = nomTexture;
            NomEffet = nomEffet;
            Échelle = échelle;
            Rotation = rotation;
            Position = position;
        }
    }
}
