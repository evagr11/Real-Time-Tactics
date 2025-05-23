using UnityEngine;

public class CrateVisuals : MonoBehaviour
{
    public float rotationSpeed = 60f; // Rotación en grados por segundo
    public float oscillationAmplitude = 0.05f; // Amplitud de oscilación vertical
    public float oscillationFrequency = 0.3f; // Frecuencia de oscilación

    private Vector3 initialPosition;
    public GameObject BreakParticlesPrefab;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        float offsetY = Mathf.Sin(Time.time * Mathf.PI * 2f * oscillationFrequency) * oscillationAmplitude;
        transform.position = initialPosition + new Vector3(0, offsetY, 0);
    }

    public void PlayDestructionParticles()
    {
        if (BreakParticlesPrefab != null)
        {
            GameObject particles = Instantiate(BreakParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(particles, 2f);
        }
    }
}
