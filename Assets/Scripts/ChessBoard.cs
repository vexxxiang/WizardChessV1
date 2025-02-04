using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    public GameObject squarePrefab; 
    public float squareSize = 1f;
    public GameObject GM;
    List<GameObject> Plansza = new List<GameObject> { };
    public void CreateBoard()
    { 
        for (int x = 0; x < 8; x++)
        {
            for (int z = 0; z < 8; z++) // Zmieniamy y na z (poziom planszy)
            {
                var Pole = Instantiate(squarePrefab, new Vector3(x, -0.05f, z), Quaternion.identity);
                Plansza.Add(Pole);
                Pole.transform.SetParent(this.transform);
                Pole.name = new string(x + " " + z);
                Pole.GetComponent<Cube>().Position = new Vector2Int (x, z);
                Pole.GetComponent<Cube>().WhiteColor = (x + z) % 2 == 0 ? true : false;
            }
        }
        foreach(GameObject i in Plansza)
        {
            i.GetComponent<Cube>().Refresh();
        }
    }
    public void odznacz()
    {
        Debug.Log("odznaczam" + Plansza);
        foreach(GameObject i in Plansza)
        {
            i.GetComponent<Cube>().Selected = false;
            i.GetComponent<Cube>().Refresh();
        }
    }  public void _UpdateState()
    {
        foreach(GameObject i in Plansza)
        {
            i.GetComponent<Cube>().UpdateState();
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
                var clickedPosition = GM.GetComponent<ChessSetup>().boardState[((int)ClickedObject.GetComponent<Cube>().Position.x), (int)ClickedObject.GetComponent<Cube>().Position.y];
                if (ClickedObject != null ) 
                {
                    if (ClickedObject.CompareTag("Plansza") && ClickedObject.gameObject.GetComponent<Cube>().Zajety == true) // Klikniêcie w bierkê
                    {
                        Debug.Log(clickedPosition.GetComponent<ChessPiece>().isWhite + "clickedPosition.GetComponent<ChessPiece>().isWhite");
                        Debug.Log(GM.GetComponent<ChessGameManager>().isWhiteTurn + "(GM.GetComponent<ChessGameManager>().isWhiteTurn");
                        if ((GM.GetComponent<ChessGameManager>().isWhiteTurn && clickedPosition.GetComponent<ChessPiece>().isWhite) || (!GM.GetComponent<ChessGameManager>().isWhiteTurn && !clickedPosition.gameObject.GetComponent<ChessPiece>().isWhite))
                        {

                            if (GM.GetComponent<ChessGameManager>().selectedPiece == null) // Jeœli ¿adna bierka nie jest wybrana
                            {
                                GM.GetComponent<ChessGameManager>().SelectPiece(clickedPosition, clickedPosition.boardPosition); // Wybór bierki
                                ClickedObject.GetComponent<Cube>().Selected = true;
                                ClickedObject.GetComponent<Cube>().Refresh();
                                Debug.Log("Wybrano bierkê: " + GM.GetComponent<ChessGameManager>().selectedPiece);
                            }
                            else if (GM.GetComponent<ChessGameManager>().selectedPiece == clickedPosition ) // Klikniêcie tej samej bierki (odznaczenie)
                            {
                                Debug.Log("Odznaczono bierkê: " + GM.GetComponent<ChessGameManager>().selectedPiece);
                                ClickedObject.GetComponent<Cube>().Selected = false;
                                ClickedObject.GetComponent<Cube>().Refresh();
                                GM.GetComponent<ChessGameManager>().selectedPiece = null;
                            }
                            else if (GM.GetComponent<ChessGameManager>().selectedPiece != null )// Klikniêcie innej bierki (zmiana wyboru)
                            {
                                Debug.Log("Zmieniono wybór na: " + clickedPosition);
                                odznacz();
                                ClickedObject.GetComponent<Cube>().Selected = true;
                                ClickedObject.GetComponent<Cube>().Refresh();
                                GM.GetComponent<ChessGameManager>().SelectPiece(clickedPosition, clickedPosition.boardPosition);
                            }
                        }
                       

                    }
                    if (ClickedObject.CompareTag("Plansza") && ClickedObject.gameObject.GetComponent<Cube>().Zajety == false) // Klikniêcie na puste pole
                    {
                        if (GM.GetComponent<ChessGameManager>().selectedPiece != null ) // Jeœli jest wybrana bierka, próbujemy ni¹ ruszyæ
                        {
                            GM.GetComponent<ChessGameManager>().MovePiece(ClickedObject.GetComponent<Cube>().Position);
                            Debug.Log("Próba ruchu bierk¹ " + GM.GetComponent<ChessGameManager>().selectedPiece + " na pole " + clickedPosition);
                            GM.GetComponent<ChessGameManager>().selectedPiece = null;
                            odznacz();
                            ClickedObject.GetComponent<Cube>().Refresh();
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
