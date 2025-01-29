using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override bool[,] GetAvailableMoves(ChessPiece[,] boardState)
    {
        bool[,] moves = new bool[8, 8]; // Tablica mo¿liwych ruchów (8x8 szachownica)

        // Mo¿liwe przesuniêcia skoczka (offsety)
        int[,] offsets = { { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 }, { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 } };

        // Iteracja po wierszach tablicy offsets
        for (int i = 0; i < offsets.GetLength(0); i++)
        {
            int xOffset = offsets[i, 0]; // Pierwszy element wiersza (xOffset)
            int yOffset = offsets[i, 1]; // Drugi element wiersza (yOffset)

            int x = boardPosition.x + xOffset; // Nowa pozycja x
            int y = boardPosition.y + yOffset; // Nowa pozycja y

            // Sprawdzamy, czy nowa pozycja mieœci siê w granicach szachownicy
            if (x >= 0 && x < 8 && y >= 0 && y < 8)
            {
                // Sprawdzamy, czy skoczek mo¿e siê tam ruszyæ lub zbiæ figurê
                if (CanMoveOrCapture(boardState, x, y))
                {
                    moves[x, y] = true; // Oznaczamy dostêpny ruch
                }
            }
        }

        return moves; // Zwracamy tablicê mo¿liwych ruchów
    }
}