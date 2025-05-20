using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int boardWidth;
    [SerializeField] int boardHeight;
    [SerializeField] GameObject boardSquarePrefab;
    [SerializeField] GameObject cursorPrefab;

    [SerializeField] GameObject player1PiecePrefab;
    [SerializeField] GameObject player2PiecePrefab;
    [SerializeField] float YOffsetForPieces = 0.5f;
    [SerializeField] int pieceNum = 3;
    [SerializeField] bool startWithRandomPlayer = true;

    Board board;
    CursorLogic cursorLogic;
    CursorVisual cursorVisual;
    List<Piece> allPieces = new List<Piece>();

    private bool isPlayer2Turn;
    private Vector3 cursorVisualOffset = new Vector3(-0.5f, 0.1f, 0);

    void Start()
    {
        board = new Board(boardWidth, boardHeight, boardSquarePrefab);

        Vector2Int cursorStartPos = new Vector2Int(boardWidth / 2, boardHeight / 2);
        cursorLogic = new CursorLogic(boardWidth, boardHeight, cursorStartPos, board);

        Square startSquareForCursorVisual = board.GetSquareAtPosition(cursorStartPos.x, cursorStartPos.y);

        Vector3 visualCursorPos = startSquareForCursorVisual.boardSquare.transform.position + cursorVisualOffset;
        GameObject cursorObject = Instantiate(cursorPrefab, visualCursorPos, Quaternion.identity);
        cursorVisual = cursorObject.GetComponent<CursorVisual>();

        InitializePieces();

        if (startWithRandomPlayer) isPlayer2Turn = Random.Range(0, 2) == 0;
        else isPlayer2Turn = false;
    }

    void Update()
    {
        HandlePlayerInput();
        UpdateCursorVisualPosition();
    }

    void HandlePlayerInput()
    {
        Vector2Int moveDirection = Vector2Int.zero;
        bool interactPressed = false;

        if (isPlayer2Turn)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) moveDirection = Vector2Int.left;
            else if (Input.GetKeyDown(KeyCode.DownArrow)) moveDirection = Vector2Int.right;
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) moveDirection = Vector2Int.down;
            else if (Input.GetKeyDown(KeyCode.RightArrow)) moveDirection = Vector2Int.up;

            if (Input.GetKeyDown(KeyCode.RightShift)) interactPressed = true;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W)) moveDirection = Vector2Int.left;
            else if (Input.GetKeyDown(KeyCode.S)) moveDirection = Vector2Int.right;
            else if (Input.GetKeyDown(KeyCode.A)) moveDirection = Vector2Int.down;
            else if (Input.GetKeyDown(KeyCode.D)) moveDirection = Vector2Int.up;

            if (Input.GetKeyDown(KeyCode.LeftShift)) interactPressed = true;
        }

        if (moveDirection != Vector2Int.zero)
        {
            cursorLogic.Move(moveDirection);
        }

        if (interactPressed)
        {
            cursorLogic.HandlePieceInteraction(isPlayer2Turn);
        }
    }

    void UpdateCursorVisualPosition()
    {
        if (cursorVisual == null || board == null || cursorLogic == null) return;

        Square currentCursorSquare = board.GetSquareAtPosition(cursorLogic.currentPosition.x, cursorLogic.currentPosition.y);
        if (currentCursorSquare != null && currentCursorSquare.boardSquare != null)
        {
            cursorVisual.UpdatePosition(currentCursorSquare.boardSquare.transform.position + cursorVisualOffset);
        }
    }

    void InitializePieces()
    {
        allPieces.Clear();

        int midX = boardWidth / 2;
        Vector2Int[] player1StartPositions = { new Vector2Int(midX - 1, 0), new Vector2Int(midX, 0), new Vector2Int(midX + 1, 0) };
        Vector2Int[] player2StartPositions = { new Vector2Int(midX - 1, boardHeight - 1), new Vector2Int(midX, boardHeight - 1), new Vector2Int(midX + 1, boardHeight - 1) };

        CreateInitialPieces(player1StartPositions, player1PiecePrefab, false);
        CreateInitialPieces(player2StartPositions, player2PiecePrefab, true);
    }

    void CreateInitialPieces(Vector2Int[] startPositions, GameObject piecePrefab, bool isPlayer2)
    {
        for (int i = 0; i < pieceNum; i++)
        {
            Vector2Int pos = startPositions[i];
            Square pieceSquare = board.GetSquareAtPosition(pos.x, pos.y);
            if (pieceSquare != null && pieceSquare.boardSquare != null)
            {
                Vector3 visualPos = pieceSquare.boardSquare.transform.position + new Vector3(0, YOffsetForPieces, 0);
                GameObject pieceObj = Instantiate(piecePrefab, visualPos, Quaternion.identity);

                Piece piece = new Piece(pos, isPlayer2, pieceObj, YOffsetForPieces);
                allPieces.Add(piece);
                board.SetEntityAtPosition(pos, piece);
            }
        }
    }
}