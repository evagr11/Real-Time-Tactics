using UnityEngine;

/// <summary>Vida que queda al romper una caja; cura un punto a la pieza que pasa por ella.</summary>
public class HealthPickup : BoardEntity
{
    HealthPickupVisual visual;

    /// <summary>Crea la vida y su objeto en la escena.</summary>
    /// <param name="position">Casilla.</param>
    /// <param name="prefab">Prefab de la vida.</param>
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

    /// <summary>Cura un punto a la pieza si no tiene la vida al máximo y desaparece.</summary>
    /// <param name="piece">Pieza que la recoge.</param>
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

    /// <summary>Borra el objeto de la vida.</summary>
    public override void DestroyVisuals()
    {
        GameObject.Destroy(visual.gameObject);
    }
}
