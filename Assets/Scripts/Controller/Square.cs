using UnityEngine;

public class Square
{
    public Vector2Int position;
    public GameObject boardSquare;
    public IGameEntity containedEntity;

    public Square(int x, int y, GameObject boardSquarePrefab, Transform parent)
    {
        position = new Vector2Int(x, y);
        boardSquare = GameObject.Instantiate(boardSquarePrefab, new Vector3(position.x, 0, position.y), Quaternion.identity, parent);
        containedEntity = null;
    }
}
