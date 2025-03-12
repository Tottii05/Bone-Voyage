using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public GameObject glowObject; // Asigna la malla duplicada en el Inspector

    void Start()
    {
        glowObject.SetActive(false); // Ocultar el glow al inicio
    }

    void OnMouseEnter()
    {
        glowObject.SetActive(true); // Mostrar el glow al pasar el mouse
    }

    void OnMouseExit()
    {
        glowObject.SetActive(false); // Ocultar el glow al salir el mouse
    }
}
