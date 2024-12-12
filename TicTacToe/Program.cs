using System;
using System.Threading;

namespace TicTacToe
{
    public class Program
    {
        private static void Main(string[] args)
        {
            // could add exception handling if this was NOT static. basically, if there are multiple entry points, then you'd want to have some exit code handling.
            // in order to make things NOT static, you have to use "this" to create an instance of the thing you don't want to be static
            try
            {
                // initialize object reference for GameManager
                GameMenuManager.StartGameLoop();
            }
            catch (Exception)
            {
                Console.Write("Unexpected error ocurred. Press any key to exit.");
                Console.ReadKey();
                /*this.*/Exit();
            }
        }

        /*public static Program Exit;
        public Program()
        {
            Exit = this;
        }*/

        public static void Exit()
        {
            Console.Write("Goodbye!");
            Thread.Sleep(1000);
            Environment.Exit(0);
        }
    }
}