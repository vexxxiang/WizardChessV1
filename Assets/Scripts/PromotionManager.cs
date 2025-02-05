using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromotionManager : MonoBehaviour
{
    public GameObject _Camera;
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
    public void Aprove(string Bierka) {
        var piece = ChessGameManager.instance.Piece;
        Debug.Log(piece);
        switch (Bierka) {
            case ("Rook"):
                gameObject.SetActive(false);
                if (piece.GetComponent<ChessPiece>().isWhite)
                {
                    ChessSetup.instance.SpawnPiece(ChessSetup.instance.whiteRookPrefab, piece.boardPosition, piece.isWhite);
                }
                else {
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


        Destroy(piece.gameObject);
        _Camera.GetComponent<CameraSettings>().RotateAroundBoard();
    }

}
