using UnityEngine;

public class Square
{
    // Este script representa una casilla del tablero, que puede contener una entidad (pieza, caja, etc.) y su visual en el mundo.

    public Vector2Int position;
    public GameObject boardSquare;
    public IGameEntity containedEntity; // Entidad que ocupa la casilla (puede ser null)


    public Square(int x, int y, GameObject boardSquarePrefab, Transform parent)
    {
        // Asigna la posici�n l�gica de la casilla
        this.position = new Vector2Int(x, y);
        // Instancia el objeto visual de la casilla en la posici�n correspondiente

        this.boardSquare = GameObject.Instantiate(
            boardSquarePrefab,
            new Vector3(position.x, 0, position.y),
            Quaternion.identity,
            parent
        );
        // Inicialmente la casilla est� vac�a
        this.containedEntity = null;
    }

}