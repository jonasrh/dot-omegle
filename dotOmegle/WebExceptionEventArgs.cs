using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace dotOmegle
{
    public class WebExceptionEventArgs : EventArgs
    {
        public WebException exception;

        public WebExceptionEventArgs(WebException e)
        {
            this.exception = e;
        }
    }

    public delegate void WebExceptionEvent(object sender, WebExceptionEventArgs e);
}