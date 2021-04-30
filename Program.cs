using System;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            VisualizeBoard(game);
        }


        /// <summary>
        /// Visualzie the board of the given game in the Terminal with ASCII symbols.
        /// </summary>
        /// <param name="game">the given game.</param>
        static void VisualizeBoard(Game game)
        {
            ConsoleColor currentBackgroundColor = Console.BackgroundColor;
            ConsoleColor currentForegroundColor = Console.ForegroundColor;
            bool white =true;

            for (int i = 8; i > 0; i--)
            {                
                for (int j = 1; j < 9; j++)
                {
                    if (white)
                    {
                        Console.BackgroundColor=ConsoleColor.Black;
                    }
                    else
                    {
                        Console.BackgroundColor=ConsoleColor.DarkGray;              
                    }
                    white=!white;

                    int pos = j*10+i;
                    if (game.board[pos]==null)
                    {
                        Console.Write("  ");
                    }
                    else
                    {
                        Console.Write(game.board[pos]+" ");
                    }
                }
                Console.BackgroundColor=currentBackgroundColor;
                Console.WriteLine();
                white=!white;
            }
            Console.BackgroundColor=currentBackgroundColor;
            Console.ForegroundColor=currentForegroundColor;
        }
    }
}
