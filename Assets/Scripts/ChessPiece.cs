using System.Collections.Generic;
using UnityEngine;
public abstract class ChessPiece : MonoBehaviour
{
    public Vector2Int boardPosition; // Przechowuje pozycjê bierki na planszy
    public bool isWhite; // Okreœla, czy bierka jest bia³a
    public bool Moved = false;
    public bool King = false;
    public bool[,] CancelingSzachMoves = new bool[8, 8];
    private bool blockingSquares;

    public void Start()
    {
        if (gameObject.CompareTag("King"))
        {
            King = true;
        }
    }




    // Metoda wirtualna do obliczenia dostêpnych ruchów dla danej bierki
    public virtual bool[,] GetAvailableMoves(ChessPiece[,] boardState)
    {
        bool[,] moves = new bool[8, 8]; // Tablica dostêpnych ruchów
        return moves;
    }
    private List<Vector2Int> GetBlockingSquares(Vector2Int attackerPos, Vector2Int kingPos)
    {
        List<Vector2Int> squares = new List<Vector2Int>();
        // Obliczenie kierunku (dla osi x i y, przyjmuj¹c, ¿e plansza u¿ywa Vector2Int)
        int dirX = (kingPos.x - attackerPos.x) == 0 ? 0 : (kingPos.x - attackerPos.x) / Mathf.Abs(kingPos.x - attackerPos.x);
        int dirY = (kingPos.y - attackerPos.y) == 0 ? 0 : (kingPos.y - attackerPos.y) / Mathf.Abs(kingPos.y - attackerPos.y);

        Vector2Int current = attackerPos;
        while (current != kingPos)
        {
            current = new Vector2Int(current.x + dirX, current.y + dirY);
            if (current != kingPos)
                squares.Add(current);
        }
        return squares;
    }
    public virtual bool[,] PreGetAvailableMovesAtSzach(ChessPiece[,] boardState, ChessPiece AtackingFigure)
    {
        bool[,] movesSzachCanceling = new bool[8, 8]; // Tablica dostêpnych ruchów blokuj¹cych szacha
        bool[,] thisAvailableMoves = GetAvailableMoves(boardState); // Dostêpne ruchy danej bierki

        // Za³ó¿, ¿e masz ju¿ zmienn¹ KingPos, która wskazuje na pozycjê króla
        Vector2Int KingPos = GameState.instance.KingPos;
        List<Vector2Int> blockingSquares = GetBlockingSquares(AtackingFigure.boardPosition, KingPos);

        for (int x = 0; x < 8; ++x)
        {
            for (int y = 0; y < 8; ++y)
            {
                if (thisAvailableMoves[x, y] && blockingSquares.Contains(new Vector2Int(x, y)))
                {
                    CancelingSzachMoves[x, y] = true;
                    movesSzachCanceling[x, y] = true;
                    ChessBoard.instance.zaznacz(new Vector2Int(x, y));
                }
            }
        }
        Debug.Log(this.gameObject.name + " " + movesSzachCanceling);
        return movesSzachCanceling;
    }


    // Ustawia now¹ pozycjê bierki zarówno w przestrzeni gry, jak i na planszy
    public virtual void SetPosition(Vector2Int newPosition) // Zmienione na virtual
    {
        ChessGameManager.instance.boardState[boardPosition.x, boardPosition.y] = null;
        boardPosition = newPosition; // Aktualizowanie pozycji bierki na planszy
        ChessGameManager.instance.boardState[boardPosition.x, boardPosition.y] = this;
        transform.position = new Vector3(newPosition.x, 0, newPosition.y); // Aktualizowanie fizycznej pozycji w 3D
    }

    // Sprawdza, czy pozycja (x, y) mieœci siê na planszy
    protected bool IsInsideBoard(int x, int y)
    {
        return x >= 0 && x < 8 && y >= 0 && y < 8;
    }

    // Sprawdza, czy bierka mo¿e siê poruszyæ lub przechwyciæ bierkê na danym polu
    protected bool CanMoveOrCapture(ChessPiece[,] boardState, int x, int y)
    {
        if (!IsInsideBoard(x, y)) return false; // Pozycja poza plansz¹
        if (boardState[x, y] == null) return true; // Puste pole
        return boardState[x, y].isWhite != isWhite; // Zasada przechwytywania bierki przeciwnej strony
    }

}