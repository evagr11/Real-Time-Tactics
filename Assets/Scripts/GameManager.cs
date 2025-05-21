using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Este script gestiona la lógica principal del juego, incluyendo la inicialización del tablero, piezas, el cursor, el control de turnos, el manejo de entrada de los jugadores y la aparición de cajas (crates).

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
        // Inicializa el tablero
        board = new Board(boardWidth, boardHeight, boardSquarePrefab);

        // Calcula la posición inicial del cursor (centro del tablero)
        Vector2Int cursorStartPos = new Vector2Int(boardWidth / 2, boardHeight / 2);
        // Inicializa la lógica del cursor
        cursorLogic = new CursorLogic(boardWidth, boardHeight, cursorStartPos, board);

        // Obtiene la casilla inicial para el visual del cursor
        Square startSquareForCursorVisual = board.GetSquareAtPosition(cursorStartPos.x, cursorStartPos.y);

        // Calcula la posición visual del cursor
        Vector3 visualCursorPos = startSquareForCursorVisual.boardSquare.transform.position + cursorVisualOffset;
        // Instancia el objeto visual del cursor
        GameObject cursorObject = Instantiate(cursorPrefab, visualCursorPos, Quaternion.identity);
        // Obtiene el componente visual del cursor
        cursorVisual = cursorObject.GetComponent<CursorVisual>();

        // Inicializa las piezas de ambos jugadores
        InitializePieces();

        // Decide aleatoriamente quién empieza si está activado
        if (startWithRandomPlayer) isPlayer2Turn = Random.Range(0, 2) == 0;
        else isPlayer2Turn = false;

        StartCoroutine(SpawnCrateCoroutine());
    }

    void Update()
    {
        // Maneja la entrada del jugador cada frame
        HandlePlayerInput();
        // Actualiza la posición visual del cursor
        UpdateCursorVisualPosition();
    }

    void HandlePlayerInput()
    {
        // Dirección de movimiento del cursor
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

        // Mueve el cursor si hay dirección
        if (moveDirection != Vector2Int.zero) cursorLogic.Move(moveDirection);
        if (interactPressed) cursorLogic.HandlePieceInteraction(isPlayer2Turn);
        if (attackPressed) board.AttackFrom(cursorLogic.currentPosition, isPlayer2Turn);
    }

    void UpdateCursorVisualPosition()
    {
        // Si falta alguna referencia, no hace nada
        if (cursorVisual == null || board == null || cursorLogic == null) return;
        // Obtiene la casilla actual del cursor
        Square currentCursorSquare = board.GetSquareAtPosition(cursorLogic.currentPosition.x, cursorLogic.currentPosition.y);
        // Si la casilla y su objeto visual existen, actualiza la posición visual del cursor
        if (currentCursorSquare != null && currentCursorSquare.boardSquare != null)
            cursorVisual.UpdatePosition(currentCursorSquare.boardSquare.transform.position + cursorVisualOffset);
    }

    void InitializePieces()
    {
        // Limpia la lista de piezas
        allPieces.Clear();
        // Calcula la posición central en X
        int midX = boardWidth / 2;
        // Define posiciones iniciales para ambos jugadores
        Vector2Int[] player1StartPositions = { new Vector2Int(midX - 1, 0), new Vector2Int(midX, 0), new Vector2Int(midX + 1, 0) };
        Vector2Int[] player2StartPositions = { new Vector2Int(midX - 1, boardHeight - 1), new Vector2Int(midX, boardHeight - 1), new Vector2Int(midX + 1, boardHeight - 1) };
        // Crea las piezas iniciales para ambos jugadores
        CreateInitialPieces(player1StartPositions, player1PiecePrefab, false, maxHealth);
        CreateInitialPieces(player2StartPositions, player2PiecePrefab, true, maxHealth);
    }

    void CreateInitialPieces(Vector2Int[] startPositions, GameObject piecePrefab, bool isPlayer2, int maxHealth)
    {
        // Crea tantas piezas como pieceNum
        for (int i = 0; i < pieceNum; i++)
        {
            Vector2Int pos = startPositions[i]; // Posición inicial
            Square pieceSquare = board.GetSquareAtPosition(pos.x, pos.y); // Casilla correspondiente
            if (pieceSquare != null && pieceSquare.boardSquare != null)
            {
                // Calcula la posición visual de la pieza
                Vector3 visualPos = pieceSquare.boardSquare.transform.position + new Vector3(0, YOffsetForPieces, 0);
                // Instancia el objeto visual de la pieza
                GameObject pieceObj = Instantiate(piecePrefab, visualPos, Quaternion.identity);
                // Crea la pieza lógica
                Piece piece = new Piece(pos, isPlayer2, pieceObj, YOffsetForPieces, maxHealth);
                // Añade la pieza a la lista
                allPieces.Add(piece);
                // Coloca la pieza en el tablero
                board.SetEntityAtPosition(pos, piece);
            }
        }
    }

    IEnumerator SpawnCrateCoroutine()
    {
        while (true)
        {
            // Si hay menos cajas que el máximo, genera una nueva
            if (CountCratesOnBoard() < maxCratesOnBoard) SpawnCrate();
            // Espera un tiempo aleatorio antes de volver a intentar
            yield return new WaitForSeconds(Random.Range(minCrateSpawnCD, maxCrateSpawnCD));
        }
    }

    int CountCratesOnBoard()
    {
        // Cuenta cuántas cajas hay actualmente en el tablero
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
        return count; // Devuelve el total
    }

    // Método que genera una caja en una posición aleatoria vacía del tablero
    void SpawnCrate()
    {
        Vector2Int position;
        // Busca una posición vacía y que no sea la del cursor
        do
        {
            position = new Vector2Int(Random.Range(0, boardWidth), Random.Range(0, boardHeight));
        } while (board.GetSquareAtPosition(position.x, position.y).containedEntity != null || position == cursorLogic.currentPosition);
        // Crea la caja y la coloca en el tablero
        Crate crate = new Crate(position, EntityID.Crate, cratePrefab);
        board.GetSquareAtPosition(position.x, position.y).containedEntity = crate;
    }

    public void RemoveCrateAtPosition(Vector2Int pos)
    {
        // Elimina la caja de la casilla indicada si existe
        var square = board.GetSquareAtPosition(pos.x, pos.y);
        if (square != null && square.containedEntity is Crate) square.containedEntity = null;
    }

    public static Vector3 Vector2IntToVector3(Vector2Int vector2IntPos)
    {
        // Convierte un Vector2Int a Vector3 para posicionar objetos en el mundo
        return new Vector3(vector2IntPos.x, 0, vector2IntPos.y);
    }
}