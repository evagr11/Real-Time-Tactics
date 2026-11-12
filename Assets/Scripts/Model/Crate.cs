using UnityEngine;

/// <summary>Caja que aparece en el tablero; al romperla deja una vida.</summary>
public class Crate : BoardEntity, IAttackable
{
    CrateVisuals visuals; // Componente visual de la caja

    /// <summary>Crea la caja y su objeto en la escena.</summary>
    /// <param name="position">Casilla.</param>
    /// <param name="iD">Tipo de entidad.</param>
    /// <param name="prefab">Prefab de la caja.</param>
    public Crate(Vector2Int position, EntityID iD, GameObject prefab) : base(position, iD)
    {
        Vector3 spawnPos = GameManager.Vector2IntToVector3(position) + new Vector3(0, 0.25f, 0);
        visuals = GameObject.Instantiate(prefab, spawnPos, Quaternion.identity).GetComponent<CrateVisuals>();
    }

    public override GameObject entityGameObject
    {
        get { return visuals.gameObject; }
    }

    /// <summary>Rompe la caja con partículas, la quita del tablero, lanza <see cref="GameEvents.CrateBroke"/> y deja una vida.</summary>
    public void Attacked()
    {
        visuals.PlayDestructionParticles();
        GameObject.Destroy(visuals.gameObject);

        GameManager.Instance.RemoveCrateAtPosition(position);
        GameEvents.CrateBroke.Invoke(position);
        GameManager.Instance.SpawnHealthPickup(position);
    }

    /// <summary>Borra el objeto de la caja.</summary>
    public override void DestroyVisuals()
    {
        GameObject.Destroy(visuals.gameObject);
    }

}
