using UnityEngine;

public class CursorVisual : MonoBehaviour
{
    [SerializeField] Material colorPlayer1;
    [SerializeField] Material colorPlayer2;
    // Este script gestiona el aspecto visual del cursor en el tablero

    // Actualiza la posición visual del cursor en el mundo
    public void UpdatePosition(Vector3 newPosition)
    {
        // Mueve el objeto visual del cursor a la posición indicada
        transform.position = newPosition;
    }

    // Actualiza el material del cursor según el turno
    public void UpdateMaterial(bool isPlayer2Turn)
    {
        // Obtiene todos los Renderers en este objeto y en sus hijos
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        // Selecciona el material adecuado según el turno
        Material newMaterial = isPlayer2Turn ? colorPlayer2 : colorPlayer1;

        // Itera por cada renderer y actualiza su material
        foreach (Renderer renderer in renderers)
        {
            renderer.material = newMaterial;
        }
    }
}
