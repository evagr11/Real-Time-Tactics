using UnityEngine;

public class Board
{
    public int width;
    public int height;
    public Square[,] squares;
    public GameObject boardContainer; 

    public Board(int width, int height, GameObject boardSquarePrefab, Transform parent = null)
    {
        this.width = width;
        this.height = height;
        squares = new Square[width, height];

        boardContainer = new GameObject("BoardContainer");

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                squares[i, j] = new Square(i, j, boardSquarePrefab, boardContainer.transform);
            }
        }
    }
}
