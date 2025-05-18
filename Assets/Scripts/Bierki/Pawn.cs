using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pawn : ChessPiece
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
    int direction = isWhite ? 1 : -1;

    // 1. Normal forward move
    if (IsInsideBoard(boardPosition.x, boardPosition.y + direction) &&
        boardState[boardPosition.x, boardPosition.y + direction] == null)
    {
        moves[boardPosition.x, boardPosition.y + direction] = true;

        // 2. Double move from starting position
        if ((isWhite && boardPosition.y == 1) || (!isWhite && boardPosition.y == 6))
        {
            if (boardState[boardPosition.x, boardPosition.y + 2 * direction] == null)
            {
                moves[boardPosition.x, boardPosition.y + 2 * direction] = true;
            }
        }
    }

    // 3. Diagonal captures (standard)
    for (int dx = -1; dx <= 1; dx += 2) // dx = -1 (left), +1 (right)
    {
        int targetX = boardPosition.x + dx;
        int targetY = boardPosition.y + direction;

        if (IsInsideBoard(targetX, targetY))
        {
            ChessPiece target = boardState[targetX, targetY];
            if (target != null && target.isWhite != isWhite)
            {
                moves[targetX, targetY] = true;
            }
        }
    }

    // 4. En passant
    for (int dx = -1; dx <= 1; dx += 2)
    {
        int adjX = boardPosition.x + dx;
        int adjY = boardPosition.y;

        if (IsInsideBoard(adjX, adjY))
        {
            ChessPiece adjacent = boardState[adjX, adjY];

            // Enemy pawn next to this one, just made a double move
            if (adjacent != null && adjacent is Pawn && adjacent.isWhite != isWhite && adjacent.lastmoved)
            {
                int targetY = boardPosition.y + direction;

                // The square behind the enemy pawn must be empty (standard for en passant)
                if (boardState[adjX, targetY] == null)
                {
                    moves[adjX, targetY] = true;
                }
            }
        }
    }

    return moves;
}
}
