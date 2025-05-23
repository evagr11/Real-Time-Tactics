using UnityEngine;
using System.Collections.Generic;

public class CursorLogic
{
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
        boardReference = board;
        allowedMoveSquares = new List<Vector2Int>();
    }

    public void Move(Vector2Int direction)
    {
        Vector2Int newPotentialPosition = currentPosition + direction;
        if (!IsHoldingPiece())
        {
            if (newPotentialPosition.x < 0)
                newPotentialPosition.x = boardWidth - 1;
            else if (newPotentialPosition.x >= boardWidth)
                newPotentialPosition.x = 0;

            if (newPotentialPosition.y < 0)
                newPotentialPosition.y = boardHeight - 1;
            else if (newPotentialPosition.y >= boardHeight)
                newPotentialPosition.y = 0;

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

    public void HandlePieceInteraction(bool isCurrentPlayer2)
    {
        if (heldPiece == null)
        {
            IGameEntity entityOnSquare = boardReference.GetEntityAtPosition(currentPosition);
            if (entityOnSquare is Piece pieceToPick)
            {
                if (pieceToPick.isPlayer2 == isCurrentPlayer2 && pieceToPick.entityGameObject.activeInHierarchy)
                {
                    heldPiece = pieceToPick;
                    heldPiece.isHeld = true;
                    originalPickUpPosition = currentPosition;
                    boardReference.SetEntityAtPosition(originalPickUpPosition, null);
                    CalculateAllowedMoveSquares();
                }
            }
        }
        else
        {
            IGameEntity entityAtTarget = boardReference.GetEntityAtPosition(currentPosition);
            if (entityAtTarget == null || entityAtTarget is HealthPickup || currentPosition == originalPickUpPosition)
            {
                if (entityAtTarget is HealthPickup healthPickup)
                    healthPickup.OnPickedUp(heldPiece);
                boardReference.SetEntityAtPosition(currentPosition, heldPiece);
                heldPiece.UpdatePosition(currentPosition, boardReference);
                heldPiece.isHeld = false;
                heldPiece = null;
                allowedMoveSquares.Clear();
            }
        }
    }

    private void CalculateAllowedMoveSquares()
    {
        allowedMoveSquares.Clear();
        if (heldPiece == null)
            return;

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
                    allowedMoveSquares.Add(targetPos);
            }
        }
    }

    public bool IsHoldingPiece()
    {
        return heldPiece != null;
    }
}
