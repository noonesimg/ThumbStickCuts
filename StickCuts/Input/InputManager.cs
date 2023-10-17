using DXNET.XInput;
using StickCuts.Actions;
using StickCuts.Config;
using StickCuts.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace StickCuts.Input
{
    internal class InputManager
    {
        ThumbStick thumbLeft = new(GamepadButtonFlags.LeftThumb, true);
        ThumbStick thumbRight = new(GamepadButtonFlags.RightThumb, false);
        GamePadButton shoulderLeft = new(GamepadButtonFlags.LeftShoulder);
        GamePadButton shoulderRight = new(GamepadButtonFlags.RightShoulder);

        public event EventHandler<Dictionary<ThumbZone, IAction?>>? OnLayoutChange;
        public event EventHandler<ThumbZone>? OnActionSelected;
        public event EventHandler? OnHideWindow;
        public event EventHandler? OnShowWindow;

        LayoutHandler layoutHandler = new();
        ConfigHandler config = new();

        Layout currentLayout = new Layout();
        Dictionary<ThumbZone, IAction?>? currentActions;

        public InputManager()
        {
            thumbRight.OnStickDown += (s, e) =>
            {
                if (OnShowWindow != null)
                    OnShowWindow(this, new EventArgs());
            };

            thumbLeft.OnStickDown += (s, e) =>
            {
                if (OnHideWindow != null)
                    OnHideWindow(this, new EventArgs());
            };

            shoulderLeft.OnButtonDown += (s, e) =>
            {
                currentLayout = layoutHandler.GetPrevious();
                if (currentLayout.TryGetActions(ThumbZone.Center, out var newActions))
                {
                    currentActions = newActions;
                    if (OnLayoutChange != null && currentActions != null)
                        OnLayoutChange(this, currentActions);
                }
            };

            shoulderRight.OnButtonDown += (s, e) =>
            {
                currentLayout = layoutHandler.GetNext();
                if (currentLayout.TryGetActions(ThumbZone.Center, out var newActions))
                {
                    currentActions = newActions;
                    if (OnLayoutChange != null && currentActions != null)
                        OnLayoutChange(this, currentActions);
                }
            };

            thumbRight.OnSelected += (s, zone) =>
            {
                if (currentLayout.TryGetActions(zone, out var newActions))
                {
                    currentActions = newActions;
                    if (OnLayoutChange != null && currentActions != null)
                        OnLayoutChange(this, currentActions);
                }
            };

            thumbLeft.OnSelected += (s, zone) =>
            {
                try
                {
                    if (OnActionSelected != null)
                        OnActionSelected(this, zone);

                    if (currentActions == null || !currentActions.ContainsKey(zone))
                        return;

                    var action = currentActions[zone];
                    if (action == null)
                    {
                        Console.WriteLine("No Action");
                        return;
                    }

                    if (action.Type == ActionTypes.RELOAD)
                    {
                        LoadLayouts();
                        return;
                    }

                    action.Perform();
                }
                catch
                {
                    // skip
                }
            };
        }

        public void Update()
        {
            thumbLeft.Update();
            thumbRight.Update();
            shoulderLeft.Update();
            shoulderRight.Update();
        }

        public void LoadLayouts()
        {
            layoutHandler.Layouts = config.LoadLayouts();
            currentLayout = layoutHandler.GetDefaultLayout();

            if (currentLayout.TryGetActions(ThumbZone.Center, out var newActions))
            {
                currentActions = newActions;
                if (OnLayoutChange != null && currentActions != null)
                    OnLayoutChange(this, currentActions);
            }
        }
    }
}
