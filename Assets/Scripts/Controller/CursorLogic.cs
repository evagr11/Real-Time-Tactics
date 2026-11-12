using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

/// <summary>
/// Lógica del cursor del tablero: moverse por las casillas, coger una pieza propia, llevarla a
/// una casilla vecina libre y soltarla.
/// </summary>
public class CursorLogic
{
    /// <summary>Casilla en la que está el cursor.</summary>
    public Vector2Int currentPosition { get; private set; }
    private int boardWidth;
    private int boardHeight;
    private Board boardReference;
    private Piece heldPiece;
    private Vector2Int originalPickUpPosition;
    private List<Vector2Int> allowedMoveSquares;

    /// <summary>Crea el cursor en una casilla del tablero.</summary>
    /// <param name="width">Ancho del tablero.</param>
    /// <param name="height">Alto del tablero.</param>
    /// <param name="startPosition">Casilla inicial.</param>
    /// <param name="board">Tablero de la partida.</param>
    public CursorLogic(int width, int height, Vector2Int startPosition, Board board)
    {
        boardWidth = width;
        boardHeight = height;
        currentPosition = startPosition;
        boardReference = board;
        allowedMoveSquares = new List<Vector2Int>();
    }

    /// <summary>
    /// Mueve el cursor una casilla. Sin pieza cogida, al salirse por un lado aparece por el otro;
    /// con una pieza cogida, solo puede ir a una casilla vecina libre, con vida, o volver a la de origen.
    /// </summary>
    /// <param name="direction">Dirección del movimiento.</param>
    public void Move(Vector2Int direction)
    {
        Vector2Int newPotentialPosition = currentPosition + direction;
        if (!IsHoldingPiece())
        {
            if (newPotentialPosition.x < 0)
            {
                newPotentialPosition.x = boardWidth - 1;
            }
            else if (newPotentialPosition.x >= boardWidth)
            {
                newPotentialPosition.x = 0;
            }

            if (newPotentialPosition.y < 0)
            {
                newPotentialPosition.y = boardHeight - 1;
            }
            else if (newPotentialPosition.y >= boardHeight)
            {
                newPotentialPosition.y = 0;
            }

            currentPosition = newPotentialPosition;
        }
        else
        {
            CalculateAllowedMoveSquares();
            if (allowedMoveSquares.Contains(newPotentialPosition))
            {
                IGameEntity entityAtTarget = boardReference.GetEntityAtPosition(newPotentialPosition);
                if (entityAtTarget == null || entityAtTarget is HealthPickup || newPotentialPosition == originalPickUpPosition)
                {
                    currentPosition = newPotentialPosition;
                    heldPiece.UpdatePosition(currentPosition, boardReference);
                }
            }
        }
    }

    /// <summary>
    /// Si no lleva pieza, coge la del jugador que tiene el turno; si lleva una, la suelta en la casilla
    /// actual si está libre, tiene una vida o es la de origen.
    /// </summary>
    /// <param name="isCurrentPlayer2">Si el turno es del jugador 2.</param>
    public void HandlePieceInteraction(bool isCurrentPlayer2)
    {
        if (heldPiece == null)
        {
            IGameEntity entityOnSquare = boardReference.GetEntityAtPosition(currentPosition);
            if (entityOnSquare is Piece pieceToPick && pieceToPick.isPlayer2 == isCurrentPlayer2 && pieceToPick.entityGameObject.activeInHierarchy)
            {
                heldPiece = pieceToPick;
                heldPiece.isHeld = true;
                originalPickUpPosition = currentPosition;
                boardReference.SetEntityAtPosition(originalPickUpPosition, null);
                CalculateAllowedMoveSquares();
            }
        }
        else
        {
            IGameEntity entityAtTarget = boardReference.GetEntityAtPosition(currentPosition);
            if (entityAtTarget == null || entityAtTarget is HealthPickup || currentPosition == originalPickUpPosition)
            {
                if (entityAtTarget is HealthPickup healthPickup)
                {
                    healthPickup.OnPickedUp(heldPiece);
                }
                boardReference.SetEntityAtPosition(currentPosition, heldPiece);
                heldPiece.UpdatePosition(currentPosition, boardReference);
                heldPiece.isHeld = false;
                heldPiece = null;
                allowedMoveSquares.Clear();
            }
        }
    }

    /// <summary>Calcula las casillas a las que se puede llevar la pieza: la de origen y las vecinas libres o con vida.</summary>
    private void CalculateAllowedMoveSquares()
    {
        allowedMoveSquares.Clear();
        if (heldPiece == null)
        {
            return;
        }

        allowedMoveSquares.Add(originalPickUpPosition);
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (Vector2Int dir in directions)
        {
            Vector2Int targetPos = currentPosition + dir;
            if (targetPos.x >= 0 && targetPos.x < boardWidth &&
                targetPos.y >= 0 && targetPos.y < boardHeight)
            {
                IGameEntity entityAtTarget = boardReference.GetEntityAtPosition(targetPos);
                if (entityAtTarget == null || entityAtTarget is HealthPickup)
                {
                    allowedMoveSquares.Add(targetPos);
                }
            }
        }
    }

    /// <returns><c>true</c> si el cursor lleva una pieza.</returns>
    public bool IsHoldingPiece()
    {
        return heldPiece != null;
    }
}
