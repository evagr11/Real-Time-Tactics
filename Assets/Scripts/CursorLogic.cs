using UnityEngine;
using System.Collections.Generic;

public class CursorLogic
{
    // Este script gestiona la l�gica de movimiento y selecci�n del cursor sobre el tablero
    public Vector2Int currentPosition { get; private set; } 

    private int boardWidth;
    private int boardHeight;
    private Board boardReference;
    private Piece heldPiece; 

    private Vector2Int originalPickUpPosition;
    private List<Vector2Int> allowedMoveSquares; 

    public CursorLogic(int width, int height, Vector2Int startPosition, Board board)
    {
        boardWidth = width; 
        boardHeight = height; 
        currentPosition = startPosition; 
        this.boardReference = board;
        this.heldPiece = null; 
        this.allowedMoveSquares = new List<Vector2Int>(); 
    }

    // Mueve el cursor en la direcci�n indicada
    public void Move(Vector2Int direction)
    {
        Vector2Int newPotentialPosition = currentPosition + direction; // Calcula la nueva posici�n

        if (!IsHoldingPiece())
        {
            // Si no est� sosteniendo una pieza, el cursor puede moverse libremente y hace wrap-around en los bordes
            if (newPotentialPosition.x < 0) newPotentialPosition.x = boardWidth - 1;
            else if (newPotentialPosition.x >= boardWidth) newPotentialPosition.x = 0;

            if (newPotentialPosition.y < 0) newPotentialPosition.y = boardHeight - 1;
            else if (newPotentialPosition.y >= boardHeight) newPotentialPosition.y = 0;

            currentPosition = newPotentialPosition; // Actualiza la posici�n del cursor
        }
        else
        {
            // Si est� sosteniendo una pieza, solo puede moverse a casillas permitidas
            if (allowedMoveSquares.Contains(newPotentialPosition))
            {
                IGameEntity entityAtTarget = boardReference.GetEntityAtPosition(newPotentialPosition);
                // Solo permite moverse si la casilla est� vac�a o es la original
                if (entityAtTarget == null || newPotentialPosition == originalPickUpPosition)
                {
                    currentPosition = newPotentialPosition;
                    heldPiece.UpdatePosition(currentPosition, boardReference);
                }
            }
        }
    }

    // Gestiona la interacci�n con piezas (recoger o soltar)
    public void HandlePieceInteraction(bool isCurrentPlayer2)
    {
        if (heldPiece == null)
        {
            // Si no est� sosteniendo una pieza, intenta recoger una
            IGameEntity entityOnSquare = boardReference.GetEntityAtPosition(currentPosition);
            if (entityOnSquare is Piece pieceToPick)
            {
                // Solo permite recoger piezas propias y activas
                if (pieceToPick.isPlayer2 == isCurrentPlayer2 && pieceToPick.entityGameObject.activeInHierarchy)
                {
                    heldPiece = pieceToPick;
                    originalPickUpPosition = currentPosition;
                    boardReference.SetEntityAtPosition(originalPickUpPosition, null); // Quita la pieza del tablero
                    CalculateAllowedMoveSquares(); // Calcula los movimientos permitidos
                }
            }
        }
        else
        {
            // Si est� sosteniendo una pieza, intenta soltarla
            IGameEntity entityAtTarget = boardReference.GetEntityAtPosition(currentPosition);
            if (entityAtTarget == null || currentPosition == originalPickUpPosition)
            {
                boardReference.SetEntityAtPosition(currentPosition, heldPiece); // Coloca la pieza en la nueva posici�n

                if (heldPiece.position != currentPosition)
                {
                    heldPiece.UpdatePosition(currentPosition, boardReference); // Actualiza la posici�n visual si es necesario
                }

                heldPiece = null; // Deja de sostener la pieza
                allowedMoveSquares.Clear(); // Limpia los movimientos permitidos
            }
        }
    }

    // Calcula las casillas a las que se puede mover la pieza sostenida
    private void CalculateAllowedMoveSquares()
    {
        allowedMoveSquares.Clear();
        if (heldPiece == null) return;

        allowedMoveSquares.Add(originalPickUpPosition); // Siempre puede volver a la casilla original

        // Solo permite moverse a las casillas ortogonales vac�as
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (Vector2Int dir in directions)
        {
            Vector2Int targetPos = originalPickUpPosition + dir;

            if (targetPos.x >= 0 && targetPos.x < boardWidth &&
                targetPos.y >= 0 && targetPos.y < boardHeight)
            {
                IGameEntity entityAtTarget = boardReference.GetEntityAtPosition(targetPos);
                if (entityAtTarget == null)
                {
                    allowedMoveSquares.Add(targetPos);
                }
            }
        }
    }

    // Indica si el cursor est� sosteniendo una pieza
    public bool IsHoldingPiece()
    {
        return heldPiece != null;
    }
}