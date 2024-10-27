
// Type: GameManager.Managers.BackgroundManager
// Assembly: Crawling Pumpkin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6FFA2B6B-8BEA-4A04-9817-EC2892B8097F


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Base;
using System.Collections.Generic;

//
namespace GameManager.Managers
{
  internal class BackgroundManager
  {
    private readonly int ResX = 1280;
    private readonly int ResY = 720;
    private List<BackgroundLayer> _layers;
    public bool isMoving;

    public BackgroundManager(ContentManager Content)
    {
      this._layers = new List<BackgroundLayer>();
      this._layers.Add(new BackgroundLayer(Content.Load<Texture2D>("Textures/Background/Static"), new Vector2(0.0f, 0.0f), this.ResX, this.ResY, 0.0f));
      this._layers.Add(new BackgroundLayer(Content.Load<Texture2D>("Textures/Background/FarTrees"), new Vector2(0.0f, 0.0f), this.ResX, this.ResY, -0.4f));
      this._layers.Add(new BackgroundLayer(Content.Load<Texture2D>("Textures/Background/TombStones"), new Vector2(0.0f, 0.0f), this.ResX, this.ResY, -1f));
      this._layers.Add(new BackgroundLayer(Content.Load<Texture2D>("Textures/Background/CloseTrees"), new Vector2(0.0f, 0.0f), this.ResX, this.ResY, -2f));
      this._layers.Add(new BackgroundLayer(Content.Load<Texture2D>("Textures/Background/Wall&Ground"), new Vector2(0.0f, 0.0f), this.ResX, this.ResY, -3f));
      this._layers.Add(new BackgroundLayer(Content.Load<Texture2D>("Textures/Background/Mist"), new Vector2(0.0f, 0.0f), this.ResX, this.ResY, -1.4f));
    }

    public void Update()
    {
      foreach (BackgroundLayer layer in this._layers)
      {
        if (this.isMoving)
          layer.Update();
        if (this._layers.IndexOf(layer) == 5)
        {
          layer.NormalizeMist(this.isMoving);
          layer.Update();
        }
      }
    }

    public void UpdateMistOnly() => this._layers[5].Update();

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (Sprite layer in this._layers)
        layer.Draw(spriteBatch);
    }
  }
}
