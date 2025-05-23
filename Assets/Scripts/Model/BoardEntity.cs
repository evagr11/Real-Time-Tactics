using UnityEngine;

public abstract class BoardEntity : IGameEntity
{
    protected Vector2Int _position; // Posición en el tablero
    public EntityID ID { get; protected set; } // Identificador de la entidad

    public BoardEntity() { }

    public BoardEntity(Vector2Int position, EntityID id)
    {
        _position = position;
        ID = id;
    }

    public virtual Vector2Int position
    {
        get { return _position; }
        set { _position = value; }
    }

    public abstract GameObject entityGameObject { get; }

    public virtual void DestroyVisuals() { }
}
