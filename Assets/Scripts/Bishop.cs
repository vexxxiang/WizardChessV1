using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{
    private Animator animator;
    private bool isMoving = false; // Flaga, która sprawdza, czy animacja jest ju¿ w trakcie

    // Inicjalizacja Animatora
    void Start()
    {
        animator = GetComponent<Animator>();  // Pobieramy Animator z tego samego obiektu
    }

    public override bool[,] GetAvailableMoves(ChessPiece[,] boardState)
    {
        bool[,] moves = new bool[8, 8];

        // Cztery kierunki po przek¹tnej
        CheckLine(boardState, moves, 1, 1);   // Prawo-góra
        CheckLine(boardState, moves, 1, -1);  // Prawo-dó³
        CheckLine(boardState, moves, -1, 1);  // Lewo-góra
        CheckLine(boardState, moves, -1, -1); // Lewo-dó³

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

    // Nowa metoda, która wykonuje ruch i uruchamia animacjê
    public void MoveToPosition(Vector3 newPosition)
    {
        if (!isMoving) // Sprawdzamy, czy animacja ju¿ jest w trakcie
        {
            // Przesuñ figurê na now¹ pozycjê
            transform.position = newPosition;

            // Uruchomienie animacji po wykonaniu ruchu
            if (animator != null)
            {
                animator.SetTrigger("MoveAnimation");  // Za³ó¿my, ¿e masz trigger "MoveAnimation"
            }

            isMoving = true; // Ustawiamy flagê, ¿eby nie wywo³ywaæ animacji ponownie, dopóki nie zakoñczy siê obecna animacja
        }
    }

    // Metoda, która bêdzie wywo³ywana po zakoñczeniu animacji
    public void OnAnimationEnd()
    {
        isMoving = false; // Resetujemy flagê, ¿eby animacja mog³a siê uruchomiæ przy nastêpnym ruchu
    }
}
