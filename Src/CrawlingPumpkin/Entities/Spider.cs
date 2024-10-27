
// Type: ScreamJam24.Entities.Spider
// Assembly: Crawling Pumpkin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6FFA2B6B-8BEA-4A04-9817-EC2892B8097F


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScreamJam24.Base;
using System;

#nullable disable
namespace ScreamJam24.Entities
{
  internal class Spider : Sprite
  {
    private const int ResX = 1280;
    private const int ResY = 720;
    private int _animationCounter;
    private int _activeFrameIndex;
    private int _numFrames;
    private float _speed;
    private Rectangle _activeFrameRect;
    private Rectangle _pumpkinRect;
    public Rectangle collisionBox;

    public Spider(Texture2D Texture, Vector2 Position, int Width, int Height)
      : base(Texture, Position, Width, Height)
    {
      this._activeFrameRect = new Rectangle(0, 0, 16, 16);
      this._numFrames = 2;
      this._speed = 6f;
      this._pumpkinRect = new Rectangle(608, 624, 64, 64);
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
      this.Position.Y += this._speed;
      if (this._pumpkinRect.Contains(new Point(this.Rect.X + this.Width / 2, this.Rect.Y + this.Height / 2)))
      {
        this._speed = -6f;
      }
      else
      {
        if ((double) this.Position.Y >= (double) -this.Height)
          return;
        if (this.kill)
          this.canBeKilled = true;
        this._speed = 6f;
      }
    }

    public static void DrawLineBetween(
      SpriteBatch spriteBatch,
      Vector2 startPos,
      Vector2 endPos,
      int thickness,
      Color color)
    {
      int width = (int) Vector2.Distance(startPos, endPos);
      Texture2D texture = new Texture2D(spriteBatch.GraphicsDevice, width, thickness);
      Color[] data = new Color[width * thickness];
      for (int index = 0; index < data.Length; ++index)
        data[index] = color;
      texture.SetData<Color>(data);
      float rotation = (float) Math.Atan2((double) endPos.Y - (double) startPos.Y, (double) endPos.X - (double) startPos.X);
      Vector2 origin = new Vector2(0.0f, (float) (thickness / 2));
      spriteBatch.Draw(texture, startPos, new Rectangle?(), Color.White, rotation, origin, 1f, SpriteEffects.None, 1f);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      Spider.DrawLineBetween(spriteBatch, new Vector2(this.Position.X + (float) (this.Width / 2), (float) -(this.Height * 2)), new Vector2(this.Position.X + (float) (this.Width / 2), this.Position.Y), 2, Color.White);
      spriteBatch.Draw(this.Texture, this.Rect, new Rectangle?(this._activeFrameRect), Color.White);
    }
  }
}
