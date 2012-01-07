using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace CleverOmegleGUI
{
    public static class Util
    {
        /// <summary>Invokes a method.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">Name of the method.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>Result.</returns>
        public static object InvokeMethod(this object obj, string name, params object[] args)
        {
            return obj.GetType().InvokeMember(name, BindingFlags.InvokeMethod, null, obj, args);
        }

        /// <summary>Gets a property.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">Name of the property.</param>
        /// <param name="args">Any arguments.</param>
        /// <returns>The value of the property.</returns>
        public static object GetProperty(this object obj, string name, params object[] args)
        {
            return obj.GetType().InvokeMember(name, BindingFlags.GetProperty, null, obj, args);
        }

        /// <summary>Sets a property.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">Name of the property.</param>
        /// <param name="args">Any arguments.</param>
        /// <returns></returns>
        public static object SetProperty(this object obj, string name, params object[] args)
        {
            return obj.GetType().InvokeMember(name, BindingFlags.SetProperty, null, obj, args);
        }

        /// <summary>Prints a formatted message to the debug console.</summary>
        /// <param name="obj">The source object.</param>
        /// <param name="args">Comma separated list of items to print.</param>
        public static void DebugLog( this object obj, params object[] args)
        {
            string str = args.Aggregate<object, string>("", (string a, object b) => {return a + b.ToString(); });
            StackFrame sf = new StackFrame(1);
            string src = sf.GetMethod().DeclaringType.Name + "(" + sf.GetMethod().Name + ")";

            Debug.Print(string.Format("{0}: {1}", src, str));
        }
    }
}
