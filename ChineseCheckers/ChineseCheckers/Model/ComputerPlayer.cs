using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseCheckers.Model
{
    class ComputerPlayer : Player
    {
        private Random random = new Random();
        private byte[,] helpmat =
        {
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }
        };

        public ComputerPlayer(bool side, Board board) :base(side, board)
		{
        }

        public List<Move>  GetMoves()
        {
            List<Move> moves = new List<Move>();
            foreach(KeyValuePair<int, Piece> piece in pieces)
            {
                List<Move> nearMoves = GetNearMoves(piece.Value);
                List<Move> farMoves = GetFarMoves(piece.Value);
                if(nearMoves!=null)
                    moves.AddRange(nearMoves);
                if (farMoves!=null)
                moves.AddRange(farMoves);
            }
			return moves;
		}

        private List<Move> GetNearMoves(Piece piece)
        {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < board.directions.Length / 2; i++)
            {
                int row = piece.row + board.directions[i, 0];
                int col = piece.col + board.directions[i, 1];
                if (row >= 0 && row < Board.HEIGHT && col >= 0 && col < Board.WIDTH)
                {
                    if (Board.initmat[row, col] != 0 && board.getPiece(row, col) == null)
                        moves.Add(new Move(piece, row, col));
                }
            }
            return moves;
        }

        private List<Move> GetFarMoves(Piece piece)
        {
            List<Move> moves = new List<Move>();
            if (GetNearMoves(piece).Count == 0)
                return null;
            ClearHelpMat();
            ScanFarMoves(piece, piece, moves);
            return moves;
        }

        private void ScanFarMoves(Piece myPiece, Piece nextPiece, List<Move> moves)
        {
            if (GetNearMoves(nextPiece) == null || helpmat[nextPiece.row,nextPiece.col] == 1 || Board.initmat[nextPiece.row, nextPiece.col] == 0)
                return;
            helpmat[nextPiece.row, nextPiece.col] = 1;
            for (int i = 0; i < board.directions.Length / 2; i++)
            {
                int row = nextPiece.row + board.directions[i, 0];
                int col = nextPiece.col + board.directions[i, 1];
                if (Islegal(row, col) && board.getPiece(row, col) != null)
                {
                    int nextRow = row + board.directions[i, 0];
                    int nextCol = col + board.directions[i, 1];
                    if (Islegal(nextRow, nextCol) && board.getPiece(nextRow, nextCol) == null)
                    {
                        nextPiece = new Piece(nextRow, nextCol, myPiece.side);
                        if(board.MoveAble(myPiece,nextRow,nextCol))
                            moves.Add(new Move(myPiece, nextRow, nextCol));
                        ScanFarMoves(myPiece, nextPiece, moves);
                    }
                }
            }

        }

        private void ClearHelpMat()
        {
            for (int i = 0; i < Board.HEIGHT; i++)
            {
                for (int j = 0; j < Board.WIDTH; j++)
                {
                    helpmat[i, j] = 0;
                }
            }
        }
       private bool Islegal(int row, int col)
        {
            return row >= 0 && row < Board.HEIGHT && col >= 0 && col < Board.WIDTH && Board.initmat[row, col] != 0;
        }

        public void MakeMove()
        {
            
            List<Move> moves = GetMoves();
            int index = random.Next(0, moves.Count);
            if (moves.Count > 0)
            {
                removePiece(moves[index].GetOrigin());
                addPiece(moves[index].GetRow(), moves[index].GetCol(), side);
            }
        }
    }
}
