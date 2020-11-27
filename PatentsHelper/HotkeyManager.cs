using Gma.System.MouseKeyHook;
using PatentsHelperSettings;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PatentsHelper
{
    public class HotkeyManager
    {

        private IKeyboardMouseEvents appHotKeyHook;
        private readonly UserSettings userSettings = new UserSettings();
        private readonly Dictionary<string, Action> ControlHotkeys = new Dictionary<string, Action>();
        private readonly Dictionary<string, Action> AltHotkeys = new Dictionary<string, Action>();
        public void Unsubscribe()
        {
            appHotKeyHook.Dispose();
        }

        public void Subscribe(Action rnAction)
        {

            appHotKeyHook = Hook.AppEvents();

            if (!string.IsNullOrEmpty(userSettings.RnShortcutLast))
            {
                if (userSettings.RnShortcutFirst == Keys.Control.ToString())
                {
                    ControlHotkeys.Add(userSettings.RnShortcutLast, rnAction);
                }
                else
                {
                    AltHotkeys.Add(userSettings.RnShortcutLast, rnAction);
                }
            }

            appHotKeyHook.KeyDown += KeyDownEvent;
        }

        private void KeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!e.Alt && !e.Shift && e.Control)
            {
                foreach (var key in ControlHotkeys)
                {
                    if (e.KeyCode.ToString() == key.Key)
                    {
                        key.Value();
                    }
                }
            }
            else if (e.Alt && !e.Shift && !e.Control)
            {
                foreach (var key in AltHotkeys)
                {
                    if (e.KeyCode.ToString() == key.Key)
                    {
                        key.Value();
                    }
                }
            }
        }


    }


}
