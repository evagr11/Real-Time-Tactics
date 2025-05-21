using UnityEngine;

public abstract class BoardEntity : IGameEntity
{
    // Clase base abstracta para entidades del tablero (piezas, cajas, etc.), implementa la interfaz IGameEntity.

    protected Vector2Int _position; // Posición de la entidad en el tablero
    public EntityID ID { get; protected set; } // Identificador de la entidad


    public BoardEntity() { } // Constructor por defecto

    public BoardEntity(Vector2Int position, EntityID id)
    {
        this._position = position; // Asigna la posición
        this.ID = id; // Asigna el ID
    }

    // Implementación de la interfaz IGameEntity
    public virtual Vector2Int position
    {
        get { return _position; }
        set { _position = value; }
    }

    // Propiedad abstracta para obtener el GameObject visual de la entidad
    public abstract GameObject entityGameObject { get; }

    // Método virtual para destruir los visuales de la entidad
    public virtual void DestroyVisuals() { }
}