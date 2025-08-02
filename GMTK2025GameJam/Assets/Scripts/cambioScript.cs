using UnityEngine;

public class cambioScript : MonoBehaviour
{
    public GameObject personajePequeño;
    public GameObject personajeMediano;
    public GameObject personajeGrande;

    public GameObject startPosition;

    public DeadController deadController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Al iniciar, solo el pequeño está activo
        personajePequeño.SetActive(true);
        personajeMediano.SetActive(false);
        personajeGrande.SetActive(false);

        // Todos los personajes en la posición del player
        personajePequeño.transform.position = startPosition.transform.position;
        personajeMediano.transform.position = startPosition.transform.position;
        personajeGrande.transform.position = startPosition.transform.position;
    }

    void SetPersonajeActivo(GameObject activo)
    {
        personajePequeño.SetActive(activo == personajePequeño);
        personajeMediano.SetActive(activo == personajeMediano);
        personajeGrande.SetActive(activo == personajeGrande);

        // Activar solo el script de movimiento del personaje activo
        personajePequeño.GetComponent<JugadorMovimientoConMejoras>().enabled = (activo == personajePequeño);
        personajeMediano.GetComponent<JugadorMovimientoConMejoras>().enabled = (activo == personajeMediano);
        personajeGrande.GetComponent<JugadorMovimientoConMejoras>().enabled = (activo == personajeGrande);
    }

    // Update is called once per frame
    void Update()
    {
        

        // Mantener todos los personajes en la misma posición que el personaje activo
        Vector3 posicionActual = Vector3.zero;
        if (personajePequeño.activeSelf)
            posicionActual = personajePequeño.transform.position;
        else if (personajeMediano.activeSelf)
            posicionActual = personajeMediano.transform.position;
        else if (personajeGrande.activeSelf)
            posicionActual = personajeGrande.transform.position;

        personajePequeño.transform.position = posicionActual;
        personajeMediano.transform.position = posicionActual;
        personajeGrande.transform.position = posicionActual;

        // Debug: Cambiar personaje con teclas 1, 2, 3
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetPersonajeActivo(personajePequeño);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SetPersonajeActivo(personajeMediano);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SetPersonajeActivo(personajeGrande);


            // Muerte por botón
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (personajePequeño.activeSelf)
            {
                deadController.MatarPequeño(posicionActual);
            }
            else if (personajeMediano.activeSelf)
            {
                deadController.MatarMediano(posicionActual);
            }
            else if (personajeGrande.activeSelf)
            {
                deadController.MatarGrande(posicionActual);
            }

            // Siempre reaparece el pequeño en el startPosition
            personajePequeño.transform.position = startPosition.transform.position;
            SetPersonajeActivo(personajePequeño);
        }
    }
}
