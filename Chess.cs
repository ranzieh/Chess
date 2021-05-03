using System;
using System.Collections;
using System.Diagnostics;
namespace Chess
{    class Game
    {
        public Hashtable board;
        private bool whitesTurn;
        private int enPassant;
        public string reasonForInvalidMove;

        public Game() : this(Utility.startPosition)
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

            if (info.Length>3)
            {
                whitesTurn = info[1]=="w";
                if (info[3]=="-")
                {
                    enPassant=-1;
                }
                else
                {
                    enPassant=Utility.GetNumericPosFromAN(info[3]);
                }
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
                switch(move[0])
                {
                    case 'Q':
                    return false;
                    break;
                    case 'B':
                    return false;
                    break;
                    case 'R':
                    return false;
                    break;
                    case 'N':
                    return false;
                    break;
                    case 'K':
                    return false;
                    break;
                    default:
                    reasonForInvalidMove=move[0] + " is not a piece in algebraic notation";
                    return false;
                }
            }
            else
            {
                //set direction in shich the pawn is moving. 1 if whites turn -1 if blacks turn
                int direction = whitesTurn? 1 : -1;

                //check if pawn is capturing
                if (move.Length>2 && move[1]=='x')
                {
                    //pawn is capturing
                    int targetpos = Utility.GetNumericPosFromAN(move.Substring(2,2));
                    if (board[targetpos]==null)
                    {
                        if(targetpos==enPassant)
                        {
                            int originfile = Char.ToUpper(move[0])-64;
                            int targetfile = targetpos/10;
                            if (Math.Abs(originfile-targetfile)!=1)
                            {
                                reasonForInvalidMove="A Pawn can only capture on adjacent Files.";
                                return false;                                  
                            }
                            int currentpos = (targetpos%10)-direction+10*originfile;

                            if (Utility.isWhitePiece((char)board[targetpos-direction])==whitesTurn)
                            {
                                //this event is theoretically unreachable in a normal game of chess
                                reasonForInvalidMove="Invalid en Passant.";
                                return false;
                            }
                            else
                            {
                                bool goodMove = MakeMove(currentpos, targetpos);
                                if (goodMove)
                                {
                                    board[targetpos-direction]=null;
                                }
                                return goodMove;
                            }

                        }
                        else
                        {
                            reasonForInvalidMove="There is no Piece to capture on target Square.";
                            return false;
                        }
                    }
                    else
                    {
                        if (Utility.isWhitePiece((char)board[targetpos])==whitesTurn)
                        {                            
                            reasonForInvalidMove="You can not capture your own Piece.";
                            return false;
                        }
                        else
                        {
                            int originfile = Char.ToUpper(move[0])-64;
                            int targetfile = targetpos/10;
                            if (Math.Abs(originfile-targetfile)!=1)
                            {
                                reasonForInvalidMove="A Pawn can only capture on adjacent Files.";
                                return false;                                  
                            }
                            int currentpos = (targetpos%10)-direction+10*originfile;
                            if (Utility.isPawn(GetPiece(currentpos), whitesTurn))
                            {
                                bool goodMove = MakeMove(currentpos,targetpos);
                                if ((targetpos%10==8 || targetpos%10==1)&&goodMove)
                                    {
                                        char promotion = move[move.Length-1];
                                        if (Utility.isPromotionPiece(promotion))
                                        {
                                            if (whitesTurn)
                                            {
                                                promotion = Char.ToLower(promotion);
                                            }
                                            board[targetpos] = promotion;
                                        }
                                        else
                                        {
                                            char queen = whitesTurn ? 'q' : 'Q';
                                            board[targetpos]=queen;                                                
                                        }
                                    }
                                return goodMove;
                            }
                            else
                            {         
                                reasonForInvalidMove="You have no Pawn at that can capture like that.";
                                return false;                                
                            }                  
                        }
                    }
                }
                else
                {
                    int targetpos = Utility.GetNumericPosFromAN(move.Substring(0,2));
                    //check if Target square is empty
                    if (board[targetpos]==null)
                    {                        
                        //check if a Pawn of the correct color on the preceeding square
                        int currentpos = targetpos-direction;
                        if (Utility.isPawn(GetPiece(currentpos), whitesTurn))
                        {
                            bool goodMove = MakeMove(currentpos,targetpos);
                            if ((targetpos%10==8 || targetpos%10==1)&&goodMove)
                            {
                                char promotion = move[move.Length-1];
                                if (Utility.isPromotionPiece(promotion))
                                {
                                    if (whitesTurn)
                                    {
                                        promotion = Char.ToLower(promotion);
                                    }
                                    board[targetpos] = promotion;
                                }
                                else
                                {
                                    char queen = whitesTurn ? 'q' : 'Q';
                                    board[targetpos]=queen;                                                
                                }
                            }
                            return goodMove;
                        }
                        else
                        {
                            //check if there is a pawn who can move 2 squares
                            if ((targetpos%10==4 && whitesTurn) || (targetpos%10==5 && !whitesTurn))
                            {
                                if (board[currentpos]==null)
                                {                                    
                                    currentpos-=direction;
                                    if (Utility.isPawn(GetPiece(currentpos), whitesTurn))
                                    {                                        
                                        bool goodMove = MakeMove(currentpos, targetpos);
                                        if (goodMove)
                                        {
                                            enPassant=targetpos-direction;
                                        }
                                    }
                                    else
                                    {
                                        reasonForInvalidMove="There is no Pawn that can move to the specified Square.";
                                        return false;                                        
                                    }
                                }
                                reasonForInvalidMove="A Piece is on the Square between your Pawn and the target Square.";
                                return false;
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
        }

        private char GetPiece(int currentpos)
        {
            if(board[currentpos]==null)
                return 'x';
            else
                return (char)board[currentpos];
        }

        private bool MakeMove(int currentpos, int targetpos)
        {
            if (OnBoard(targetpos)&&OnBoard(currentpos))
            {
                board[targetpos]=board[currentpos];
                board[currentpos]=null;
                whitesTurn=!whitesTurn;
                enPassant=-1;
                return true;                
            }
            else
            {
                reasonForInvalidMove="The given square is not on the Board.";
                return false;
            }
        }

        private bool OnBoard(int targetpos)
        {
            return targetpos%10>0 && targetpos%10<9 && (targetpos-targetpos%10)>0 && (targetpos-targetpos%10)<81;
        }
    }
    static class Utility
    {
        public static string startPosition="rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        static string promotionPieces = "RNBQ";
        static string pieces = "RNBQKP";
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

        internal static bool isPawn(char piece, bool isWhite)
        {
            if (piece == 'x')
                return false;

            if (isWhite)
                return piece=='P';
            else
                return piece=='p';
        }

        internal static bool isPromotionPiece(char v)
        {
            return promotionPieces.Contains(v);
        }

        internal static bool isPieceChar(char v)
        {
            return pieces.Contains(v);
        }
    }

    class Move
    {
        public int targetSquare;
        public int targetFile;
        public int targetRank;
        public int originSquare;
        public int originFile;
        public int originRank;
        public char piece;
        public char promotion;
        public bool capture;
        public bool check;

        public Move(string move)
        {
            if (move[move.Length-1]=='#' || move[move.Length-1]=='+')
            {
                check=true;
                move=move.Remove(move.Length - 1);
            }
            if (Utility.isPromotionPiece(move[move.Length-1]))
            {
                promotion=move[move.Length-1];
                move=move.Remove(move.Length - 1);
                if (move[move.Length-1]=='=')
                {
                    move=move.Remove(move.Length - 1);
                }
            }

            targetRank = Convert.ToInt32(Char.GetNumericValue(move[move.Length-1]));
            move=move.Remove(move.Length - 1);
            targetFile = Char.ToUpper(move[move.Length-1])-64;
            move=move.Remove(move.Length - 1);
            piece='P';
            if (move.Length==0)
                return;

            if (move[move.Length-1]=='x')
            {
                capture=true;
                move=move.Remove(move.Length - 1);
            }
            if (move.Length==0)
                return;

            if (Char.IsDigit(move[move.Length-1]))
            {
                originRank=Convert.ToInt32(Char.GetNumericValue(move[move.Length-1]));
                move=move.Remove(move.Length - 1);
            }
            if (move.Length==0)
                return;

            if (Char.IsLower(move[move.Length-1]))
            {
                originFile = Char.ToUpper(move[move.Length-1])-64;
                move=move.Remove(move.Length - 1);
            }
            if (move.Length==0)
                return;            

            if (Utility.isPieceChar(move[move.Length-1]))
            {
                piece = move[move.Length-1];
                move=move.Remove(move.Length - 1);
            }
        }
    }
}