/*
 
 tetris.NET (16k)
 Author: Krzysztof Cieślak (K!)  

 */
using System;
using System.Windows.Forms;

namespace tetris.NET
{
    static class Tetris
    {
        [STAThread]
        public static int Main()
        {
            Application.Run(new MainForm());
            return 0;
        }
    }
}