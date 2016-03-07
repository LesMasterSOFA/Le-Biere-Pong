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
        string NomModèle { get; set; }
        string NomTexture { get; set; }
        string NomEffet { get; set; }

        RessourcesManager<Model> GestionnaireDeModèles { get; set; }
        RessourcesManager<Texture2D> GestionTextures { get; set; }
        RessourcesManager<Effect> GestionEffets { get; set; }

        Caméra CaméraJeu { get; set; }

        public float Échelle { get; set; }

        public Vector3 Rotation { get; set; }

        public Vector3 Position { get; set; }

        protected Model Modèle { get; set; }
        protected Texture2D TextureModèle { get; set; }

        protected Matrix[] TransformationsModèle { get; set; }

        protected Matrix Monde { get; set; }
        Effect EffetShader { get; set; }

        public ObjetDeBase(Game jeu, string nomModèle,string nomTexture,string nomEffet, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale)
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
            TransformationsModèle = new Matrix[Modèle.Bones.Count];
            Modèle.CopyAbsoluteBoneTransformsTo(TransformationsModèle);
            EffetShader = GestionEffets.Find(NomEffet);
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
                    //InitialiserEffet(mondeLocal, (BasicEffect)modelMeshPart.Effect);
                }
                modelMesh.Draw();
            }
            GraphicsDevice.BlendState = blendState;
            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.DepthStencilState = depthStencilState;
            base.Draw(gameTime);
        }
        void InitialiserEffet(Matrix mondeLocal, BasicEffect effet)
        {

            effet.EnableDefaultLighting();
            effet.LightingEnabled = true; // turn on the lighting subsystem.

            effet.EmissiveColor = new Vector3(0.8f, 0.8f, 0.8f);
            effet.SpecularColor = new Vector3(0.2f, 0.2f, 0.2f);
            effet.SpecularPower = 10f;

            effet.DirectionalLight0.Enabled = true;
            effet.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
            effet.DirectionalLight0.Direction = new Vector3((float)Math.Cos(0), -1, (float)Math.Sin(0));

            effet.DirectionalLight1.Enabled = true;
            effet.DirectionalLight1.DiffuseColor = Color.White.ToVector3();
            effet.DirectionalLight1.Direction = new Vector3((float)Math.Cos(MathHelper.TwoPi / 3), -1, (float)Math.Sin(MathHelper.TwoPi / 3));

            effet.DirectionalLight2.Enabled = true;
            effet.DirectionalLight2.DiffuseColor = Color.White.ToVector3();
            effet.DirectionalLight2.Direction = new Vector3((float)Math.Cos(2 * MathHelper.TwoPi / 3), -1, (float)Math.Sin(2*MathHelper.TwoPi / 3));

            effet.Projection = CaméraJeu.Projection;
            effet.View = CaméraJeu.Vue;
            effet.World = mondeLocal;
            effet.TextureEnabled = true;
            effet.Texture = TextureModèle;
        }

        public virtual Matrix GetMonde()
        {
            return Monde;
        }
    }
}
