using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public GameObject characterPrefab;
    public GameObject characterWorldMapPrefab;
    public Animator animator;
    public GameObject gameManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager");
    }

    void OnMouseEnter()
    {
        animator.SetBool("selected", true);
    }

    void OnMouseExit()
    {
        animator.SetBool("selected", false);
    }

    void OnMouseDown()
    {
        gameManager.GetComponent<GameManagerScript>().player = characterPrefab;
        gameManager.GetComponent<GameManagerScript>().playerWorldMap = characterWorldMapPrefab;
    }
}
