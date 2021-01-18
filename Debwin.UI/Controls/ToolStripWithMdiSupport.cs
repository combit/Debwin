using System.Windows.Forms;

namespace Debwin.UI.Controls
{

    // Workaround for problem in #20288: Buttons in toolstrips within MDI forms (i.e. DockContent) do not fire the click event
    // if the parent window was out of focus when the user clicked. We need to focus the application on WM_MOUSEACTIVATE.
    public class ToolStripWithMdiSupport : ToolStrip
    {
        private const int WM_MOUSEACTIVATE = 0x21;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEACTIVATE && this.CanFocus && !this.Focused)
                this.Focus();

            base.WndProc(ref m);
        }
    }
}
