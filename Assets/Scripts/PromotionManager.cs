using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromotionManager : MonoBehaviour
{
    public string _Bierka;
    public GameObject _Camera;
    public static PromotionManager instance;
    public GameObject[] Heads;


    public void HeadColorUpdate()
    {
        foreach (GameObject head in Heads)
        {
            head.GetComponent<PromoPanelHeadLooking>().UpdateColor();
        }
    }

    public void Start()
    {
        instance = this;
    }
    public void Rook() {
        Aprove("Rook");
    }
    public void Bishop()
    {
        Aprove("Bishop");
    }
    public void Queen()
    {
        Aprove("Queen");
    }
    public void Knight()
    {
        Aprove("Knight");
    }
    public void Aprove(string Bierka) 
    {
        _Bierka = Bierka;

        var piece = ChessGameManager.instance.selectedPieceForPromotion;
        //Debug.Log("Bierka kt�ra podlega promocji -->> " + piece + " i zamienia sie na _Bierka");
        switch (_Bierka)
        {
            case ("Rook"):
                gameObject.SetActive(false);
                if (piece.GetComponent<ChessPiece>().isWhite)
                {
                    ChessSetup.instance.SpawnPiece(ChessSetup.instance.whiteRookPrefab, piece.boardPosition, piece.isWhite);
                }
                else
                {
                    ChessSetup.instance.SpawnPiece(ChessSetup.instance.blackRookPrefab, piece.boardPosition, piece.isWhite);
                }


                break;
            case ("Bishop"):
                gameObject.SetActive(false);
                if (piece.GetComponent<ChessPiece>().isWhite)
                {
                    ChessSetup.instance.SpawnPiece(ChessSetup.instance.whiteBishopPrefab, piece.boardPosition, piece.isWhite);
                }
                else
                {
                    ChessSetup.instance.SpawnPiece(ChessSetup.instance.blackBishopPrefab, piece.boardPosition, piece.isWhite);
                }

                break;
            case ("Queen"):
                gameObject.SetActive(false);
                if (piece.GetComponent<ChessPiece>().isWhite)
                {
                    ChessSetup.instance.SpawnPiece(ChessSetup.instance.whiteQueenPrefab, piece.boardPosition, piece.isWhite);
                }
                else
                {
                    ChessSetup.instance.SpawnPiece(ChessSetup.instance.blackQueenPrefab, piece.boardPosition, piece.isWhite);
                }

                break;
            case ("Knight"):
                gameObject.SetActive(false);
                if (piece.GetComponent<ChessPiece>().isWhite)
                {
                    ChessSetup.instance.SpawnPiece(ChessSetup.instance.whiteKnightPrefab, piece.boardPosition, piece.isWhite);
                }
                else
                {
                    ChessSetup.instance.SpawnPiece(ChessSetup.instance.blackKnightPrefab, piece.boardPosition, piece.isWhite);
                }

                break;




        }
        ChessGameManager.instance.zmianaTury();
        ChessBoard.instance.selecting = true;
        
        Destroy(piece.gameObject);

    }

        


}


