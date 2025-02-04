using UnityEngine;

public class ChessGameManager : MonoBehaviour
{
    public static ChessGameManager instance;
    public ChessPiece[,] boardState = new ChessPiece[8, 8];  // Tablica stanu planszy (bierek)
    public ChessPiece selectedPiece; // Wybrana bierka
    public bool isWhiteTurn = true;  // Sprawdzanie, której stronie nale¿y tura
    public GameObject _Camera, _ChessBoard;
    

    void Start()
    {
        instance = this;
        _ChessBoard.GetComponent<ChessBoard>().CreateBoard();
        this.gameObject.GetComponent<ChessSetup>().SetupPieces();
    }

    public void SelectPiece(ChessPiece piece, Vector2Int position)
    {
        if ((isWhiteTurn && piece.isWhite) || (!isWhiteTurn && !piece.isWhite))
        {
            selectedPiece = piece;
            Debug.Log("Wybrano bierkê: " + piece.name + " na pozycji: " + position);
        }
    }
    public void MovePiece(Vector2Int targetPosition)
    {
        //Debug.Log("jestem w gm i chce ruszyc");
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
                selectedPiece.boardPosition = targetPosition;
                
                _Camera.GetComponent<CameraSettings>().RotateAroundBoard();
            }
            else
            {
                _ChessBoard.GetComponent<ChessBoard>().illegalMove(new Vector2Int(targetPosition.x,targetPosition.y));

            }
        }
        else
        {
            Debug.Log("Nie wybrano bierki.");
        }
    }
}
