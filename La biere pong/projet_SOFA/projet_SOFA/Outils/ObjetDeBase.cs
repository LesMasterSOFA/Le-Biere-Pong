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
        string NomMod�le { get; set; }
        string NomTexture { get; set; }
        string NomEffet { get; set; }

        RessourcesManager<Model> GestionnaireDeMod�les { get; set; }
        RessourcesManager<Texture2D> GestionTextures { get; set; }
        RessourcesManager<Effect> GestionEffets { get; set; }

        Cam�ra Cam�raJeu { get; set; }

        public float �chelle { get; set; }

        public Vector3 Rotation { get; set; }

        public Vector3 Position { get; set; }

        protected Model Mod�le { get; set; }
        protected Texture2D TextureMod�le { get; set; }

        protected Matrix[] TransformationsMod�le { get; set; }

        protected Matrix Monde { get; set; }
        Effect EffetShader { get; set; }

        public ObjetDeBase(Game jeu, string nomMod�le,string nomTexture,string nomEffet, float �chelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale)
            : base(jeu)
        {
            NomEffet = nomEffet;
            NomMod�le = nomMod�le;
            NomTexture = nomTexture;
            Position = positionInitiale;
            �chelle = �chelleInitiale;
            Rotation = rotationInitiale;
        }

        public override void Initialize()
        {
            Monde = Matrix.Identity;
            Monde *= Matrix.CreateScale(�chelle);
            Monde *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
            Monde *= Matrix.CreateTranslation(Position);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Cam�raJeu = Game.Services.GetService(typeof(Cam�ra)) as Cam�ra;
            GestionnaireDeMod�les = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            GestionTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            GestionEffets = Game.Services.GetService(typeof(RessourcesManager<Effect>)) as RessourcesManager<Effect>;
            Mod�le = GestionnaireDeMod�les.Find(NomMod�le);
            TextureMod�le = GestionTextures.Find(NomTexture);
            TransformationsMod�le = new Matrix[Mod�le.Bones.Count];
            Mod�le.CopyAbsoluteBoneTransformsTo(TransformationsMod�le);
            EffetShader = GestionEffets.Find(NomEffet);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            BlendState blendState = GraphicsDevice.BlendState;
            RasterizerState rasterizerState = GraphicsDevice.RasterizerState;
            DepthStencilState depthStencilState = GraphicsDevice.DepthStencilState;
            foreach (ModelMesh modelMesh in Mod�le.Meshes)
            {
                Matrix mondeLocal = TransformationsMod�le[modelMesh.ParentBone.Index] * GetMonde();
                foreach (ModelMeshPart modelMeshPart in modelMesh.MeshParts)
                {
                    modelMeshPart.Effect = EffetShader;
                    EffetShader.Parameters["WorldMatrix"].SetValue(mondeLocal);
                    EffetShader.Parameters["ViewMatrix"].SetValue(Cam�raJeu.Vue);
                    EffetShader.Parameters["ProjectionMatrix"].SetValue(Cam�raJeu.Projection);
                    EffetShader.Parameters["WorldInverseTransposeMatrix"].SetValue(Matrix.Transpose(Matrix.Invert(mondeLocal)));
                    EffetShader.Parameters["ModelTexture"].SetValue(TextureMod�le);
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

            effet.Projection = Cam�raJeu.Projection;
            effet.View = Cam�raJeu.Vue;
            effet.World = mondeLocal;
            effet.TextureEnabled = true;
            effet.Texture = TextureMod�le;
        }

        public virtual Matrix GetMonde()
        {
            return Monde;
        }
    }
}
