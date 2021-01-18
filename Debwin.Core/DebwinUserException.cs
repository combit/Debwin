using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Debwin.Core
{
    /// <summary>
    /// Exception type with a message that is intended to be displayed to the user as-is.
    /// </summary>
    public class DebwinUserException : Exception
    {

        public DebwinUserException(string message, Exception innerException) : base(message, innerException) { }
    }
}
