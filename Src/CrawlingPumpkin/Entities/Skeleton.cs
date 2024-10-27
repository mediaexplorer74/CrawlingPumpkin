
// Type: ScreamJam24.Entities.Skeleton
// Assembly: Crawling Pumpkin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6FFA2B6B-8BEA-4A04-9817-EC2892B8097F


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScreamJam24.Base;

#nullable disable
namespace ScreamJam24.Entities
{
  internal class Skeleton : Sprite
  {
    private const int ResX = 1280;
    private const int ResY = 720;
    private int _animationCounter;
    private int _numFrames;
    private int _activeFrameIndex;
    private int _boneFrameIndex;
    private float _speed;
    private Rectangle _activeFrameRect;
    private Rectangle _boneFrameRect;
    private Rectangle _boneTargetRect;
    public Rectangle BoneRect;
    private bool _hasMovedIn;
    private bool _hasMovedOut;
    private bool _throwBone;
    private bool _throwSFX;

    public Skeleton(Texture2D Texture, Vector2 Position, int Width, int Height)
      : base(Texture, Position, Width, Height)
    {
      this._activeFrameRect = new Rectangle(0, 0, 16, 16);
      this._boneFrameRect = new Rectangle(0, 16, 16, 16);
      this._numFrames = 3;
      this._speed = 1f;
      this._boneTargetRect = new Rectangle(this.Rect.X + Width, this.Rect.Y, Width, Height);
      this.BoneRect = new Rectangle(this._boneTargetRect.X + Width / 4, this._boneTargetRect.Y + Height / 4, 8, 8);
    }

    public void Update()
    {
      if (!this._hasMovedIn)
      {
        this.Position.X += this._speed;
        if ((double) this.Position.X + (double) this.Width >= (double) this.Width)
          this._hasMovedIn = true;
      }
      else if (!this._throwBone)
      {
        ++this._animationCounter;
        if (this._animationCounter == 30)
        {
          if (this._activeFrameIndex < this._numFrames - 1)
            ++this._activeFrameIndex;
          this._activeFrameRect = new Rectangle(16 * this._activeFrameIndex, 0, 16, 16);
          this._animationCounter = 0;
        }
      }
      if (this._activeFrameIndex == 2 && !this._throwBone)
      {
        this._throwBone = true;
        this._animationCounter = 0;
      }
      if (!this._throwBone)
        return;
      if (!this._hasMovedOut)
      {
        this.Position.X -= this._speed;
        if ((double) this.Position.X <= (double) -this.Width)
          this._hasMovedOut = true;
      }
      this._boneTargetRect.X += (int) this._speed * 10;
      this.BoneRect = new Rectangle(this._boneTargetRect.X + this.Width / 4, this._boneTargetRect.Y + this.Height / 4, 8, 8);
      ++this._animationCounter;
      if (this._animationCounter > 15)
      {
        ++this._boneFrameIndex;
        if (this._boneFrameIndex > this._numFrames - 1)
          this._boneFrameIndex = 0;
        this._boneFrameRect = new Rectangle(16 * this._boneFrameIndex, 16, 16, 16);
        this._animationCounter = 0;
      }
      if (this._boneTargetRect.X <= 1280)
        return;
      if (this.kill)
        this.canBeKilled = true;
      this.Reset();
    }

    private void Reset()
    {
      this._boneTargetRect = new Rectangle(this.Rect.X + this.Width, this.Rect.Y, this.Width, this.Height);
      this.BoneRect = new Rectangle(this._boneTargetRect.X + this.Width / 4, this._boneTargetRect.Y + this.Height / 4, 8, 8);
      this._activeFrameIndex = 0;
      this._activeFrameRect = new Rectangle(16 * this._activeFrameIndex, 0, 16, 16);
      this._hasMovedOut = false;
      this._hasMovedIn = false;
      this._throwBone = false;
      this._throwSFX = false;
    }

    public bool ThrowSFX()
    {
      if (!this._throwBone || this._throwSFX)
        return false;
      this._throwSFX = true;
      return true;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this.Texture, this.Rect, new Rectangle?(this._activeFrameRect), Color.White);
      if (!this._throwBone)
        return;
      spriteBatch.Draw(this.Texture, this._boneTargetRect, new Rectangle?(this._boneFrameRect), Color.White);
    }
  }
}
