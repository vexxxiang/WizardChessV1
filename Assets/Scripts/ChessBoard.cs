using System.Collections.Generic;
using UnityEngine;
using System;

public class ChessBoard : MonoBehaviour
{
    public GameObject squarePrefab;
    public float squareSize = 1f;
    public ChessGameManager GM;
    List<GameObject> Plansza = new List<GameObject> { };
    public Vector2Int PreLastClick;
    public GameObject _Camera, promotionPanel;
    public static ChessBoard instance;
    public bool selecting = true;
    public Vector2Int ClickedPlane;
    public ChessPiece ClickedFigure;
   

    public void Start()
    {
        instance = this;
        GM = ChessGameManager.instance;
    }
    public void CreateBoard()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int z = 0; z < 8; z++) // Zmieniamy y na z (poziom planszy)
            {
                var Pole = Instantiate(squarePrefab, new Vector3(x, 0f, z), Quaternion.identity);
                Plansza.Add(Pole);
                Pole.transform.SetParent(this.transform);
                Pole.name = new string(x + " " + z);
                Pole.GetComponent<Cube>().Position = new Vector2Int(x, z);
                Pole.GetComponent<Cube>().WhiteColor = (x + z) % 2 == 0 ? true : false;
                Pole.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        foreach (GameObject i in Plansza)
        {
            i.GetComponent<Cube>().PreRefresh(0);
        }
    }
    public void zaznacz(Vector2Int position)
    {
        //Debug.Log("Zaznaczam pole: " + position);

        foreach (GameObject i in Plansza)
        {
            if (position == i.GetComponent<Cube>().Position)
            {
                i.gameObject.GetComponent<MeshRenderer>().enabled = true;
                i.gameObject.GetComponent<Cube>().Selected = true;
                i.gameObject.GetComponent<Cube>().PreRefresh(0);
            }
        }
    }
    public void odznacz()
    {
        //Debug.Log("Odznaczam pole: " + position);

        foreach (GameObject i in Plansza)
        {
            if (i.GetComponent<Cube>().Selected == true)
            {
                i.GetComponent<Cube>().Selected = false;
                i.GetComponent<Cube>().PreRefresh(0);
            }
        }
    }
    public void empty(Vector2Int position)
    {
        //Debug.Log("Klikni�to puste pole: " + position);

        foreach (GameObject i in Plansza)
        {
            if (position == i.GetComponent<Cube>().Position)
            {
                i.gameObject.GetComponent<MeshRenderer>().enabled = true;
                i.gameObject.GetComponent<Renderer>().material.color = Color.gray;
                i.gameObject.GetComponent<Cube>().PreRefresh(1f);
            }

        }
    }
    public void illegalMove(Vector2Int position)
    {
        //Debug.Log("Nielegalny ruch dla tej bierki: " + position);

        foreach (GameObject i in Plansza)
        {
            if (position == i.GetComponent<Cube>().Position)
            {
                i.gameObject.GetComponent<MeshRenderer>().enabled = true;
                i.gameObject.GetComponent<Renderer>().material.color = Color.red;
                i.gameObject.GetComponent<Cube>().PreRefresh(1f);
            }

        }

    }

    void Update()
    {


        if (Input.GetMouseButtonDown(0) && selecting)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                if (hit.collider.gameObject.CompareTag("Plansza") && selecting)
                {
                    Debug.Log("hit");
                    ClickedPlane = hit.collider.gameObject.GetComponent<Cube>().Position; // <--- klikniete pole

                    
                    //klikni�cie pustego pola nie maj�c wybranej bierki
                    if (GM.boardState[ClickedPlane.x, ClickedPlane.y] == null && GM.selectedPiece == null)
                    {
                        empty(ClickedPlane);
                    }
                    else if (selecting && GM.boardState[ClickedPlane.x, ClickedPlane.y] == null && GM.selectedPiece != null)
                    {
                        GM.selectedPiece.GetLegalMoves(GM.boardState);

                        // En Passant logic
                        if (GM.selectedPiece is Pawn)
                        {
                            int dx = ClickedPlane.x - GM.selectedPiece.boardPosition.x;
                            int dy = ClickedPlane.y - GM.selectedPiece.boardPosition.y;

                            // Check if it's a diagonal move into an empty square (en passant)
                            if (Math.Abs(dx) == 1 && dy == (GM.selectedPiece.isWhite ? 1 : -1))
                            {
                                int capturedPawnX = ClickedPlane.x;
                                int capturedPawnY = ClickedPlane.y + (GM.selectedPiece.isWhite ? -1 : 1);

                                ChessPiece capturedPawn = GM.boardState[capturedPawnX, capturedPawnY];
                                if (capturedPawn != null && capturedPawn is Pawn && capturedPawn.isWhite != GM.selectedPiece.isWhite && capturedPawn.lastmoved)
                                {
                                    // Kill the pawn
                                    Destroy( capturedPawn.gameObject );
                                    GM.boardState[capturedPawnX, capturedPawnY] = null;
                                }
                            }
                        }
                        odznacz();
                        GM.MovePiece(ClickedPlane, false);

                    }
                    else if (GM.boardState[ClickedPlane.x, ClickedPlane.y] != null && GM.selectedPiece != null)
                    {
                        if (GM.boardState[ClickedPlane.x, ClickedPlane.y].isWhite == GM.isWhiteTurn)
                        {
                            //roszada
                            if (GM.boardState[ClickedPlane.x, ClickedPlane.y].gameObject.CompareTag("King") && GM.selectedPiece.gameObject.CompareTag("Rook") && !GM.boardState[ClickedPlane.x, ClickedPlane.y].Moved && !GM.selectedPiece.Moved ||
                                GM.boardState[ClickedPlane.x, ClickedPlane.y].gameObject.CompareTag("Rook") && GM.selectedPiece.gameObject.CompareTag("King") && !GM.boardState[ClickedPlane.x, ClickedPlane.y].Moved && !GM.selectedPiece.Moved)
                            {

                                if (GM.selectedPiece.CompareTag("Rook"))
                                {
                                    var RookPos = GM.selectedPiece.boardPosition;

                                    GM.Roszada(RookPos);
                                    

                                }
                                else if (GM.selectedPiece.CompareTag("King"))
                                {

                                    var RookPos = GM.boardState[ClickedPlane.x, ClickedPlane.y].boardPosition;
                                    GM.Roszada(RookPos);
                                    

                                }
                                else
                                {
                                    Debug.Log("Nieczekiwany b��d 002");
                                }
                            }
                            //odznaczanie bierki
                            else if (GM.selectedPiece.boardPosition == ClickedPlane && GM.selectedPiece != null)
                            {
                                GM.UnSelectPiece();
                                odznacz();
                            }
                            // je�eli mamy zaznaczon� bierke i klikniemy inn� tego samego koloru to podmieniamy wybran� bierke
                            else if (GM.boardState[ClickedPlane.x, ClickedPlane.y].isWhite == GM.isWhiteTurn)
                            {
                                

                                odznacz();
                                GM.UnSelectPiece();
                                zaznacz(ClickedPlane);
                                GM.SelectPiece(GM.boardState[ClickedPlane.x, ClickedPlane.y], ClickedPlane);
                            }

                        }
                        //atakowanie
                        else
                        {
                            ChessRules.instance.EvaluateGameState();
                            GM.selectedPiece.GetComponent<ChessPiece>().GetLegalMoves(GM.boardState);
                            bool[,] availableMoves = GM.selectedPiece.CancelingSzachMoves;
                            if (availableMoves[ClickedPlane.x, ClickedPlane.y] && ChessRules.instance.EvaluateGameState() == "Nothing" || GM.selectedPiece.CancelingSzachMoves[ClickedPlane.x,ClickedPlane.y] == true && ChessRules.instance.EvaluateGameState() == "szach")
                            {
                                odznacz();
                                Debug.Log(GM.selectedPiece + " Atakuje: " + GM.boardState[ClickedPlane.x, ClickedPlane.y].GetComponent<ChessPiece>() + "Na pozycji:" + ClickedPlane);
                                GM.AtackPiece(ClickedPlane);
                            }
                            else
                            {
                                Debug.Log("Bicie niemozliwe");
                                illegalMove(ClickedPlane);
                            }
                        }
                    }
                    else if (GM.boardState[ClickedPlane.x, ClickedPlane.y] != null && GM.selectedPiece == null)
                    {
                        if (GM.boardState[ClickedPlane.x, ClickedPlane.y].isWhite == GM.isWhiteTurn)
                        {
                           //wybieranie figury
                            if (GM.selectedPiece == null)

                            {
                                //jezeli cos jest na polu to wybieramy t� figure za klikni�t�
                                GM.SelectPiece(GM.boardState[ClickedPlane.x, ClickedPlane.y].GetComponent<ChessPiece>(), ClickedPlane);
                                zaznacz(ClickedPlane);

                            }
                        }
                        else
                        {
                            Debug.Log("Klikasz bierke, ale to nie jej tura");
                        }
                    }



                }

            }



        }
    }
}



