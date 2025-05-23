using UnityEngine;

public class HealthPickupVisual : MonoBehaviour
{
    public float rotationSpeed = 60f;
    public float oscillationAmplitude = 0.05f;
    public float oscillationFrequency = 0.3f;

    private Vector3 initialPosition;
    private float currentYRotation = 0f;

    void Start()
    {
        initialPosition = transform.position;
        currentYRotation = transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(-90, currentYRotation, 0);
    }

    void Update()
    {
        currentYRotation += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(-90, currentYRotation, 0);

        float offsetY = Mathf.Sin(Time.time * Mathf.PI * 2f * oscillationFrequency) * oscillationAmplitude;
        transform.position = initialPosition + new Vector3(0, offsetY, 0);
    }
}
