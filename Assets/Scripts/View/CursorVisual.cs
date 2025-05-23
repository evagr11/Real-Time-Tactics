using UnityEngine;

public class CursorVisual : MonoBehaviour
{
    [SerializeField] Material colorPlayer1;
    [SerializeField] Material colorPlayer2;

    public void UpdatePosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void UpdateMaterial(bool isPlayer2Turn)
    {
        Material newMaterial = isPlayer2Turn ? colorPlayer2 : colorPlayer1;
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.material = newMaterial;
        }
    }
}
