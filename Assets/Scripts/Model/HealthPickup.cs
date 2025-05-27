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

    public override GameObject entityGameObject
    {
        get { return visual.gameObject; }
    }

    public void OnPickedUp(Piece piece)
    {
        if (piece.currentHealth < piece.maxHealth)
        {
            piece.Heal(1);
            PieceVisual pv = piece.pieceGameObject.GetComponent<PieceVisual>();
            pv.PlayHealParticles();
        }
        GameManager.Instance.RemoveHealthPickupAtPosition(position);
        GameObject.Destroy(visual.gameObject);
    }

    public override void DestroyVisuals()
    {
        GameObject.Destroy(visual.gameObject);
    }
}
