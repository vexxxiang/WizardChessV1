using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public ChessPiece[] AtackingFigure;
    public void CheckState()
    {
        if (!MatState() && !SzachState() && !PatState())
        { 
            
        }
    }

    public virtual bool MatState()
    {
        var Pieces = ChessGameManager.instance.boardState;
        var KingCount = 0;
        foreach (ChessPiece i in Pieces)
        {
            if (i.gameObject.CompareTag("King"))
            {
                KingCount++;
            }
        }
        if (KingCount >= 2)
        {
            return true;
        }
        else {
            return false;
        }

    }
    public virtual bool SzachState()
    {
        var Pieces = ChessGameManager.instance.boardState;
        var isWhiteTurn = ChessGameManager.instance.isWhiteTurn;

        var KingPos = new Vector2Int(0,0);
        

        foreach (ChessPiece i in Pieces)
        {
            if (i.gameObject.CompareTag("King") && i.isWhite == isWhiteTurn)
            {
                KingPos = i.boardPosition;
            }
            else {
                Debug.Log("Nie znaleizono króla dla aktualnie graj¹cych bierek");
            }
        }
        foreach (ChessPiece i in Pieces)
        {
            bool[,] availableMoves = i.GetAvailableMoves(Pieces);
            if (availableMoves[KingPos.x, KingPos.y])
            {
                
                AtackingFigure[AtackingFigure.GetLength(0) + 1] = i;
                Debug.Log("Król atakowany przez: " + i);
            }
            
        }

            return false;
    }
    public virtual bool PatState()
    {
        var Pieces = ChessGameManager.instance.boardState;
        var AvailableMovesCount = 0;
        foreach (ChessPiece i in Pieces)
        {
            bool[,] availableMoves = i.GetAvailableMoves(Pieces);
            if (availableMoves[i.boardPosition.x,i.boardPosition.y])
            {
                AvailableMovesCount++;
            }
        }
        if (AvailableMovesCount >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
