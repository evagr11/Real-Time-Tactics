using UnityEngine;

public class Board
{
    public int width;
    public int height;
    public Square[,] squares;
    public GameObject boardContainer;

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

    public Square GetSquareAtPosition(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return null;
        return squares[x, y];
    }

    public IGameEntity GetEntityAtPosition(Vector2Int position)
    {
        return GetSquareAtPosition(position.x, position.y)?.containedEntity;
    }

    public void SetEntityAtPosition(Vector2Int position, IGameEntity entity)
    {
        Square square = GetSquareAtPosition(position.x, position.y);
        if (square == null) return;
        square.containedEntity = entity;
        if (entity != null)
            entity.position = position;
    }

    public bool Attack(Vector2Int position, bool isPlayer2Attacker, Piece attackingPiece)
    {
        if (IsOutOfBounds(position) || !attackingPiece.isHeld)
            return false;

        IGameEntity entity = GetEntityAtPosition(position);
        if (entity == null)
            return false;

        if (entity is Piece targetPiece)
        {
            if (attackingPiece.isOnAttackCooldown)
                return false;

            if (targetPiece.isPlayer2 != isPlayer2Attacker &&
                targetPiece.entityGameObject != null &&
                targetPiece.entityGameObject.activeInHierarchy)
            {
                IAttackable attackableEntity = targetPiece as IAttackable;
                if (attackableEntity != null)
                {
                    attackableEntity.Attacked();

                    int currentPieces = GameManager.Instance.GetActivePiecesCount(attackingPiece.isPlayer2);
                    int initialPieces = GameManager.Instance.pieceNum;
                    attackingPiece.StartAttackCooldown(currentPieces, initialPieces);
                    return true;
                }
            }
        }
        else if (entity is IAttackable attackable)
        {
            attackable.Attacked();
            return true;
        }

        return false;
    }

    public void AttackFrom(Vector2Int position, bool isPlayer2Attacker, Piece attackingPiece)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (var dir in directions)
        {
            Attack(position + dir, isPlayer2Attacker, attackingPiece);
        }
    }

    public bool IsOutOfBounds(Vector2Int pos)
    {
        return pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height;
    }

    public void ClearEntityAtPosition(Vector2Int position)
    {
        SetEntityAtPosition(position, null);
    }

    public void PlaceCrateAtPosition(Vector2Int position, IGameEntity crate)
    {
        GetSquareAtPosition(position.x, position.y).containedEntity = crate;
    }
}
