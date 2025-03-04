using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessRules : MonoBehaviour
{
    public Transform FiguresSimulation;
    public static ChessRules instance;
    ChessPiece kingCheck = null;
    public GameObject DestroyKingB, DestroyKingW;

    private void Awake()
    {
        instance = this;
    }

    public bool IsInCheck(ChessPiece[,] boardState, bool isWhiteTurn)
    {
        ChessPiece[,] simulation = DeepCopyBoard(boardState);
        kingCheck = null;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (simulation[x, y] != null && simulation[x, y] is King && simulation[x, y].isWhite == isWhiteTurn)
                {
                    kingCheck = simulation[x, y];
                }
            }
        }

        if (kingCheck == null)
        {
            Debug.LogError("Błąd: Król nie został poprawnie skopiowany w DeepCopyBoard!");
        }

        // Dla każdej bierki przeciwnika sprawdź, czy może zagrać na polu, na którym stoi król
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece piece = boardState[x, y];
                if (piece != null && piece.isWhite != isWhiteTurn)
                {
                    bool[,] moves = piece.GetAvailableMoves(boardState);
                    if (moves[kingCheck.boardPosition.x, kingCheck.boardPosition.y])
                    {
                        Debug.Log("Figura " + piece.name + " z " + (piece.isWhite ? "białych" : "czarnych") + " atakuje króla.");
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool HasLegalMoves(bool isWhiteTurn)
    {
        ChessPiece[,] boardState = ChessGameManager.instance.boardState;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece piece = boardState[x, y];
                if (piece != null && piece.isWhite == isWhiteTurn)
                {
                    bool[,] moves = piece.GetLegalMoves(boardState);

                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {

                            if (moves[i, j])
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    public ChessPiece[,] DeepCopyBoard(ChessPiece[,] original)
    {
        ChessPiece[,] copy = new ChessPiece[8, 8];
        if (FiguresSimulation.childCount > 0)
        {
            foreach (Transform child in FiguresSimulation)
            {
                Destroy(child.gameObject);
            }
        }

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (original[x, y] != null)
                {
                    var obj = new GameObject(original[x, y].name + "kopy");
                    obj.transform.SetParent(FiguresSimulation);
                    var piece = original[x, y];
                    switch (piece)
                    {
                        case Bishop bishop:
                            copy[x, y] = bishop.DeepCopy<Bishop>(obj);
                            break;
                        case King king:
                            copy[x, y] = king.DeepCopy<King>(obj);
                            break;
                        case Knight knight:
                            copy[x, y] = knight.DeepCopy<Knight>(obj);
                            break;
                        case Pawn pawn:
                            copy[x, y] = pawn.DeepCopy<Pawn>(obj);
                            break;
                        case Queen queen:
                            copy[x, y] = queen.DeepCopy<Queen>(obj);
                            break;
                        case Rook rook:
                            copy[x, y] = rook.DeepCopy<Rook>(obj);
                            break;
                    }
                    //copy[x, y] = Instantiate(original[x, y]);  // Klonujemy figurę
                    //copy[x, y] = original[x,y].DeepCopy(obj);
                    //copy[x, y].boardPosition = new Vector2Int(x, y);
                }
            }
        }
        return copy;
    }

    public void SetCancelingMoves(ChessPiece _attackingFigure, Vector2Int _kingPos)
    {
        ChessPiece[,] boardState = ChessGameManager.instance.boardState;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece piece = boardState[x, y];
                if (piece != null && piece.isWhite == ChessGameManager.instance.isWhiteTurn)
                {
                    bool[,] pieceMoves = piece.GetAvailableMoves(boardState);

                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (pieceMoves[i, j])
                            {
                                // Tworzymy symulowaną kopię planszy
                                ChessPiece[,] simulation = DeepCopyBoard(boardState);

                                // Pobieramy klon figury
                                ChessPiece clonedPiece = simulation[x, y];
                                simulation[x, y] = null;
                                clonedPiece.boardPosition = new Vector2Int(i, j);
                                simulation[i, j] = clonedPiece;


                                // Sprawdzamy, czy po ruchu król nadal jest w szachu
                                bool isValid = !IsInCheck(simulation, clonedPiece.isWhite);

                                // Ustawiamy wynik dla danego ruchu
                                piece.CancelingSzachMoves[i, j] = isValid;
                            }
                            else
                            {
                                piece.CancelingSzachMoves[i, j] = false;
                            }

                        }
                    }
                }
            }
        }
    }


    public string EvaluateGameState()
    {
        bool isWhiteTurn = ChessGameManager.instance.isWhiteTurn;
        ChessPiece[,] boardState = ChessGameManager.instance.boardState;
        

        bool inCheck = IsInCheck(boardState, isWhiteTurn);
        bool legalMoves = HasLegalMoves(isWhiteTurn);

        if (inCheck)
        {
            if (!legalMoves)
            {
                Debug.Log("Mat! Koniec gry");
                return "Mat";
            }
            else
            {
                return "szach";
            }
        }
        else
        {
            if (!legalMoves)
            {
                return "Pat";
            }
            else
            {
                return "Nothing";
            }
        }
    





    }

    public ChessPiece FindKing(bool isWhite)
    {
        ChessPiece[,] boardState = ChessGameManager.instance.boardState;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (boardState[x, y] is King king && king.isWhite == isWhite)
                {
                    return king;
                }
            }
        }

        return null;
    }
}
