using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{

   

    // Inicjalizacja Animatora
    void Start()
    {

    }

    public override bool[,] GetAvailableMoves(ChessPiece[,] boardState)
    {
        bool[,] moves = new bool[8, 8];

        // Cztery kierunki po przek¹tnej
        CheckLine(boardState, moves, 1, 1);   // Prawo-góra
        CheckLine(boardState, moves, 1, -1);  // Prawo-dó³
        CheckLine(boardState, moves, -1, 1);  // Lewo-góra
        CheckLine(boardState, moves, -1, -1); // Lewo-dó³

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

            if (boardState[x, y] == null)
            {
                moves[x, y] = true;
            }
            else
            {
                if (boardState[x, y].isWhite != isWhite)
                {
                    moves[x, y] = true;
                }
                break;
            }
        }
    }

}
