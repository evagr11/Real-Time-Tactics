using UnityEngine;

/// <summary>Aspecto del cursor: su posición y el color del jugador con el turno.</summary>
public class CursorVisual : MonoBehaviour
{
    [SerializeField] Material colorPlayer1;
    [SerializeField] Material colorPlayer2;

    /// <summary>Mueve el cursor.</summary>
    /// <param name="newPosition">Posición en la escena.</param>
    public void UpdatePosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    /// <summary>Pinta el cursor con el material del jugador.</summary>
    /// <param name="isPlayer2Turn">Si el turno es del jugador 2.</param>
    public void UpdateMaterial(bool isPlayer2Turn)
    {
        Material newMaterial;
        if (isPlayer2Turn)
        {
            newMaterial = colorPlayer2;
        }
        else
        {
            newMaterial = colorPlayer1;
        }

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.material = newMaterial;
        }
    }
}
