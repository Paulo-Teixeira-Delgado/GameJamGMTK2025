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

    [Header("Detección")]
    public Transform puntoSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;
    public Transform ladoWall;
    public float radioWall = 0.2f;
    public LayerMask capaWall;

    Rigidbody2D rb;
    bool enSuelo;
    bool enWall;
    int wallDirection = 0;
    bool wallJumping = false;
    float coyoteTimer;
    float bufferTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");

        enSuelo = Physics2D.OverlapCircle(puntoSuelo.position, radioSuelo, capaSuelo);
        if (enSuelo) coyoteTimer = coyoteTime;
        else coyoteTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Jump")) bufferTimer = jumpBufferTime;
        else bufferTimer -= Time.deltaTime;

        enWall = puedeWallJump && Physics2D.OverlapCircle(ladoWall.position, radioWall, capaWall);
        wallDirection = enWall ? (h > 0 ? 1 : h < 0 ? -1 : 0) : 0;

        if (bufferTimer > 0 && coyoteTimer > 0 && puedeSaltar)
        {
            Jump();
            bufferTimer = 0;
        }
        else if (puedeWallJump && enWall && !enSuelo && Input.GetButtonDown("Jump"))
        {
            WallJump();
        }

        if (!wallJumping)
            rb.linearVelocity = new Vector2(h * velocidad, rb.linearVelocity.y);

        if (rb.linearVelocity.y < 0)
            rb.gravityScale = gravedadExtraCaida;
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
            rb.gravityScale = gravedadExtraSaltoCorto;
        else
            rb.gravityScale = 1f;
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
    }

    void WallJump()
    {
        wallJumping = true;
        Invoke(nameof(ResetWallJump), wallJumpDuration);
        rb.linearVelocity = new Vector2(-wallDirection * wallJumpPush, fuerzaSalto);
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
        if (ladoWall != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(ladoWall.position, radioWall);
        }
    }
}
