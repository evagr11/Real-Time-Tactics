using UnityEngine;

public class Piece : IGameEntity, IAttackable
{
    public bool isPlayer2 { get; private set; }
    public GameObject pieceGameObject { get; private set; }
    public GameObject entityGameObject => pieceGameObject;
    public Vector2Int position { get; set; }
    public float yOffsetOnBoard;

    public int maxHealth { get; private set; } // Ahora resiste 3 ataques
    public int currentHealth { get; private set; } // Ahora resiste 3 ataques

    public Piece(Vector2Int startPosition, bool isPlayer2, GameObject pieceGameObject, float yOffset, int maxHealth)
    {
        position = startPosition;
        this.isPlayer2 = isPlayer2;
        this.pieceGameObject = pieceGameObject;
        this.yOffsetOnBoard = yOffset;
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
    }

    public void UpdatePosition(Vector2Int newLogicalPosition, Board boardReference)
    {
        position = newLogicalPosition;
        if (pieceGameObject != null && boardReference != null)
        {
            Square targetSquare = boardReference.GetSquareAtPosition(newLogicalPosition.x, newLogicalPosition.y);
            if (targetSquare != null && targetSquare.boardSquare != null)
            {
                pieceGameObject.transform.position = targetSquare.boardSquare.transform.position + new Vector3(0, yOffsetOnBoard, 0);
            }
        }
    }

    public void Attacked()
    {
        currentHealth--;
        if (pieceGameObject != null)
        {
            PieceVisual visual = pieceGameObject.GetComponent<PieceVisual>();
            if (visual != null)
            {
                visual.ShrinkOnHit(currentHealth, maxHealth);
            }
        }
        if (currentHealth <= 0 && pieceGameObject != null)
        {
            pieceGameObject.SetActive(false);
            // Aquí puedes añadir lógica de eliminación, efectos, etc.
        }
    }
}