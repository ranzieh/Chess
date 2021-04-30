using System;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            Console.WriteLine(game.board.Keys.Count);
            foreach (string key in game.board.Keys)
            {
                char piece = (char) game.board[key];
                Console.WriteLine(key+":"+piece);
            }
        }
    }
}
