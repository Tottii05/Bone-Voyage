using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPickerBehaviour : MonoBehaviour
{
    public Canvas canvas;

    public void Start()
    {
        canvas.gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canvas.gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canvas.gameObject.SetActive(false);
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }
}
