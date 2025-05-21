using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceVisual : MonoBehaviour
{
    // Este script gestiona el aspecto visual de una pieza, permitiendo que se reduzca su tama�o en el eje Y al recibir da�o.

    private Vector3 initialScale; // Escala inicial de la pieza
    private Vector3 initialLocalPosition; // Posici�n local inicial de la pieza


    void Awake()
    {
        initialScale = transform.localScale;
        initialLocalPosition = transform.localPosition;
    }

    // Llama a este m�todo desde Piece.Attacked(), pasando la vida actual y la m�xima
    public void ShrinkOnHit(int currentHealth, int maxHealth)
    {
        // Si la vida es 0 o menos, desaparece
        if (currentHealth <= 0)
        {
            transform.localScale = new Vector3(initialScale.x, 0f, initialScale.z);
            transform.localPosition -= new Vector3(0, initialScale.y * 0.5f, 0); // Solo ajusta sin restaurar posición
            return;
        }

        // Escalado proporcional en Y según la vida restante
        float yScale = (float)currentHealth / maxHealth;
        transform.localScale = new Vector3(initialScale.x, initialScale.y * yScale, initialScale.z);

        // Ajusta la posición local en Y para que la base siga apoyada sin restablecer `initialLocalPosition`
        float yOffset = (initialScale.y - initialScale.y * yScale) * 0.5f;
        transform.localPosition -= new Vector3(0, yOffset, 0); // Mantiene la posición actual
    }

}