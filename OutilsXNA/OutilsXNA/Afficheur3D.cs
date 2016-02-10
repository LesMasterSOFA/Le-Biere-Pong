using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace AtelierXNA
{
   public class Afficheur3D : DrawableGameComponent
   {
      InputManager GestionInput { get; set; }
      DepthStencilState JeuDepthBufferState { get; set; }
      RasterizerState JeuRasterizerState { get; set; }
      BlendState JeuBlendState { get; set; }


      bool estAffichéEnWireframe;
      bool EstAffichéEnWireframe
      {
         get { return estAffichéEnWireframe; }
         set
         {
            JeuRasterizerState = new RasterizerState();
            estAffichéEnWireframe = value;
            JeuRasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            JeuRasterizerState.FillMode = estAffichéEnWireframe ? FillMode.WireFrame : FillMode.Solid;
            Game.GraphicsDevice.RasterizerState = JeuRasterizerState;
         }
      }

      public Afficheur3D(Game jeu)
         : base(jeu)
      { }

      public override void Initialize()
      {
         JeuDepthBufferState = new DepthStencilState();
         JeuDepthBufferState.DepthBufferEnable = true;
         JeuRasterizerState = new RasterizerState();
         JeuRasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
         JeuBlendState = BlendState.Opaque;
         base.Initialize();
      }

      protected override void LoadContent()
      {
         GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
      }

      public override void Update(GameTime gameTime)
      {
         GestionClavier();
      }

      public override void Draw(GameTime gameTime)
      {
         GraphicsDevice.DepthStencilState = JeuDepthBufferState;
         GraphicsDevice.RasterizerState = JeuRasterizerState;
         GraphicsDevice.BlendState = JeuBlendState;
         GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
      }

      void GestionClavier()
      {
         if (GestionInput.EstNouvelleTouche(Keys.F))
         {
            EstAffichéEnWireframe = !EstAffichéEnWireframe;
         }
      }
   }
}