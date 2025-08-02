using UnityEngine;

public class DeadController : MonoBehaviour
{
    public Animator animPequeño;
    public Animator animMediano;
    public Animator animViejo;

    public GameObject bloquePequeñoPrefab;
    public GameObject bloqueMedianoPrefab;
    public GameObject bloqueGrandePrefab;


    // Etapa 1: Pequeño
    public void MatarPequeño(Vector3 posicion)
    {
        Debug.Log("Animacion de muerte del pequeño...");
        //animPequeño.SetTrigger("Muerte");
        // Instanciar bloque empujable en la posición del personaje
        GameObject bloque = Instantiate(bloquePequeñoPrefab, posicion, Quaternion.identity);
        // Añadir componente para empujar solo por el personaje mediano
        bloque.AddComponent<BloqueEmpujableMediano>();
        // Desactivar el personaje pequeño
        gameObject.SetActive(false);
    }

    // Etapa 2: Mediano
    public void MatarMediano(Vector3 posicion)
    {
        Debug.Log("Animacion de muerte del mediano...");
        //animMediano.SetTrigger("Muerte");
        // Instanciar bloque inamovible en la posición del personaje
        GameObject bloque = Instantiate(bloqueMedianoPrefab, posicion, Quaternion.identity);
        // Añadir componente para que sea inamovible
        //bloque.AddComponent<BloqueInamovible>();
        // Desactivar el personaje mediano
        gameObject.SetActive(false);
    }

    // Etapa 3: Grande
    public void MatarGrande(Vector3 posicion)
    {
        Debug.Log("Animacion de muerte del viejo...");
        //animViejo.SetTrigger("Muerte");
        // Instanciar bloque inamovible especial en la posición del personaje
        GameObject bloque = Instantiate(bloqueGrandePrefab, posicion, Quaternion.identity);
        // Añadir componente para que desaparezca al tocarlo
        bloque.AddComponent<BloqueDesapareceAlTocar>();
        // Desactivar el personaje grande
        gameObject.SetActive(false);
    }
}


public class BloqueInamovible : MonoBehaviour
{
    // Aquí iría la lógica para que el bloque no se mueva
}

public class BloqueDesapareceAlTocar : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Tocado");
        // Si el personaje lo toca, desaparece
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Mediano"))
        {
            Destroy(gameObject);
        }
    }
}
