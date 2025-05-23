using UnityEngine;

public class HealthPickup : BoardEntity
{
    HealthPickupVisual visual;

    public HealthPickup(Vector2Int position, GameObject prefab)
        : base(position, EntityID.HealthPickup)
    {
        Vector3 spawnPos = GameManager.Vector2IntToVector3(position) + new Vector3(0, 0.4f, 0);
        visual = GameObject.Instantiate(prefab, spawnPos, Quaternion.identity)
                             .GetComponent<HealthPickupVisual>();
    }

    public override GameObject entityGameObject => visual != null ? visual.gameObject : null;

    public void OnPickedUp(Piece piece)
    {
        if (piece != null && piece.currentHealth < piece.maxHealth)
        {
            piece.Heal(1);
            PieceVisual pv = piece.pieceGameObject.GetComponent<PieceVisual>();
            if (pv != null)
                pv.PlayHealParticles();
        }
        GameManager.Instance.RemoveHealthPickupAtPosition(position);
        if (visual != null)
            GameObject.Destroy(visual.gameObject);
    }

    public override void DestroyVisuals()
    {
        if (visual != null)
            GameObject.Destroy(visual.gameObject);
    }
}
