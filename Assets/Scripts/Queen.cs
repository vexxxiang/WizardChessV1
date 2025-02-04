using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public override bool[,] GetAvailableMoves(ChessPiece[,] boardState)
    {
        bool[,] moves = new bool[8, 8];

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; // Pomijamy brak ruchu
                if (CanMoveOrCapture(boardState, boardPosition.x + x, boardPosition.y + y))
                {
                    moves[boardPosition.x + x, boardPosition.y + y] = true;
                }
            }
        }

        return moves;
    }
}
