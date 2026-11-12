using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>En las escenas de victoria, vuelve a la partida al pulsar Espacio.</summary>
public class ResetGame : MonoBehaviour
{
    /// <summary>Reinicia al pulsar Espacio.</summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    /// <summary>Carga la escena «Principal».</summary>
    void RestartGame()
    {
        SceneManager.LoadScene("Principal"); // Reemplaza con el nombre real de tu escena inicial
    }
}
