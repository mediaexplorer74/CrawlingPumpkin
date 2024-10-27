
// Type: ScreamJam24.Entities.Wisp
// Assembly: Crawling Pumpkin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6FFA2B6B-8BEA-4A04-9817-EC2892B8097F


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScreamJam24.Base;

#nullable disable
namespace ScreamJam24.Entities
{
  internal class Wisp : Sprite
  {
    private const int ResX = 1280;
    private const int ResY = 720;
    private int _animationCounter;
    private int _numFrames;
    private int _bottomBorder;
    private int _rightBorder;
    private bool _isMovingRight;
    private bool _isMovingBottom;
    public int activeFrameIndex;
    private float _speed;
    private Rectangle _activeFrameRect;
    public Rectangle collisionBox = default;
    //private bool _resetOnNext = false;

    public Wisp(Texture2D Texture, Vector2 Position, int Width, int Height)
      : base(Texture, Position, Width, Height)
    {
      this._activeFrameRect = new Rectangle(0, 0, 16, 16);
      this._numFrames = 2;
      this._speed = 3f;
      this._bottomBorder = 720 - Height;
      this._rightBorder = 1280 - Width;
    }

    public void Update()
    {
      ++this._animationCounter;
      if (this._animationCounter == 30)
      {
        ++this.activeFrameIndex;
        if (this.activeFrameIndex >= this._numFrames)
          this.activeFrameIndex = 0;
        this._activeFrameRect = new Rectangle(16 * this.activeFrameIndex, 0, 16, 16);
        this._animationCounter = 0;
      }
      if (this.kill)
      {
        this.Position.Y -= this._speed;
        if ((double) this.Position.Y >= (double) -this.Height)
          return;
        this.canBeKilled = true;
      }
      else
      {
        if ((double) this.Position.Y > (double) this._bottomBorder)
          this._isMovingBottom = false;
        else if ((double) this.Position.Y < 0.0)
          this._isMovingBottom = true;
        if ((double) this.Position.X > (double) this._rightBorder)
          this._isMovingRight = false;
        else if ((double) this.Position.X < 0.0)
          this._isMovingRight = true;
        if (this._isMovingRight)
        {
          this.Position.X += this._speed;
          this._activeFrameRect.Y = 16;
        }
        else
        {
          this.Position.X -= this._speed;
          this._activeFrameRect.Y = 0;
        }
        if (this._isMovingBottom)
          this.Position.Y += this._speed;
        else
          this.Position.Y -= this._speed;
      }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this.Texture, this.Rect, new Rectangle?(this._activeFrameRect), Color.White);
    }
  }
}
