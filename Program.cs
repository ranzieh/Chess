using System;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game;
            if (args.Length==0)
                game = new Game();
            else
                game = new Game(args[0]);
            
            Console.WriteLine("Chess by ranzieh: https://github.com/ranzieh/Chess");
            Console.WriteLine("Enter moves in Algebraic Notation (https://en.wikipedia.org/wiki/Algebraic_notation_(chess))");
            Console.WriteLine("Have Fun!");
            bool playing = true;
            while (playing)
            {
                VisualizeBoard(game);
                string turn = game.IsWhitesTurn()? "White" : "Black";
                Console.WriteLine($"It is {turn}'s turn. Please enter your move:");
                string move = Console.ReadLine();
                if (move=="exit")
                {
                    playing=false;
                    continue;
                }
                else
                {
                    if(!game.PlayMoveAN(move))
                        Console.WriteLine(game.reasonForInvalidMove);
                }
            }
        }


        /// <summary>
        /// Visualzie the board of the given game in the Terminal with ASCII symbols.
        /// </summary>
        /// <param name="game">the given game.</param>
        static void VisualizeBoard(Game game)
        {
            ConsoleColor currentBackgroundColor = Console.BackgroundColor;
            ConsoleColor currentForegroundColor = Console.ForegroundColor;
            bool white = true;

            for (int i = 8; i > 0; i--)
            {                
                for (int j = 1; j < 9; j++)
                {
                    Console.BackgroundColor=white ? ConsoleColor.DarkGray : ConsoleColor.DarkRed;
                    white=!white;

                    int pos = j*10+i;
                    if (game.board[pos]==null)
                    {
                        Console.Write("  ");
                    }
                    else
                    {
                        char piece = (char)game.board[pos];
                        bool whitePiece = Utility.isWhitePiece(piece);

                        Console.ForegroundColor=whitePiece ? ConsoleColor.White : ConsoleColor.Black;
                        
                        piece=Char.ToUpper(piece);
                        Console.Write(piece+" ");
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
