using UnityEngine;

public class BotonInterruptor : MonoBehaviour
{
    public enum TipoBoton
    {
        Simple,
        Complejo
    }

    [Header("Configuración del botón")]
    public TipoBoton tipo = TipoBoton.Simple;
    public GameObject puertaObjetivo;

    private bool activado = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Mediano"))
            return;

        if (tipo == TipoBoton.Simple && !activado)
        {
            AbrirPuerta();
            activado = true;
        }
        else if (tipo == TipoBoton.Complejo)
        {
            AbrirPuerta();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Mediano"))
            return;

        if (tipo == TipoBoton.Complejo)
        {
            CerrarPuerta();
        }
    }

    void AbrirPuerta()
    {
        if (puertaObjetivo != null)
        {
            puertaObjetivo.SetActive(false); // "Abrir" = Desaparecer
        }
    }

    void CerrarPuerta()
    {
        if (puertaObjetivo != null)
        {
            puertaObjetivo.SetActive(true); // "Cerrar" = Mostrar
        }
    }
}
