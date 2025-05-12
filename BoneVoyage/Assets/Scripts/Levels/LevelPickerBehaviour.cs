using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Player;

public class LevelPickerBehaviour : MonoBehaviour, IInteractActions
{
    public GameObject canvas;
    private Player playerActions;
    private bool playerInTrigger = false;
    public string sceneName;
    public bool canBeShown;

    private void Awake()
    {
        playerActions = new Player();
        playerActions.Interact.SetCallbacks(this);
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }

    public void Start()
    {
        canvas.gameObject.SetActive(false);             
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canBeShown)
        {
            canvas.gameObject.SetActive(true);
            playerInTrigger = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canvas.gameObject.SetActive(false);
            playerInTrigger = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && playerInTrigger)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}