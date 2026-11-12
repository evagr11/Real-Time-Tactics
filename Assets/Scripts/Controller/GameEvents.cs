using System;
using UnityEngine;

/// <summary>Eventos del juego.</summary>
public static class GameEvents
{
    /// <summary>Se lanza al romper una caja, con la casilla donde estaba.</summary>
    public static Action<Vector2Int> CrateBroke = delegate { };
}
