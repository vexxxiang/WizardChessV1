using UnityEngine;
public abstract class ChessPiece : MonoBehaviour
{
    public Vector2Int boardPosition; // Przechowuje pozycjê bierki na planszy
    public bool isWhite; // Okreœla, czy bierka jest bia³a


    // Metoda wirtualna do obliczenia dostêpnych ruchów dla danej bierki
    public virtual bool[,] GetAvailableMoves(ChessPiece[,] boardState)
    {
        bool[,] moves = new bool[8, 8]; // Tablica dostêpnych ruchów
        return moves;
    }

    // Ustawia now¹ pozycjê bierki zarówno w przestrzeni gry, jak i na planszy
    public virtual void SetPosition(Vector2Int newPosition) // Zmienione na virtual
    {
        boardPosition = newPosition; // Aktualizowanie pozycji bierki na planszy
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