using UnityEngine;

public class Square
{
    public Vector2Int Position { get; private set; }
    public GameObject Instance { get; private set; }
    public Vector2Int Index { get; private set; }

    public Square(Vector2Int position, GameObject prefab, Vector2Int index)
    {
        Position = position;
        Index = index;
        // Instantiate the prefab at the given position (with y = 0) in the scene
        Instance = GameObject.Instantiate(prefab, new Vector3(position.x, 0, position.y), Quaternion.identity);
    }
}
