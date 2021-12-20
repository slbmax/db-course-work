using System;
using ControllerLib;

namespace ConsoleProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Controller controller = new Controller();
            GUIController.RunConsoleInterface(controller);
        }
    }
}
