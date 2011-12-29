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
        public string url, postData;
        public PostSubmitter.PostTypeEnum method;

        public WebExceptionEventArgs(WebException e, string url, string postData, 
            PostSubmitter.PostTypeEnum method)
        {
            this.exception = e;
            this.url = url;
            this.postData = postData;
            this.method = method;
        }
    }

    public delegate void WebExceptionEvent(object sender, WebExceptionEventArgs e);
}