using UnityEngine;

public class Board
{
    // Este script representa el tablero de juego, gestionando las casillas, entidades y ataques.

    public int width;
    public int height;
    public Square[,] squares;
    public GameObject boardContainer;

    public Board(int width, int height, GameObject boardSquarePrefab, Transform parent = null)
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

    public Square GetSquareAtPosition(int x, int y)
    {
        // Devuelve la casilla en la posición dada, o null si está fuera de límites
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return null;
        }
        return squares[x, y];
    }

    public IGameEntity GetEntityAtPosition(Vector2Int position)
    {
        // Devuelve la entidad en la posición dada
        Square square = GetSquareAtPosition(position.x, position.y);
        return square?.containedEntity;
    }

    public void SetEntityAtPosition(Vector2Int position, IGameEntity entity)
    {
        // Coloca una entidad en la posición dada
        Square square = GetSquareAtPosition(position.x, position.y);
        if (square != null)
        {
            square.containedEntity = entity;
            if (entity != null)
            {
                entity.position = position;
            }
        }
    }

    public bool Attack(Vector2Int position, bool isPlayer2Attacker)
    {
        // Realiza un ataque en la posición dada
        if (IsOutOfBounds(position))
            return false;

        IGameEntity entity = GetEntityAtPosition(position);
        if (entity == null)
            return false;

        // Si es una pieza, solo ataca si es del jugador contrario y está activa
        Piece targetPiece = entity as Piece;
        if (targetPiece != null)
        {
            // Verificar que el GameObject no sea nulo antes de acceder a activeInHierarchy
            if (targetPiece.isPlayer2 != isPlayer2Attacker &&
            targetPiece.entityGameObject != null &&
            targetPiece.entityGameObject.activeInHierarchy)
            {
                IAttackable attackableEntity = targetPiece as IAttackable;
                if (attackableEntity != null)
                {
                    attackableEntity.Attacked();
                    return true;
                }
            }
            // No retornes aquí, deja que siga para comprobar si es otra entidad atacable
        }

        // Si es una crate, la ataca siempre
        if (entity is IAttackable attackable && !(entity is Piece))
        {
            attackable.Attacked();
            return true;
        }

        return false;
    }

    public void AttackFrom(Vector2Int position, bool isPlayer2Attacker)
    {
        // Ataca en las 4 direcciones desde la posición dada
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (var dir in directions)
        {
            Vector2Int target = position + dir;
            Attack(target, isPlayer2Attacker);
        }
    }

    public bool IsOutOfBounds(Vector2Int pos)
    {
        // Devuelve true si la posición está fuera del tablero
        return pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height;
    }

    public void ClearEntityAtPosition(Vector2Int position)
    {
        // Elimina la entidad de la posición dada
        while (GetSquareAtPosition(position.x, position.y).containedEntity != null)
        {
            SetEntityAtPosition(position, null);
        }
    }

    public void PlaceCrateAtPosition(Vector2Int position, IGameEntity crate)
    {
        // Coloca una caja en la posición dada
        GetSquareAtPosition(position.x, position.y).containedEntity = crate;
    }
}