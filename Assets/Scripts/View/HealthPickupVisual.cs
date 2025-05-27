using UnityEngine;

public class HealthPickupVisual : MonoBehaviour
{
    public float rotationSpeed = 60f;      
    public float moveSpeed = 0.1f;       
    public float moveDistance = 0.05f;    

    private Vector3 initialPosition;
    private bool movingUp = true;
    private float currentYRotation = 0f;

    void Start()
    {
        initialPosition = transform.position;
        currentYRotation = transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(-90, currentYRotation, 0);
    }

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
}
