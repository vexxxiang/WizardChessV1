using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessRules : MonoBehaviour
{
    public static ChessRules instance;

    private void Awake()
    {
        instance = this;
    }

    public bool IsInCheck(ChessPiece[,] boardState, bool isWhiteTurn)
    {
        ChessPiece king = null;
        // Znajd� kr�la dla aktualnego gracza
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece piece = boardState[x, y];
                if (piece != null && piece.CompareTag("King") && piece.isWhite == isWhiteTurn)
                {
                    king = piece;
                    break;
                }
            }
            if (king != null) break;
        }
        if (king == null)
        {
            Debug.LogError("Nie znaleziono kr�la dla " + (isWhiteTurn ? "bia�ych" : "czarnych"));
            return true; // Traktujemy to jako szach � b��d w stanie gry.
        }

        // Dla ka�dej bierki przeciwnika sprawd�, czy mo�e zagra� na polu, na kt�rym stoi kr�l (sprawdzanie czy jest szach)
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece piece = boardState[x, y];
                if (piece != null && piece.isWhite != isWhiteTurn)
                {
                    bool[,] moves = piece.GetAvailableMoves(boardState);
                    if (moves[king.boardPosition.x, king.boardPosition.y])
                    {
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
                    bool[,] moves = piece.GetAvailableMoves(boardState);

                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (moves[i, j])
                            {
                                // Tworzymy kopi� planszy
                                ChessPiece[,] simulation = DeepCopyBoard(boardState);

                                // Pobieramy klon figury z oryginalnej pozycji
                                ChessPiece movedPiece = simulation[x, y];
                                simulation[x, y] = null;
                                movedPiece.boardPosition = new Vector2Int(i, j);
                                simulation[i, j] = movedPiece;

                                // Sprawdzamy, czy po symulowanym ruchu kr�l nie jest w szachu
                                if (!IsInCheck(simulation, isWhiteTurn))
                                {
                                    return true;
                                }
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

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (original[x, y] != null)
                {
                    var obj = new GameObject(original[x, y].name+"kopy");
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
                    //copy[x, y] = Instantiate(original[x, y]);  // Klonujemy figur�
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
                                // Tworzymy symulowan� kopi� planszy
                                ChessPiece[,] simulation = DeepCopyBoard(boardState);

                                // Pobieramy klon figury
                                ChessPiece clonedPiece = simulation[x, y];
                                simulation[x, y] = null;
                                clonedPiece.boardPosition = new Vector2Int(i, j);
                                simulation[i, j] = clonedPiece;

                                // Sprawdzamy, czy po ruchu kr�l nadal jest w szachu
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
                Debug.Log("Mat! " + (isWhiteTurn ? "Biali" : "Czarni") + " przegrywaj�.");
                return "Mat";
            }
            else
            {
                return "Szach";
            }
        }
        else
        {
            if (!legalMoves)
            {
                Debug.Log("Pat!");
                return "Pat";
            }
            else
            {
                Debug.Log("Gra kontynuuje.");
                return "Nothing";
            }
        }
    }
}
