using DXNET.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThumbStickCuts.Input
{
    internal class GamePadButton
    {
        GamepadButtonFlags buttonFlag;
        bool active = false;

        float timeToWait = 200;
        float buttonTimeOut = 0;

        public event EventHandler? OnButtonDown = null;
        Controller controller;

        public GamePadButton(GamepadButtonFlags button)
        {
            buttonFlag = button;

        }

        public void Update(float dt = 1000 / 60)
        {
            if (controller == null)
                return;

            if (buttonTimeOut > 0)
                buttonTimeOut -= dt;

            var state = controller.GetState();
            if (state.Gamepad.Buttons == buttonFlag &&
                OnButtonDown != null &&
                buttonTimeOut <= 0)
            {
                OnButtonDown(this, new EventArgs());
                buttonTimeOut = timeToWait;
            }
        }
    }
}
