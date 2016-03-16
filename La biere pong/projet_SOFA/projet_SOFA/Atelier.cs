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
   public class Atelier : Microsoft.Xna.Framework.Game
   {
      const float INTERVALLE_CALCUL_FPS = 1f;
      const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
      GraphicsDeviceManager PériphériqueGraphique { get; set; }
      SpriteBatch GestionSprites { get; set; }

      RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }
      RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
      RessourcesManager<Model> GestionnaireDeModèles { get; set; }
      RessourcesManager<Effect> GestionnaireDeShaders { get; set; }
      CaméraSubjective camérajeu { get; set; }
      Vector2 Résolution { get; set; }

      public InputManager GestionInput { get; private set; }
      public Atelier()
      {
         PériphériqueGraphique = new GraphicsDeviceManager(this);
         Content.RootDirectory = "Content";
         PériphériqueGraphique.SynchronizeWithVerticalRetrace = false;
         Résolution = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
         PériphériqueGraphique.PreferMultiSampling = false;
         PériphériqueGraphique.PreferredBackBufferWidth = (int)Résolution.X;
         PériphériqueGraphique.PreferredBackBufferHeight = (int)Résolution.Y;
         //PériphériqueGraphique.IsFullScreen = true;
         IsFixedTimeStep = false;
         IsMouseVisible = true;
      }

      protected override void Initialize()
      {
          //camérajeu = new CaméraSubjective(this, new Vector3(0, 100, 150), new Vector3(0, 80, 0), Vector3.Up, INTERVALLE_MAJ_STANDARD);
          //Components.Add(camérajeu);
          //Services.AddService(typeof(Caméra), camérajeu);

         GestionnaireDeFonts = new RessourcesManager<SpriteFont>(this, "Fonts");
         GestionnaireDeTextures = new RessourcesManager<Texture2D>(this, "Textures");
         GestionnaireDeModèles = new RessourcesManager<Model>(this, "Models");
         GestionnaireDeShaders = new RessourcesManager<Effect>(this, "Effects"); 
         GestionInput = new InputManager(this);

         Components.Add(new Menu(this));
         

         Components.Add(GestionInput);
         Components.Add(new Afficheur3D(this));
         Components.Add(new AfficheurFPS(this, INTERVALLE_CALCUL_FPS));

         
         Services.AddService(typeof(Random), new Random());
         Services.AddService(typeof(RessourcesManager<SpriteFont>), GestionnaireDeFonts);
         Services.AddService(typeof(RessourcesManager<Texture2D>), GestionnaireDeTextures);
         Services.AddService(typeof(RessourcesManager<Model>), GestionnaireDeModèles);
         Services.AddService(typeof(RessourcesManager<Effect>), GestionnaireDeShaders);
         Services.AddService(typeof(InputManager), GestionInput);
         GestionSprites = new SpriteBatch(GraphicsDevice);
         Services.AddService(typeof(SpriteBatch), GestionSprites);
         base.Initialize();
      }

      protected override void Update(GameTime gameTime)
      {
         GérerClavier();
         base.Update(gameTime);
      }

      private void GérerClavier()
      {

         if (GestionInput.EstEnfoncée(Keys.Escape))
         {
            Exit();
         }
      }

      protected override void Draw(GameTime gameTime)
      {
         GraphicsDevice.Clear(Color.White);
         base.Draw(gameTime);
      }
   }
}

