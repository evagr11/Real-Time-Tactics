using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Crate : BoardEntity, IAttackable
{
    // Este script representa una caja (crate) en el tablero, que puede ser atacada y destruida.

    CrateVisuals visuals; // Referencia al componente visual de la caja

    public Crate(Vector2Int position, EntityID iD, GameObject prefab) : base(position, iD)
    {
        // Calcula la posici�n de aparici�n de la caja
        Vector3 spawnPos = GameManager.Vector2IntToVector3(position) + new Vector3(0, 0.25f, 0);
        // Instancia el objeto visual de la caja
        visuals = GameObject.Instantiate(
            prefab,
            spawnPos,
            Quaternion.identity
        ).GetComponent<CrateVisuals>();
    }

    // Devuelve el GameObject visual de la caja
    public override GameObject entityGameObject => visuals != null ? visuals.gameObject : null;

    public void Attacked()
    {
        // Elimina la crate del tablero
        GameManager.Instance.RemoveCrateAtPosition(position);

        // Lanza el evento si lo necesitas
        GameEvents.CrateBroke.Invoke(position);

        // Instancia el HealthPickup en la misma posición
        GameManager.Instance.SpawnHealthPickup(position);

        // Destruye el objeto visual
        if (visuals != null)
        {
            GameObject.Destroy(visuals.gameObject);
        }
    }

    // Destruye el objeto visual si existe
    public override void DestroyVisuals()
    {
        if (visuals != null)
            GameObject.Destroy(visuals.gameObject);
    }
}