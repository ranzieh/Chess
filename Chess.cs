using System;
using System.Collections;
using System.Diagnostics;
namespace Chess
{    class Game
    {
        public static string startPosition="rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        public Hashtable board;

        public Game() : this(startPosition)
        {
        }

        /// <summary>
        /// Creates a game with the given notation as basis.
        /// </summary>
        /// <param name="feNotation">The Forsyth-Edwards-Notation of the Game to be created (https://de.wikipedia.org/wiki/Forsyth-Edwards-Notation)</param>
        public Game(string feNotation)
        {
            board=new Hashtable();

            string[] info = feNotation.Split(" ");
            Debug.Assert(info.Length>1);

            string boardInfo=info[0];
            string[] rows=boardInfo.Split("/");  

            int currentRow = 8;
            foreach (string row in rows)
            {
                char currentColumn = 'A';
                foreach (char c in row)
                {
                    double value=char.GetNumericValue(c);
                    bool isNumeric=value!=-1;
                    if (isNumeric)
                    {
                        for (int i = 0; i < value; i++)
                        {
                            currentColumn++;
                        }
                    }
                    else
                    {
                        char[] chars = {currentColumn, Convert.ToChar(currentRow)};
                        string s = new string(chars);
                        board.Add(s,c);
                        currentColumn++;
                    }
                }
                currentRow--;
            }
        }
        
    }
    class Piece
    {
        
    }
    class Square
    {

    }
}