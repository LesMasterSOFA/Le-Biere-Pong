using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace AtelierXNA
{
    public class Personnage : ObjetDeBase
    {
        AnimationPlayer animationPlayer { get; set; }


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
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
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
