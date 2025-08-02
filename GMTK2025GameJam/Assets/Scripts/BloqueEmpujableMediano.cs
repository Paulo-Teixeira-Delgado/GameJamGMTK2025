using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BloqueEmpujableMediano : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        rb.mass = 100f; // Muy pesado por defecto
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mediano"))
        {
            // Solo si el que colisiona es mediano, se desbloquea el movimiento
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Solo rota bloqueada, se puede mover en X
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mediano"))
        {
            // Se vuelve a congelar el eje X cuando se deja de tocar
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
