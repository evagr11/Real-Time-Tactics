using UnityEngine;

/// <summary>Aspecto de la caja: gira, flota y suelta partículas al romperse.</summary>
public class CrateVisuals : MonoBehaviour
{
    public float rotationSpeed = 60f;     
    public float moveSpeed = 0.1f;       
    public float moveDistance = 0.05f;  

    private Vector3 initialPosition;
    private bool movingUp = true;
    public GameObject BreakParticlesPrefab;

    /// <summary>Guarda la posición inicial.</summary>
    void Start()
    {
        initialPosition = transform.position;
    }

    /// <summary>Gira el objeto y lo hace subir y bajar un poco, para que se vea que se puede recoger.</summary>
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        if (movingUp)
        {
            transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
            if (transform.position.y >= initialPosition.y + moveDistance)
            {
                transform.position = new Vector3(transform.position.x, initialPosition.y + moveDistance, transform.position.z);
                movingUp = false;
            }
        }
        else
        {
            transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
            if (transform.position.y <= initialPosition.y - moveDistance)
            {
                transform.position = new Vector3(transform.position.x, initialPosition.y - moveDistance, transform.position.z);
                movingUp = true;
            }
        }
    }

    /// <summary>Crea las partículas de rotura y las borra a los 2 segundos.</summary>
    public void PlayDestructionParticles()
    {
        GameObject particles = Instantiate(BreakParticlesPrefab, transform.position, Quaternion.identity);
        Destroy(particles, 2f);
    }
}
