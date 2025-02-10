using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ChessGameManager : MonoBehaviour
{
    public static ChessGameManager instance;
    public ChessPiece[,] boardState = new ChessPiece[8, 8];  // Tablica stanu planszy (bierek)
    public ChessPiece selectedPiece; // Wybrana bierka
    public bool isWhiteTurn = true;  // Sprawdzanie, której stronie nale¿y tura
    public bool atack = false;
    public bool PromotionNow = false;
    public GameObject _Camera, _ChessBoard;
    public GameObject PromotionPanel;
    public ChessPiece Piece, promotionPiece;
    public Vector3 finalPos;

   
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
            //Debug.Log("Wybrano bierkê: " + piece.name + " na pozycji: " + position);
        }
    }


    public IEnumerator RotateTowardsPlane()
    {
        
       


        if (selectedPiece == null) yield break;

        var startRotation = selectedPiece.transform.rotation;
        Debug.Log(finalPos + "<- final pos | boardPosition -> " + selectedPiece.boardPosition);

        Vector3 direction = new Vector3(
            finalPos.x - selectedPiece.boardPosition.x,
            0, // Wysokoœæ nie zmienia siê
            finalPos.z - selectedPiece.boardPosition.y
        ); ;


        Quaternion targetRotation = Quaternion.LookRotation(direction);

        Debug.Log(direction + "<-kierunek | rotacja -> " + targetRotation);
   

        while (Quaternion.Angle(selectedPiece.transform.rotation, targetRotation) > AnimatorManager.instace.rotationThreshold)
        {
            selectedPiece.transform.rotation = Quaternion.Slerp(selectedPiece.transform.rotation, targetRotation, Time.deltaTime * AnimatorManager.instace.rotationSpeed);
            yield return null;
        }

        StartCoroutine(MoveAnimation());





    }
    public void MovePiece(Vector2Int targetPosition)
    {
        finalPos = new Vector3(targetPosition.x, 0, targetPosition.y);
       if (selectedPiece != null )
        {
            // Sprawdzamy, czy ruch jest dozwolony
            bool[,] availableMoves = selectedPiece.GetAvailableMoves(boardState);
            if (availableMoves[targetPosition.x, targetPosition.y] && atack == false)
            {
              
           


                StartCoroutine(RotateTowardsPlane());


            }
            else if (atack)
            {
                finalPos = new Vector3(targetPosition.x, 0, targetPosition.y);
                StartCoroutine(MoveAnimation());
                


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
           
        }

    }

    public void promotion() {

        StartCoroutine(RotateTowardsPlane());





    }
    IEnumerator MoveAnimation()
    {
        var startpos = selectedPiece.transform.position;
        var elapsedTime = 0f;

        if (selectedPiece == null) yield break;
        while (Vector3.Distance(selectedPiece.transform.position, finalPos) > 0.001f)
        {
            float t = elapsedTime / 1; // Normalizacja czasu (od 0 do 1)
            t = Mathf.SmoothStep(0f, 1f, t); // Dodanie efektu ease in-out

            selectedPiece.transform.position = Vector3.Lerp(startpos, finalPos, t);

            elapsedTime += Time.deltaTime;
            yield return null; // Czekaj na kolejny frame
        }
        selectedPiece.transform.position = finalPos;
        selectedPiece.GetComponent<ChessPiece>().Moved = true;
        // Ruch dozwolony - ustawiamy now¹ pozycjê bierki
        selectedPiece.SetPosition(new Vector2Int((int)finalPos.x,(int)finalPos.z));
        /*
        if (isWhiteTurn)
        {
            selectedPiece.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            selectedPiece.gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        */

        // Prze³¹czamy turê

        if (!PromotionNow)
        {
            isWhiteTurn = !isWhiteTurn;
            selectedPiece.boardPosition = new Vector2Int((int)finalPos.x, (int)finalPos.z);
            atack = false;
            Invoke("rotate", 1f);
            selectedPiece = null;
        }
        else {
            PromotionManager.instance.changeFigure();
            selectedPiece.GetComponent<ChessPiece>().Moved = true;
            isWhiteTurn = !isWhiteTurn;
            selectedPiece.boardPosition = new Vector2Int((int)finalPos.x, (int)finalPos.z);
            boardState[(int)finalPos.x,(int)finalPos.z] = promotionPiece;
            Invoke("rotate", 1f);
            selectedPiece = null;
            PromotionNow = false;

        }
       
        



    }
    public void rotate()
    {
        _Camera.GetComponent<CameraSettings>().RotateAroundBoard();
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
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            for(int x= 0; x<=7; ++x)
            {
                for (int z = 0; z <= 7; ++z)
                {
                    Debug.Log(boardState[x,z] + "   x:" +x  + " z:"+z);
                }
            }
            
        }
    }
}
