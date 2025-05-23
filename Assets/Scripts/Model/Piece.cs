using UnityEngine;

public class Piece : IGameEntity, IAttackable
{
    public bool isPlayer2 { get; private set; }
    public GameObject pieceGameObject { get; private set; }
    public GameObject entityGameObject => pieceGameObject;
    public Vector2Int position { get; set; }
    public float yOffsetOnBoard;
    public int maxHealth { get; private set; }
    public int currentHealth { get; private set; }
    public bool isOnAttackCooldown { get; private set; }
    private float currentAttackCooldownTimer;
    public float attackCooldownDuration { get; set; } = 2f;
    public bool isHeld { get; set; } = false;

    public Piece(Vector2Int startPosition, bool isPlayer2, GameObject pieceGameObject, float yOffset, int maxHealth)
    {
        position = startPosition;
        this.isPlayer2 = isPlayer2;
        this.pieceGameObject = pieceGameObject;
        this.yOffsetOnBoard = yOffset;
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
        isOnAttackCooldown = false;
        currentAttackCooldownTimer = 0f;
    }

    public void UpdatePosition(Vector2Int newLogicalPosition, Board boardReference)
    {
        position = newLogicalPosition;
        if (pieceGameObject != null && boardReference != null)
        {
            Square targetSquare = boardReference.GetSquareAtPosition(newLogicalPosition.x, newLogicalPosition.y);
            if (targetSquare != null && targetSquare.boardSquare != null)
                pieceGameObject.transform.position = targetSquare.boardSquare.transform.position + new Vector3(0, yOffsetOnBoard, 0);
            if (targetSquare != null && targetSquare.containedEntity is HealthPickup healthPickup)
                healthPickup.OnPickedUp(this);
        }
    }

    public void Attacked()
    {
        currentHealth--;
        if (pieceGameObject != null)
        {
            PieceVisual visual = pieceGameObject.GetComponent<PieceVisual>();
            if (visual != null)
                visual.ShrinkOnHit(currentHealth, maxHealth);
        }
        if (currentHealth <= 0 && pieceGameObject != null)
        {
            PieceVisual visual = pieceGameObject.GetComponent<PieceVisual>();
            if (visual != null)
                visual.PlayDestructionParticles();

            int remainingPieces = GameManager.Instance.GetActivePiecesCount(isPlayer2) - 1;
            remainingPieces = Mathf.Max(remainingPieces, 0);
            int initialPieces = GameManager.Instance.pieceNum;
            float multiplier = (float)remainingPieces / initialPieces;
            multiplier = Mathf.Max(multiplier, 0.2f);
            float newCooldown = attackCooldownDuration * multiplier;

            GameObject.Destroy(pieceGameObject);
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        if (pieceGameObject != null)
        {
            PieceVisual visual = pieceGameObject.GetComponent<PieceVisual>();
            if (visual != null)
                visual.ShrinkOnHit(currentHealth, maxHealth);
        }
    }

    public void StartAttackCooldown(int currentPieces, int initialPieces)
    {
        if (!isOnAttackCooldown)
        {
            PieceVisual visual = pieceGameObject.GetComponent<PieceVisual>();
            if (visual != null)
            {
                isOnAttackCooldown = true;
                float multiplier = (float)currentPieces / initialPieces;
                multiplier = Mathf.Max(multiplier, 0.2f);
                currentAttackCooldownTimer = attackCooldownDuration * multiplier;
                visual.UpdateCooldownVisual(true, currentAttackCooldownTimer);
            }
        }
    }

    public void Cooldown(float deltaTime)
    {
        if (isOnAttackCooldown)
        {
            currentAttackCooldownTimer -= deltaTime;
            if (currentAttackCooldownTimer <= 0)
            {
                isOnAttackCooldown = false;
                currentAttackCooldownTimer = 0;
                PieceVisual visual = pieceGameObject.GetComponent<PieceVisual>();
                if (visual != null)
                    visual.UpdateCooldownVisual(false, attackCooldownDuration);
            }
        }
    }
}
