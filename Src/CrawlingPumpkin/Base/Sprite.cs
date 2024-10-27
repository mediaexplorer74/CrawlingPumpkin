
// Type: ScreamJam24.Base.Sprite
// Assembly: Crawling Pumpkin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6FFA2B6B-8BEA-4A04-9817-EC2892B8097F


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace ScreamJam24.Base
{
  internal class Sprite
  {
    public Texture2D Texture;
    public Vector2 Position;
    public int Width;
    public int Height;
    public bool kill;
    public bool canBeKilled;

    public Sprite(Texture2D texture, Vector2 position, int Width, int Height)
    {
      this.Texture = texture;
      this.Position = position;
      this.Width = Width;
      this.Height = Height;
    }

    public Rectangle Rect
    {
      get => new Rectangle((int) this.Position.X, (int) this.Position.Y, this.Width, this.Height);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this.Texture, this.Rect, Color.White);
    }
  }
}
