using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceVisual : MonoBehaviour
{
    // Este script gestiona el aspecto visual de una pieza, permitiendo que se reduzca su tama�o en el eje Y al recibir da�o.

    private Vector3 initialScale; // Escala inicial de la pieza
    private Vector3 initialLocalPosition; // Posici�n local inicial de la pieza

    [SerializeField] Color cooldownColor;

    public Piece pieceReference;
    public float cooldownTransitionTime; // Variable para almacenar el tiempo de transición de cooldown


    private Material originalMaterial;
    private Renderer pieceRenderer;
    private bool isInterpolating = false;
    private float cooldownTimer;
    private Color originalColor;

    public GameObject healParticlesPrefab;
    [SerializeField] private ParticleSystem destructionParticles;

    void Start()
    {
        if (pieceReference != null)
        {
            cooldownTransitionTime = pieceReference.attackCooldownDuration;
        }

        originalColor = originalMaterial.color;
    }


    void Update()
    {
        InterpolateMaterial();
    }

    void Awake()
    {
        initialScale = transform.localScale;
        initialLocalPosition = transform.localPosition;

        pieceRenderer = GetComponent<Renderer>();
        originalMaterial = pieceRenderer.material;
    }

    // Llama a este m�todo desde Piece.Attacked(), pasando la vida actual y la m�xima
    public void ShrinkOnHit(int currentHealth, int maxHealth)
    {
        // Si la vida es 0 o menos, desaparece
        if (currentHealth <= 0)
        {
            transform.localScale = new Vector3(initialScale.x, 0f, initialScale.z);
            transform.localPosition -= new Vector3(0, initialScale.y * 0.5f, 0); // Solo ajusta sin restaurar posición
            return;
        }

        // Escalado proporcional en Y según la vida restante
        float yScale = (float)currentHealth / maxHealth;
        transform.localScale = new Vector3(initialScale.x, initialScale.y * yScale, initialScale.z);

        // Ajusta la posición local en Y para que la base siga apoyada sin restablecer `initialLocalPosition`
        float yOffset = (initialScale.y - initialScale.y * yScale) * 0.5f;
        transform.localPosition -= new Vector3(0, yOffset, 0); // Mantiene la posición actual
    }

    public void UpdateCooldownVisual(bool isOnCooldown, float cooldownTime)
    {
        if (isOnCooldown)
        {
            isInterpolating = true;
            cooldownTimer = 0f;
            cooldownTransitionTime = cooldownTime; // Actualizar la duración visual del cooldown
        }
    }

    void InterpolateMaterial()
    {
        if (isInterpolating)
        {
            cooldownTimer += Time.deltaTime;
            float t = Mathf.Clamp01(cooldownTimer / cooldownTransitionTime);

            pieceRenderer.material.color = Color.Lerp(cooldownColor, originalColor, t);

            if (t >= 1f)
            {
                isInterpolating = false;
            }
        }
    }

    public void PlayHealParticles()
    {
        if (healParticlesPrefab != null)
        {
            // Instancia el prefab de partículas en la posición actual de la pieza
            GameObject particlesObject = Instantiate(healParticlesPrefab, transform.position, Quaternion.identity);

            // Intenta obtener el componente ParticleSystem del objeto instanciado
            ParticleSystem ps = particlesObject.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                // Configura el color de inicio del sistema de partículas usando el color actual del material
                var mainModule = ps.main;
                // Se usa el color actual del renderer; puede ser originalMaterial o pieceRenderer.material.color
                mainModule.startColor = pieceRenderer.material.color;
            }

            // Destruye el objeto de partículas una vez que hayan terminado su efecto (ajusta el tiempo según tu prefab)
            Destroy(particlesObject, 2f);
        }

    }

    public void PlayDestructionParticles()
    {
        ParticleSystem particlesInstance = Instantiate(destructionParticles, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        ParticleSystemRenderer particleRenderer = particlesInstance.GetComponent<ParticleSystemRenderer>();
        if (particleRenderer != null && pieceRenderer != null)
        {
            particleRenderer.material = pieceRenderer.material;
        }

        particlesInstance.Play();

        Destroy(particlesInstance.gameObject, particlesInstance.main.duration);
    }

}