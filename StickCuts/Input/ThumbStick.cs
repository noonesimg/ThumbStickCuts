using DXNET.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StickCuts.Input
{
    public class ThumbStick
    {
        bool retrigger;

        Controller controller;
        public event EventHandler<ThumbZone>? OnSelected = null;
        public event EventHandler? OnStickDown = null;

        bool active = false;

        float timeToWait = 200;
        float zoneTimeOut = 0;
        float buttonTimeOut = 0;
        int numRepetitions = 0;

        GamepadButtonFlags buttonFlag;

        public ThumbStick(
            GamepadButtonFlags thumbStick = GamepadButtonFlags.LeftThumb,
            bool retrigger = true
        )
        {
            this.retrigger = retrigger;
            buttonFlag = thumbStick;
            controller = new Controller(UserIndex.One);
        }

        public void Update(float dt = 1000 / 60)
        {
            if (controller == null)
                return;

            if (zoneTimeOut > 0)
                zoneTimeOut -= dt;

            if (buttonTimeOut > 0)
                buttonTimeOut -= dt;

            Vector2 value = new Vector2(0, 0);

            var state = controller.GetState();

            if (state.Gamepad.Buttons == buttonFlag &&
                OnStickDown != null &&
                buttonTimeOut <= 0)
            {
                OnStickDown(this, new EventArgs());
                buttonTimeOut = timeToWait;
            }


            switch (buttonFlag)
            {
                case GamepadButtonFlags.LeftThumb:
                    value.X = state.Gamepad.LeftThumbX;
                    value.Y = state.Gamepad.LeftThumbY;
                    break;
                case GamepadButtonFlags.RightThumb:
                    value.X = state.Gamepad.RightThumbX;
                    value.Y = state.Gamepad.RightThumbY;
                    break;
            }

            value.X = Remap(value.X, short.MinValue, short.MaxValue, -1.0f, 1.0f);
            value.Y = Remap(value.Y, short.MinValue, short.MaxValue, -1.0f, 1.0f);

            // reset
            if (value.Length < 0.8)
            {
                if (active && OnSelected != null)
                {
                    OnSelected(this, ThumbZone.Center);
                }

                active = false;
                zoneTimeOut = 0.0f;
                numRepetitions = 0;
                return;
            }
            // active
            else if (!active || zoneTimeOut <= 0 && retrigger)
            {
                active = true;
                if (numRepetitions < timeToWait)
                {
                    numRepetitions += 1;
                    numRepetitions *= 2;
                }
                zoneTimeOut = timeToWait - Math.Min(numRepetitions, timeToWait - 1);

                if (OnSelected != null)
                {
                    ThumbZone zone = GetZone(value);
                    OnSelected(this, zone);
                }
            }
        }

        private ThumbZone GetZone(Vector2 value)
        {
            double angle = Math.Atan2(value.Y, value.X) * (180 / Math.PI);
            angle = (angle + 360 + 15) % 360;

            var zones = new List<(ThumbZone Zone, double Min, double Max)>()
            {
                (ThumbZone.Right,       0,      30),
                (ThumbZone.TopRight,    30,     90),
                (ThumbZone.Top,         90,     120),
                (ThumbZone.TopLeft,     120,    180),
                (ThumbZone.Left,        180,    210),
                (ThumbZone.BottomLeft,  210,    270),
                (ThumbZone.Bottom,      270,    310),
                (ThumbZone.BottomRight, 310,    360)
            };

            foreach (var zone in zones)
            {
                if (angle > zone.Min && angle <= zone.Max)
                {
                    return zone.Zone;
                }
            }

            return ThumbZone.Center;
        }

        private double Remap(double value, double fromLow, double fromHigh, double toLow, double toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }
    }
}
