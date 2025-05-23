using UnityEngine;

public class Crate : BoardEntity, IAttackable
{
    CrateVisuals visuals; // Componente visual de la caja

    public Crate(Vector2Int position, EntityID iD, GameObject prefab) : base(position, iD)
    {
        Vector3 spawnPos = GameManager.Vector2IntToVector3(position) + new Vector3(0, 0.25f, 0);
        visuals = GameObject.Instantiate(prefab, spawnPos, Quaternion.identity).GetComponent<CrateVisuals>();
    }

    public override GameObject entityGameObject => visuals != null ? visuals.gameObject : null;

    public void Attacked()
    {
        if (visuals != null)
        {
            visuals.PlayDestructionParticles();
            GameObject.Destroy(visuals.gameObject);
        }
        GameManager.Instance.RemoveCrateAtPosition(position);
        GameEvents.CrateBroke.Invoke(position);
        GameManager.Instance.SpawnHealthPickup(position);
    }

    public override void DestroyVisuals()
    {
        if (visuals != null)
            GameObject.Destroy(visuals.gameObject);
    }
}
