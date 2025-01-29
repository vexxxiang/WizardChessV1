using UnityEngine;

public class ChessGameManager : MonoBehaviour
{
    public ChessPiece[,] boardState = new ChessPiece[8, 8];  // Tablica stanu planszy (bierek)
    private ChessPiece selectedPiece; // Wybrana bierka
    public bool isWhiteTurn = true;  // Sprawdzanie, której stronie nale¿y tura
    public GameObject _Camera;

    void Start()
    {
        // Przyk³ad inicjalizacji gry - np. umieszczanie bierek
        // Mo¿esz to dostosowaæ do swojego kodu
    }

    // Funkcja do wyboru bierki (klikniêcie na bierkê)
    public void SelectPiece(ChessPiece piece, Vector2Int position)
    {
        if ((isWhiteTurn && piece.isWhite) || (!isWhiteTurn && !piece.isWhite))
        {
            selectedPiece = piece;
            Debug.Log("Wybrano bierkê: " + piece.name + " na pozycji: " + position);
        }
    }

    // Funkcja do wykonania ruchu bierk¹ (klikniêcie na pole)
    public void MovePiece(Vector2Int targetPosition)
    {
        if (selectedPiece != null)
        {
            // Sprawdzamy, czy ruch jest dozwolony
            bool[,] availableMoves = selectedPiece.GetAvailableMoves(boardState);

            if (availableMoves[targetPosition.x, targetPosition.y])
            {
                // Ruch dozwolony - ustawiamy now¹ pozycjê bierki
                selectedPiece.SetPosition(targetPosition);

                // Prze³¹czamy turê
                isWhiteTurn = !isWhiteTurn;

                // Resetujemy wybran¹ bierkê
                selectedPiece = null;

                _Camera.GetComponent<CameraSettings>().RotateAroundBoard();


            }
            else
            {
                Debug.Log("Niedozwolony ruch na to pole.");
            }
        }
        else
        {
            Debug.Log("Nie wybrano bierki.");
        }
    }
}
