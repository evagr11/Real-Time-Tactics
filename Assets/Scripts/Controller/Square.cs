using UnityEngine;

/// <summary>Una casilla del tablero: su posición, su objeto en la escena y lo que hay encima.</summary>
public class Square
{
    public Vector2Int position;
    public GameObject boardSquare;
    public IGameEntity containedEntity;

    /// <summary>Crea la casilla y su objeto en la escena.</summary>
    /// <param name="x">Columna.</param>
    /// <param name="y">Fila.</param>
    /// <param name="boardSquarePrefab">Prefab de la casilla.</param>
    /// <param name="parent">Objeto que agrupa las casillas.</param>
    public Square(int x, int y, GameObject boardSquarePrefab, Transform parent)
    {
        position = new Vector2Int(x, y);
        boardSquare = GameObject.Instantiate(boardSquarePrefab, new Vector3(position.x, 0, position.y), Quaternion.identity, parent);
        containedEntity = null;
    }
}
