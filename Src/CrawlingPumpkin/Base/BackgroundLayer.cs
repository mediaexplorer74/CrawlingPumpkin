
// Type: ScreamJam24.Base.BackgroundLayer
// Assembly: Crawling Pumpkin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6FFA2B6B-8BEA-4A04-9817-EC2892B8097F


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace ScreamJam24.Base
{
  internal class BackgroundLayer : Sprite
  {
    private readonly int ResX = 1280;
    private readonly int ResY = 720;
    private float _speedFactor;

    public BackgroundLayer(
      Texture2D Texture,
      Vector2 Position,
      int Width,
      int Height,
      float SpeedFactor)
      : base(Texture, Position, Width, Height)
    {
      this._speedFactor = SpeedFactor;
    }

    public void Update()
    {
      this.Position.X += this._speedFactor;
      if ((double) this.Position.X > (double) -this.ResX && (double) this.Position.X < (double) this.ResX)
        return;
      this.Position.X = 0.0f;
    }

    public void NormalizeMist(bool isMoving)
    {
      if (isMoving)
        this._speedFactor = -2f;
      else
        this._speedFactor = -1.4f;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this.Texture, new Rectangle((int) this.Position.X, (int) this.Position.Y, this.ResX, this.ResY), Color.White);
      spriteBatch.Draw(this.Texture, new Rectangle((double) this._speedFactor < 0.0 ? (int) this.Position.X + this.ResX : (int) this.Position.X - this.ResX, (int) this.Position.Y, this.ResX, this.ResY), Color.White);
    }
  }
}
