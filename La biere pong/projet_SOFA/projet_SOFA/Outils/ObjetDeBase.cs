using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtelierXNA
{
    public class ObjetDeBase : DrawableGameComponent
    {
        string NomMod�le { get; set; }
        string NomTexture { get; set; }

        RessourcesManager<Model> GestionnaireDeMod�les { get; set; }
        RessourcesManager<Texture2D> GestionTextures { get; set; }

        Cam�ra Cam�raJeu { get; set; }

        public float �chelle { get; set; }

        public Vector3 Rotation { get; set; }

        public Vector3 Position { get; set; }

        protected Model Mod�le { get; set; }
        protected Texture2D TextureMod�le { get; set; }

        protected Matrix[] TransformationsMod�le { get; set; }

        protected Matrix Monde { get; set; }

        public ObjetDeBase(Game jeu, string nomMod�le,string nomTexture, float �chelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale)
            : base(jeu)
        {
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
            Mod�le = GestionnaireDeMod�les.Find(NomMod�le);
            TextureMod�le = GestionTextures.Find(NomTexture);
            TransformationsMod�le = new Matrix[Mod�le.Bones.Count];
            Mod�le.CopyAbsoluteBoneTransformsTo(TransformationsMod�le);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            BlendState blendState = GraphicsDevice.BlendState;
            RasterizerState rasterizerState = GraphicsDevice.RasterizerState;
            DepthStencilState depthStencilState = GraphicsDevice.DepthStencilState;
            foreach (ModelMesh modelMesh in Mod�le.Meshes)
            {
                Matrix mondeLocal = this.TransformationsMod�le[modelMesh.ParentBone.Index] * this.GetMonde();
                foreach (ModelMeshPart modelMeshPart in modelMesh.MeshParts)
                {
                    InitialiserEffet(mondeLocal, (BasicEffect)modelMeshPart.Effect);
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
            effet.EmissiveColor = Vector3.One;
            effet.SpecularColor = new Vector3(0.5f,0.5f,0.5f);
            effet.SpecularPower = 16f;
            //effet.DirectionalLight0.Enabled = true;
            //effet.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
            //effet.DirectionalLight0.Direction = Vector3.Down;
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
