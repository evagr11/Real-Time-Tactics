using UnityEngine;

public class Board
{
    private Square[,] squares;

    public Board(int width, int height, GameObject whitePrefab, GameObject blackPrefab)
    {
        squares = new Square[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int index = new Vector2Int(x + 1, y + 1);
                GameObject selectedPrefab = ((x + y) % 2 == 0) ? whitePrefab : blackPrefab;
                squares[x, y] = new Square(new Vector2Int(x, y), selectedPrefab, index);
            }
        }
    }

    public Square GetSquare(int x, int y)
    {
        if (x >= 0 && x < squares.GetLength(0) && y >= 0 && y < squares.GetLength(1))
            return squares[x, y];
        return null;
    }
}
