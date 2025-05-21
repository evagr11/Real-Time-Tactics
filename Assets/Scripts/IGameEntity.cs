using UnityEngine;

public interface IGameEntity
{
    // Interfaz que representa una entidad del juego que puede estar en una casilla del tablero.
    // Obliga a implementar una referencia a su GameObject visual y su posici�n l�gica en el tablero.
    GameObject entityGameObject { get; } // Referencia al GameObject visual de la entidad
    Vector2Int position { get; set; }    // Posici�n l�gica de la entidad en el tablero
}