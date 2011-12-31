using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CleverOmegleGUI
{
    public static class Util
    {
        public static object InvokeMethod(this object obj, string name, params object[] args)
        {
            return obj.GetType().InvokeMember(name, BindingFlags.InvokeMethod, null, obj, args);
        }

        public static object GetProperty(this object obj, string name, params object[] args)
        {
            return obj.GetType().InvokeMember(name, BindingFlags.GetProperty, null, obj, args);
        }

        public static object SetProperty(this object obj, string name, params object[] args)
        {
            return obj.GetType().InvokeMember(name, BindingFlags.SetProperty, null, obj, args);
        }
    }
}
