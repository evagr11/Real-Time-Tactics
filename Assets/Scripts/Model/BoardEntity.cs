using UnityEngine;

/// <summary>Base de lo que puede ocupar una casilla, salvo las piezas: cajas y vidas.</summary>
public abstract class BoardEntity : IGameEntity
{
    protected Vector2Int _position; // Posición en el tablero
    /// <summary>Tipo de entidad.</summary>
    public EntityID ID { get; protected set; } // Identificador de la entidad

    public BoardEntity() { }

    /// <summary>Crea la entidad en una casilla.</summary>
    /// <param name="position">Casilla.</param>
    /// <param name="id">Tipo de entidad.</param>
    public BoardEntity(Vector2Int position, EntityID id)
    {
        _position = position;
        ID = id;
    }

    /// <summary>Casilla en la que está.</summary>
    public virtual Vector2Int position
    {
        get { return _position; }
        set { _position = value; }
    }

    /// <summary>Objeto de la escena que la representa.</summary>
    public abstract GameObject entityGameObject { get; }

    /// <summary>Borra su objeto de la escena.</summary>
    public virtual void DestroyVisuals() { }
}
