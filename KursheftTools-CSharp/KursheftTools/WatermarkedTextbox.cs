using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KursheftTools
{
    /// <summary>
    /// CC-BY-SA (c) https://stackoverflow.com/users/254882/g-t ||
    /// https://stackoverflow.com/a/5450496/13053641
    /// </summary>
    class WatermarkedTextBox : TextBox
    {
        private static class NativeMethods
        {
            private const uint ECM_FIRST = 0x1500;
            internal const uint EM_SETCUEBANNER = ECM_FIRST + 1;

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, string lParam);
        }

        private string _cue;

        public string Cue
        {
            get
            {
                return _cue;
            }
            set
            {
                _cue = value;
                UpdateCue();
            }
        }

        private void UpdateCue()
        {
            if (IsHandleCreated && _cue != null)
            {
                NativeMethods.SendMessage(Handle, NativeMethods.EM_SETCUEBANNER, (IntPtr)1, _cue);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateCue();
        }
    }
}

