using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    public GameObject squarePrefab; 
    public float squareSize = 1f;
    public GameObject GM;
    List<GameObject> Plansza = new List<GameObject> { };
    public Vector2Int PreLastClick;
    public GameObject _Camera;
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
                Pole.GetComponent<Cube>().Position = new Vector2Int (x, z);
                Pole.GetComponent<Cube>().WhiteColor = (x + z) % 2 == 0 ? true : false;
                Pole.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        foreach(GameObject i in Plansza)
        {
            i.GetComponent<Cube>().PreRefresh(0);
        }
    }
    public void odznacz(Vector2Int position)
    {
        //Debug.Log("odznaczam" + Plansza);
        foreach(GameObject i in Plansza)
        {
            if (position == i.GetComponent<Cube>().Position)
            {
                i.GetComponent<Cube>().Selected = false;
                i.GetComponent<Cube>().PreRefresh(0);
            }
        }
    }  public void _UpdateState()
    {
        foreach(GameObject i in Plansza)
        {
            i.GetComponent<Cube>().UpdateState();
        }
    }
    public void illegalMove(Vector2Int position) {
        //Debug.Log("czerwony"+ position);
        foreach (GameObject i in Plansza)
        {
            if (position == i.GetComponent<Cube>().Position)
            {
                i.gameObject.GetComponent<Renderer>().material.color = Color.red;
                i.gameObject.GetComponent<Cube>().PreRefresh(1f);
            }
            
        }
    
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) 
            {
                _UpdateState();
                Renderer ClickedObject = hit.transform.GetComponent<Renderer>();
                
                var clickedPosition = ChessGameManager.instance.boardState[((int)ClickedObject.GetComponent<Cube>().Position.x), (int)ClickedObject.GetComponent<Cube>().Position.y];
                if (ClickedObject != null ) 
                {
                    if (ClickedObject.CompareTag("Plansza") && ClickedObject.gameObject.GetComponent<Cube>().Zajety == true) // Klikniêcie w bierkê
                    {                       
                        
                        //atakowanie enemy
                        if ((ChessGameManager.instance.isWhiteTurn && !clickedPosition.GetComponent<ChessPiece>().isWhite) && 
                            ChessGameManager.instance.selectedPiece.isWhite && ChessGameManager.instance.selectedPiece != null || 
                            (!GM.GetComponent<ChessGameManager>().isWhiteTurn && clickedPosition.gameObject.GetComponent<ChessPiece>().isWhite && 
                            !ChessGameManager.instance.selectedPiece.isWhite && ChessGameManager.instance.selectedPiece != null))
                        {

                            //Debug.Log("Atak");
                            ChessGameManager.instance.selectedPiece = ChessGameManager.instance.boardState[PreLastClick.x, PreLastClick.y];
                            if (ChessGameManager.instance.TestMovePiece(clickedPosition.boardPosition))
                            {
                                ClickedObject.GetComponent<Cube>().Selected = false;
                                ClickedObject.GetComponent<Cube>().PreRefresh(0f);
                                GM.GetComponent<AnimatorManager>().StartAnimation(ChessGameManager.instance.selectedPiece, clickedPosition);

                                ChessGameManager.instance.selectedPiece = null;
                                //ChessGameManager.instance.MovePiece(clickedPosition.boardPosition);
                                //Destroy(clickedPosition.gameObject);
                            }
                            else {
                                //Debug.Log("Bicie Niemozliwe");
                            }
                            odznacz(PreLastClick);
                            
                            ClickedObject.GetComponent<Cube>().Selected = false;
                            ClickedObject.GetComponent<Cube>().PreRefresh(0f);
                            PreLastClick = ClickedObject.GetComponent<Cube>().Position;

                        }


                        else if ((ChessGameManager.instance.isWhiteTurn && clickedPosition.GetComponent<ChessPiece>().isWhite) || 
                            (!GM.GetComponent<ChessGameManager>().isWhiteTurn && !clickedPosition.gameObject.GetComponent<ChessPiece>().isWhite))
                        {

                            //Wybieranie bierki
                            if (ChessGameManager.instance.selectedPiece == null) // Jeœli ¿adna bierka nie jest wybrana
                            {
                                ChessGameManager.instance.SelectPiece(clickedPosition, clickedPosition.boardPosition); // Wybór bierki
                                ClickedObject.GetComponent<Cube>().Selected = true;
                                ClickedObject.GetComponent<Cube>().PreRefresh(0f);
                               // Debug.Log("Wybrano bierkê: " + GM.GetComponent<ChessGameManager>().selectedPiece);
                                PreLastClick = ClickedObject.GetComponent<Cube>().Position;
                            }
                            //Odznaczanie bierki
                            else if (ChessGameManager.instance.selectedPiece == clickedPosition) // Klikniêcie tej samej bierki (odznaczenie)
                            {
                               // Debug.Log("Odznaczono bierkê: " + ChessGameManager.instance.selectedPiece);
                                ClickedObject.GetComponent<Cube>().Selected = false;
                                ClickedObject.GetComponent<Cube>().PreRefresh(0f);
                                ChessGameManager.instance.selectedPiece = null;
                                PreLastClick = ClickedObject.GetComponent<Cube>().Position;
                            }
                            //roszada
                            else if (ChessGameManager.instance.selectedPiece != null)// Klikniêcie innej bierki (zmiana wyboru)
                            {
                                if ((ChessGameManager.instance.isWhiteTurn && clickedPosition.GetComponent<ChessPiece>().isWhite &&
                                ChessGameManager.instance.selectedPiece.gameObject.CompareTag("Rook") && clickedPosition.gameObject.CompareTag("King") &&
                                !ChessGameManager.instance.boardState[1, 0] && !ChessGameManager.instance.boardState[2, 0] && !ChessGameManager.instance.boardState[3, 0] ||

                                ChessGameManager.instance.isWhiteTurn && clickedPosition.GetComponent<ChessPiece>().isWhite &&
                                ChessGameManager.instance.selectedPiece.gameObject.CompareTag("Rook") && clickedPosition.gameObject.CompareTag("King") &&
                                !ChessGameManager.instance.boardState[5, 0] && !ChessGameManager.instance.boardState[6, 0] ||

                                ChessGameManager.instance.isWhiteTurn && clickedPosition.GetComponent<ChessPiece>().isWhite &&
                                ChessGameManager.instance.selectedPiece.gameObject.CompareTag("King") && clickedPosition.gameObject.CompareTag("Rook") &&
                                !ChessGameManager.instance.boardState[1, 0] && !ChessGameManager.instance.boardState[2, 0] && !ChessGameManager.instance.boardState[3, 0] ||

                                ChessGameManager.instance.isWhiteTurn && clickedPosition.GetComponent<ChessPiece>().isWhite &&
                                ChessGameManager.instance.selectedPiece.gameObject.CompareTag("King") && clickedPosition.gameObject.CompareTag("Rook") &&
                                !ChessGameManager.instance.boardState[5, 0] && !ChessGameManager.instance.boardState[6, 0]) ||

                                !ChessGameManager.instance.isWhiteTurn && !clickedPosition.GetComponent<ChessPiece>().isWhite &&
                                ChessGameManager.instance.selectedPiece.gameObject.CompareTag("Rook") && clickedPosition.gameObject.CompareTag("King") &&
                                !ChessGameManager.instance.boardState[1, 7] && !ChessGameManager.instance.boardState[2, 7] && !ChessGameManager.instance.boardState[3, 7] ||

                                !ChessGameManager.instance.isWhiteTurn && !clickedPosition.GetComponent<ChessPiece>().isWhite &&
                                ChessGameManager.instance.selectedPiece.gameObject.CompareTag("Rook") && clickedPosition.gameObject.CompareTag("King") &&
                                !ChessGameManager.instance.boardState[5, 7] && !ChessGameManager.instance.boardState[6, 7] ||

                                !ChessGameManager.instance.isWhiteTurn && !clickedPosition.GetComponent<ChessPiece>().isWhite &&
                                ChessGameManager.instance.selectedPiece.gameObject.CompareTag("King") && clickedPosition.gameObject.CompareTag("Rook")&&
                                !ChessGameManager.instance.boardState[1, 7] && !ChessGameManager.instance.boardState[2, 7] && !ChessGameManager.instance.boardState[3, 7] ||

                                !ChessGameManager.instance.isWhiteTurn && !clickedPosition.GetComponent<ChessPiece>().isWhite &&
                                ChessGameManager.instance.selectedPiece.gameObject.CompareTag("King") && clickedPosition.gameObject.CompareTag("Rook")&&
                                !ChessGameManager.instance.boardState[5, 7] && !ChessGameManager.instance.boardState[6, 7])
                                {

                                    if (!clickedPosition.GetComponent<ChessPiece>().Moved && !ChessGameManager.instance.selectedPiece.GetComponent<ChessPiece>().Moved)
                                    {
                                        //roszada dla bia³ych
                                        if (ChessGameManager.instance.isWhiteTurn)
                                        {


                                            if (clickedPosition.gameObject.CompareTag("Rook"))
                                            {
                                                if (clickedPosition.gameObject.GetComponent<ChessPiece>().boardPosition == new Vector2Int(0, 0)
                                                    && !ChessGameManager.instance.boardState[1, 0] && !ChessGameManager.instance.boardState[2, 0]
                                                    && !ChessGameManager.instance.boardState[3, 0])
                                                {
                                                    odznacz(ChessGameManager.instance.selectedPiece.boardPosition);
                                                    ChessGameManager.instance.CastleMovePiece(new Vector2Int(2, 0));
                                                    ChessGameManager.instance.selectedPiece = clickedPosition.gameObject.GetComponent<ChessPiece>();
                                                    ChessGameManager.instance.CastleMovePiece(new Vector2Int(3,0));
                                                    ChessGameManager.instance.isWhiteTurn = !ChessGameManager.instance.isWhiteTurn;
                                                    _Camera.GetComponent<CameraSettings>().RotateAroundBoard();
                                                    
                                                    //D³uga roszada dla bia³ych
                                                    //Debug.Log("rook Roszada wariacie d");

                                                }
                                                else if (clickedPosition.gameObject.GetComponent<ChessPiece>().boardPosition == new Vector2Int(7, 0)
                                                    && !ChessGameManager.instance.boardState[5, 0] && !ChessGameManager.instance.boardState[6, 0])
                                                {
                                                    odznacz(ChessGameManager.instance.selectedPiece.boardPosition);
                                                    ChessGameManager.instance.CastleMovePiece(new Vector2Int(6, 0));
                                                    ChessGameManager.instance.selectedPiece = clickedPosition.gameObject.GetComponent<ChessPiece>();
                                                    ChessGameManager.instance.CastleMovePiece(new Vector2Int(5, 0));
                                                    ChessGameManager.instance.isWhiteTurn = !ChessGameManager.instance.isWhiteTurn;
                                                    _Camera.GetComponent<CameraSettings>().RotateAroundBoard();

                                                    //krótka roszada dla bia³ych
                                                    //Debug.Log("rook Roszada wariacie k");

                                                }
                                            }
                                            else if (clickedPosition.gameObject.CompareTag("King"))
                                            {
                                                if (ChessGameManager.instance.selectedPiece.GetComponent<ChessPiece>().boardPosition == new Vector2Int(0, 0)
                                                    && !ChessGameManager.instance.boardState[1, 0] && !ChessGameManager.instance.boardState[2, 0]
                                                    && !ChessGameManager.instance.boardState[3, 0])
                                                {
                                                    odznacz(ChessGameManager.instance.selectedPiece.boardPosition);
                                                    ChessGameManager.instance.CastleMovePiece(new Vector2Int(3, 0));
                                                    ChessGameManager.instance.selectedPiece = clickedPosition.gameObject.GetComponent<ChessPiece>();
                                                    ChessGameManager.instance.CastleMovePiece(new Vector2Int(2, 0));
                                                    ChessGameManager.instance.isWhiteTurn = !ChessGameManager.instance.isWhiteTurn;
                                                    _Camera.GetComponent<CameraSettings>().RotateAroundBoard();
                                                    //D³uga roszada dla bia³ych
                                                   // Debug.Log("king Roszada wariacie d");

                                                }
                                                else if (ChessGameManager.instance.selectedPiece.GetComponent<ChessPiece>().boardPosition == new Vector2Int(7, 0)
                                                    && !ChessGameManager.instance.boardState[5, 0] && !ChessGameManager.instance.boardState[6, 0])
                                                {
                                                    odznacz(ChessGameManager.instance.selectedPiece.boardPosition);
                                                    ChessGameManager.instance.CastleMovePiece(new Vector2Int(5, 0));
                                                    ChessGameManager.instance.selectedPiece = clickedPosition.gameObject.GetComponent<ChessPiece>();
                                                    ChessGameManager.instance.CastleMovePiece(new Vector2Int(6, 0));
                                                    ChessGameManager.instance.isWhiteTurn = !ChessGameManager.instance.isWhiteTurn;
                                                    _Camera.GetComponent<CameraSettings>().RotateAroundBoard();
                                                    //krótka roszada dla bia³ych
                                                    //Debug.Log("king Roszada wariacie k");

                                                }

                                            }
                                        }


                                        //Roszada Czarne
                                        if (!ChessGameManager.instance.isWhiteTurn)
                                        {


                                            if (clickedPosition.gameObject.CompareTag("Rook"))
                                            {
                                                if (clickedPosition.gameObject.GetComponent<ChessPiece>().boardPosition == new Vector2Int(0, 7)
                                                    && !ChessGameManager.instance.boardState[1, 7] && !ChessGameManager.instance.boardState[2, 7]
                                                    && !ChessGameManager.instance.boardState[3, 7])
                                                {
                                                    odznacz(ChessGameManager.instance.selectedPiece.boardPosition);
                                                    ChessGameManager.instance.CastleMovePiece(new Vector2Int(2, 7));
                                                    ChessGameManager.instance.selectedPiece = clickedPosition.gameObject.GetComponent<ChessPiece>();
                                                    ChessGameManager.instance.CastleMovePiece(new Vector2Int(3, 7));
                                                    ChessGameManager.instance.isWhiteTurn = !ChessGameManager.instance.isWhiteTurn;
                                                    _Camera.GetComponent<CameraSettings>().RotateAroundBoard();
                                                    //D³uga roszada dla bia³ych
                                                    //Debug.Log("rook Roszada wariacie d");

                                                }
                                                else if (clickedPosition.gameObject.GetComponent<ChessPiece>().boardPosition == new Vector2Int(7, 7)
                                                    && !ChessGameManager.instance.boardState[5, 7] && !ChessGameManager.instance.boardState[6, 7])
                                                {
                                                    odznacz(ChessGameManager.instance.selectedPiece.boardPosition);
                                                    ChessGameManager.instance.CastleMovePiece(new Vector2Int(6, 7));
                                                    ChessGameManager.instance.selectedPiece = clickedPosition.gameObject.GetComponent<ChessPiece>();
                                                    ChessGameManager.instance.CastleMovePiece(new Vector2Int(5, 7));
                                                    ChessGameManager.instance.isWhiteTurn = !ChessGameManager.instance.isWhiteTurn;
                                                    _Camera.GetComponent<CameraSettings>().RotateAroundBoard();
                                                    //krótka roszada dla bia³ych
                                                    //Debug.Log("rook Roszada wariacie k");

                                                }
                                            }
                                            else if (clickedPosition.gameObject.CompareTag("King"))
                                            {
                                                if (ChessGameManager.instance.selectedPiece.GetComponent<ChessPiece>().boardPosition == new Vector2Int(0, 7)
                                                    && !ChessGameManager.instance.boardState[1, 7] && !ChessGameManager.instance.boardState[2, 7]
                                                    && !ChessGameManager.instance.boardState[3, 7])
                                                {
                                                    odznacz(ChessGameManager.instance.selectedPiece.boardPosition);
                                                    ChessGameManager.instance.CastleMovePiece(new Vector2Int(3, 7));
                                                    ChessGameManager.instance.selectedPiece = clickedPosition.gameObject.GetComponent<ChessPiece>();
                                                    ChessGameManager.instance.CastleMovePiece(new Vector2Int(2, 7));
                                                    ChessGameManager.instance.isWhiteTurn = !ChessGameManager.instance.isWhiteTurn;
                                                    _Camera.GetComponent<CameraSettings>().RotateAroundBoard();
                                                    //D³uga roszada dla bia³ych
                                                    //Debug.Log("king Roszada wariacie d");

                                                }
                                                else if (ChessGameManager.instance.selectedPiece.GetComponent<ChessPiece>().boardPosition == new Vector2Int(7, 7)
                                                    && !ChessGameManager.instance.boardState[5, 7] && !ChessGameManager.instance.boardState[6, 7])
                                                {
                                                    odznacz(ChessGameManager.instance.selectedPiece.boardPosition);
                                                    ChessGameManager.instance.CastleMovePiece(new Vector2Int(5, 7));
                                                    ChessGameManager.instance.selectedPiece = clickedPosition.gameObject.GetComponent<ChessPiece>();
                                                    ChessGameManager.instance.CastleMovePiece(new Vector2Int(6, 7));
                                                    ChessGameManager.instance.isWhiteTurn = !ChessGameManager.instance.isWhiteTurn;
                                                    _Camera.GetComponent<CameraSettings>().RotateAroundBoard();
                                                    //krótka roszada dla bia³ych
                                                    //Debug.Log("king Roszada wariacie k");

                                                }

                                            }
                                        }




                                    }


                                }
                                //Zmiana bierki na inn¹
                                else
                                {
                                    //Debug.Log("Zmieniono wybór na: " + clickedPosition);
                                    odznacz(PreLastClick);
                                    ClickedObject.GetComponent<Cube>().Selected = true;
                                    ClickedObject.GetComponent<Cube>().PreRefresh(0f);
                                    ChessGameManager.instance.SelectPiece(clickedPosition, clickedPosition.boardPosition);
                                    PreLastClick = ClickedObject.GetComponent<Cube>().Position;
                                }

                                
                            }


                        }
                       

                    }
                    //Ruch wybran¹ bierk¹
                    else if (ClickedObject.CompareTag("Plansza") && ClickedObject.gameObject.GetComponent<Cube>().Zajety == false) // Klikniêcie na puste pole
                    {
                        if (ChessGameManager.instance.selectedPiece != null ) // Jeœli jest wybrana bierka, próbujemy ni¹ ruszyæ
                        {
                            ChessGameManager.instance.MovePiece(ClickedObject.GetComponent<Cube>().Position);
                            //Debug.Log("Próba ruchu bierk¹ " + ChessGameManager.instance.selectedPiece + " na pole " + clickedPosition);
                            ChessGameManager.instance.selectedPiece = null;
                            
                            odznacz(PreLastClick);
                            
                            PreLastClick = ClickedObject.GetComponent<Cube>().Position;
                        }
                        else
                        {
                            Debug.Log("Klikniêto puste pole, ale ¿adna bierka nie jest wybrana.");
                        }
                    }
                    else // Klikniêcie w coœ innego
                    {
                        Debug.Log("Klikniêto niezidentyfikowany obiekt: " + clickedPosition);
                    }
                }
            }
        }
    }
}
