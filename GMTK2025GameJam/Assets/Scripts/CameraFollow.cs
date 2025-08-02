using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform jugador; // Arrastra aquí el objeto jugador en el inspector

    void LateUpdate()
    {
        if (jugador != null)
        {
            // Mantén la cámara centrada en el jugador (ajusta el valor de Z si es necesario)
            transform.position = new Vector3(jugador.position.x, jugador.position.y, transform.position.z);
        }
    }
}