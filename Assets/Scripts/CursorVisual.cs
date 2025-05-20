using UnityEngine;

public class CursorVisual : MonoBehaviour
{
    public void UpdatePosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}