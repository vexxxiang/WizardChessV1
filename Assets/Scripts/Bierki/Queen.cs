using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece
{
    public AudioClip[] _Sounds;
    public override AudioClip[] Sounds
    {
        get { return _Sounds; }
        set { Sounds = _Sounds; }
    }



    public override bool[,] GetAvailableMoves(ChessPiece[,] boardState)
    {
        bool[,] moves = new bool[8, 8];

        // Wszystkie kierunki
        Rook rookPart = new Rook { boardPosition = boardPosition, isWhite = isWhite };
        Bishop bishopPart = new Bishop { boardPosition = boardPosition, isWhite = isWhite };

        bool[,] rookMoves = rookPart.GetAvailableMoves(boardState);
        bool[,] bishopMoves = bishopPart.GetAvailableMoves(boardState);

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                moves[x, y] = rookMoves[x, y] || bishopMoves[x, y];
            }
        }

        return moves;
    }
}
