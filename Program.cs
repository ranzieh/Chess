using System;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            Console.WriteLine(game.board.Keys.Count);
            foreach (int key in game.board.Keys)
            {
                char piece = (char) game.board[key];
                Console.WriteLine(key+":"+piece);
            }
        }


        private void VisualizeBoard(Board board)
        {

        }
    }
}
