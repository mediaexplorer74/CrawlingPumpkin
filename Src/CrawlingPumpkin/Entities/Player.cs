
// Type: ScreamJam24.Entities.Player
// Assembly: Crawling Pumpkin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6FFA2B6B-8BEA-4A04-9817-EC2892B8097F


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScreamJam24.Base;

#nullable disable
namespace ScreamJam24.Entities
{
  internal class Player : Sprite
  {
    private const int ResX = 1280;
    private const int ResY = 720;
    private Rectangle _activeFrameRect;
    private int _activeFrameIndex;
    private int _numFrames;
    public int animationCounter;
    public bool isMoving;
    public bool isBroken;
    public bool playerWins;

    public Player(Texture2D Texture, Vector2 Position, int Width, int Height)
      : base(Texture, Position, Width, Height)
    {
      this._numFrames = 4;
      this._activeFrameRect = new Rectangle(0, 0, 16, 16);
    }

    public void Update()
    {
      if (this.playerWins)
      {
        if ((double) this.Position.X > 1280.0)
          return;
        this.Position.X += 5f;
        ++this.animationCounter;
        if (this.animationCounter != 10)
          return;
        ++this._activeFrameIndex;
        if (this._activeFrameIndex >= this._numFrames)
          this._activeFrameIndex = 1;
        this._activeFrameRect.X = 16 * this._activeFrameIndex;
        this.animationCounter = 0;
      }
      else if (this.isBroken)
      {
        ++this.animationCounter;
        if (this.animationCounter != 10 || this._activeFrameIndex >= this._numFrames - 1)
          return;
        ++this._activeFrameIndex;
        this._activeFrameRect.X = 16 * this._activeFrameIndex;
        this.animationCounter = 0;
      }
      else if (this.isMoving)
      {
        ++this.animationCounter;
        if (this.animationCounter != 10)
          return;
        ++this._activeFrameIndex;
        if (this._activeFrameIndex >= this._numFrames)
          this._activeFrameIndex = 1;
        this._activeFrameRect.X = 16 * this._activeFrameIndex;
        this.animationCounter = 0;
      }
      else
        this._activeFrameRect = new Rectangle(0, 0, 16, 16);
    }

    public void BreakPumpkin()
    {
      this.isBroken = true;
      this.animationCounter = 0;
      this._activeFrameIndex = 0;
      this._activeFrameRect.X = 0;
      this._activeFrameRect.Y = 16;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this.Texture, this.Rect, new Rectangle?(this._activeFrameRect), Color.White);
    }
  }
}
