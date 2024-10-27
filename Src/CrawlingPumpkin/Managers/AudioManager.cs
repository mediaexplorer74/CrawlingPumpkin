
// Type: GameManager.Managers.AudioManager
// Assembly: Crawling Pumpkin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6FFA2B6B-8BEA-4A04-9817-EC2892B8097F


using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Threading.Tasks;


namespace GameManager.Managers
{
  internal class AudioManager
  {
    private 
    //
    SoundEffect _background;
    private SoundEffect _crow;
    private SoundEffect _spider;
    private SoundEffect _zombie;
    private SoundEffect _skeleton;
    private SoundEffect _wisp;
    private SoundEffect _pumpkin;
    private SoundEffect _win;
    private readonly SoundEffectInstance _backgroundInstance;
    private ContentManager _Content;

    public AudioManager(ContentManager Content)
    {
      this._background = Content.Load<SoundEffect>("Audio/Background");
      this._backgroundInstance = this._background.CreateInstance();
      this._backgroundInstance.Volume = 0.5f;
      this._backgroundInstance.IsLooped = true;
      this._Content = Content;
      this.LoadAssetsAsync();
    }

    public async void LoadAssetsAsync()
    {
      await Task.Run((Action) (() =>
      {
        this._crow = this._Content.Load<SoundEffect>("Audio/Crow");
        this._spider = this._Content.Load<SoundEffect>("Audio/Spider");
        this._zombie = this._Content.Load<SoundEffect>("Audio/Zombie");
        this._skeleton = this._Content.Load<SoundEffect>("Audio/Skeleton");
        this._wisp = this._Content.Load<SoundEffect>("Audio/Wisp");
        this._win = this._Content.Load<SoundEffect>("Audio/Win");
        this._pumpkin = this._Content.Load<SoundEffect>("Audio/Pumpkin");
      }));
    }

    public void PlaySFX(string name)
    {
      if (name == null)
        return;
      switch (name.Length)
      {
        case 3:
          if (!(name == "win"))
            break;
          this._win.Play();
          break;
        case 4:
          switch (name[0])
          {
            case 'c':
              if (!(name == "crow"))
                return;
              this._crow.Play();
              return;
            case 'w':
              if (!(name == "wisp"))
                return;
              this._wisp.Play();
              return;
            default:
              return;
          }
        case 6:
          switch (name[0])
          {
            case 's':
              if (!(name == "spider"))
                return;
              this._spider.Play();
              return;
            case 'z':
              if (!(name == "zombie"))
                return;
              this._zombie.Play();
              return;
            default:
              return;
          }
        case 7:
          if (!(name == "pumpkin"))
            break;
          this._pumpkin.Play();
          break;
        case 8:
          if (!(name == "skeleton"))
            break;
          this._skeleton.Play();
          break;
        case 10:
          if (!(name == "background") || this._backgroundInstance.State == SoundState.Playing)
            break;
          this._backgroundInstance.Play();
          break;
      }
    }
  }
}
