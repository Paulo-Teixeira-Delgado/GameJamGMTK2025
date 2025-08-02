using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JugadorMovimientoConMejoras : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;

    [Header("Salto")]
    public bool puedeSaltar = true;
    public float fuerzaSalto = 12f;
    public float gravedadExtraCaida = 2.5f;
    public float gravedadExtraSaltoCorto = 2f;

    [Header("Timing de Jump")]
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;

    [Header("Wall Jump")]
    public bool puedeWallJump = true;
    public float wallJumpForce = 10f;
    public float wallJumpPush = 5f;
    public float wallJumpDuration = 0.2f;

    [Header("Wall Slide")]
    public float velocidadDeslizamientoPared = -2f;
    public bool deslizandoEnPared = false;

    [Header("Detección")]
    public Transform puntoSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;
    public Transform ladoWallDerecha;
    public Transform ladoWallIzquierda;
    public float radioWall = 0.2f;
    public LayerMask capaWall;

    Rigidbody2D rb;
    Animator anim;
    bool enSuelo;
    bool enWallDerecha;
    bool enWallIzquierda;
    bool enWall;
    int wallDirection = 0;
    bool wallJumping = false;
    float coyoteTimer;
    float bufferTimer;
    bool saltandoAnim = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");

        enSuelo = Physics2D.OverlapCircle(puntoSuelo.position, radioSuelo, capaSuelo);
        if (enSuelo) coyoteTimer = coyoteTime;
        else coyoteTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            bufferTimer = jumpBufferTime;
            saltandoAnim = true; // Activar animación de salto inmediatamente
        }
        else
        {
            bufferTimer -= Time.deltaTime;
        }

        // Detección de pared derecha e izquierda
        enWallDerecha = puedeWallJump && Physics2D.OverlapCircle(ladoWallDerecha.position, radioWall, capaWall);
        enWallIzquierda = puedeWallJump && Physics2D.OverlapCircle(ladoWallIzquierda.position, radioWall, capaWall);
        enWall = enWallDerecha || enWallIzquierda;
        wallDirection = enWallDerecha ? 1 : enWallIzquierda ? -1 : 0;

        // --- WALL SLIDE ---
        bool sueloBajoDerecha = Physics2D.OverlapCircle(ladoWallDerecha.position + Vector3.down * radioWall, radioSuelo * 0.9f, capaSuelo);
        bool sueloBajoIzquierda = Physics2D.OverlapCircle(ladoWallIzquierda.position + Vector3.down * radioWall, radioSuelo * 0.9f, capaSuelo);
        bool sueloDebajo = Physics2D.OverlapCircle(puntoSuelo.position, radioSuelo, capaSuelo);

        bool puedeDeslizarDerecha = enWallDerecha && !sueloBajoDerecha && !sueloDebajo;
        bool puedeDeslizarIzquierda = enWallIzquierda && !sueloBajoIzquierda && !sueloDebajo;

        // NUEVO: Si hay suelo debajo, nunca deslizar
        if (!sueloDebajo && (puedeDeslizarDerecha || puedeDeslizarIzquierda) && !enSuelo && !wallJumping && rb.linearVelocity.y <= 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, velocidadDeslizamientoPared);
            deslizandoEnPared = true;
        }
        else
        {
            deslizandoEnPared = false;
            if (!wallJumping)
                rb.linearVelocity = new Vector2(h * velocidad, rb.linearVelocity.y);
        }

        if (bufferTimer > 0 && coyoteTimer > 0 && puedeSaltar)
        {
            Jump();
            bufferTimer = 0;
        }
        else if (puedeWallJump && enWall && !enSuelo && Input.GetButtonDown("Jump"))
        {
            WallJump();
        }

        if (rb.linearVelocity.y < 0)
            rb.gravityScale = gravedadExtraCaida;
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
            rb.gravityScale = gravedadExtraSaltoCorto;
        else
            rb.gravityScale = 1f;

        // Cuando toca el suelo, desactivar animación de salto
        if (enSuelo)
            saltandoAnim = false;

        // Animator: Suelo
        anim.SetBool("Suelo", enSuelo);

        // Animator: Saltar (prioridad)
        anim.SetBool("Saltar", saltandoAnim || !enSuelo);

        // Animator: Andar (solo si no está saltando)
        anim.SetBool("Andar", enSuelo && Mathf.Abs(h) > 0.01f && !saltandoAnim);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        saltandoAnim = true; // Activar animación de salto justo al saltar
    }

    void WallJump()
    {
        wallJumping = true;
        Invoke(nameof(ResetWallJump), wallJumpDuration);
        rb.linearVelocity = new Vector2(-wallDirection * wallJumpPush, fuerzaSalto);
        saltandoAnim = true; // Activar animación de salto justo al saltar de pared
    }

    void ResetWallJump()
    {
        wallJumping = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoSuelo != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(puntoSuelo.position, radioSuelo);
        }
        if (ladoWallDerecha != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(ladoWallDerecha.position, radioWall);
        }
        if (ladoWallIzquierda != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(ladoWallIzquierda.position, radioWall);
        }
    }
}
