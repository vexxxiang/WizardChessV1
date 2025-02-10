using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public static AnimatorManager instace;
    public float rotationSpeed ; // Pr�dko�� obrotu
    public float movingSpeed; // Pr�dko�� obrotu
    public float rotationThreshold; // Pr�g, kiedy uznajemy obr�t za zako�czony
    public float distanceThreshold; 
    public float offsetStopMoving ;
    public float TRPawn = 1f, TRRook = 0.16f, TRBishop = 2.16f, TRKnight = 1.5f, TRQueen = 1.16f, TRKing = 1.5f;
    public GameObject  _Pawn, _Rook, _Bishop, _Knight, _Queen, _King, _PawnB, _RookB, _BishopB, _KnightB, _QueenB, _KingB;
    private bool isRunning = false;
    GameObject model, destroy;
    Vector3 Position;
    public bool first = true;


    public ChessPiece movingFigure, targetFigure;
    public bool looking = false;
    public Vector2Int startPos;
    public void Start()
    {
        instace = this;
    }
    public void StartAnimation(ChessPiece _movingFigure, ChessPiece _targetFigure)
    {
        movingFigure = _movingFigure;
        targetFigure = _targetFigure;
        StartCoroutine(RotateTowardsTarget());
        startPos = movingFigure.boardPosition;


    }
        IEnumerator RotateTowardsTarget()
    {

        if (targetFigure == null) yield break;

        // R�nica w pozycjach mi�dzy targetFigure a movingFigure
        Vector3 direction = new Vector3(
            targetFigure.boardPosition.x - movingFigure.boardPosition.x,
            0, // Wysoko�� nie zmienia si�
            targetFigure.boardPosition.y - movingFigure.boardPosition.y
        );

        // Wykorzystanie funkcji LookRotation do obliczenia celu obrotu
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // P�ynne obracanie obiektu w stron� targetFigure
        while (Quaternion.Angle(movingFigure.transform.rotation, targetRotation) > rotationThreshold)
        {
            movingFigure.transform.rotation = Quaternion.Slerp(movingFigure.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null; // Czekamy na kolejny frame
        }

        // Obr�t zako�czony, wykonujemy akcj�
        //Debug.Log("Obr�t zako�czony!");

       
            StartCoroutine(MoveAniamtion());
        
        
       
       
       

    }

    IEnumerator MoveAniamtion()
    {
        //Debug.Log("second step animation");
        if (targetFigure == null) yield break;

        // Obliczanie r�nicy w pozycjach (pozostawiamy Z i X do przesuni�cia)
        Vector3 difference = new Vector3(
            targetFigure.boardPosition.x - movingFigure.boardPosition.x,
            0, // Wysoko�� nie zmienia si�
            targetFigure.boardPosition.y - movingFigure.boardPosition.y
        );

        // Sprawdzamy po kt�rej stronie znajduje si� movingFigure wzgl�dem targetFigure
        Vector3 direction = difference.normalized;

        // Ustalanie, po kt�rej stronie jest movingFigure:
        // Je�li movingFigure jest po prawej stronie targetFigure na osi X
        bool isRightSide = difference.x > 0;
        bool isUpSide = difference.z > 0;

        // Obliczamy przesuni�cie, uwzgl�dniaj�c offset
        Vector3 QuatersLogics = new Vector3(
            (Mathf.Abs(difference.x) - offsetStopMoving) * (isRightSide ? 1 : -1),  // Kierunek zale�ny od strony (lewa/prawa)
            0,  // Wysoko�� (Y) nie zmienia si�
            (Mathf.Abs(difference.z) - offsetStopMoving) * (isUpSide ? 1 : -1) // Kierunek w osi Z
        );



        // Obliczenie nowej pozycji docelowej z uwzgl�dnieniem offsetu
        Vector3 preTargetPosition = movingFigure.transform.position + QuatersLogics;

        var startpos = movingFigure.transform.position;
        var elapsedTime = 0f;
        // P�ynne przesuwanie
        while (Vector3.Distance(movingFigure.transform.position, preTargetPosition) > distanceThreshold)
        {

            float t = elapsedTime / 1; // Normalizacja czasu (od 0 do 1)
            t = Mathf.SmoothStep(0f, 1f, t); // Dodanie efektu ease in-out

            movingFigure.transform.position = Vector3.Lerp(startpos, preTargetPosition, t);

            elapsedTime += Time.deltaTime;
         
            yield return null;  // Czekaj na kolejny frame
        }
        movingFigure.transform.position = preTargetPosition;
        // Gdy obiekt dotrze do docelowej pozycji

        looking = true;
        PlayingAnimation();


    }


    public void PlayingAnimation()
    {
        //Debug.Log(movingFigure.transform.position);
        
        movingFigure.GetComponent<Animator>().SetBool("MoveAnimation", true);
        isRunning = true;
    }

    void Update()
    {

        if (targetFigure != null && looking)
        {
            Vector3 directionToTarget = targetFigure.transform.position - movingFigure.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            movingFigure.transform.rotation = Quaternion.Lerp(movingFigure.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        if (isRunning)
        {
            
            var timeRemaining = 0f;
            //dla bia�ych
            if (targetFigure.CompareTag("Pawn") && targetFigure.isWhite) { timeRemaining = TRPawn; model = _Pawn; }
            else if (targetFigure.CompareTag("Rook") && targetFigure.isWhite) { timeRemaining = TRRook; model = _Rook; }
            else if (targetFigure.CompareTag("King") && targetFigure.isWhite) { timeRemaining = TRKing; model = _King; }
            else if (targetFigure.CompareTag("Bishop") && targetFigure.isWhite) { timeRemaining = TRBishop; model = _Bishop; }
            else if (targetFigure.CompareTag("Knight") && targetFigure.isWhite) { timeRemaining = TRKnight; model = _Knight; }
            else if (targetFigure.CompareTag("Queen") && targetFigure.isWhite) { timeRemaining = TRQueen; model = _Queen; }
            //dla czarnych
            else if (targetFigure.CompareTag("Pawn") && !targetFigure.isWhite) { timeRemaining = TRPawn; model = _PawnB; }
            else if (targetFigure.CompareTag("Rook") && !targetFigure.isWhite) { timeRemaining = TRRook; model = _RookB; }
            else if (targetFigure.CompareTag("King") && !targetFigure.isWhite) { timeRemaining = TRKing; model = _KingB; }
            else if (targetFigure.CompareTag("Bishop") && !targetFigure.isWhite) { timeRemaining = TRBishop; model = _BishopB; }
            else if (targetFigure.CompareTag("Knight") && !targetFigure.isWhite) { timeRemaining = TRKnight; model = _KnightB; }
            else if (targetFigure.CompareTag("Queen") && !targetFigure.isWhite) { timeRemaining = TRQueen; model = _QueenB; }

            
            Invoke("changeModel", timeRemaining);

            isRunning = false;


        }
        
    }
    public void changeModel()
    {
        movingFigure.GetComponent<CollisionScript>().SwichMeshCollider();
        looking = false;
        Position = targetFigure.transform.position;
        Quaternion Quat = targetFigure.transform.rotation;
        Destroy(targetFigure.gameObject);

        destroy = Instantiate(model, Position, Quat);

        Invoke("end", 2f);
        
    }
    public void end()
    {
        Destroy(destroy.gameObject);
        movingFigure.GetComponent<CollisionScript>().SwichMeshCollider();
        //Debug.Log(new Vector2Int((int)Position.x, (int)Position.z));
        ChessGameManager.instance.boardState[(int)Position.x,(int)Position.z] = null;
        ChessGameManager.instance.atack = true;
        ChessGameManager.instance.MovePiece(new Vector2Int((int)Position.x,(int)Position.z));
        
    }


}


