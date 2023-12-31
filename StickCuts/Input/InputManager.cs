﻿using DXNET.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ThumbStickCuts.Actions;
using ThumbStickCuts.Config;
using ThumbStickCuts.Layouts;

namespace ThumbStickCuts.Input
{
    internal class InputManager
    {
        ThumbStick thumbLeft;
        ThumbStick thumbRight;

        public event EventHandler<Dictionary<ThumbZone, IAction?>>? OnLayoutChange;
        public event EventHandler<ThumbZone>? OnActionSelected;
        public event EventHandler<WindowStyleDto> OnWindowStyleChanged;
        public event EventHandler? ToggleHidden;
        public event EventHandler? NextLayout;

        LayoutHandler layoutHandler = new();
        ConfigHandler config = new();

        Layout currentLayout = new Layout();
        Dictionary<ThumbZone, IAction?>? currentActions;

        bool active = true;

        public InputManager()
        {
            thumbLeft = new(GamepadButtonFlags.LeftThumb);
            thumbRight = new(GamepadButtonFlags.RightThumb);

            thumbRight.OnStickDown += (s, e) =>
            {
                currentLayout = layoutHandler.GetNext();
                if (currentLayout.TryGetActions(ThumbZone.Center, out var newActions))
                {
                    currentActions = newActions;
                    if (OnLayoutChange != null && currentActions != null)
                        OnLayoutChange(this, currentActions);

                    if (OnWindowStyleChanged != null)
                        OnWindowStyleChanged(this, currentLayout.Window);
                }
            };

            thumbLeft.OnStickDown += (s, e) =>
            {
                active = !active;
                if (ToggleHidden != null)
                    ToggleHidden(this, new EventArgs());
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
                    if (!active)
                        return;
                    
                    if (OnActionSelected != null)
                        OnActionSelected(this, zone);

                    if (currentActions == null || !currentActions.ContainsKey(zone))
                        return;

                    var action = currentActions[zone];
                    if (action == null)
                        return;

                    switch (action.Type)
                    {
                        case ActionTypes.RELOAD:
                            LoadLayouts();
                            break;
                        default:
                            action.Perform();
                            break;
                    }
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

            if (OnWindowStyleChanged != null)
                OnWindowStyleChanged(this, currentLayout.Window);
        }
    }
}
