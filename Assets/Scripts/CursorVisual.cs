using UnityEngine;

public class CursorVisual : MonoBehaviour
{
    // Este script gestiona el aspecto visual del cursor en el tablero

    // Actualiza la posición visual del cursor en el mundo
    public void UpdatePosition(Vector3 newPosition)
    {
        // Mueve el objeto visual del cursor a la posición indicada
        transform.position = newPosition;
    }
}