using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System.Collections.Generic;

public class ChessGameManager : MonoBehaviour
{
    public static ChessGameManager instance;
    public ChessPiece[,] boardState = new ChessPiece[8, 8];  // Tablica stanu planszy (bierek)
    public ChessPiece selectedPiece; // Wybrana bierka
    public bool isWhiteTurn = true;  // Sprawdzanie, której stronie należy tura
    public bool showTour = true;  // Sprawdzanie, której stronie należy tura

    public Transform _bierki;
    public GameObject _Camera, _ChessBoard;
    public GameObject PromotionPanel;
    public ChessPiece Piece, promotionPiece;
    public Vector3 finalPos;
    public ChessBoard CP;
    public ChessPiece targetFigure;
    public bool looking = false;
    public bool atackIsRunning = false;
    public GameObject model;
    public ChessPiece selectedPieceForPromotion;
    private GameObject destroy;
    private Vector3 targetPosition;
    public GameObject turaB, turaW;
    public bool isSzach = false;
    public float TRPawn = 1f, TRRook = 0.16f, TRBishop = 2.16f, TRKnight = 1.5f, TRQueen = 1.16f, TRKing = 1.5f; // <- Czas 
    public GameObject _Pawn, _Rook, _Bishop, _Knight, _Queen, _King, _PawnB, _RookB, _BishopB, _KnightB, _QueenB, _KingB; // <- Prefaby Destroyed
    
    public AudioClip[] movingSounds;
    public AudioSource movingAudioSource;

    public float time;
    public bool TimerOn;

    void Start()
    {

        instance = this;
        _ChessBoard.GetComponent<ChessBoard>().CreateBoard();
        this.gameObject.GetComponent<ChessSetup>().SetupPieces();
    }
    public void PlaySound()
    {

        if (isWhiteTurn)
        {

            movingAudioSource.PlayOneShot(movingSounds[0]);
            
        }
        if (!isWhiteTurn)
        {

            movingAudioSource.PlayOneShot(movingSounds[1]);

        }


    }
    public void SelectPiece(ChessPiece piece, Vector2Int position)
    {
        if ((isWhiteTurn && piece.isWhite) || (!isWhiteTurn && !piece.isWhite))
        {
            selectedPiece = piece;
           //Debug.Log("Wybrano bierkę: " + piece.name + " na pozycji: " + position);
        }

    }
    public void UnSelectPiece()
    {
        //Debug.Log("Anulowano Wybór bierkę: " + selectedPiece.name + " na pozycji: " + selectedPiece.boardPosition);
        selectedPiece = null;
    }
    public void MovePiece(Vector2Int targetPosition, bool Forced)
    {


        if (ChessRules.instance.EvaluateGameState() == "szach")
        {
            //CP = ChessBoard.instance;
            if (selectedPiece != null)
            {
                selectedPieceForPromotion = selectedPiece;
                // Sprawdzamy, czy ruch jest dozwolony

                bool[,] availableMoves = selectedPiece.GetLegalMoves(boardState);
                if (availableMoves[targetPosition.x, targetPosition.y] || Forced)
                {
                    if (Promocja() == true)
                    {
                        ChessBoard.instance.selecting = false;
                        StartCoroutine(AnimationFigureMove());


                    }
                    else
                    {
                        ChessBoard.instance.selecting = false;
                        StartCoroutine(AnimationFigureMove());
                    }



                }
                else
                {
                    ChessBoard.instance.illegalMove(new Vector2Int(targetPosition.x, targetPosition.y));
                    selectedPiece = null;
                }



            }
            else
            {
                Debug.Log("Nie wybrano bierki.");
            }

        }
        else
        {


            //CP = ChessBoard.instance;
            if (selectedPiece != null)
            {
                selectedPieceForPromotion = selectedPiece;
                // Sprawdzamy, czy ruch jest dozwolony
                if (isSzach)
                {
                    bool[,] availableMoves = selectedPiece.CancelingSzachMoves;
                    if (availableMoves[targetPosition.x, targetPosition.y] || Forced)
                    {
                        if (Promocja() == true)
                        {
                            ChessBoard.instance.selecting = false;
                            StartCoroutine(AnimationFigureMove());


                        }
                        else
                        {
                            ChessBoard.instance.selecting = false;
                            StartCoroutine(AnimationFigureMove());
                        }



                    }
                    else
                    {
                        ChessBoard.instance.illegalMove(new Vector2Int(targetPosition.x, targetPosition.y));
                        selectedPiece = null;
                    }
                }
                else
                {
                    bool[,] availableMoves = selectedPiece.CancelingSzachMoves;
                    if (availableMoves[targetPosition.x, targetPosition.y] || Forced)
                    {
                        if (Promocja() == true)
                        {
                            ChessBoard.instance.selecting = false;
                            StartCoroutine(AnimationFigureMove());


                        }
                        else
                        {
                            ChessBoard.instance.selecting = false;
                            StartCoroutine(AnimationFigureMove());
                        }



                    }
                    else
                    {
                        ChessBoard.instance.illegalMove(new Vector2Int(targetPosition.x, targetPosition.y));
                        selectedPiece = null;
                    }
                }


            }
            else
            {
                Debug.Log("Nie wybrano bierki.");
            }
            if (ChessRules.instance.EvaluateGameState() == "Pat")
            { 
                Debug.Log("pat");
            }


            if (ChessRules.instance.EvaluateGameState() == "Mat")
            {
                if (isWhiteTurn)
                {
                    Debug.Log("wygrały czarne");
                    DestroyKing();
                }
                else
                {
                    Debug.Log("wygrały białe");
                    DestroyKing();
                }
            }

        }


        

    }
        
    public void AtackPiece(Vector2Int targetPosition)
    {
        targetFigure = boardState[ChessBoard.instance.ClickedPlane.x, ChessBoard.instance.ClickedPlane.y];
        if (selectedPiece)
        {
            selectedPieceForPromotion = selectedPiece;
            ChessBoard.instance.selecting = false;
            if (Promocja() == true)
            {
                ChessBoard.instance.selecting = false;
                
                //Debug.Log("GM: atakuje ->> " + targetPosition);
                StartCoroutine(AnimationFigureAtack());
                
            }
            else {
                //Debug.Log("GM: atakuje ->> " + targetPosition);
                StartCoroutine(AnimationFigureAtack());
            }
            

        }
        else
        {
            Debug.Log("Atakujesz a nie ma wybranej bierki");
        }

    }
    IEnumerator AnimationFigureMove()
    {
        
        var rotationThreshold = 0.001f;
        var rotationSpeed = 5f;
        var elapsedTime = 0f;
        if (selectedPiece == null) yield break;


        Vector3 direction = new Vector3(ChessBoard.instance.ClickedPlane.x - selectedPiece.boardPosition.x, 0, ChessBoard.instance.ClickedPlane.y - selectedPiece.boardPosition.y);


        Quaternion targetRotation = Quaternion.LookRotation(direction);

        //Debug.Log(direction + "<-kierunek | rotacja -> " + targetRotation);


        PlaySound();
        while (Vector3.Distance(selectedPiece.transform.position, new Vector3(ChessBoard.instance.ClickedPlane.x, 0, ChessBoard.instance.ClickedPlane.y)) > 0.001f)
        {
            float t = elapsedTime / 7.0f; // Normalizacja czasu (od 0 do 1)--------------------------------------------------------------------------------------------- tu przy poruszaniu czas zmieniasz
            t = Mathf.SmoothStep(0f, 1f, t); // Dodanie efektu ease in-out

            selectedPiece.transform.position = Vector3.Lerp(selectedPiece.transform.position, new Vector3(ChessBoard.instance.ClickedPlane.x, 0, ChessBoard.instance.ClickedPlane.y), t);
            selectedPiece.transform.rotation = Quaternion.Slerp(selectedPiece.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            elapsedTime += Time.deltaTime;
            yield return null; // Czekaj na kolejny frame
        }
        selectedPiece.transform.position = new Vector3(ChessBoard.instance.ClickedPlane.x, 0, ChessBoard.instance.ClickedPlane.y);
        selectedPiece.GetComponent<ChessPiece>().Moved = true;
        selectedPiece.SetPosition(ChessBoard.instance.ClickedPlane, boardState);
        selectedPiece = null;
        if (!Promocja())
        {

            zmianaTury();

        }
        else 
        {
            ShowPromoPanel();
        }
        
        


    }
    IEnumerator AnimationFigureAtack()
    {
        targetFigure = boardState[ChessBoard.instance.ClickedPlane.x,ChessBoard.instance.ClickedPlane.y];
        var rotationThreshold = 0.001f;
        var rotationSpeed = 5f;
        var offsetStopMoving = 0.65f;
        var distanceThreshold = 0.001f;

        if (selectedPiece == null) yield break;


        Vector3 directionr = new Vector3(ChessBoard.instance.ClickedPlane.x - selectedPiece.boardPosition.x, 0, ChessBoard.instance.ClickedPlane.y - selectedPiece.boardPosition.y);


        Quaternion targetRotation = Quaternion.LookRotation(directionr);

        //Debug.Log(directionr + "<-kierunek | rotacja -> " + targetRotation);

        if (targetFigure == null) yield break;

        // Obliczanie różnicy w pozycjach (pozostawiamy Z i X do przesunięcia)
        Vector3 difference = new Vector3(
            targetFigure.boardPosition.x - selectedPiece.boardPosition.x,
            0, // Wysokość nie zmienia się
            targetFigure.boardPosition.y - selectedPiece.boardPosition.y
        );

        // Sprawdzamy po której stronie znajduje się movingFigure względem targetFigure
        Vector3 direction = difference.normalized;
        Vector3 QuatersLogics;
        // Ustalanie, po której stronie jest movingFigure:
        // Jeśli movingFigure jest po prawej stronie targetFigure na osi X

            Vector2 movePosition = new Vector2(0,0);

            Vector2 sPos = new Vector2(selectedPiece.transform.position.x, selectedPiece.transform.position.z);
            Vector2 ePos = new Vector2(targetFigure.transform.position.x, targetFigure.transform.position.z);

            if (sPos.x > ePos.x && sPos.y == ePos.y)//zprawa
            {
                movePosition.x = ePos.x-sPos.x + (offsetStopMoving+0.2f);
            }
            else if (sPos.x < ePos.x && sPos.y == ePos.y)//zlewa
            {
                movePosition.x = ePos.x - sPos.x - (offsetStopMoving + 0.2f);
            }
            else if(sPos.x == ePos.x && sPos.y > ePos.y)//zgora
            {
                movePosition.y = ePos.y - sPos.y + (offsetStopMoving + 0.2f);
            }
            else if(sPos.x == ePos.x && sPos.y < ePos.y)//zdol
            {
                movePosition.y = ePos.y - sPos.y - (offsetStopMoving + 0.2f);
            }
            else {
                bool isRightSide = difference.x > 0;
                bool isUpSide = difference.z > 0;


                movePosition.x = (Mathf.Abs(difference.x) - offsetStopMoving) * (isRightSide ? 1 : -1);
                movePosition.y = (Mathf.Abs(difference.z) - offsetStopMoving) * (isUpSide ? 1 : -1);
            }

            QuatersLogics = new Vector3(movePosition.x, 0, movePosition.y);
        

            
          

        // Obliczenie nowej pozycji docelowej z uwzględnieniem offsetu
        Vector3 preTargetPosition = selectedPiece.transform.position + QuatersLogics;
        PlaySound();
        var startpos = selectedPiece.transform.position;
        var elapsedTimeMove = 0f;
        // Płynne przesuwanie
        
        while (Vector3.Distance(selectedPiece.transform.position, preTargetPosition) > distanceThreshold)
        {

            float t = elapsedTimeMove / 1.0f; // Normalizacja czasu (od 0 do 1) ------------------------------------------------------------------------------- tu przy atak czas zmieniasz
            //t = Mathf.SmoothStep(0f, 1f, t); // Dodanie efektu ease in-out
            t = -t * -Mathf.Sqrt(t*t*1.2f);

            selectedPiece.transform.position = Vector3.Lerp(startpos, preTargetPosition, t);
            selectedPiece.transform.rotation = Quaternion.Slerp(selectedPiece.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            elapsedTimeMove += Time.deltaTime;

            yield return null;  // Czekaj na kolejny frame

            

        }

        selectedPiece.transform.position = preTargetPosition;
        // Gdy obiekt dotrze do docelowej pozycji

        looking = true;
        selectedPiece.GetComponent<ChessPiece>().PlaySound();
        selectedPiece.GetComponent<CollisionScript>().SwichMeshCollider();
        targetFigure.GetComponent<CollisionScript>().SwichMeshColliderCapsule();
        

        selectedPiece.GetComponent<Animator>().SetBool("MoveAnimation", true);
        time = 0;
        TimerOn = true;
        //atackIsRunning = true;







        // Reszta inicjalizacji...



        /*

        tu dzwiek po ataku
        i sprawdzasz jaka to bierka taką innstrukcją


        if (selectedPiece.CompareTag("King,Knight,Pawn...")
        {
            tu komenda do wywołania dzwieku
        }
        if (selectedPiece.CompareTag("King,Knight,Pawn...")
        {
            tu komenda do wywołania dzwieku
        }
        if (selectedPiece.CompareTag("King,Knight,Pawn...")
        {
            tu komenda do wywołania dzwieku
        }
        if (selectedPiece.CompareTag("King,Knight,Pawn...")
        {
            tu komenda do wywołania dzwieku
        }



        */

    }
    IEnumerator MoveRoszada(ChessPiece FirstPiece, Vector2Int FirstPos, ChessPiece SecPiece, Vector2Int SecPos)
    {

        if (FirstPiece == null || FirstPos == null || SecPiece == null || SecPos == null) yield break;


        var Threshold = 0.001f;
        var rotationSpeed = 5f;
        var elapsedTime = 0f;
        if (selectedPiece == null) yield break;


        Vector3 direction1 = new Vector3(FirstPos.x - FirstPiece.boardPosition.x, 0, FirstPos.y - FirstPiece.boardPosition.y);
        Vector3 direction2 = new Vector3(SecPos.x - SecPiece.boardPosition.x, 0, SecPos.y - SecPiece.boardPosition.y);


        Quaternion targetRotation1 = Quaternion.LookRotation(direction1);
        Quaternion targetRotation2 = Quaternion.LookRotation(direction2);

        //Debug.Log(direction1 + "<-kierunek | rotacja -> " + targetRotation1);
        //Debug.Log(direction2 + "<-kierunek | rotacja -> " + targetRotation2);

        
        while (Quaternion.Angle(FirstPiece.transform.rotation, targetRotation1) > Threshold)
        {
            FirstPiece.transform.rotation = Quaternion.Slerp(FirstPiece.transform.rotation, targetRotation1, Time.deltaTime * rotationSpeed);
            SecPiece.transform.rotation = Quaternion.Slerp(SecPiece.transform.rotation, targetRotation2, Time.deltaTime * rotationSpeed);
            yield return null;
        }
        FirstPiece.transform.rotation = targetRotation1;
        SecPiece.transform.rotation = targetRotation2;
        PlaySound();
        while (Vector3.Distance(FirstPiece.transform.position, new Vector3(FirstPos.x, 0, FirstPos.y)) > Threshold)
        {
            float t = elapsedTime / 8.0f; // Normalizacja czasu (od 0 do 1)------------------------------------------------------------------------------------------------------------------- tu przy roszadzie czas zmieniasz
            t = Mathf.SmoothStep(0f, 1f, t); // Dodanie efektu ease in-out

            FirstPiece.transform.position = Vector3.Lerp(FirstPiece.transform.position, new Vector3(FirstPos.x, 0, FirstPos.y), t);
            SecPiece.transform.position = Vector3.Lerp(SecPiece.transform.position, new Vector3(SecPos.x, 0, SecPos.y), t);

            elapsedTime += Time.deltaTime;
            yield return null; // Czekaj na kolejny frame
        }
        FirstPiece.transform.position = new Vector3(FirstPos.x, 0, FirstPos.y);
        SecPiece.transform.position = new Vector3(SecPos.x, 0, SecPos.y);

        FirstPiece.GetComponent<ChessPiece>().Moved = true;
        SecPiece.GetComponent<ChessPiece>().Moved = true;

        FirstPiece.SetPosition(FirstPos, boardState);
        SecPiece.SetPosition(SecPos, boardState);
        selectedPiece = null;
        ChessBoard.instance.odznacz();
        zmianaTury();
        



    }

    public void Roszada(Vector2Int RookPos)
    {
        List<(int x, int y)> positionsToCheck = new List<(int x, int y)>();
        bool canCastle = false;

        if (isWhiteTurn)
        {
            if (RookPos.x == 0) // Queenside castling for white
            {
                if (boardState[1, 0] == null && boardState[2, 0] == null && boardState[3, 0] == null)
                {
                    positionsToCheck.Add((2, 0)); // King's destination
                    positionsToCheck.Add((3, 0)); // Square it passes through
                    positionsToCheck.Add((4, 0)); // King's current position

                    if (!ChessRules.instance.IsInCheckPosition(boardState, isWhiteTurn, positionsToCheck))
                    {
                        ChessBoard.instance.selecting = false;
                        StartCoroutine(MoveRoszada(boardState[0, 0], new Vector2Int(3, 0), boardState[4, 0], new Vector2Int(2, 0)));
                    }
                    else
                    {
                        ChessBoard.instance.odznacz();
                        ChessBoard.instance.selecting = true;
                        selectedPiece = null;
                    }
                }
            }
            else // Kingside castling for white
            {
                if (boardState[5, 0] == null && boardState[6, 0] == null)
                {
                    positionsToCheck.Add((4, 0));
                    positionsToCheck.Add((5, 0));
                    positionsToCheck.Add((6, 0));

                    if (!ChessRules.instance.IsInCheckPosition(boardState, isWhiteTurn, positionsToCheck))
                    {
                        _ChessBoard.GetComponent<ChessBoard>().selecting = false;
                        StartCoroutine(MoveRoszada(boardState[7, 0], new Vector2Int(5, 0), boardState[4, 0], new Vector2Int(6, 0)));
                    }
                    else
                    {
                        ChessBoard.instance.odznacz();
                        ChessBoard.instance.selecting = true;
                        selectedPiece = null;
                    }
                }
            }
        }
        else
        {
            if (RookPos.x == 0) // Queenside castling for black
            {
                if (boardState[1, 7] == null && boardState[2, 7] == null && boardState[3, 7] == null)
                {
                    positionsToCheck.Add((2, 7));
                    positionsToCheck.Add((3, 7));
                    positionsToCheck.Add((4, 7));

                    if (!ChessRules.instance.IsInCheckPosition(boardState, isWhiteTurn, positionsToCheck))
                    {
                        StartCoroutine(MoveRoszada(boardState[0, 7], new Vector2Int(3, 7), boardState[4, 7], new Vector2Int(2, 7)));
                    }
                    else
                    {
                        ChessBoard.instance.odznacz();
                        ChessBoard.instance.selecting = true;
                        selectedPiece = null;
                    }
                }
            }
            else // Kingside castling for black
            {
                if (boardState[5, 7] == null && boardState[6, 7] == null)
                {
                    positionsToCheck.Add((4, 7));
                    positionsToCheck.Add((5, 7));
                    positionsToCheck.Add((6, 7));

                    if (!ChessRules.instance.IsInCheckPosition(boardState, isWhiteTurn, positionsToCheck))
                    {
                        StartCoroutine(MoveRoszada(boardState[7, 7], new Vector2Int(5, 7), boardState[4, 7], new Vector2Int(6, 7)));
                    }
                    else {
                        ChessBoard.instance.odznacz();
                        ChessBoard.instance.selecting = true;
                        selectedPiece = null;
                    }
                }
            }
        }
    }

    public bool SimulateMoveForCheck(Vector2Int from, Vector2Int to)
    {
        ChessPiece[,] tempBoard = new ChessPiece[8, 8];

        // Kopiujemy aktualny stan planszy
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                tempBoard[x, y] = boardState[x, y];
            }
        }

        ChessPiece movedPiece = tempBoard[from.x, from.y];
        ChessPiece capturedPiece = tempBoard[to.x, to.y];

        // Wykonujemy symulację ruchu
        movedPiece.SetPosition(to, tempBoard);

        // Sprawdzamy czy król jest w szachu
        bool inCheck = ChessRules.instance.IsInCheck(boardState, isWhiteTurn);


        // Cofamy ruch
        movedPiece.SetPosition(from, tempBoard);
        tempBoard[to.x, to.y] = capturedPiece;

        return !inCheck;
    }

    public void zmianaTury()
    {
        isWhiteTurn = !isWhiteTurn;
        string gameState = ChessRules.instance.EvaluateGameState();

        //if (!ChessRules.instance.HasLegalMoves(isWhiteTurn) && ChessRules.instance.IsInCheck(boardState,isWhiteTurn))
        if (gameState == "Mat")
        {
            // Znajdź i usuń króla z planszy
            ChessPiece king = this.gameObject.GetComponent<ChessRules>().FindKing(!isWhiteTurn);
            if (king != null)
            {
                Destroy(king.gameObject);
            }

            // Wyświetl komunikat o wygranej
            if (isWhiteTurn)
            {
                Debug.Log("Wygrały białe");

            }
            else
            {
                Debug.Log("Wygrały czarne");
            }
            DestroyKing();
            return; // Przerywamy dalszą zmianę tury, bo gra się kończy
        }
        else if (gameState == "Szach")
        {
            _Camera.GetComponent<CameraSettings>().RotateAroundBoard();
            
            ChessBoard.instance.selecting = true;
            isSzach = true;
        }
        else
        {
            _Camera.GetComponent<CameraSettings>().RotateAroundBoard();
            
            ChessBoard.instance.selecting = true;
            isSzach = false;
        }
    }

    public virtual bool Promocja()
    {
        if (selectedPieceForPromotion.gameObject.CompareTag("Pawn"))
        {
            if (isWhiteTurn && ChessBoard.instance.ClickedPlane.y == 7 || !isWhiteTurn && ChessBoard.instance.ClickedPlane.y == 0)
            {
                //Debug.Log("ruch Promocyjny");
                return true;
            }
            else
            {
                //Debug.Log("ruch Niepromocyjny err01");
                return false;
            }
        }
        else 
        {
            //Debug.Log("ruch Niepromocyjny err02");
            return false;
        }
        
    }
    /*public void changeModel()
    {

        selectedPiece.GetComponent<CollisionScript>().SwichMeshCollider();
        looking = false;
        Quaternion Quat = targetFigure.transform.rotation;
        targetPosition = targetFigure.transform.position;
        Destroy(targetFigure.gameObject);

        destroy = Instantiate(model, targetPosition, Quat);

    }
    */
    public void end()
    {

        looking = false;
        Quaternion Quat = targetFigure.transform.rotation;
        targetPosition = targetFigure.transform.position;
        selectedPiece.GetComponent<CollisionScript>().SwichMeshCollider();
        ChessGameManager.instance.boardState[(int)targetPosition.x, (int)targetPosition.z] = null;
        ChessGameManager.instance.MovePiece(new Vector2Int((int)targetPosition.x, (int)targetPosition.z), true);
        if (Promocja())
        {
            ShowPromoPanel();
        }
    }
    public void ShowPromoPanel()
    {
        PromotionPanel.SetActive(true);

    }
    public void DestroyKing()
    {
        foreach (Transform piece in _bierki)
        {
            if (piece.gameObject.CompareTag("King") && piece.GetComponent<ChessPiece>().isWhite == !isWhiteTurn)
            {
                if (piece.GetComponent<ChessPiece>().isWhite)
                {
                    Instantiate(_KingB, new Vector3(piece.gameObject.transform.position.x, piece.gameObject.transform.position.y, piece.gameObject.transform.position.z), Quaternion.identity, this.transform);
                }
                else
                {
                    Instantiate(_KingB, new Vector3(piece.gameObject.transform.position.x, piece.gameObject.transform.position.y, piece.gameObject.transform.position.z), Quaternion.identity, this.transform);
                }
                Destroy(piece.gameObject);
            }
        }
    }
    
    public void FixedUpdate()
    {
       
        if (TimerOn)
        {
            time += Time.deltaTime;
        }
          
        
        if (isWhiteTurn && showTour)
        {
            turaB.SetActive(false);
            turaW.SetActive(true);
        }
        else if (!isWhiteTurn && showTour)
        {
            turaB.SetActive(true);
            turaW.SetActive(false);
        }
        else if(!showTour){
            if (turaB.activeSelf == true || turaW.activeSelf == true)
            {
                turaB.SetActive(false);
                turaW.SetActive(false);
            }
        }

        if (looking)
        {
            selectedPiece.transform.LookAt(new Vector3(ChessBoard.instance.ClickedPlane.x, selectedPiece.transform.position.y, ChessBoard.instance.ClickedPlane.y));
        }
        
        /*
        
        if (atackIsRunning)
        {
            var timeRemaining = 0f;
            //dla białych
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

            atackIsRunning = false;


        }
        */
        if (Input.GetKeyDown(KeyCode.K))
        {
            string output = "\n"; // Nowa linia dla czytelności

            for (int j = boardState.GetLength(0) - 1; j >= 0; j--) // Iteracja po wierszach
            {
                for (int i = 0; i < boardState.GetLength(1); i++) // Iteracja po kolumnach
                {
                    if (boardState[i, j] == null)
                    {
                        output += "[null]\t"; // Jeśli brak obiektu
                    }
                    else
                    {
                        output += "[" + boardState[i, j].gameObject.tag + "]\t"; // Jeśli istnieje, wypisuje jego tag
                    }
                }
                output += "\n"; // Nowa linia po każdym wierszu
            }

            Debug.Log(output); // Wypisuje sformatowaną tablicę do konsoli
        }


        if (Input.GetKeyDown(KeyCode.P))
        {
            string output = "\n"; // Nowa linia dla czytelności

            for (int j = selectedPiece.CancelingSzachMoves.GetLength(0) - 1; j >= 0; j--) // Iteracja po wierszach
            {
                for (int i = 0; i < selectedPiece.CancelingSzachMoves.GetLength(1); i++) // Iteracja po kolumnach
                {
                    if (selectedPiece.CancelingSzachMoves[i, j] == false)
                    {
                        output += "[false]\t"; // Jeśli brak obiektu
                    }
                    else
                    {
                        output += "[" + selectedPiece.CancelingSzachMoves[i, j] + "]\t"; // Jeśli istnieje, wypisuje jego tag
                    }
                }
                output += "\n"; // Nowa linia po każdym wierszu
            }

            Debug.Log(output); // Wypisuje sformatowaną tablicę do konsoli
        }
  
    }
}
