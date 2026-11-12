using UnityEngine;

/// <summary>Cualquier cosa que ocupa una casilla del tablero.</summary>
public interface IGameEntity
{
    GameObject entityGameObject { get; } // GameObject visual de la entidad
    Vector2Int position { get; set; } // Posición lógica de la entidad en el tablero
}
