using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    public Vector2Int boardPosition;
    public bool isWhite;
    public bool Moved = false;
    public bool King = false;
    public bool[,] CancelingSzachMoves = new bool[8, 8];
    public bool lastmoved = false;

    public abstract AudioClip[] Sounds { get; set; }
    public AudioSource speakingAudioSource;

    public void Start()
    {
        if (gameObject.CompareTag("King"))
        {
            King = true;
        }
    }

    public void PlaySound()
    {
        
        var number = Random.Range(0, 4);
        if (speakingAudioSource == null)
        {
            speakingAudioSource = SoundsSetings.instance.speakingAudioSource;
        }
        if(speakingAudioSource != null)
        {
            speakingAudioSource.PlayOneShot(Sounds[number]);
        }


    }
    public bool[,] GetLegalMoves(ChessPiece[,] boardState)
    {
        bool[,] possibleMoves = GetAvailableMoves(boardState);
        bool[,] legalMoves = new bool[8, 8];

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                CancelingSzachMoves[x, y] = false;
                if (possibleMoves[x, y])
                {
                    // Tworzymy symulacj� planszy
                    ChessPiece[,] simulation = ChessRules.instance.DeepCopyBoard(boardState);
                    ChessPiece movedPiece = simulation[boardPosition.x, boardPosition.y];

                    // Wykonujemy ruch w symulacji
                    simulation[boardPosition.x, boardPosition.y] = null;
                    movedPiece.boardPosition = new Vector2Int(x, y);
                    simulation[x, y] = movedPiece;

                    // Sprawdzamy, czy po ruchu kr�l jest w szachu
                    if (!ChessRules.instance.IsInCheck(simulation, isWhite))
                    {
                        legalMoves[x, y] = true;
                        // tylko podgladowo     \/
                        CancelingSzachMoves[x, y] = true;
                    }
                }
            }
        }

        return legalMoves;
    }
    public virtual bool[,] GetAvailableMoves(ChessPiece[,] boardState)
    {
        bool[,] moves = new bool[8, 8];
        return moves;
    }

    public virtual void SetPosition(Vector2Int newPosition, ChessPiece[,] boardState)
    {
        // Reset lastmoved on all other pieces
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece piece = boardState[x, y];
                if (piece != null && piece != this)
                {
                    piece.lastmoved = false;
                }
            }
        }

        // Special case: if this is a pawn and it's moving two squares forward
        if (this is Pawn)
        {
            int direction = isWhite ? 1 : -1;
            if (Mathf.Abs(newPosition.y - boardPosition.y) == 2 && newPosition.x == boardPosition.x)
            {
                lastmoved = true;
            }
        }
        // Usu� figur� ze starego miejsca
        boardState[boardPosition.x, boardPosition.y] = null;

        // Aktualizuj pozycj� i plansz�
        boardPosition = newPosition;
        boardState[newPosition.x, newPosition.y] = this;

        // Aktualizacja fizycznej pozycji w �wiecie gry
        transform.position = new Vector3(newPosition.x, 0, newPosition.y);
    }

    protected bool IsInsideBoard(int x, int y)
    {
        return x >= 0 && x < 8 && y >= 0 && y < 8;
    }

    protected bool CanMoveOrCapture(ChessPiece[,] boardState, int x, int y)
    {
        if (!IsInsideBoard(x, y)) return false;
        if (boardState[x, y] == null) return true;
        return boardState[x, y].isWhite != isWhite;
    }
}
