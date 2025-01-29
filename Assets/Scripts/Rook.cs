using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Rook : ChessPiece
{
    public override bool[,] GetAvailableMoves(ChessPiece[,] boardState)
    {
        bool[,] moves = new bool[8, 8];

        // Ruch w 4 kierunkach (pionowo i poziomo)
        CheckLine(boardState, moves, 1, 0);  // W prawo
        CheckLine(boardState, moves, -1, 0); // W lewo
        CheckLine(boardState, moves, 0, 1);  // W górê
        CheckLine(boardState, moves, 0, -1); // W dó³

        return moves;
    }

    private void CheckLine(ChessPiece[,] boardState, bool[,] moves, int xDirection, int yDirection)
    {
        int x = boardPosition.x;
        int y = boardPosition.y;

        while (IsInsideBoard(x + xDirection, y + yDirection))
        {
            x += xDirection;
            y += yDirection;

            if (boardState[x, y] == null) // Puste pole
            {
                moves[x, y] = true;
            }
            else
            {
                if (boardState[x, y].isWhite != isWhite) // Mo¿na zbiæ
                {
                    moves[x, y] = true;
                }
                break; // Przeszkoda
            }
        }
    }
}
