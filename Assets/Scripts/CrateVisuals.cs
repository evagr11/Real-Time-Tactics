using UnityEngine;

public class CrateVisuals : MonoBehaviour
{
    // Este script gestiona la animaci�n visual de la caja (crate): rotaci�n y oscilaci�n vertical.

    public float rotationSpeed = 60f; // grados por segundo
    public float oscillationAmplitude = 0.05f; // altura m�xima de la oscilaci�n (muy bajo)
    public float oscillationFrequency = 0.3f; // ciclos por segundo (muy lento)

    private Vector3 initialPosition;

    public GameObject BreakParticlesPrefab; // Prefab de partículas

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // Rotaci�n continua en Y
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);

        // Oscilaci�n vertical
        float offsetY = Mathf.Sin(Time.time * Mathf.PI * 2f * oscillationFrequency) * oscillationAmplitude;
        Vector3 pos = initialPosition + new Vector3(0, offsetY, 0);
        transform.position = pos;
    }

    public void PlayDestructionParticles()
    {
        if (BreakParticlesPrefab != null)
        {
            GameObject particles = Instantiate(BreakParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(particles, 2f); // Destruye las partículas después de 2 segundos
        }
    }
}