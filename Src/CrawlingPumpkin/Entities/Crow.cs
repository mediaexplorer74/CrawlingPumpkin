
// Type: GameManager.Entities.Crow
// Assembly: Crawling Pumpkin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6FFA2B6B-8BEA-4A04-9817-EC2892B8097F


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Base;

//
namespace GameManager.Entities
{
  internal class Crow : Sprite
  {
    private const int ResX = 1280;
    private const int ResY = 720;
    private int _animationCounter;
    private int _activeFrameIndex;
    private int _numFrames;
    private float _speed;
    private readonly float _startY;
    private Rectangle _activeFrameRect;
    private Rectangle _pumpkinRect;
    public Rectangle collisionBox;
    private bool _attackPlayer;

    public Crow(Texture2D Texture, Vector2 Position, int Width, int Height)
      : base(Texture, Position, Width, Height)
    {
      this._activeFrameRect = new Rectangle(0, 0, 16, 16);
      this._numFrames = 2;
      this._speed = 6f;
      this._pumpkinRect = new Rectangle(576, 528, 128, 128);
      this._startY = Position.Y;
    }

    public void Update()
    {
      this.collisionBox = new Rectangle(this.Rect.X + this.Width / 2, this.Rect.Y + this.Height / 2, this.Width / 2, this.Height / 2);
      ++this._animationCounter;
      if (this._animationCounter == 20)
      {
        ++this._activeFrameIndex;
        if (this._activeFrameIndex >= this._numFrames)
          this._activeFrameIndex = 0;
        this._activeFrameRect = new Rectangle(16 * this._activeFrameIndex, 0, 16, 16);
        this._animationCounter = 0;
      }
      this.Position.X -= this._speed;
      if (!this._attackPlayer)
      {
        if (this._activeFrameIndex == 0)
          this.Position.Y -= this._speed / 10f;
        else
          this.Position.Y += this._speed / 10f;
      }
      else if ((double) this.Position.X > (double) this._pumpkinRect.X && (double) this.Position.Y < (double) this._pumpkinRect.Y)
        this.Position.Y += this._speed;
      if ((double) this.Position.X >= (double) -this.Width)
        return;
      this.Position.X = (float) (1280 + this.Width);
      this.Position.Y = this._startY;
      if (!this.kill)
        return;
      this.canBeKilled = true;
    }

    public void NormalizeMovement(bool isMoving)
    {
      if (isMoving)
      {
        this._attackPlayer = true;
        this._speed = 8f;
        if ((double) this.Position.X <= (double) this._pumpkinRect.X)
          return;
        this._activeFrameRect.Y = 16;
      }
      else
      {
        this._attackPlayer = false;
        this._speed = 6f;
        this._activeFrameRect.Y = 0;
      }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this.Texture, this.Rect, new Rectangle?(this._activeFrameRect), Color.White);
    }
  }
}
