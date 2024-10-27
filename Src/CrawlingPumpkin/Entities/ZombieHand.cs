
// Type: GameManager.Entities.ZombieHand
// Assembly: Crawling Pumpkin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6FFA2B6B-8BEA-4A04-9817-EC2892B8097F


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Base;

//
namespace GameManager.Entities
{
  internal class ZombieHand : Sprite
  {
    private const int ResX = 1280;
    private const int ResY = 720;
    private int _animationCounter;
    private int _numFrames;
    public int activeFrameIndex;
    private float _speed;
    private Rectangle _activeFrameRect;
    public Rectangle collisionBox = default;
    private bool _resetOnNext;

    public ZombieHand(Texture2D Texture, Vector2 Position, int Width, int Height)
      : base(Texture, Position, Width, Height)
    {
      this._activeFrameRect = new Rectangle(0, 0, 16, 16);
      this._numFrames = 4;
      this._speed = 3f;
    }

    public void Update(bool isMoving)
    {
      ++this._animationCounter;
      if (this._animationCounter == 60)
      {
        if (this._resetOnNext)
        {
          this._resetOnNext = false;
          this.activeFrameIndex = 0;
        }
        else
        {
          ++this.activeFrameIndex;
          if (this.activeFrameIndex >= this._numFrames)
          {
            this.activeFrameIndex = 1;
            this._resetOnNext = true;
          }
        }
        this._activeFrameRect = new Rectangle(16 * this.activeFrameIndex, 0, 16, 16);
        this._animationCounter = 0;
      }
      if (isMoving)
        this.Position.X -= this._speed;
      if ((double) this.Position.X >= (double) -this.Width)
        return;
      if (this.kill)
        this.canBeKilled = true;
      this.Position.X = (float) (1280 + this.Width);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this.Texture, this.Rect, new Rectangle?(this._activeFrameRect), Color.White);
    }
  }
}
