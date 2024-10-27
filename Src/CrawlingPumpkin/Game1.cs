
// Type: ScreamJam24.Game1
// Assembly: Crawling Pumpkin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6FFA2B6B-8BEA-4A04-9817-EC2892B8097F


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ScreamJam24.Entities;
using ScreamJam24.Managers;
using System;
using System.Threading.Tasks;

#nullable enable
namespace ScreamJam24
{
  public class Game1 : Game
  {
    private 
    #nullable disable
    GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private BackgroundManager _backgroundManager;
    private InputManager _inputManager;
    private AudioManager _audioManager;
    private TextRenderer _textRenderer;
    private Texture2D _letterAtlas;
    private Player _player;
    private Texture2D _playerTexture;
    private float _score;
    private readonly string _infoText;
    private const int ResX = 1280;
    private const int ResY = 720;
    private Crow _crow;
    private Texture2D _crowTexture;
    private Spider _spider;
    private Texture2D _spiderTexture;
    private ZombieHand _zombieHand;
    private Texture2D _zombieHandTexture;
    private Skeleton _skeleton;
    private Texture2D _skeletonTexture;
    private Wisp _wisp;
    private Texture2D _wispTexture;
    private int _timeElapsed;
    private Game1.GameState _gameState;

    // true - for debugging, false - for production :)
    public bool GodMode = true; 

    public Game1()
    {
      this._graphics = new GraphicsDeviceManager((Game) this);
      this._graphics.PreferredBackBufferWidth = 1366;//1280;
      this._graphics.PreferredBackBufferHeight = 768;// 720;
      this._graphics.IsFullScreen = true;
      this.Content.RootDirectory = "Content";
      this.IsMouseVisible = true;
      this._timeElapsed = 0;
      this._gameState = Game1.GameState.Game;
      this._infoText = "You are alone, in a gloomy cemetery... " +
                "Disguised as a Pumpkin??? Leave at once. " +
                "Press SPACEBAR to move towards the exit.";
    }

    protected override void Initialize()
    {
      this._backgroundManager = new BackgroundManager(this.Content);
      this._inputManager = new InputManager();
      this._audioManager = new AudioManager(this.Content);
      this._audioManager.PlaySFX("background");
      base.Initialize();
    }

    protected override void LoadContent()
    {
      this._spriteBatch = new SpriteBatch(this.GraphicsDevice);
      this._playerTexture = this.Content.Load<Texture2D>("Textures/Entities/SpiderPumpkin");
      this._player = new Player(this._playerTexture, new Vector2(576f, 528f), 128, 128);
      this._letterAtlas = this.Content.Load<Texture2D>("Textures/Font/LetterAtlas");
      this._textRenderer = new TextRenderer(this._letterAtlas);
      this._textRenderer.SetFontScale(4);
      this._textRenderer.CondenseLetterSpacing(2);
      this.LoadAssetsAsync();
    }

    public async void LoadAssetsAsync()
    {
      await Task.Run((Action) (() =>
      {
        this._crowTexture = this.Content.Load<Texture2D>("Textures/Entities/Crow");
        this._spiderTexture = this.Content.Load<Texture2D>("Textures/Entities/Spider");
        this._zombieHandTexture = this.Content.Load<Texture2D>("Textures/Entities/ZombieHand");
        this._skeletonTexture = this.Content.Load<Texture2D>("Textures/Entities/Skeleton");
        this._wispTexture = this.Content.Load<Texture2D>("Textures/Entities/Wisp");
      }));
    }

    protected override void Update(GameTime gameTime)
    {
      switch (this._gameState)
      {
        case Game1.GameState.Game:
          bool isMoving = this._inputManager.Update();
          if (isMoving && !this._player.playerWins)
            this._score += 0.1f;
          if ((double) this._score > 0.0 && !this._player.playerWins)
            this._timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

          if ((double) this._score > 350.0)
            this.WinGame();

          if (this._crow != null)
          {
            this._crow.Update();
            this._crow.NormalizeMovement(isMoving);
            if (this._crow.collisionBox.Intersects(this._player.Rect) & isMoving)
              this.EndGame();
            if (this._crow.canBeKilled)
              this._crow = (Crow) null;
          }
          else if ((double) this._score > 20.0 && (double) this._score < 170.0)
          {
            this._crow = new Crow(this._crowTexture, new Vector2(1408f, 100f), 128, 128);
            this._audioManager.PlaySFX("crow");
          }
          Rectangle rect;
          if (this._spider != null)
          {
            this._spider.Update();
            rect = this._spider.Rect;
            if (rect.Intersects(this._player.Rect) & isMoving)
              this.EndGame();
            if (this._spider.canBeKilled)
              this._spider = (Spider) null;
          }
          else if ((double) this._score > 50.0 && (double) this._score < 200.0)
          {
            this._spider = new Spider(this._spiderTexture, 
                new Vector2(576f, (float) sbyte.MinValue), 128, 128);
            this._audioManager.PlaySFX("spider");
          }
          if (this._zombieHand != null)
          {
            this._zombieHand.Update(isMoving);
            rect = this._zombieHand.Rect;

            if (rect.Intersects(this._player.Rect) 
              & isMoving && this._zombieHand.activeFrameIndex == 3)
              this.EndGame();

            if (this._zombieHand.canBeKilled)
              this._zombieHand = (ZombieHand) null;
          }
          else if ((double) this._score > 80.0 && (double) this._score < 230.0)
          {
            this._zombieHand = new ZombieHand(this._zombieHandTexture, 
                new Vector2(1408f, 528f), 128, 128);
            this._audioManager.PlaySFX("zombie");
          }
          if (this._skeleton != null)
          {
            this._skeleton.Update();
            if (this._skeleton.ThrowSFX())
              this._audioManager.PlaySFX("skeleton");
            if (this._skeleton.BoneRect.Intersects(this._player.Rect) & isMoving)
              this.EndGame();
            if (this._skeleton.canBeKilled)
              this._skeleton = (Skeleton) null;
          }
          else if ((double) this._score > 110.0 && (double) this._score < 260.0)
            this._skeleton = new Skeleton(this._skeletonTexture, 
                new Vector2((float) sbyte.MinValue, 528f), 128, 128);

          if (this._wisp != null)
          {
            this._wisp.Update();
            rect = this._wisp.Rect;

            if (rect.Intersects(this._player.Rect) & isMoving)
              this.EndGame();

            if (this._wisp.canBeKilled)
              this._wisp = (Wisp) null;
          }
          else if ((double) this._score > 140.0 && (double) this._score < 290.0)
          {
            this._wisp = new Wisp(this._wispTexture, new Vector2(1280f, 720f), 128, 128);
            this._audioManager.PlaySFX("wisp");
          }
          this._backgroundManager.isMoving = isMoving;
          if (!this._player.playerWins)
            this._backgroundManager.Update();
          else
            this._backgroundManager.UpdateMistOnly();
          this._player.isMoving = isMoving;
          this._player.Update();
          float score = this._score;
          if ((double) score <= 290.0)
          {
            if ((double) score <= 260.0)
            {
              if ((double) score <= 230.0)
              {
                if ((double) score <= 200.0)
                {
                  if ((double) score > 170.0 && this._crow != null)
                  {
                    this._crow.kill = true;
                    break;
                  }
                  break;
                }
                if (this._spider != null)
                {
                  this._spider.kill = true;
                  break;
                }
                break;
              }
              if (this._zombieHand != null)
              {
                this._zombieHand.kill = true;
                break;
              }
              break;
            }
            if (this._skeleton != null)
            {
              this._skeleton.kill = true;
              break;
            }
            break;
          }
          if (this._wisp != null)
          {
            this._wisp.kill = true;
            break;
          }
          break;
        case Game1.GameState.Title:
          if (this._crow != null)
          {
            this._crow.kill = true;
            this._crow.Update();
            if (this._crow.canBeKilled)
              this._crow = (Crow) null;
          }
          if (this._spider != null)
          {
            this._spider.kill = true;
            this._spider.Update();
            if (this._spider.canBeKilled)
              this._spider = (Spider) null;
          }
          if (this._zombieHand != null)
          {
            this._zombieHand.kill = true;
            this._zombieHand.Update(false);
            if (this._zombieHand.canBeKilled)
              this._zombieHand = (ZombieHand) null;
          }
          if (this._skeleton != null)
          {
            this._skeleton.kill = true;
            this._skeleton.Update();
            if (this._skeleton.canBeKilled)
              this._skeleton = (Skeleton) null;
          }
          if (this._wisp != null)
          {
            this._wisp.kill = true;
            this._wisp.Update();
            if (this._wisp.canBeKilled)
              this._wisp = (Wisp) null;
          }
          this._player.Update();
          this._backgroundManager.UpdateMistOnly();

            if (this._player.playerWins)
            {
               // if (Keyboard.GetState().IsKeyDown(Keys.Space))
               // {
               //     this._gameState = Game1.GameState.Game;
               //     this.ResetGame();
               //     break;
               // }
            }

          break;
      }
      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      this.GraphicsDevice.Clear(Color.Black);
      this._spriteBatch.Begin(samplerState: SamplerState.PointClamp);
      this._backgroundManager.Draw(this._spriteBatch);
      this._player.Draw(this._spriteBatch);
      this._crow?.Draw(this._spriteBatch);
      this._spider?.Draw(this._spriteBatch);
      this._zombieHand?.Draw(this._spriteBatch);
      this._skeleton?.Draw(this._spriteBatch);
      this._wisp?.Draw(this._spriteBatch);
      switch (this._gameState)
      {
        case Game1.GameState.Game:
          if ((double) this._score > 20.0 && (double) this._score < 30.0)
            this._textRenderer.DrawStringWrapAroundCentered(this._spriteBatch,
                "Beware of the crow!!!", new Vector2(50f, 310f), 1180);
          if ((double) this._score > 50.0 && (double) this._score < 60.0)
            this._textRenderer.DrawStringWrapAroundCentered(this._spriteBatch,
                "A huge spider!!!", new Vector2(50f, 310f), 1180);
          if ((double) this._score > 80.0 && (double) this._score < 90.0)
            this._textRenderer.DrawStringWrapAroundCentered(this._spriteBatch,
                "What's that on the ground?", new Vector2(50f, 310f), 1180);
          if ((double) this._score > 110.0 && (double) this._score < 120.0)
            this._textRenderer.DrawStringWrapAroundCentered(this._spriteBatch, 
                "Behind you!!!", new Vector2(50f, 310f), 1180);
          if ((double) this._score > 140.0 && (double) this._score < 150.0)
            this._textRenderer.DrawStringWrapAroundCentered(this._spriteBatch,
                "Well, that thing is ugly. Better stay clear...", 
                new Vector2(50f, 310f), 1180);
          if ((double) this._score > 170.0 && (double) this._score < 180.0)
            this._textRenderer.DrawStringWrapAroundCentered(this._spriteBatch,
                "The crow appears to be giving up, but where is that exit?",
                new Vector2(50f, 310f), 1180);
          if ((double) this._score > 200.0 && (double) this._score < 210.0)
            this._textRenderer.DrawStringWrapAroundCentered(this._spriteBatch,
                "The spider's had enough, is the exit close?", new Vector2(50f, 310f), 1180);
          if ((double) this._score > 230.0 && (double) this._score < 240.0)
            this._textRenderer.DrawStringWrapAroundCentered(this._spriteBatch,
                "Now that zombie can hopefully rest in peace, is that the exit?",
                new Vector2(50f, 310f), 1180);
          if ((double) this._score > 260.0 && (double) this._score < 270.0)
            this._textRenderer.DrawStringWrapAroundCentered(this._spriteBatch,
                "The bone will no longer be a problem, but how much further?", 
                new Vector2(50f, 310f), 1180);
          if ((double) this._score > 290.0 && (double) this._score < 300.0)
            this._textRenderer.DrawStringWrapAroundCentered(this._spriteBatch, 
                "That thing was... Is it over?", new Vector2(50f, 310f), 1180);
          if ((double) this._score > 0.0)
          {
            this._textRenderer.DrawString(this._spriteBatch, 
                ((int) this._score).ToString(), 
                new Vector2((float) (640 - this._textRenderer.MeasureString(
                    ((int) this._score).ToString()).Width / 2), 16f));
            break;
          }
          this._textRenderer.DrawStringWrapAroundCentered(this._spriteBatch, 
              this._infoText, new Vector2(50f, 310f), 1180);
          break;
        case Game1.GameState.Title:
          if (!this._player.playerWins)
            this._textRenderer.DrawStringWrapAroundCentered(this._spriteBatch, 
                "Score: " + ((int) this._score).ToString(), new Vector2(50f, 310f), 1180);
          else
            this._textRenderer.DrawStringWrapAroundCentered(this._spriteBatch, 
                "YOU WIN!!! It took you " + (this._timeElapsed / 1000).ToString()
                + " seconds.", new Vector2(50f, 310f), 1180);

          //this._textRenderer.DrawStringWrapAroundCentered(this._spriteBatch,
          //    "Press SPACE to restart.", new Vector2(50f, 360f), 1180);
          break;
      }
      this._spriteBatch.End();
      base.Draw(gameTime);
    }

    public void EndGame()
    {
      if (GodMode)
       return;
      this._gameState = Game1.GameState.Title;
      this._player.BreakPumpkin();
      this._audioManager.PlaySFX("pumpkin");
    }

    public void WinGame()
    {
      this._player.playerWins = true;
      this._gameState = Game1.GameState.Title;
      this._audioManager.PlaySFX("win");
    }

    public void ResetGame()
    {
      this._crow = (Crow) null;
      this._spider = (Spider) null;
      this._zombieHand = (ZombieHand) null;
      this._skeleton = (Skeleton) null;
      this._wisp = (Wisp) null;
      this._score = 0.0f;
      this._player = new Player(this._playerTexture, new Vector2(576f, 528f), 128, 128);
      this._timeElapsed = 0;
    }

    public enum GameState
    {
      Game,
      Title,
    }
  }
}
