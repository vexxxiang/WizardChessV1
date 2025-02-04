public class Pawn : ChessPiece
{
    
    public override bool[,] GetAvailableMoves(ChessPiece[,] boardState)
    {
        bool[,] moves = new bool[8, 8];

        // Normalny ruch do przodu (jeœli pole jest puste)
        int direction = isWhite ? 1 : -1;  // Okreœlamy kierunek (w górê lub w dó³)
        if (IsInsideBoard(boardPosition.x, boardPosition.y + direction) && boardState[boardPosition.x, boardPosition.y + direction] == null)
        {
            moves[boardPosition.x, boardPosition.y + direction] = true;

            // Pierwszy ruch: mo¿e przesun¹æ siê o 2 pola do przodu (jeœli oba pola s¹ puste)
            if ((isWhite && boardPosition.y == 1) || (!isWhite && boardPosition.y == 6))
            {
                // Sprawdzamy, czy oba pola s¹ puste, aby pionek móg³ siê poruszyæ o 2 pola
                if (boardState[boardPosition.x, boardPosition.y + direction] == null && boardState[boardPosition.x, boardPosition.y + 2 * direction] == null)
                {
                    moves[boardPosition.x, boardPosition.y + 2 * direction] = true;
                }
            }
        }

        // Atak na skos (jeœli jest przeciwnik)
        if (IsInsideBoard(boardPosition.x + 1, boardPosition.y + direction) && boardState[boardPosition.x + 1, boardPosition.y + direction] != null &&
            boardState[boardPosition.x + 1, boardPosition.y + direction].isWhite != isWhite)
        {
            moves[boardPosition.x + 1, boardPosition.y + direction] = true;
        }

        if (IsInsideBoard(boardPosition.x - 1, boardPosition.y + direction) && boardState[boardPosition.x - 1, boardPosition.y + direction] != null &&
            boardState[boardPosition.x - 1, boardPosition.y + direction].isWhite != isWhite)
        {
            moves[boardPosition.x - 1, boardPosition.y + direction] = true;
        }

        return moves;
    }

    
}
