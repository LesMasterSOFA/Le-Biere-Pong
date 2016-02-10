using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtelierXNA
{
    public class ObjetDeBase : DrawableGameComponent
    {
        string NomModèle { get; set; }
        string NomTexture { get; set; }

        RessourcesManager<Model> GestionnaireDeModèles { get; set; }
        RessourcesManager<Texture2D> GestionTextures { get; set; }

        Caméra CaméraJeu { get; set; }

        public float Échelle { get; set; }

        public Vector3 Rotation { get; set; }

        public Vector3 Position { get; set; }

        protected Model Modèle { get; set; }
        protected Texture2D TextureModèle { get; set; }

        protected Matrix[] TransformationsModèle { get; set; }

        protected Matrix Monde { get; set; }

        public ObjetDeBase(Game jeu, string nomModèle,string nomTexture, float échelleInitiale, Vector3 rotationInitiale, Vector3 positionInitiale)
            : base(jeu)
        {
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
            Modèle = GestionnaireDeModèles.Find(NomModèle);
            TextureModèle = GestionTextures.Find(NomTexture);
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
                Matrix mondeLocal = this.TransformationsModèle[modelMesh.ParentBone.Index] * this.GetMonde();
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

            effet.EmissiveColor = Vector3.One;
            effet.Projection = CaméraJeu.Projection;
            effet.View = CaméraJeu.Vue;
            effet.World = mondeLocal;
            effet.EnableDefaultLighting();
            effet.TextureEnabled = true;
            effet.Texture = TextureModèle; 
            effet.LightingEnabled = true; // turn on the lighting subsystem.
            effet.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0, 0); // a red light
            effet.DirectionalLight0.Direction = new Vector3(1, 0, 0);  // coming along the x-axis
            effet.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0);
        }

        public virtual Matrix GetMonde()
        {
            return Monde;
        }
    }
}
