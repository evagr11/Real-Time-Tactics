using UnityEngine;

public class Square
{
    public Vector2Int position;
    GameObject boardSquare;

    public Square(int x, int y, GameObject boardSquarePrefab, Transform parent)
    {
        this.position = new Vector2Int(x, y);
        this.boardSquare = GameObject.Instantiate(
            boardSquarePrefab,
            new Vector3(position.x, 0, position.y),
            Quaternion.identity,
            parent  
        );
    }
}
