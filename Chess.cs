using System;
using System.Collections;
using System.Diagnostics;
namespace Chess
{    class Game
    {
        public static string startPosition="rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        public Hashtable board;
        private bool whitesTurn;
        private string reasonForInvalidMove;

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

            int currentRank = 8;
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
                        int pos = currentFile * 10 + currentRank; 
                        board.Add(pos,c);
                        currentFile++;
                    }
                }
                currentRank--;
            }

            if (info.Length>1)
            {
                whitesTurn = info[1]=="w";
            }
            else
            {
                whitesTurn=true;
            }
        }

        internal bool IsWhitesTurn()
        {
            return whitesTurn;
        }

        internal bool PlayMoveAN(string move, bool checkOnly=false)
        {
            if (Char.IsUpper(move[0]))
            {
                //non-pawn move
                throw new NotImplementedException("Non pawn moves are not implemented yet. Try a pawn move!");
            }
            else
            {
                //set direction in shich the pawn is moving. 1 if whites turn -1 if blacks turn
                int direction = whitesTurn? 1 : -1;

                //check if pawn is capturing
                if (move.Length>2 && move[2]=='x')
                {
                    //pawn is capturing
                }
                else
                {
                    int targetpos = Utility.GetNumericPosFromAN(move.Substring(0,2));
                    if (board[targetpos]==null)
                    {
                        int currentpos = targetpos-direction;
                        if (Utility.IsPawn((char)board[currentpos], whitesTurn))
                        {
                            Move(currentpos,targetpos);
                        }
                        else
                        {
                            if ((targetpos%10==4 && whitesTurn) || (targetpos%10==5 && !whitesTurn))
                            {
                                if (board[currentpos]==null)
                                {                                    
                                    reasonForInvalidMove="A Piece is on the Square between your Pawn and the target Square.";
                                    return false;
                                }
                                currentpos-=direction;
                                if (Utility.IsPawn((char)board[currentpos], whitesTurn))
                                {
                                    Move(currentpos,targetpos);                                    
                                }
                            }
                            reasonForInvalidMove="There is no Pawn that can move to the specified Square.";
                            return false;
                        }                        
                    }
                    else
                    {
                        reasonForInvalidMove="A Piece is on the Square you are trying to move to.";
                        return false;
                    }
                }
            }
            return true;
        }

        private void Move(int currentpos, int targetpos)
        {
            board[targetpos]=board[currentpos];
            board[currentpos]=null;
        }
    }
    static class Utility
    {
        public static bool isWhitePiece(char piece)
        {
            return !Char.IsLower(piece);
        }

        internal static int GetNumericPosFromAN(string v)
        {
            int file = Char.ToUpper(v[0])-64;
            int rank = Convert.ToInt32(Char.GetNumericValue(v[1]));
            int rv = file * 10 + rank;
            return rv;
        }

        internal static bool IsPawn(char piece, bool isWhite)
        {
            if (piece == null)
                return false;

            if (isWhite)
                return piece=='P';
            else
                return piece=='p';
        }
    }
}