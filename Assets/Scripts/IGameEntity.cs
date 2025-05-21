using UnityEngine;

public interface IGameEntity
{
    // Interfaz que representa una entidad del juego que puede estar en una casilla del tablero.
    // Obliga a implementar una referencia a su GameObject visual y su posición lógica en el tablero.
    GameObject entityGameObject { get; } // Referencia al GameObject visual de la entidad
    Vector2Int position { get; set; }    // Posición lógica de la entidad en el tablero
}