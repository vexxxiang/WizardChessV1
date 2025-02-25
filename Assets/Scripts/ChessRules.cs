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
        // ZnajdŸ króla dla aktualnego gracza
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
            Debug.LogError("Nie znaleziono króla dla " + (isWhiteTurn ? "bia³ych" : "czarnych"));
            return true; // Traktujemy to jako szach – b³¹d w stanie gry.
        }

        // Dla ka¿dej bierki przeciwnika sprawdŸ, czy mo¿e zagraæ na polu, na którym stoi król
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
                                // Tworzymy kopiê planszy
                                ChessPiece[,] simulation = DeepCopyBoard(boardState);

                                // Symulujemy ruch
                                simulation[x, y] = null;
                                simulation[i, j] = piece;

                                // Sprawdzamy, czy po ruchu król nadal jest w szachu
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
                    copy[x, y] = Instantiate(original[x, y]);  // Tworzymy now¹ instancjê figury
                    copy[x, y].boardPosition = new Vector2Int(x, y);
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
                                // Tworzymy symulowan¹ kopiê planszy
                                ChessPiece[,] simulation = DeepCopyBoard(boardState);

                                // Symulujemy ruch
                                simulation[x, y] = null; // Usuwamy bierkê ze starego miejsca
                                simulation[i, j] = piece; // Przenosimy na now¹ pozycjê
                                Vector2Int oldPos = piece.boardPosition;
                                piece.boardPosition = new Vector2Int(i, j);

                                // Sprawdzamy, czy król nadal jest w szachu
                                bool isValid = !IsInCheck(simulation, piece.isWhite);

                                // Przywracamy oryginaln¹ pozycjê
                                piece.boardPosition = oldPos;

                                // Jeœli ruch ratuje króla, ustawiamy CancelingSzachMoves
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
                Debug.Log("Mat! " + (isWhiteTurn ? "Biali" : "Czarni") + " przegrywaj¹.");
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
