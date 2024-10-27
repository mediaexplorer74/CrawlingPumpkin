
// Type: GameManager.Game1
// Assembly: Crawling Pumpkin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6FFA2B6B-8BEA-4A04-9817-EC2892B8097F


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameManager.Entities;
using GameManager.Managers;
using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input.Touch;


namespace GameManager
{
  public class Game1 : Game
  {
    public bool GodMode = false; // set it *false* for W10M (or *true* to do PC debugging...)

    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    Vector2 baseScreenSize = new Vector2(1280, 720);
    private Matrix globalTransformation;
    int backbufferWidth, backbufferHeight;
    private BackgroundManager backgroundManager;
    private InputManager inputManager;
    private AudioManager audioManager;
    private TextRenderer textRenderer;
    private Texture2D letterAtlas;
    private Player player;
    private Texture2D playerTexture;
    private float score;
    private readonly string infoText;
    private const int ResX = 1280;
    private const int ResY = 720;
    private Crow crow;
    private Texture2D crowTexture;
    private Spider spider;
    private Texture2D spiderTexture;
    private ZombieHand zombieHand;
    private Texture2D zombieHandTexture;
    private Skeleton skeleton;
    private Texture2D skeletonTexture;
    private Wisp wisp;
    private Texture2D wispTexture;
    private int timeElapsed;
    private Game1.GameState gameState;

    
    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);

#if WINDOWS_PHONE
      TargetElapsedTime = TimeSpan.FromTicks(333333);
#endif
 
      graphics.IsFullScreen = true;//set it *true* for W10M (or *false* to do PC debugging...)

      graphics.PreferredBackBufferWidth = 1280;
      graphics.PreferredBackBufferHeight = 720;

      graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft
                | DisplayOrientation.LandscapeRight;// | DisplayOrientation.Portrait;

      this.IsMouseVisible = true;

      this.Content.RootDirectory = "Content";

      this.timeElapsed = 0;
      this.gameState = Game1.GameState.Game;
      this.infoText = "You are alone, in a gloomy cemetery... " +
                "Disguised as a Pumpkin??? Leave at once. " +
                "Hit space / touch screen to move towards the exit.";
    }

    protected override void Initialize()
    {

      this.backgroundManager = new BackgroundManager(this.Content);
      this.inputManager = new InputManager();
      this.audioManager = new AudioManager(this.Content);
      this.audioManager.PlaySFX("background");
      base.Initialize();
    }

    protected override void LoadContent()
    {
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
      this.playerTexture = this.Content.Load<Texture2D>("Textures/Entities/SpiderPumpkin");
      this.player = new Player(this.playerTexture, new Vector2(576f, 528f), 128, 128);
      this.letterAtlas = this.Content.Load<Texture2D>("Textures/Font/LetterAtlas");
      this.textRenderer = new TextRenderer(this.letterAtlas);
      this.textRenderer.SetFontScale(4);
      this.textRenderer.CondenseLetterSpacing(2);
      this.LoadAssetsAsync();
    }

    public async void LoadAssetsAsync()
    {
      await Task.Run((Action) (() =>
      {
        this.crowTexture = this.Content.Load<Texture2D>("Textures/Entities/Crow");
        this.spiderTexture = this.Content.Load<Texture2D>("Textures/Entities/Spider");
        this.zombieHandTexture = this.Content.Load<Texture2D>("Textures/Entities/ZombieHand");
        this.skeletonTexture = this.Content.Load<Texture2D>("Textures/Entities/Skeleton");
        this.wispTexture = this.Content.Load<Texture2D>("Textures/Entities/Wisp");
      }));
    }


    // Update
    protected override void Update(GameTime gameTime)
    {

      if (backbufferHeight != GraphicsDevice.PresentationParameters.BackBufferHeight ||
            backbufferWidth != GraphicsDevice.PresentationParameters.BackBufferWidth)
      {
        ScalePresentationArea();
      }

      switch (this.gameState)
      {
        case Game1.GameState.Game:
          bool isMoving = this.inputManager.Update();
          if (isMoving && !this.player.playerWins)
            this.score += 0.1f;
          if ((double) this.score > 0.0 && !this.player.playerWins)
            this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

          if ((double) this.score > 350.0)
            this.WinGame();

          if (this.crow != null)
          {
            this.crow.Update();
            this.crow.NormalizeMovement(isMoving);
            
            if (this.crow.collisionBox.Intersects(this.player.Rect) & isMoving)
              this.EndGame();

            if (this.crow.canBeKilled)
              this.crow = (Crow) null;
          }
          else if ((double) this.score > 20.0 && (double) this.score < 170.0)
          {
            this.crow = new Crow(this.crowTexture, new Vector2(1408f, 100f), 128, 128);
            this.audioManager.PlaySFX("crow");
          }
          Rectangle rect;
          if (this.spider != null)
          {
            this.spider.Update();
            rect = this.spider.Rect;
            if (rect.Intersects(this.player.Rect) & isMoving)
              this.EndGame();
            if (this.spider.canBeKilled)
              this.spider = (Spider) null;
          }
          else if ((double) this.score > 50.0 && (double) this.score < 200.0)
          {
            this.spider = new Spider(this.spiderTexture, 
                new Vector2(576f, (float) sbyte.MinValue), 128, 128);
            this.audioManager.PlaySFX("spider");
          }
          if (this.zombieHand != null)
          {
            this.zombieHand.Update(isMoving);
            rect = this.zombieHand.Rect;

            if (rect.Intersects(this.player.Rect) 
              & isMoving && this.zombieHand.activeFrameIndex == 3)
              this.EndGame();

            if (this.zombieHand.canBeKilled)
              this.zombieHand = (ZombieHand) null;
          }
          else if ((double) this.score > 80.0 && (double) this.score < 230.0)
          {
            this.zombieHand = new ZombieHand(this.zombieHandTexture, 
                new Vector2(1408f, 528f), 128, 128);
            this.audioManager.PlaySFX("zombie");
          }
          if (this.skeleton != null)
          {
            this.skeleton.Update();
            if (this.skeleton.ThrowSFX())
              this.audioManager.PlaySFX("skeleton");
            if (this.skeleton.BoneRect.Intersects(this.player.Rect) & isMoving)
              this.EndGame();
            if (this.skeleton.canBeKilled)
              this.skeleton = (Skeleton) null;
          }
          else if ((double) this.score > 110.0 && (double) this.score < 260.0)
            this.skeleton = new Skeleton(this.skeletonTexture, 
                new Vector2((float) sbyte.MinValue, 528f), 128, 128);

          if (this.wisp != null)
          {
            this.wisp.Update();
            rect = this.wisp.Rect;

            if (rect.Intersects(this.player.Rect) & isMoving)
              this.EndGame();

            if (this.wisp.canBeKilled)
              this.wisp = (Wisp) null;
          }
          else if ((double) this.score > 140.0 && (double) this.score < 290.0)
          {
            this.wisp = new Wisp(this.wispTexture, new Vector2(1280f, 720f), 128, 128);
            this.audioManager.PlaySFX("wisp");
          }
          this.backgroundManager.isMoving = isMoving;
          if (!this.player.playerWins)
            this.backgroundManager.Update();
          else
            this.backgroundManager.UpdateMistOnly();
          this.player.isMoving = isMoving;
          this.player.Update();
          float score = this.score;
          if ((double) score <= 290.0)
          {
            if ((double) score <= 260.0)
            {
              if ((double) score <= 230.0)
              {
                if ((double) score <= 200.0)
                {
                  if ((double) score > 170.0 && this.crow != null)
                  {
                    this.crow.kill = true;
                    break;
                  }
                  break;
                }
                if (this.spider != null)
                {
                  this.spider.kill = true;
                  break;
                }
                break;
              }
              if (this.zombieHand != null)
              {
                this.zombieHand.kill = true;
                break;
              }
              break;
            }
            if (this.skeleton != null)
            {
              this.skeleton.kill = true;
              break;
            }
            break;
          }
          if (this.wisp != null)
          {
            this.wisp.kill = true;
            break;
          }
          break;
        case Game1.GameState.Title:
          if (this.crow != null)
          {
            this.crow.kill = true;
            this.crow.Update();
            if (this.crow.canBeKilled)
              this.crow = (Crow) null;
          }
          if (this.spider != null)
          {
            this.spider.kill = true;
            this.spider.Update();
            if (this.spider.canBeKilled)
              this.spider = (Spider) null;
          }
          if (this.zombieHand != null)
          {
            this.zombieHand.kill = true;
            this.zombieHand.Update(false);
            if (this.zombieHand.canBeKilled)
              this.zombieHand = (ZombieHand) null;
          }
          if (this.skeleton != null)
          {
            this.skeleton.kill = true;
            this.skeleton.Update();
            if (this.skeleton.canBeKilled)
              this.skeleton = (Skeleton) null;
          }
          if (this.wisp != null)
          {
            this.wisp.kill = true;
            this.wisp.Update();
            if (this.wisp.canBeKilled)
              this.wisp = (Wisp) null;
          }
          this.player.Update();
          this.backgroundManager.UpdateMistOnly();

            if (this.gameState == Game1.GameState.Title)//(this.player.playerWins)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space) || TouchPanel.GetState().Count > 0)
                {
                    this.gameState = Game1.GameState.Game;
                    this.ResetGame();
                    break;
                }
            }

          break;
      }
      base.Update(gameTime);
    }//


    // Draw
    protected override void Draw(GameTime gameTime)
    {
      this.GraphicsDevice.Clear(Color.Black);

      //this.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
      this.spriteBatch.Begin(SpriteSortMode.Deferred/*.BackToFront*/, null, 
          samplerState: SamplerState.PointClamp,
          null, null, null, globalTransformation);

      this.backgroundManager.Draw(this.spriteBatch);
      this.player.Draw(this.spriteBatch);
      this.crow?.Draw(this.spriteBatch);
      this.spider?.Draw(this.spriteBatch);
      this.zombieHand?.Draw(this.spriteBatch);
      this.skeleton?.Draw(this.spriteBatch);
      this.wisp?.Draw(this.spriteBatch);
      switch (this.gameState)
      {
        case Game1.GameState.Game:
          if ((double) this.score > 20.0 && (double) this.score < 30.0)
            this.textRenderer.DrawStringWrapAroundCentered(this.spriteBatch,
                "Beware of the crow!!!", new Vector2(50f, 310f), 1180);
          if ((double) this.score > 50.0 && (double) this.score < 60.0)
            this.textRenderer.DrawStringWrapAroundCentered(this.spriteBatch,
                "A huge spider!!!", new Vector2(50f, 310f), 1180);
          if ((double) this.score > 80.0 && (double) this.score < 90.0)
            this.textRenderer.DrawStringWrapAroundCentered(this.spriteBatch,
                "What's that on the ground?", new Vector2(50f, 310f), 1180);
          if ((double) this.score > 110.0 && (double) this.score < 120.0)
            this.textRenderer.DrawStringWrapAroundCentered(this.spriteBatch, 
                "Behind you!!!", new Vector2(50f, 310f), 1180);
          if ((double) this.score > 140.0 && (double) this.score < 150.0)
            this.textRenderer.DrawStringWrapAroundCentered(this.spriteBatch,
                "Well, that thing is ugly. Better stay clear...", 
                new Vector2(50f, 310f), 1180);
          if ((double) this.score > 170.0 && (double) this.score < 180.0)
            this.textRenderer.DrawStringWrapAroundCentered(this.spriteBatch,
                "The crow appears to be giving up, but where is that exit?",
                new Vector2(50f, 310f), 1180);
          if ((double) this.score > 200.0 && (double) this.score < 210.0)
            this.textRenderer.DrawStringWrapAroundCentered(this.spriteBatch,
                "The spider's had enough, is the exit close?", new Vector2(50f, 310f), 1180);
          if ((double) this.score > 230.0 && (double) this.score < 240.0)
            this.textRenderer.DrawStringWrapAroundCentered(this.spriteBatch,
                "Now that zombie can hopefully rest in peace, is that the exit?",
                new Vector2(50f, 310f), 1180);
          if ((double) this.score > 260.0 && (double) this.score < 270.0)
            this.textRenderer.DrawStringWrapAroundCentered(this.spriteBatch,
                "The bone will no longer be a problem, but how much further?", 
                new Vector2(50f, 310f), 1180);
          if ((double) this.score > 290.0 && (double) this.score < 300.0)
            this.textRenderer.DrawStringWrapAroundCentered(this.spriteBatch, 
                "That thing was... Is it over?", new Vector2(50f, 310f), 1180);
          if ((double) this.score > 0.0)
          {
            this.textRenderer.DrawString(this.spriteBatch, 
                ((int) this.score).ToString(), 
                new Vector2((float) (640 - this.textRenderer.MeasureString(
                    ((int) this.score).ToString()).Width / 2), 16f));
            break;
          }
          this.textRenderer.DrawStringWrapAroundCentered(this.spriteBatch, 
              this.infoText, new Vector2(50f, 310f), 1180);
          break;
        case Game1.GameState.Title:
          if (!this.player.playerWins)
            this.textRenderer.DrawStringWrapAroundCentered(this.spriteBatch, 
                "Score: " + ((int) this.score).ToString(), new Vector2(50f, 310f), 1180);
          else
            this.textRenderer.DrawStringWrapAroundCentered(this.spriteBatch, 
                "YOU WIN!!! It took you " + (this.timeElapsed / 1000).ToString()
                + " seconds.", new Vector2(50f, 310f), 1180);

          //this.textRenderer.DrawStringWrapAroundCentered(this.spriteBatch,
          //    "Press SPACE to restart.", new Vector2(50f, 360f), 1180);
          break;
      }
      this.spriteBatch.End();
      base.Draw(gameTime);
    }

    public void EndGame()
    {
      if (GodMode)
       return;
      this.gameState = Game1.GameState.Title;
      this.player.BreakPumpkin();
      this.audioManager.PlaySFX("pumpkin");
    }

    public void WinGame()
    {
      this.player.playerWins = true;
      this.gameState = Game1.GameState.Title;
      this.audioManager.PlaySFX("win");
    }

    public void ResetGame()
    {
      this.crow = (Crow) null;
      this.spider = (Spider) null;
      this.zombieHand = (ZombieHand) null;
      this.skeleton = (Skeleton) null;
      this.wisp = (Wisp) null;
      this.score = 0.0f;
      this.player = new Player(this.playerTexture, new Vector2(576f, 528f), 128, 128);
      this.timeElapsed = 0;
    }


    //!
    public void ScalePresentationArea()
    {
        //Work out how much we need to scale our graphics to fill the screen
        backbufferWidth = GraphicsDevice.PresentationParameters.BackBufferWidth - 0; // 40 - dirty hack for Astoria!
        backbufferHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

        float horScaling = backbufferWidth / baseScreenSize.X;
        float verScaling = backbufferHeight / baseScreenSize.Y;

        Vector3 screenScalingFactor = new Vector3(horScaling, verScaling, 1);

        globalTransformation = Matrix.CreateScale(screenScalingFactor);

        System.Diagnostics.Debug.WriteLine("Screen Size - Width["
            + GraphicsDevice.PresentationParameters.BackBufferWidth + "] " +
            "Height [" + GraphicsDevice.PresentationParameters.BackBufferHeight + "]");
    }//Scale...



    public enum GameState
    {
      Game,
      Title,
    }
  }
}
