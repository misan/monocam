using System;
using System.Collections.Generic;
using System.Text;

namespace monoCAM
{ 
    class Program
    {
        [STAThread] // required so that file-open dialog works (?)
        static void Main(string[] args)
        {
            
            GLWindow TestWindow = new GLWindow();
            TestWindow.ShowDialog();
        }
    }
}