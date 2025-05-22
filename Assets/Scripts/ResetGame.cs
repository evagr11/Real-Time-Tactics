using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene("Principal"); // Reemplaza con el nombre real de tu escena inicial
    }
}
