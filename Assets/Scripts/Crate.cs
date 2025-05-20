using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Crate : BoardEntity, IAttackable
{
    CrateVisuals visuals;

    public Crate(Vector2Int position, EntityID iD, GameObject prefab) : base(position, iD)
    {
        Vector3 spawnPos = GameManager.Vector2IntToVector3(position) + new Vector3(0, 0.25f, 0);
        visuals = GameObject.Instantiate(
            prefab,
            spawnPos,
            Quaternion.identity
        ).GetComponent<CrateVisuals>();
    }

    public override GameObject entityGameObject => visuals != null ? visuals.gameObject : null;

    public void Attacked()
    {
        // Elimina la crate del tablero
        GameManager.Instance.RemoveCrateAtPosition(position);

        // Lanza el evento si lo necesitas
        GameEvents.CrateBroke.Invoke(position);

        // Destruye el objeto visual
        if (visuals != null)
        {
            GameObject.Destroy(visuals.gameObject);
        }
    }

    public override void DestroyVisuals()
    {
        if (visuals != null)
            GameObject.Destroy(visuals.gameObject);
    }
}