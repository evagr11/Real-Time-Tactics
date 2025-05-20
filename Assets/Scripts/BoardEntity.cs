using UnityEngine;

public abstract class BoardEntity : IGameEntity
{
    protected Vector2Int _position;
    public EntityID ID { get; protected set; }

    public BoardEntity() { }

    public BoardEntity(Vector2Int position, EntityID id)
    {
        this._position = position;
        this.ID = id;
    }

    // Implementación de la interfaz IGameEntity
    public virtual Vector2Int position
    {
        get => _position;
        set => _position = value;
    }

    public abstract GameObject entityGameObject { get; }

    public virtual void DestroyVisuals() { }
}