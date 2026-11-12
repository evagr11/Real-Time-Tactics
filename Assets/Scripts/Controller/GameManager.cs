using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// Controla la partida: crea el tablero, las piezas y el cursor, reparte los turnos por tiempo,
/// lee los controles de cada jugador, genera cajas y comprueba quién gana.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Medidas tablero")]
    [SerializeField] int boardWidth;
    [SerializeField] int boardHeight;
    [Header("Prefabs")]
    [SerializeField] GameObject boardSquarePrefab;
    [SerializeField] GameObject cursorPrefab;
    [SerializeField] GameObject player1PiecePrefab;
    [SerializeField] GameObject player2PiecePrefab;
    [SerializeField] GameObject cratePrefab;
    [SerializeField] GameObject healthPickupPrefab;

    [Header("Turnos")]
    [SerializeField] float turnTime;
    [SerializeField] Image timerBar;
    [SerializeField] bool startWithRandomPlayer = true;

    [Header("Cajas")]
    [SerializeField] int maxCratesOnBoard;
    [SerializeField] float minCrateSpawnCD;
    [SerializeField] float maxCrateSpawnCD;

    [Header("Piezas")]
    [SerializeField] float YOffsetForPieces = 0.5f;
    [SerializeField] public int pieceNum = 3;
    [SerializeField] public int maxHealth;
    [SerializeField] Color playerColor1;
    [SerializeField] Color playerColor2;

    [Header("Transición")]
    [SerializeField] Image panel;
    [SerializeField] float targetY;
    [SerializeField] float speed;

    /// <summary>Única instancia del gestor, para que el modelo pueda llamarlo.</summary>
    public static GameManager Instance { get; private set; }

    Board board;
    CursorLogic cursorLogic;
    CursorVisual cursorVisual;
    List<Piece> allPieces = new List<Piece>();

    private float timeLeftInTurn;
    private bool isPlayer2Turn;
    private Vector3 cursorVisualOffset = new Vector3(-0.5f, -0.05f, 0);

    /// <summary>Guarda la instancia.</summary>
    void Awake()
    {
        Instance = this;
    }

    /// <summary>Crea el tablero, el cursor y las piezas, elige quién empieza y arranca el primer turno y las cajas.</summary>
    void Start()
    {
        board = new Board(boardWidth, boardHeight, boardSquarePrefab);
        Vector2Int cursorStartPos = new Vector2Int(boardWidth / 2, boardHeight / 2);
        cursorLogic = new CursorLogic(boardWidth, boardHeight, cursorStartPos, board);
        Vector3 visualCursorPos = board.GetSquareAtPosition(cursorStartPos.x, cursorStartPos.y).boardSquare.transform.position + cursorVisualOffset;
        GameObject cursorObject = Instantiate(cursorPrefab, visualCursorPos, Quaternion.identity);
        cursorVisual = cursorObject.GetComponent<CursorVisual>();

        InitializePieces();
        if (startWithRandomPlayer)
        {
            isPlayer2Turn = (Random.Range(0, 2) == 0);
        }
        else
        {
            isPlayer2Turn = false;
        }

        StartNewTurn();
        StartCoroutine(SpawnCrateCoroutine());
    }

    /// <summary>Lee los controles, actualiza la barra de tiempo, el cursor y las esperas de las piezas, y comprueba el final.</summary>
    void Update()
    {
        if (timeLeftInTurn > 0)
        {
            HandlePlayerInput();
        }

        UpdateTurnTimerVisuals();
        UpdateCursorVisualPosition();
        UpdatePieceCooldowns();
        CheckGameOver();
    }

    /// <summary>
    /// Controles del jugador con el turno. Jugador 1: WASD, Ctrl izquierdo para coger o soltar y Espacio
    /// para atacar. Jugador 2: flechas, Ctrl derecho y Enter.
    /// </summary>
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

        if (moveDirection != Vector2Int.zero)
        {
            cursorLogic.Move(moveDirection);
        }

        if (interactPressed)
        {
            cursorLogic.HandlePieceInteraction(isPlayer2Turn);
        }
        if (attackPressed)
        {
            board.AttackFrom(cursorLogic.currentPosition, isPlayer2Turn, GetCurrentPlayerPiece());
        }
    }

    /// <summary>Coloca el cursor visual sobre su casilla.</summary>
    void UpdateCursorVisualPosition()
    {
        Square currentCursorSquare = board.GetSquareAtPosition(cursorLogic.currentPosition.x, cursorLogic.currentPosition.y);
        cursorVisual.UpdatePosition(currentCursorSquare.boardSquare.transform.position + cursorVisualOffset);
    }

    /// <summary>Crea las piezas de los dos jugadores en el centro de su fila de salida.</summary>
    void InitializePieces()
    {
        allPieces.Clear();
        int midX = boardWidth / 2;
        Vector2Int[] player1StartPositions = { new Vector2Int(midX - 1, 0), new Vector2Int(midX, 0), new Vector2Int(midX + 1, 0) };
        Vector2Int[] player2StartPositions = { new Vector2Int(midX - 1, boardHeight - 1), new Vector2Int(midX, boardHeight - 1), new Vector2Int(midX + 1, boardHeight - 1) };

        CreateInitialPieces(player1StartPositions, player1PiecePrefab, false, maxHealth);
        CreateInitialPieces(player2StartPositions, player2PiecePrefab, true, maxHealth);
    }

    /// <summary>Crea las piezas de un jugador.</summary>
    /// <param name="startPositions">Casillas de salida.</param>
    /// <param name="piecePrefab">Prefab de la pieza.</param>
    /// <param name="isPlayer2">Si son del jugador 2.</param>
    /// <param name="maxHealth">Vida máxima de cada pieza.</param>
    void CreateInitialPieces(Vector2Int[] startPositions, GameObject piecePrefab, bool isPlayer2, int maxHealth)
    {
        for (int i = 0; i < pieceNum; i++)
        {
            Vector2Int pos = startPositions[i];
            Vector3 visualPos = board.GetSquareAtPosition(pos.x, pos.y).boardSquare.transform.position + new Vector3(0, YOffsetForPieces, 0);
            GameObject pieceObj = Instantiate(piecePrefab, visualPos, Quaternion.identity);
            Piece piece = new Piece(pos, isPlayer2, pieceObj, YOffsetForPieces, maxHealth);
            allPieces.Add(piece);
            board.SetEntityAtPosition(pos, piece);
        }
    }

    /// <summary>Cada cierto tiempo al azar crea una caja, si no se ha llegado al máximo.</summary>
    IEnumerator SpawnCrateCoroutine()
    {
        while (true)
        {
            if (CountCratesOnBoard() < maxCratesOnBoard)
            {
                SpawnCrate();
            }
            yield return new WaitForSeconds(Random.Range(minCrateSpawnCD, maxCrateSpawnCD));
        }
    }

    /// <returns>El número de cajas que hay en el tablero.</returns>
    int CountCratesOnBoard()
    {
        int count = 0;
        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < boardHeight; y++)
            {
                if (board.GetSquareAtPosition(x, y).containedEntity is Crate)
                {
                    count++;
                }
            }
        }
        return count;
    }

    /// <summary>Crea una caja en una casilla libre al azar que no sea la del cursor.</summary>
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

    /// <summary>Quita la caja de una casilla.</summary>
    /// <param name="pos">Casilla de la caja.</param>
    public void RemoveCrateAtPosition(Vector2Int pos)
    {
        board.GetSquareAtPosition(pos.x, pos.y).containedEntity = null;
    }

    /// <summary>Pasa una casilla del tablero a una posición en la escena (X, 0, Y).</summary>
    /// <param name="vector2IntPos">Casilla.</param>
    /// <returns>Posición en la escena.</returns>
    public static Vector3 Vector2IntToVector3(Vector2Int vector2IntPos)
    {
        return new Vector3(vector2IntPos.x, 0, vector2IntPos.y);
    }

    /// <summary>Deja una vida en una casilla.</summary>
    /// <param name="pos">Casilla.</param>
    public void SpawnHealthPickup(Vector2Int pos)
    {
        HealthPickup pickup = new HealthPickup(pos, healthPickupPrefab);
        board.GetSquareAtPosition(pos.x, pos.y).containedEntity = pickup;
    }

    /// <summary>Quita la vida de una casilla.</summary>
    /// <param name="pos">Casilla.</param>
    public void RemoveHealthPickupAtPosition(Vector2Int pos)
    {
        board.GetSquareAtPosition(pos.x, pos.y).containedEntity = null;
    }

    /// <summary>Reinicia el tiempo del turno y pinta el cursor y la barra del color del jugador.</summary>
    void StartNewTurn()
    {
        timeLeftInTurn = turnTime;
        cursorVisual.UpdateMaterial(isPlayer2Turn);
        UpdateTimerBarColor();
    }

    /// <summary>Vacía la barra de tiempo según lo que queda de turno.</summary>
    void UpdateTurnTimerVisuals()
    {
        timerBar.fillAmount = (timeLeftInTurn / turnTime);
    }

    /// <summary>Pone la barra del color del jugador y la hace vaciarse hacia su lado.</summary>
    void UpdateTimerBarColor()
    {
        if (isPlayer2Turn)
        {
            timerBar.color = playerColor2;
            timerBar.fillOrigin = (int)Image.OriginHorizontal.Right;
        }
        else
        {
            timerBar.color = playerColor1;
            timerBar.fillOrigin = (int)Image.OriginHorizontal.Left;
        }
    }


    /// <summary>Descuenta el tiempo del turno y lo termina al llegar a 0.</summary>
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

    /// <summary>Si un jugador se queda sin piezas, baja el panel y carga la escena de victoria del otro.</summary>
    void CheckGameOver()
    {
        bool player1HasPieces = allPieces.Exists(piece => !piece.isPlayer2);
        bool player2HasPieces = allPieces.Exists(piece => piece.isPlayer2);

        if (!player1HasPieces)
        {
            StartCoroutine(LowerPanelCoroutine(panel, targetY, speed, "Win2"));
        }
        else if (!player2HasPieces)
        {
            StartCoroutine(LowerPanelCoroutine(panel, targetY, speed, "Win1"));
        }
    }

    /// <summary>Baja el panel de transición y carga la escena.</summary>
    /// <param name="panel">Panel que baja.</param>
    /// <param name="targetY">Altura final.</param>
    /// <param name="speed">Velocidad.</param>
    /// <param name="sceneToLoad">Escena que se carga al terminar.</param>
    IEnumerator LowerPanelCoroutine(Image panel, float targetY, float speed, string sceneToLoad)
    {
        Vector3 startPos = panel.rectTransform.position;
        Vector3 targetPos = new Vector3(startPos.x, targetY, startPos.z);

        while (panel.rectTransform.position.y > targetY)
        {
            panel.rectTransform.position = Vector3.MoveTowards(panel.rectTransform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }

    /// <summary>Suelta la pieza que se llevaba y pasa el turno al otro jugador.</summary>
    void EndTurn()
    {
        cursorLogic.HandlePieceInteraction(isPlayer2Turn);
        isPlayer2Turn = !isPlayer2Turn;
        StartNewTurn();
    }

    /// <summary>Quita del tablero las piezas destruidas y descuenta la espera de ataque de las demás.</summary>
    void UpdatePieceCooldowns()
    {
        float dt = Time.deltaTime;
        for (int i = allPieces.Count - 1; i >= 0; i--)
        {
            Piece piece = allPieces[i];
            if (piece.pieceGameObject == null || !piece.pieceGameObject.activeInHierarchy)
            {
                board.SetEntityAtPosition(piece.position, null);
                allPieces.RemoveAt(i);
                continue;
            }
            piece.Cooldown(dt);
        }
    }

    /// <returns>La pieza del jugador con el turno que está bajo el cursor, o <c>null</c>.</returns>
    Piece GetCurrentPlayerPiece()
    {
        foreach (Piece piece in allPieces)
        {
            if (piece.isPlayer2 == isPlayer2Turn && piece.position == cursorLogic.currentPosition)
                return piece;
        }
        return null;
    }

    /// <summary>Pone en espera de ataque la pieza que está bajo el cursor.</summary>
    public void ApplyCooldownToCurrentPiece()
    {
        Piece currentPiece = GetCurrentPlayerPiece();
        currentPiece.StartAttackCooldown(GetActivePiecesCount(currentPiece.isPlayer2), pieceNum);
    }

    /// <param name="isPlayer2">Jugador del que se cuentan las piezas.</param>
    /// <returns>Las piezas que le quedan.</returns>
    public int GetActivePiecesCount(bool isPlayer2)
    {
        return allPieces.FindAll(p => p.isPlayer2 == isPlayer2).Count;
    }
}