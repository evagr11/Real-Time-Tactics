using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] int boardWidth;
    [SerializeField] int boardHeight;
    [SerializeField] GameObject boardSquarePrefab;
    [SerializeField] GameObject cursorPrefab;
    [SerializeField] GameObject player1PiecePrefab;
    [SerializeField] GameObject player2PiecePrefab;
    [SerializeField] GameObject cratePrefab;
    [SerializeField] float YOffsetForPieces = 0.5f;
    [SerializeField] int pieceNum = 3;
    [SerializeField] bool startWithRandomPlayer = true;
    [SerializeField] public int maxHealth;

    [SerializeField] float minCrateSpawnCD;
    [SerializeField] float maxCrateSpawnCD;
    [SerializeField] int maxCratesOnBoard = 5;

    Board board;
    CursorLogic cursorLogic;
    CursorVisual cursorVisual;
    List<Piece> allPieces = new List<Piece>();
    private bool isPlayer2Turn;
    private Vector3 cursorVisualOffset = new Vector3(-0.5f, -0.05f, 0);

    void Awake()
    {
        Instance = this;
    }

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

        StartCoroutine(SpawnCrateCoroutine());
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
        bool attackPressed = false;

        if (isPlayer2Turn)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) moveDirection = Vector2Int.left;
            else if (Input.GetKeyDown(KeyCode.DownArrow)) moveDirection = Vector2Int.right;
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) moveDirection = Vector2Int.down;
            else if (Input.GetKeyDown(KeyCode.RightArrow)) moveDirection = Vector2Int.up;
            if (Input.GetKeyDown(KeyCode.RightControl)) interactPressed = true;
            if (Input.GetKeyDown(KeyCode.KeypadEnter)) attackPressed = true;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W)) moveDirection = Vector2Int.left;
            else if (Input.GetKeyDown(KeyCode.S)) moveDirection = Vector2Int.right;
            else if (Input.GetKeyDown(KeyCode.A)) moveDirection = Vector2Int.down;
            else if (Input.GetKeyDown(KeyCode.D)) moveDirection = Vector2Int.up;
            if (Input.GetKeyDown(KeyCode.LeftControl)) interactPressed = true;
            if (Input.GetKeyDown(KeyCode.Space)) attackPressed = true;
        }

        if (moveDirection != Vector2Int.zero) cursorLogic.Move(moveDirection);
        if (interactPressed) cursorLogic.HandlePieceInteraction(isPlayer2Turn);
        if (attackPressed) board.AttackFrom(cursorLogic.currentPosition, isPlayer2Turn);
    }

    void UpdateCursorVisualPosition()
    {
        if (cursorVisual == null || board == null || cursorLogic == null) return;
        Square currentCursorSquare = board.GetSquareAtPosition(cursorLogic.currentPosition.x, cursorLogic.currentPosition.y);
        if (currentCursorSquare != null && currentCursorSquare.boardSquare != null)
            cursorVisual.UpdatePosition(currentCursorSquare.boardSquare.transform.position + cursorVisualOffset);
    }

    void InitializePieces()
    {
        allPieces.Clear();
        int midX = boardWidth / 2;
        Vector2Int[] player1StartPositions = { new Vector2Int(midX - 1, 0), new Vector2Int(midX, 0), new Vector2Int(midX + 1, 0) };
        Vector2Int[] player2StartPositions = { new Vector2Int(midX - 1, boardHeight - 1), new Vector2Int(midX, boardHeight - 1), new Vector2Int(midX + 1, boardHeight - 1) };
        CreateInitialPieces(player1StartPositions, player1PiecePrefab, false, maxHealth);
        CreateInitialPieces(player2StartPositions, player2PiecePrefab, true, maxHealth);
    }

    void CreateInitialPieces(Vector2Int[] startPositions, GameObject piecePrefab, bool isPlayer2, int maxHealth)
    {
        for (int i = 0; i < pieceNum; i++)
        {
            Vector2Int pos = startPositions[i];
            Square pieceSquare = board.GetSquareAtPosition(pos.x, pos.y);
            if (pieceSquare != null && pieceSquare.boardSquare != null)
            {
                Vector3 visualPos = pieceSquare.boardSquare.transform.position + new Vector3(0, YOffsetForPieces, 0);
                GameObject pieceObj = Instantiate(piecePrefab, visualPos, Quaternion.identity);
                Piece piece = new Piece(pos, isPlayer2, pieceObj, YOffsetForPieces, maxHealth);
                allPieces.Add(piece);
                board.SetEntityAtPosition(pos, piece);
            }
        }
    }

    IEnumerator SpawnCrateCoroutine()
    {
        while (true)
        {
            if (CountCratesOnBoard() < maxCratesOnBoard) SpawnCrate();
            yield return new WaitForSeconds(Random.Range(minCrateSpawnCD, maxCrateSpawnCD));
        }
    }

    int CountCratesOnBoard()
    {
        int count = 0;
        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < boardHeight; y++)
            {
                var entity = board.GetSquareAtPosition(x, y).containedEntity;
                if (entity != null && entity is Crate)
                {
                    count++;
                }
            }
        }
        return count;
    }

    // Método que genera una caja en una posición aleatoria vacía del tablero
    void SpawnCrate()
    {
        Vector2Int position;
        do
        {
            position = new Vector2Int(Random.Range(0, boardWidth), Random.Range(0, boardHeight));
        } while (board.GetSquareAtPosition(position.x, position.y).containedEntity != null || position == cursorLogic.currentPosition);
        Crate crate = new Crate(position, EntityID.Crate, cratePrefab);
        board.GetSquareAtPosition(position.x, position.y).containedEntity = crate;
    }

    public void RemoveCrateAtPosition(Vector2Int pos)
    {
        var square = board.GetSquareAtPosition(pos.x, pos.y);
        if (square != null && square.containedEntity is Crate) square.containedEntity = null;
    }

    public static Vector3 Vector2IntToVector3(Vector2Int vector2IntPos)
    {
        return new Vector3(vector2IntPos.x, 0, vector2IntPos.y);
    }
}