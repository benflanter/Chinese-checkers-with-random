using System;
using System.Collections.Generic;
using System.Drawing;

namespace ChineseCheckers
{
	public class Player
	{
        protected Dictionary<int, Piece> pieces;
	    public bool side;
		protected Board board;

		public Player(bool side, Board board)
		{
			this.board = board;
			this.side = side;
			pieces = new Dictionary<int, Piece>();
			if (side) // up side
            {
				for (int i = 0; i < 4; i++)
					for (int j = 0; j < Board.WIDTH; j++)
					{
						if (Board.initmat[i, j] == 2)
							pieces.Add(i * Board.WIDTH + j, new Piece(i, j, side));
					}
			}
            else // down side
            {
				for (int i = Board.HEIGHT - 4; i < Board.HEIGHT; i++)
					for (int j = 0; j < Board.WIDTH; j++)
					{
						if (Board.initmat[i, j] == 3)
							pieces.Add(i * Board.WIDTH + j, new Piece(i, j, side));
					}
			}
		}

		public void Draw(Graphics graphics)
		{
			foreach (Piece piece in pieces.Values)
				piece.Draw(graphics);
		}
		public Piece getPiece(int row, int col)
		{
			int key = row * Board.WIDTH + col;
			if (!pieces.ContainsKey(key))
				return null;
			return pieces[key];
		}
		public void removePiece(Piece piece)
		{
			int key = piece.row * Board.WIDTH + piece.col;
			pieces.Remove(key);
		}
		public void addPiece(int rowDest, int colDest, bool side)
		{
			int key = rowDest * Board.WIDTH + colDest;
			pieces.Add(key, new Piece(rowDest, colDest, side));
		}

		public bool CheckPlayerWin()
		{
			Piece piece;
			if (this.side)
			{	//player is up, checking down
				for (int i = Board.HEIGHT - 4; i < Board.HEIGHT; i++)
				{
					for (int j = 0; j < Board.WIDTH; j++)
					{
						if (Board.initmat[i, j] == 3)
						{
							piece = getPiece(i, j);
							if (piece == null || piece.side != this.side)
								return false;
						}
					}
				}
				return true;
			}
			else
            {   //player is down, checking up
                for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < Board.WIDTH; j++)
					{
                        if(Board.initmat[i, j] == 2)
                        {
                            piece = getPiece(i, j);
                            if (piece == null|| piece.side != this.side)
                                return false;
                        }
                    }
				}
				return true;
            }
		}
	}
}