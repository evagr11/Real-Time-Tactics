using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceVisual : MonoBehaviour
{
    // Este script gestiona el aspecto visual de una pieza, permitiendo que se reduzca su tamaño en el eje Y al recibir daño.

    private Vector3 initialScale; // Escala inicial de la pieza
    private Vector3 initialLocalPosition; // Posición local inicial de la pieza


    void Awake()
    {
        initialScale = transform.localScale;
        initialLocalPosition = transform.localPosition;
    }

    // Llama a este método desde Piece.Attacked(), pasando la vida actual y la máxima
    public void ShrinkOnHit(int currentHealth, int maxHealth)
    {
        // Si la vida es 0 o menos, desaparece
        if (currentHealth <= 0)
        {
            transform.localScale = new Vector3(initialScale.x, 0f, initialScale.z);
            transform.localPosition = initialLocalPosition - new Vector3(0, initialScale.y * 0.5f, 0);
            return;
        }

        // Escalado proporcional en Y según la vida restante
        float yScale = (float)currentHealth / maxHealth;
        transform.localScale = new Vector3(initialScale.x, initialScale.y * yScale, initialScale.z);

        // Ajusta la posición local en Y para que la base siga apoyada
        float yOffset = (initialScale.y - initialScale.y * yScale) * 0.5f;
        transform.localPosition = initialLocalPosition - new Vector3(0, yOffset, 0);
    }
}