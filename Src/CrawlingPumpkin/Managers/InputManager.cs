
// Type: GameManager.Managers.InputManager
// Assembly: Crawling Pumpkin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6FFA2B6B-8BEA-4A04-9817-EC2892B8097F


using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

//
namespace GameManager.Managers
{
  internal class InputManager
  {
        public bool Update()
        {
            bool state = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Space)) 
                state = true;
            if (TouchPanel.GetState().Count > 0)
                state = true;
            return state;
        }
    }
}
