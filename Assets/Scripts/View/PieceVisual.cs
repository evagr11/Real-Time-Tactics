using UnityEngine;

/// <summary>Aspecto de la pieza: encoge al perder vida, cambia de color en la espera de ataque y suelta partículas.</summary>
public class PieceVisual : MonoBehaviour
{
    private Vector3 initialScale;
    private Vector3 initialLocalPosition;

    [SerializeField] Color cooldownColor;
    public Piece pieceReference;
    public float cooldownTransitionTime;
    private Material originalMaterial;
    private Renderer pieceRenderer;
    private bool isInterpolating = false;
    private float cooldownTimer;
    private Color originalColor;

    public GameObject healParticlesPrefab;
    [SerializeField] private ParticleSystem destructionParticles;

    /// <summary>Guarda la escala, la posición y el material iniciales.</summary>
    void Awake()
    {
        initialScale = transform.localScale;
        initialLocalPosition = transform.localPosition;
        pieceRenderer = GetComponent<Renderer>();
        originalMaterial = pieceRenderer.material;
    }

    /// <summary>Guarda el color original.</summary>
    void Start()
    {
        if (pieceReference != null)
        {
            cooldownTransitionTime = pieceReference.attackCooldownDuration;
        }
        originalColor = originalMaterial.color;
    }

    /// <summary>Avanza el cambio de color de la espera.</summary>
    void Update()
    {
        InterpolateMaterial();
    }

    /// <summary>Ajusta la altura de la pieza a la vida que le queda.</summary>
    /// <param name="currentHealth">Vida actual.</param>
    /// <param name="maxHealth">Vida máxima.</param>
    public void ShrinkOnHit(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
        {
            transform.localScale = new Vector3(initialScale.x, 0f, initialScale.z);
            transform.localPosition -= new Vector3(0, initialScale.y * 0.5f, 0);
            return;
        }
        float yScale = (float)currentHealth / maxHealth;
        transform.localScale = new Vector3(initialScale.x, initialScale.y * yScale, initialScale.z);
        float yOffset = (initialScale.y - initialScale.y * yScale) * 0.5f;
        transform.localPosition -= new Vector3(0, yOffset, 0);
    }

    /// <summary>Empieza el cambio de color de la espera.</summary>
    /// <param name="isOnCooldown">Si empieza la espera.</param>
    /// <param name="cooldownTime">Duración de la espera.</param>
    public void UpdateCooldownVisual(bool isOnCooldown, float cooldownTime)
    {
        if (isOnCooldown)
        {
            isInterpolating = true;
            cooldownTimer = 0f;
            cooldownTransitionTime = cooldownTime;
        }
    }

    /// <summary>Pasa poco a poco del color de espera al original.</summary>
    void InterpolateMaterial()
    {
        if (isInterpolating)
        {
            cooldownTimer += Time.deltaTime;
            float t = (cooldownTimer / cooldownTransitionTime);
            pieceRenderer.material.color = Color.Lerp(cooldownColor, originalColor, t);
            if (t >= 1f)
            {
                isInterpolating = false;
            }
        }
    }

    /// <summary>Crea partículas de curación del color de la pieza.</summary>
    public void PlayHealParticles()
    {
        GameObject particlesObject = Instantiate(healParticlesPrefab, transform.position, Quaternion.identity);
        ParticleSystem ps = particlesObject.GetComponent<ParticleSystem>();
        var mainModule = ps.main;
        mainModule.startColor = pieceRenderer.material.color;
        Destroy(particlesObject, 2f);
        
    }

    /// <summary>Crea partículas de destrucción con el material de la pieza.</summary>
    public void PlayDestructionParticles()
    {
        ParticleSystem particlesInstance = Instantiate(destructionParticles, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        ParticleSystemRenderer particleRenderer = particlesInstance.GetComponent<ParticleSystemRenderer>();
        particleRenderer.material = pieceRenderer.material;
        particlesInstance.Play();
        Destroy(particlesInstance.gameObject, particlesInstance.main.duration);
    }
}
