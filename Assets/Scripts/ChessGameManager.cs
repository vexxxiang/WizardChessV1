using UnityEngine;
using UnityEngine.UIElements;

public class ChessGameManager : MonoBehaviour
{
    public static ChessGameManager instance;
    public ChessPiece[,] boardState = new ChessPiece[8, 8];  // Tablica stanu planszy (bierek)
    public ChessPiece selectedPiece; // Wybrana bierka
    public bool isWhiteTurn = true;  // Sprawdzanie, której stronie nale¿y tura
    public bool atack = false;
    public GameObject _Camera, _ChessBoard;
    public GameObject PromotionPanel;
    public ChessPiece Piece;


   
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
        Debug.Log("jestem w gm i chce ruszyc");
        if (selectedPiece != null)
        {
            // Sprawdzamy, czy ruch jest dozwolony
            bool[,] availableMoves = selectedPiece.GetAvailableMoves(boardState);
            Debug.Log("przed sprawdzeniem avaiblemoves");
            Debug.Log("mozliwy ruch ?: " + availableMoves[targetPosition.x, targetPosition.y]);
            if (availableMoves[targetPosition.x, targetPosition.y])
            {
                Debug.Log("po sprawdzeniem avaiblemoves");
                selectedPiece.GetComponent<ChessPiece>().Moved = true;
                // Ruch dozwolony - ustawiamy now¹ pozycjê bierki
                selectedPiece.SetPosition(targetPosition);

                if (isWhiteTurn)
                {
                    selectedPiece.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                }
                else
                {
                    selectedPiece.gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
                }


                // Prze³¹czamy turê
                isWhiteTurn = !isWhiteTurn;
                selectedPiece.boardPosition = targetPosition;



            }
            else if (atack)
            {
                Debug.Log("ruch ponad wszystko");
                selectedPiece.GetComponent<ChessPiece>().Moved = true;
                // Ruch dozwolony - ustawiamy now¹ pozycjê bierki
                selectedPiece.SetPosition(targetPosition);

                if (isWhiteTurn)
                {
                    selectedPiece.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                }
                else
                {
                    selectedPiece.gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
                }


                // Prze³¹czamy turê
                isWhiteTurn = !isWhiteTurn;
                selectedPiece.boardPosition = targetPosition;



                atack = false;
            }
            else
            {
                _ChessBoard.GetComponent<ChessBoard>().illegalMove(new Vector2Int(targetPosition.x, targetPosition.y));

            }
        }
        else
        {
            Debug.Log("Nie wybrano bierki.");
        }
        if (selectedPiece.CompareTag("Pawn"))
        {
            if (selectedPiece.isWhite && selectedPiece.boardPosition.y == 7)
            {
                PromotionPanel.SetActive(true);
                Debug.Log("Promotion white");

                Piece = selectedPiece;
                    


            }
            else if (!selectedPiece.isWhite && selectedPiece.boardPosition.y == 0)
            {
                PromotionPanel.SetActive(true);
                Debug.Log("Promotion black");

                Piece = selectedPiece;



            }
            else
            {
                _Camera.GetComponent<CameraSettings>().RotateAroundBoard();
            }
        }
        else
        {
            _Camera.GetComponent<CameraSettings>().RotateAroundBoard();
        }

    }
    public void CastleMovePiece(Vector2Int targetPosition)
    {
       
            // Sprawdzamy, czy ruch jest dozwolony
           
                selectedPiece.GetComponent<ChessPiece>().Moved = true;
                // Ruch dozwolony - ustawiamy now¹ pozycjê bierki
                selectedPiece.SetPosition(targetPosition);

                // Prze³¹czamy turê
                
                selectedPiece.boardPosition = targetPosition;
                selectedPiece = null;
        
        
        
           
        
       
    }
    public bool TestMovePiece(Vector2Int targetPosition)
    {
        //Debug.Log("jestem w gm i chce ruszyc");
        if (selectedPiece != null)
        {

            // Sprawdzamy, czy ruch jest dozwolony
            bool[,] availableMoves = selectedPiece.GetAvailableMoves(boardState);

            if (availableMoves[targetPosition.x, targetPosition.y])
            {
                return true;
            }
            else
            {
                _ChessBoard.GetComponent<ChessBoard>().illegalMove(targetPosition);
                return false;
            }
        }
        else {
            Debug.Log("Nie wybrano bierki");
            return false;
        }
    }
}
