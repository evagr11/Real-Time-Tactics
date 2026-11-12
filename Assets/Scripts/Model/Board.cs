using UnityEngine;

/// <summary>El tablero: una matriz de <see cref="Square"/> con lo que hay en cada casilla, y los ataques.</summary>
public class Board
{
    public int width;
    public int height;
    public Square[,] squares;
    public GameObject boardContainer;

    /// <summary>Crea todas las casillas.</summary>
    /// <param name="width">Ancho.</param>
    /// <param name="height">Alto.</param>
    /// <param name="boardSquarePrefab">Prefab de la casilla.</param>
    public Board(int width, int height, GameObject boardSquarePrefab)
    {
        this.width = width;
        this.height = height;
        squares = new Square[width, height];

        boardContainer = new GameObject("BoardContainer");

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                squares[i, j] = new Square(i, j, boardSquarePrefab, boardContainer.transform);
            }
        }
    }

    /// <param name="x">Columna.</param>
    /// <param name="y">Fila.</param>
    /// <returns>La casilla, o <c>null</c> si está fuera del tablero.</returns>
    public Square GetSquareAtPosition(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {  
            return null; 
        }
        return squares[x, y];
    }

    /// <param name="position">Casilla.</param>
    /// <returns>Lo que hay en la casilla, o <c>null</c>.</returns>
    public IGameEntity GetEntityAtPosition(Vector2Int position)
    {
        return squares[position.x, position.y].containedEntity;
    }


    /// <summary>Pone una entidad en una casilla y actualiza su posición.</summary>
    /// <param name="position">Casilla.</param>
    /// <param name="entity">Entidad, o <c>null</c> para vaciarla.</param>
    public void SetEntityAtPosition(Vector2Int position, IGameEntity entity)
    {
        squares[position.x, position.y].containedEntity = entity;
        if (entity != null)
        {
            entity.position = position;
        }
    }


    /// <summary>
    /// Ataca una casilla. A una pieza rival le quita vida si la pieza atacante no está en espera; una
    /// caja se rompe. Solo ataca la pieza que se lleva cogida.
    /// </summary>
    /// <param name="position">Casilla atacada.</param>
    /// <param name="isPlayer2Attacker">Si ataca el jugador 2.</param>
    /// <param name="attackingPiece">Pieza que ataca.</param>
    /// <returns><c>true</c> si el ataque ha dado a algo.</returns>
    public bool Attack(Vector2Int position, bool isPlayer2Attacker, Piece attackingPiece)
    {
        if (IsOutOfBounds(position) || !attackingPiece.isHeld)
        {  
            return false; 
        }

        IGameEntity entity = GetEntityAtPosition(position);
        if (entity == null)
        {
            return false;
        }

        if (entity is Piece targetPiece)
        {
            if (attackingPiece.isOnAttackCooldown)
            {
                return false;
            }

            if (targetPiece.isPlayer2 != isPlayer2Attacker &&
                targetPiece.entityGameObject.activeInHierarchy)
            {
                targetPiece.Attacked();

                int currentPieces = GameManager.Instance.GetActivePiecesCount(attackingPiece.isPlayer2);
                int initialPieces = GameManager.Instance.pieceNum;
                attackingPiece.StartAttackCooldown(currentPieces, initialPieces);
                return true;
            }
        }
        else if (entity is IAttackable attackable)
        {
            attackable.Attacked();
            return true;
        }

        return false;
    }

    /// <summary>Ataca las cuatro casillas vecinas.</summary>
    /// <param name="position">Casilla desde la que se ataca.</param>
    /// <param name="isPlayer2Attacker">Si ataca el jugador 2.</param>
    /// <param name="attackingPiece">Pieza que ataca.</param>
    public void AttackFrom(Vector2Int position, bool isPlayer2Attacker, Piece attackingPiece)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (var dir in directions)
        {
            Attack(position + dir, isPlayer2Attacker, attackingPiece);
        }
    }

    /// <param name="pos">Casilla.</param>
    /// <returns><c>true</c> si está fuera del tablero.</returns>
    public bool IsOutOfBounds(Vector2Int pos)
    {
        return pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height;
    }

    /// <summary>Vacía una casilla.</summary>
    /// <param name="position">Casilla.</param>
    public void ClearEntityAtPosition(Vector2Int position)
    {
        SetEntityAtPosition(position, null);
    }

    /// <summary>Pone una caja en una casilla.</summary>
    /// <param name="position">Casilla.</param>
    /// <param name="crate">Caja.</param>
    public void PlaceCrateAtPosition(Vector2Int position, IGameEntity crate)
    {
        GetSquareAtPosition(position.x, position.y).containedEntity = crate;
    }
}
