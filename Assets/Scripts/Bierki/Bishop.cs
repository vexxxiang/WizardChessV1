using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{
    private Animator animator;
    private bool isMoving = false; // Flaga, kt�ra sprawdza, czy animacja jest ju� w trakcie

    // Inicjalizacja Animatora
    void Start()
    {
        animator = GetComponent<Animator>();  // Pobieramy Animator z tego samego obiektu
    }

    public override bool[,] GetAvailableMoves(ChessPiece[,] boardState)
    {
        bool[,] moves = new bool[8, 8];

        // Cztery kierunki po przek�tnej
        CheckLine(boardState, moves, 1, 1);   // Prawo-g�ra
        CheckLine(boardState, moves, 1, -1);  // Prawo-d�
        CheckLine(boardState, moves, -1, 1);  // Lewo-g�ra
        CheckLine(boardState, moves, -1, -1); // Lewo-d�

        return moves;
    }

    private void CheckLine(ChessPiece[,] boardState, bool[,] moves, int xDirection, int yDirection)
    {
        int x = boardPosition.x;
        int y = boardPosition.y;

        while (IsInsideBoard(x + xDirection, y + yDirection))
        {
            x += xDirection;
            y += yDirection;

            if (boardState[x, y] == null)
            {
                moves[x, y] = true;
            }
            else
            {
                if (boardState[x, y].isWhite != isWhite)
                {
                    moves[x, y] = true;
                }
                break;
            }
        }
    }

    // Nowa metoda, kt�ra wykonuje ruch i uruchamia animacj�
    public void MoveToPosition(Vector3 newPosition)
    {
        if (!isMoving) // Sprawdzamy, czy animacja ju� jest w trakcie
        {
            // Przesu� figur� na now� pozycj�
            transform.position = newPosition;

            // Uruchomienie animacji po wykonaniu ruchu
            if (animator != null)
            {
                animator.SetTrigger("MoveAnimation");  // Za��my, �e masz trigger "MoveAnimation"
            }

            isMoving = true; // Ustawiamy flag�, �eby nie wywo�ywa� animacji ponownie, dop�ki nie zako�czy si� obecna animacja
        }
    }

    // Metoda, kt�ra b�dzie wywo�ywana po zako�czeniu animacji
    public void OnAnimationEnd()
    {
        isMoving = false; // Resetujemy flag�, �eby animacja mog�a si� uruchomi� przy nast�pnym ruchu
    }
}
