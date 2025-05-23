using UnityEngine;

public interface IGameEntity
{
    GameObject entityGameObject { get; } // GameObject visual de la entidad
    Vector2Int position { get; set; } // Posición lógica de la entidad en el tablero
}
