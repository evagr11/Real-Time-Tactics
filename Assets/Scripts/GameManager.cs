using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    // Este script gestiona la l�gica principal del juego, incluyendo la inicializaci�n del tablero, piezas, el cursor, el control de turnos, el manejo de entrada de los jugadores y la aparici�n de cajas (crates).

    public static GameManager Instance { get; private set; }

    [SerializeField] int boardWidth;
    [SerializeField] int boardHeight;
    [SerializeField] GameObject boardSquarePrefab;
    [SerializeField] GameObject cursorPrefab;
    [SerializeField] GameObject player1PiecePrefab;
    [SerializeField] GameObject player2PiecePrefab;
    [SerializeField] GameObject cratePrefab;
    [SerializeField] GameObject healthPickupPrefab;

    [SerializeField] float YOffsetForPieces = 0.5f;
    [SerializeField] int pieceNum = 3;
    [SerializeField] bool startWithRandomPlayer = true;
    [SerializeField] public int maxHealth;

    [SerializeField] float minCrateSpawnCD;
    [SerializeField] float maxCrateSpawnCD;
    [SerializeField] int maxCratesOnBoard;
    [SerializeField] float turnTime;
    [SerializeField] Image timerBar;
    [SerializeField] Color playerColor1;
    [SerializeField] Color playerColor2;

    Board board;
    CursorLogic cursorLogic;
    CursorVisual cursorVisual;
    List<Piece> allPieces = new List<Piece>();
    private float timeLeftInTurn;
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

        // Calcula la posici�n inicial del cursor (centro del tablero)
        Vector2Int cursorStartPos = new Vector2Int(boardWidth / 2, boardHeight / 2);
        // Inicializa la l�gica del cursor
        cursorLogic = new CursorLogic(boardWidth, boardHeight, cursorStartPos, board);

        // Obtiene la casilla inicial para el visual del cursor
        Square startSquareForCursorVisual = board.GetSquareAtPosition(cursorStartPos.x, cursorStartPos.y);

        // Calcula la posici�n visual del cursor
        Vector3 visualCursorPos = startSquareForCursorVisual.boardSquare.transform.position + cursorVisualOffset;
        // Instancia el objeto visual del cursor
        GameObject cursorObject = Instantiate(cursorPrefab, visualCursorPos, Quaternion.identity);
        // Obtiene el componente visual del cursor
        cursorVisual = cursorObject.GetComponent<CursorVisual>();

        // Inicializa las piezas de ambos jugadores
        InitializePieces();

        // Decide aleatoriamente qui�n empieza si est� activado
        if (startWithRandomPlayer) isPlayer2Turn = Random.Range(0, 2) == 0;
        else isPlayer2Turn = false;

        StartNewTurn();

        StartCoroutine(SpawnCrateCoroutine());
    }

    void Update()
    {
        if (timeLeftInTurn > 0)
        {
            HandlePlayerInput();
        }

        UpdateTurnTimerVisuals();
        // Actualiza la posici�n visual del cursor
        UpdateCursorVisualPosition();

        UpdatePieceCooldowns();
    }

    void HandlePlayerInput()
    {
        // Direcci�n de movimiento del cursor
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
            if (Input.GetKeyDown(KeyCode.Return)) attackPressed = true;
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

        // Mueve el cursor si hay direcci�n
        if (moveDirection != Vector2Int.zero) cursorLogic.Move(moveDirection);
        if (interactPressed) cursorLogic.HandlePieceInteraction(isPlayer2Turn);
        if (attackPressed)
            board.AttackFrom(cursorLogic.currentPosition, isPlayer2Turn, GetCurrentPlayerPiece());

    }

    void UpdateCursorVisualPosition()
    {
        // Si falta alguna referencia, no hace nada
        if (cursorVisual == null || board == null || cursorLogic == null) return;
        // Obtiene la casilla actual del cursor
        Square currentCursorSquare = board.GetSquareAtPosition(cursorLogic.currentPosition.x, cursorLogic.currentPosition.y);
        // Si la casilla y su objeto visual existen, actualiza la posici�n visual del cursor
        if (currentCursorSquare != null && currentCursorSquare.boardSquare != null)
        {
            cursorVisual.UpdatePosition(currentCursorSquare.boardSquare.transform.position + cursorVisualOffset);
        }
    }

    void InitializePieces()
    {
        // Limpia la lista de piezas
        allPieces.Clear();
        // Calcula la posici�n central en X
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
            Vector2Int pos = startPositions[i]; // Posici�n inicial
            Square pieceSquare = board.GetSquareAtPosition(pos.x, pos.y); // Casilla correspondiente
            if (pieceSquare != null && pieceSquare.boardSquare != null)
            {
                // Calcula la posici�n visual de la pieza
                Vector3 visualPos = pieceSquare.boardSquare.transform.position + new Vector3(0, YOffsetForPieces, 0);
                // Instancia el objeto visual de la pieza
                GameObject pieceObj = Instantiate(piecePrefab, visualPos, Quaternion.identity);
                // Crea la pieza l�gica
                Piece piece = new Piece(pos, isPlayer2, pieceObj, YOffsetForPieces, maxHealth);
                // A�ade la pieza a la lista
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
            // Si hay menos cajas que el m�ximo, genera una nueva
            if (CountCratesOnBoard() < maxCratesOnBoard) SpawnCrate();
            // Espera un tiempo aleatorio antes de volver a intentar
            yield return new WaitForSeconds(Random.Range(minCrateSpawnCD, maxCrateSpawnCD));
        }
    }

    int CountCratesOnBoard()
    {
        // Cuenta cu�ntas cajas hay actualmente en el tablero
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

    // M�todo que genera una caja en una posici�n aleatoria vac�a del tablero
    void SpawnCrate()
    {
        Vector2Int position;
        // Busca una posici�n vac�a y que no sea la del cursor
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
    public void SpawnHealthPickup(Vector2Int pos)
    {
        HealthPickup pickup = new HealthPickup(pos, healthPickupPrefab);
        board.GetSquareAtPosition(pos.x, pos.y).containedEntity = pickup;
    }

    public void RemoveHealthPickupAtPosition(Vector2Int pos)
    {
        var square = board.GetSquareAtPosition(pos.x, pos.y);
        if (square != null && square.containedEntity is HealthPickup)
            square.containedEntity = null;
    }

    void StartNewTurn()
    {
        timeLeftInTurn = turnTime;

        cursorVisual.UpdateMaterial(isPlayer2Turn);

        UpdateTimerBarColor();
    }

    void UpdateTurnTimerVisuals()
    {
        if (timerBar != null)
        {
            timerBar.fillAmount = Mathf.Clamp01(timeLeftInTurn / turnTime);
        }
    }
    

    void UpdateTimerBarColor()
    {
        if (timerBar != null)
        {
            timerBar.color = isPlayer2Turn ? playerColor2 : playerColor1;
            timerBar.fillOrigin = isPlayer2Turn ? (int)Image.OriginHorizontal.Right : (int)Image.OriginHorizontal.Left;
        }
    }
    

    void FixedUpdate()
    {
        if (timeLeftInTurn > 0)
        {
            timeLeftInTurn -= Time.fixedDeltaTime;
            if (timeLeftInTurn <= 0)
            {
                timeLeftInTurn = 0;
                EndTurn();
            }
        }
    }

    void EndTurn()
    {

        if (cursorLogic != null)
        {
            cursorLogic.HandlePieceInteraction(isPlayer2Turn);
        }

        isPlayer2Turn = !isPlayer2Turn;
        StartNewTurn();
        
    }

    void UpdatePieceCooldowns()
    {
        float dt = Time.deltaTime;
        for (int i = allPieces.Count - 1; i >= 0; i--)
        {
            Piece piece = allPieces[i];

            // Verificar si el GameObject aún existe antes de acceder a sus propiedades
            if (piece.pieceGameObject == null || !piece.pieceGameObject.activeInHierarchy)
            {
                // Si la pieza ya no existe, limpiarla del tablero y eliminarla de la lista
                if (board.GetEntityAtPosition(piece.position) == piece)
                {
                    board.SetEntityAtPosition(piece.position, null);
                }
                allPieces.RemoveAt(i);
                continue; // Evita seguir procesando una pieza que ya ha sido eliminada
            }

            // Si la pieza existe, continuar con el cooldown
            piece.Cooldown(dt);
        }
    }


    Piece GetCurrentPlayerPiece()
    {
        foreach (Piece piece in allPieces)
        {
            if (piece.isPlayer2 == isPlayer2Turn && piece.position == cursorLogic.currentPosition)
            {
                return piece;
            }
        }
        return null; // Si no encuentra una pieza en la posición actual
    }

}