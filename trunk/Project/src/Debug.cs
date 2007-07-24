using System;
using System.Collections.Generic;
using System.Text;

namespace Debug
{
    class DebugClient
    {
        // This is the base class for debug support
        // Usage:
        // To throw a debug message you inerheit from this class(class testclass : DebugClient)
        // then in your class you do ThrowDebugMessage(this, 0, "test debugevent");
        //
        // TestClass Test = new TestClass();
        // Test.DebugEvent +=new DebugClient.DebugEventHandler(test_DebugEvent);
        //  static void test_DebugEvent(object o, int level, string message) {
        //  //Do stuff when we get a debug event.
        //  } 

        public delegate void DebugEventHandler(object o, int level, string message);
        public event DebugEventHandler DebugEvent;

        protected void ThrowDebugMessage(object o, int level, string message)
        {
            if (DebugEvent != null)
                DebugEvent(o, level, message);
        }
    }
}
