using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState instance;
    public ChessPiece AtackingFigure;
    public Vector2Int KingPos;

    public void Start()
    {
        instance = this;
        
    }
    public void CheckState()
    {
        
    }

    public virtual bool MatState()
    {
        var Pieces = ChessGameManager.instance.boardState;
        var KingCount = 0;
        foreach (ChessPiece i in Pieces)
        {
            if (i != null)
            {
                if (i.gameObject.CompareTag("King"))
                {
                    KingCount++;
                }
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
    public virtual bool SzachState(ChessPiece[,] boardState,bool simulation)
    {
        
        var Pieces = boardState;
        var isWhiteTurn = ChessGameManager.instance.isWhiteTurn;
        
        var AtackingFigureCount = 0;
        

        foreach (ChessPiece i in Pieces)
        {
            if (i != null)
            {
                if (i.GetComponent<ChessPiece>().CompareTag("King") && i.GetComponent<ChessPiece>().isWhite == isWhiteTurn)
                {
                    KingPos = i.GetComponent<ChessPiece>().boardPosition;
                    //Debug.Log(KingPos);
                }
                else
                {
                    //Debug.Log("Nie znaleizono króla dla aktualnie graj¹cych bierek");
                }
            }
            
        }
        foreach (ChessPiece i in Pieces)
        {
            if (i != null)
            {
                bool[,] availableMoves = i.GetAvailableMoves(Pieces);
                if (availableMoves[KingPos.x, KingPos.y])
                {
                    if (i.GetComponent<ChessPiece>().CompareTag("Pawn") && i.GetComponent<ChessPiece>().boardPosition.x > KingPos.x || i.GetComponent<ChessPiece>().CompareTag("Pawn") && KingPos.x > i.GetComponent<ChessPiece>().boardPosition.x)
                    {
                        AtackingFigure = i;
                        
                        AtackingFigureCount++;
                    }
                    else if(i.GetComponent<ChessPiece>().CompareTag("King") || i.GetComponent<ChessPiece>().CompareTag("Queen") || i.GetComponent<ChessPiece>().CompareTag("Knight") || i.GetComponent<ChessPiece>().CompareTag("Rook") || i.GetComponent<ChessPiece>().CompareTag("Bishop")) {
                        AtackingFigure = i;

                        AtackingFigureCount++;
                    }
                    
                    
                }
            }
            
        }

        if (AtackingFigure != null)
        {
            Debug.Log("szach przez " + AtackingFigure);
            if (!simulation)
            {
                SetCancelingMoves();
            }
            
            return true;
            
        }
        else {

            Debug.Log("brak szacha, Gra kontynu.... ");
            return false;
        }

        

    }
    private ChessPiece[,] DeepCopyBoard(ChessPiece[,] original)
    {
        int dim0 = original.GetLength(0);
        int dim1 = original.GetLength(1);
        ChessPiece[,] copy = new ChessPiece[dim0, dim1];
        for (int i = 0; i < dim0; i++)
        {
            for (int j = 0; j < dim1; j++)
            {
                copy[i, j] = original[i, j]; // Skopiowanie referencji – jeœli obiekty nie musz¹ byæ replikowane g³êboko
            }
        }
        return copy;
    }

    public void SetCancelingMoves() 
    {

        Debug.Log("ustawiam dla danego koloru mozliwe ruchy na bierkach");
        var Pieces = ChessGameManager.instance.boardState;
        

        foreach (ChessPiece i in Pieces)
        {
            if (i != null)
            {
                if (i.GetComponent<ChessPiece>().isWhite != AtackingFigure.GetComponent<ChessPiece>().isWhite)
                {


                    bool[,] MovesSzach = i.GetComponent<ChessPiece>().PreGetAvailableMovesAtSzach(ChessGameManager.instance.boardState, AtackingFigure.GetComponent<ChessPiece>());
                    for (int x = 0; x < 8; ++x)
                    {
                        for (int y = 0; y < 8; ++y)
                        {
                            var SimulationBoardState = DeepCopyBoard(ChessGameManager.instance.boardState);


                            if (MovesSzach[x, y])
                            {

                                SimulationBoardState[i.boardPosition.x, i.boardPosition.y] = null;
                                SimulationBoardState[x, y] = i;
                                
                                if (!SzachState(SimulationBoardState, true))
                                {
                                    Debug.Log("dodano mozliwy ruch na X:" + x + " Y: " + y + "dla" + i.name);
                                   
                                    
                                    
                                }
                                else if (SzachState(SimulationBoardState, true))
                                {
                                    Debug.Log("dodano mozliwy ruch na X:" + x + " Y: " + y + "dla" + i.name);
                                    
                                    
                                }


                            }


                        }
                    }
                }
            }
           
        }

    }
    public virtual bool PatState()
    {
        var Pieces = ChessGameManager.instance.boardState;
        var AvailableMovesCount = 0;
        foreach (ChessPiece i in Pieces)
        {
            if (i != null)
            {
                bool[,] availableMoves = i.GetComponent<ChessPiece>().GetAvailableMoves(Pieces);
                if (availableMoves[i.boardPosition.x, i.boardPosition.y])
                {
                    AvailableMovesCount++;
                }
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
