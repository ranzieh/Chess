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
            Debug.Assert(info.Length>0);

            string boardInfo=info[0];
            string[] rows=boardInfo.Split("/");  

            int currentRake = 8;
            foreach (string row in rows)
            {
                int currentFile = 1;
                foreach (char c in row)
                {
                    double numericValue=char.GetNumericValue(c);
                    bool isNumeric=numericValue!=-1;
                    if (isNumeric)
                    {
                        currentFile+=Convert.ToInt32(numericValue);
                    }
                    else
                    {
                        int pos = currentFile * 10 + currentRake; 
                        board.Add(pos,c);
                        currentFile++;
                    }
                }
                currentRake--;
            }
        }
        
    }
    static class Utility
    {
        public static bool isWhitePiece(char piece)
        {
            return !Char.IsLower(piece);
        }        
    }
}