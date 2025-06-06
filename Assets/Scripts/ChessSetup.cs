using System.Collections.Generic;
using UnityEngine;

public class ChessSetup : MonoBehaviour
{
    // Referencja do skryptu ChessBoard
    public ChessBoard chessBoard; 

    // Prefaby figur
    public GameObject whitePawnPrefab;
    public GameObject blackPawnPrefab;
    public GameObject whiteRookPrefab;
    public GameObject blackRookPrefab;
    public GameObject whiteKnightPrefab;
    public GameObject blackKnightPrefab;
    public GameObject whiteBishopPrefab;
    public GameObject blackBishopPrefab;
    public GameObject whiteQueenPrefab;
    public GameObject blackQueenPrefab;
    public GameObject whiteKingPrefab;
    public GameObject blackKingPrefab;

    //Obiekt na scenie kt�ry przechowuje wszystkie bierki
    public GameObject _Bierki;

    public static ChessSetup instance;

    void Start()
    {
        instance = this;
        chessBoard = chessBoard.GetComponent<ChessBoard>();
        
    }

    public void SetupPieces()
    {
        // Ustawienie pionk�w
        
        for (int x = 0; x < 8; x++)
        {
            // Bia�e pionki na drugim rz�dzie
            SpawnPiece(whitePawnPrefab, new Vector2Int(x, 1), true);

            // Czarne pionki na si�dmym rz�dzie
            SpawnPiece(blackPawnPrefab, new Vector2Int(x, 6), false);
        }
        
       
        // Ustawienie wie�
        SpawnPiece(whiteRookPrefab, new Vector2Int(0, 0), true);
        SpawnPiece(whiteRookPrefab, new Vector2Int(7, 0), true);
        SpawnPiece(blackRookPrefab, new Vector2Int(0, 7), false);
        SpawnPiece(blackRookPrefab, new Vector2Int(7, 7), false);
        
        // Ustawienie skoczk�w
        SpawnPiece(whiteKnightPrefab, new Vector2Int(1, 0), true);
        SpawnPiece(whiteKnightPrefab, new Vector2Int(6, 0), true);
        SpawnPiece(blackKnightPrefab, new Vector2Int(1, 7), false);
        SpawnPiece(blackKnightPrefab, new Vector2Int(6, 7), false);

        // Ustawienie go�c�w
        SpawnPiece(whiteBishopPrefab, new Vector2Int(2, 0), true);
        SpawnPiece(whiteBishopPrefab, new Vector2Int(5, 0), true);
        SpawnPiece(blackBishopPrefab, new Vector2Int(2, 7), false);
        SpawnPiece(blackBishopPrefab, new Vector2Int(5, 7), false);
        
        // Ustawienie hetman�w
        SpawnPiece(whiteQueenPrefab, new Vector2Int(3, 0), true);
        SpawnPiece(blackQueenPrefab, new Vector2Int(3, 7), false);
        

        // Ustawienie kr�l�w
        SpawnPiece(whiteKingPrefab, new Vector2Int(4, 0), true);
        SpawnPiece(blackKingPrefab, new Vector2Int(4, 7), false);
       
     
        
    }

    public void SpawnPiece(GameObject prefab, Vector2Int position, bool isWhite)
    {
        Vector3 worldPosition = new Vector3(position.x, 0f, position.y);
        if (isWhite == true)
        {
            GameObject pieceObject = Instantiate(prefab, worldPosition, Quaternion.Euler(0, 0, 0), _Bierki.transform);
            ChessPiece piece = pieceObject.GetComponent<ChessPiece>();
            piece.boardPosition = position;
            piece.isWhite = isWhite;
            ChessGameManager.instance.boardState[position.x, position.y] = piece;
        }
        else {
            GameObject pieceObject = Instantiate(prefab, worldPosition, Quaternion.Euler(0, 180, 0), _Bierki.transform);
            ChessPiece piece = pieceObject.GetComponent<ChessPiece>();
            piece.boardPosition = position;
            piece.isWhite = isWhite;
            ChessGameManager.instance.boardState[position.x, position.y] = piece;
        }
    }
}