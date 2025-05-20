using UnityEngine;

public class Piece : IGameEntity
{
    public bool isPlayer2 { get; private set; }
    public GameObject pieceGameObject { get; private set; }
    public GameObject entityGameObject => pieceGameObject;
    public Vector2Int position { get; set; }
    public float yOffsetOnBoard;

    public Piece(Vector2Int startPosition, bool isPlayer2, GameObject pieceGameObject, float yOffset)
    {
        position = startPosition;
        this.isPlayer2 = isPlayer2;
        this.pieceGameObject = pieceGameObject;
        this.yOffsetOnBoard = yOffset;
    }

    public void UpdatePosition(Vector2Int newLogicalPosition, Board boardReference)
    {
        position = newLogicalPosition;
        if (pieceGameObject != null && boardReference != null)
        {
            Square targetSquare = boardReference.GetSquareAtPosition(newLogicalPosition.x, newLogicalPosition.y);
            if (targetSquare != null && targetSquare.boardSquare != null)
            {
                pieceGameObject.transform.position = targetSquare.boardSquare.transform.position + new Vector3(0, yOffsetOnBoard, 0);
            }
        }
    }
}