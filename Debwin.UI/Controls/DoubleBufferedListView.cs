using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Debwin.UI.Controls
{
    public class DoubleBufferedListView : ListView
    {

        public event ScrollEventHandler Scroll;
        public event EventHandler ScrolledToEnd;

        private const int WM_VSCROLL = 0x115;
        private const int WM_MOUSEWHEEL = 0x20a;

        private const int LVM_FIRST = 0x1000;
        private const int LVM_SETITEMSTATE = LVM_FIRST + 43;

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessageLVItem(IntPtr hWnd, int msg, int wParam, ref LVITEM lvi);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct LVITEM
        {
            public int mask;
            public int iItem;
            public int iSubItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszText;
            public int cchTextMax;
            public int iImage;
            public IntPtr lParam;
            public int iIndent;
            public int iGroupId;
            public int cColumns;
            public IntPtr puColumns;
        };


        public DoubleBufferedListView()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_VSCROLL)
            {
                var eventType = (ScrollEventType)(m.WParam.ToInt32() & 0xffff);
                OnScroll(new ScrollEventArgs(eventType, 0));
                if (eventType == ScrollEventType.EndScroll)
                    CheckHasScrolledToEnd();
            }
            else if (m.Msg == WM_MOUSEWHEEL)
            {
                OnScroll(new ScrollEventArgs(ScrollEventType.ThumbTrack, 0));
                CheckHasScrolledToEnd();
            }
        }


        protected virtual void OnScroll(ScrollEventArgs e)
        {
            this.Scroll?.Invoke(this, e);
        }

        private void CheckHasScrolledToEnd()
        {
            if (ScrolledToEnd == null)
                return;

            var scrollInfo = new NativeMethods.SCROLLINFO();
            scrollInfo.cbSize = (uint)Marshal.SizeOf(typeof(NativeMethods.SCROLLINFO));
            scrollInfo.fMask = (uint)(NativeMethods.ScrollInfoMask.SIF_ALL);  // query position and range
            var res = NativeMethods.GetScrollInfo(this.Handle, NativeMethods.SBOrientation.SB_VERT, ref scrollInfo);

            if (res && scrollInfo.nPos >= scrollInfo.nMax - scrollInfo.nPage)
            {
                ScrolledToEnd(this, new EventArgs());
            }

        }


        public void FastSelectAllItems()
        {
            SetItemState(this, -1, 2, 2);  // http://stackoverflow.com/a/1118396/3680727
        }

        private static void SetItemState(ListView list, int itemIndex, int mask, int value)
        {
            LVITEM lvItem = new LVITEM();
            lvItem.stateMask = mask;
            lvItem.state = value;
            SendMessageLVItem(list.Handle, LVM_SETITEMSTATE, itemIndex, ref lvItem);
        }
    }
}
