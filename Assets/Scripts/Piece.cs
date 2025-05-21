using UnityEngine;

public class Piece : IGameEntity, IAttackable
{
    // Este script representa una pieza del juego, que puede recibir daño, ser movida y tiene un aspecto visual asociado.

    public bool isPlayer2 { get; private set; } // Indica si la pieza pertenece al jugador 2
    public GameObject pieceGameObject { get; private set; } // Referencia al GameObject visual de la pieza
    public GameObject entityGameObject => pieceGameObject; // Implementación de IGameEntity
    public Vector2Int position { get; set; } // Posición lógica de la pieza en el tablero
    public float yOffsetOnBoard; // Desplazamiento en Y para el visual

    public int maxHealth { get; private set; } // Ahora resiste 3 ataques
    public int currentHealth { get; private set; } // Ahora resiste 3 ataques

    public Piece(Vector2Int startPosition, bool isPlayer2, GameObject pieceGameObject, float yOffset, int maxHealth)
    {
        position = startPosition; // Asigna la posición inicial
        this.isPlayer2 = isPlayer2; // Asigna el jugador propietario
        this.pieceGameObject = pieceGameObject; // Asigna el objeto visual
        this.yOffsetOnBoard = yOffset; // Asigna el offset visual
        this.maxHealth = maxHealth; // Asigna la vida máxima
        this.currentHealth = maxHealth; // Inicializa la vida actual
    }

    public void UpdatePosition(Vector2Int newLogicalPosition, Board boardReference)
    {
        // Actualiza la posición lógica
        position = newLogicalPosition;
        // Si existe el objeto visual y la referencia al tablero
        if (pieceGameObject != null && boardReference != null)
        {
            // Obtiene la casilla destino
            Square targetSquare = boardReference.GetSquareAtPosition(newLogicalPosition.x, newLogicalPosition.y);
            if (targetSquare != null && targetSquare.boardSquare != null)
            {
                // Actualiza la posición visual de la pieza
                pieceGameObject.transform.position = targetSquare.boardSquare.transform.position + new Vector3(0, yOffsetOnBoard, 0);
            }
        }
    }

    public void Attacked()
    {
        // Reduce la vida en 1
        currentHealth--;
        if (pieceGameObject != null)
        { 
            // Obtiene el componente visual y actualiza su aspecto según la vida
            PieceVisual visual = pieceGameObject.GetComponent<PieceVisual>();
            if (visual != null)
            {
                visual.ShrinkOnHit(currentHealth, maxHealth);
            }
        }
        if (currentHealth <= 0 && pieceGameObject != null)
        {
            GameObject.Destroy(pieceGameObject);
        }
    }
}