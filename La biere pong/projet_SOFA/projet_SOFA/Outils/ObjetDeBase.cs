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
    public class ObjetDeBase : DrawableGameComponent
    {
        public string NomModèle { get; private set; }
        public string NomTexture { get; private set; }
        public string NomEffet { get; private set; }

        RessourcesManager<Model> GestionnaireDeModèles { get; set; }
        RessourcesManager<Texture2D> GestionTextures { get; set; }
        RessourcesManager<Effect> GestionEffets { get; set; }

        public Caméra CaméraJeu { get; protected set; }

        public float Échelle { get; protected set; }

        public Vector3 Rotation { get; protected set; }

        public Vector3 Position { get; protected set; }

        public Model Modèle { get; protected set; }
        public Texture2D TextureModèle { get; protected set; }
        public Effect EffetShader { get; protected set; }

        public Matrix[] TransformationsModèle { get; protected set; }

        public Matrix Monde { get; protected set; }

        public ObjetDeBase(Game jeu, string nomModèle,string nomTexture, string nomEffet, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale)
            : base(jeu)
        {
            NomEffet = nomEffet;
            NomModèle = nomModèle;
            NomTexture = nomTexture;
            Position = positionInitiale;
            Échelle = échelleInitiale;
            Rotation = rotationInitiale;
        }

        public override void Initialize()
        {
            Monde = Matrix.Identity;
            Monde *= Matrix.CreateScale(Échelle);
            Monde *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
            Monde *= Matrix.CreateTranslation(Position);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            CaméraJeu = Game.Services.GetService(typeof(Caméra)) as Caméra;
            GestionnaireDeModèles = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            GestionTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            GestionEffets = Game.Services.GetService(typeof(RessourcesManager<Effect>)) as RessourcesManager<Effect>;
            Modèle = GestionnaireDeModèles.Find(NomModèle);
            TextureModèle = GestionTextures.Find(NomTexture);
            EffetShader = GestionEffets.Find(NomEffet);
            TransformationsModèle = new Matrix[Modèle.Bones.Count];
            Modèle.CopyAbsoluteBoneTransformsTo(TransformationsModèle);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            BlendState blendState = GraphicsDevice.BlendState;
            RasterizerState rasterizerState = GraphicsDevice.RasterizerState;
            DepthStencilState depthStencilState = GraphicsDevice.DepthStencilState;
            foreach (ModelMesh modelMesh in Modèle.Meshes)
            {
                Matrix mondeLocal = TransformationsModèle[modelMesh.ParentBone.Index] * GetMonde();
                foreach (ModelMeshPart modelMeshPart in modelMesh.MeshParts)
                {
                    modelMeshPart.Effect = EffetShader;
                    EffetShader.Parameters["WorldMatrix"].SetValue(mondeLocal);
                    EffetShader.Parameters["ViewMatrix"].SetValue(CaméraJeu.Vue);
                    EffetShader.Parameters["ProjectionMatrix"].SetValue(CaméraJeu.Projection);
                    EffetShader.Parameters["WorldInverseTransposeMatrix"].SetValue(Matrix.Transpose(Matrix.Invert(mondeLocal)));
                    EffetShader.Parameters["ModelTexture"].SetValue(TextureModèle);
                }
                modelMesh.Draw();
            }
            GraphicsDevice.BlendState = blendState;
            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.DepthStencilState = depthStencilState;
            base.Draw(gameTime);
        }
        public virtual Matrix GetMonde()
        {
            return Monde;
        }
    }
}
