using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform jugador;
    public float suavidad = 0.15f;
    public Vector2 zonaMuerta = new Vector2(2f, 1.5f);

    private Vector3 velocidadSuavizado = Vector3.zero;
    private Vector3 posicionDeseada;

    void LateUpdate()
    {
        if (jugador == null) return;

        Vector3 posCamara = transform.position;
        Vector3 posJugador = jugador.position;

        // Calcula offset desde el centro de la cámara
        Vector2 offset = new Vector2(posJugador.x - posCamara.x, posJugador.y - posCamara.y);

        // Aplica zona muerta
        if (Mathf.Abs(offset.x) > zonaMuerta.x)
        {
            posCamara.x = posJugador.x - Mathf.Sign(offset.x) * zonaMuerta.x;
        }

        if (Mathf.Abs(offset.y) > zonaMuerta.y)
        {
            posCamara.y = posJugador.y - Mathf.Sign(offset.y) * zonaMuerta.y + 1.6f;
        }

        // Interpola suavemente hacia la nueva posición deseada
        posicionDeseada = new Vector3(posCamara.x, posCamara.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, posicionDeseada, ref velocidadSuavizado, suavidad);
    }

    void OnDrawGizmosSelected()
    {
        if (jugador == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(jugador.position, new Vector3(zonaMuerta.x * 2, zonaMuerta.y * 2, 0));
    }
}
