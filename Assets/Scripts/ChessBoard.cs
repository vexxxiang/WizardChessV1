using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    public GameObject squarePrefab; 
    public float squareSize = 1f;
    public GameObject GM;
    List<GameObject> Plansza = new List<GameObject> { };
    public Vector2Int PreLastClick;
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
                    if (ClickedObject.CompareTag("Plansza") && ClickedObject.gameObject.GetComponent<Cube>().Zajety == true) // Klikni�cie w bierk�
                    {
                        if ((ChessGameManager.instance.isWhiteTurn && clickedPosition.GetComponent<ChessPiece>().isWhite) || (!GM.GetComponent<ChessGameManager>().isWhiteTurn && !clickedPosition.gameObject.GetComponent<ChessPiece>().isWhite))
                        {
                            if (ChessGameManager.instance.selectedPiece == null) // Je�li �adna bierka nie jest wybrana
                            {
                                ChessGameManager.instance.SelectPiece(clickedPosition, clickedPosition.boardPosition); // Wyb�r bierki
                                ClickedObject.GetComponent<Cube>().Selected = true;
                                ClickedObject.GetComponent<Cube>().PreRefresh(0f);
                                Debug.Log("Wybrano bierk�: " + GM.GetComponent<ChessGameManager>().selectedPiece);
                                PreLastClick = ClickedObject.GetComponent<Cube>().Position ;
                            }
                            else if (ChessGameManager.instance.selectedPiece == clickedPosition) // Klikni�cie tej samej bierki (odznaczenie)
                            {
                                Debug.Log("Odznaczono bierk�: " + ChessGameManager.instance.selectedPiece);
                                ClickedObject.GetComponent<Cube>().Selected = false;
                                ClickedObject.GetComponent<Cube>().PreRefresh(0f);
                                ChessGameManager.instance.selectedPiece = null;
                                PreLastClick = ClickedObject.GetComponent<Cube>().Position;
                            }
                            else if (ChessGameManager.instance.selectedPiece != null )// Klikni�cie innej bierki (zmiana wyboru)
                            {
                                Debug.Log("Zmieniono wyb�r na: " + clickedPosition);
                                odznacz(PreLastClick);
                                ClickedObject.GetComponent<Cube>().Selected = true;
                                ClickedObject.GetComponent<Cube>().PreRefresh(0f);
                                ChessGameManager.instance.SelectPiece(clickedPosition, clickedPosition.boardPosition);
                                PreLastClick = ClickedObject.GetComponent<Cube>().Position;
                            }


                        }

                    }
                    if (ClickedObject.CompareTag("Plansza") && ClickedObject.gameObject.GetComponent<Cube>().Zajety == false) // Klikni�cie na puste pole
                    {
                        if (ChessGameManager.instance.selectedPiece != null ) // Je�li jest wybrana bierka, pr�bujemy ni� ruszy�
                        {
                            ChessGameManager.instance.MovePiece(ClickedObject.GetComponent<Cube>().Position);
                            Debug.Log("Pr�ba ruchu bierk� " + ChessGameManager.instance.selectedPiece + " na pole " + clickedPosition);
                            ChessGameManager.instance.selectedPiece = null;
                            
                            odznacz(PreLastClick);
                            
                            PreLastClick = ClickedObject.GetComponent<Cube>().Position;
                        }
                        else
                        {
                            Debug.Log("Klikni�to puste pole, ale �adna bierka nie jest wybrana.");
                        }
                    }
                    else // Klikni�cie w co� innego
                    {
                        Debug.Log("Klikni�to niezidentyfikowany obiekt: " + clickedPosition);
                    }
                }
            }
        }
    }
}
