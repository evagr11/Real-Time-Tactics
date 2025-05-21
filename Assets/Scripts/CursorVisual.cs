using UnityEngine;

public class CursorVisual : MonoBehaviour
{
    // Este script gestiona el aspecto visual del cursor en el tablero

    // Actualiza la posici�n visual del cursor en el mundo
    public void UpdatePosition(Vector3 newPosition)
    {
        // Mueve el objeto visual del cursor a la posici�n indicada
        transform.position = newPosition;
    }
}