using UnityEngine;

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

    void Awake()
    {
        initialScale = transform.localScale;
        initialLocalPosition = transform.localPosition;
        pieceRenderer = GetComponent<Renderer>();
        originalMaterial = pieceRenderer.material;
    }

    void Start()
    {
        if (pieceReference != null)
            cooldownTransitionTime = pieceReference.attackCooldownDuration;
        originalColor = originalMaterial.color;
    }

    void Update()
    {
        InterpolateMaterial();
    }

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

    public void UpdateCooldownVisual(bool isOnCooldown, float cooldownTime)
    {
        if (isOnCooldown)
        {
            isInterpolating = true;
            cooldownTimer = 0f;
            cooldownTransitionTime = cooldownTime;
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
                isInterpolating = false;
        }
    }

    public void PlayHealParticles()
    {
        if (healParticlesPrefab != null)
        {
            GameObject particlesObject = Instantiate(healParticlesPrefab, transform.position, Quaternion.identity);
            ParticleSystem ps = particlesObject.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var mainModule = ps.main;
                mainModule.startColor = pieceRenderer.material.color;
            }
            Destroy(particlesObject, 2f);
        }
    }

    public void PlayDestructionParticles()
    {
        ParticleSystem particlesInstance = Instantiate(destructionParticles, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        ParticleSystemRenderer particleRenderer = particlesInstance.GetComponent<ParticleSystemRenderer>();
        if (particleRenderer != null && pieceRenderer != null)
            particleRenderer.material = pieceRenderer.material;
        particlesInstance.Play();
        Destroy(particlesInstance.gameObject, particlesInstance.main.duration);
    }
}
