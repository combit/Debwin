using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Debwin.UI.Util
{

    /// <summary>
    /// Creates an invisble Win32 window with a custom class name and allows to specify a handler for the window message it receives.
    /// </summary>
    public class CustomWin32Window : IDisposable
    {
        public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        private bool _disposed;
        private readonly WndProc _wndProcHandler;
        private readonly WndProc _userWndProc;
        private IntPtr _handle;

        public CustomWin32Window(string className, WndProc messageHandler)
        {
            _userWndProc = messageHandler;
            _wndProcHandler = CustomWndProc;

            var wind_class = new NativeMethods.WNDCLASS();
            wind_class.lpszClassName = className;
            wind_class.lpfnWndProc = Marshal.GetFunctionPointerForDelegate(_wndProcHandler);

            UInt16 class_atom = NativeMethods.RegisterClassW(ref wind_class);

            if (class_atom == 0)
            {
                int last_error = Marshal.GetLastWin32Error();

                if (last_error == NativeMethods.ERROR_CLASS_ALREADY_EXISTS)
                    throw new InvalidOperationException("The specified class name is already in use.");
                else
                    throw new Win32Exception(last_error);
            }

            _handle = NativeMethods.CreateWindowExW(0, className, String.Empty, 0, 0, 0, 0, 0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }

        private IntPtr CustomWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            _userWndProc?.Invoke(hWnd, msg, wParam, lParam);
            return NativeMethods.DefWindowProcW(hWnd, msg, wParam, lParam);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing) { }

                // Dispose unmanaged resources
                if (_handle != IntPtr.Zero)
                {
                    NativeMethods.DestroyWindow(_handle);
                    _handle = IntPtr.Zero;
                }

                _disposed = true;
            }
        }
    }

}
