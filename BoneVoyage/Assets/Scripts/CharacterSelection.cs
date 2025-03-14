using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public GameObject characterPrefab;
    public GameObject characterWorldMapPrefab;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnMouseEnter()
    {
        animator.SetBool("selected", true);
    }

    void OnMouseExit()
    {
        animator.SetBool("selected", false);
    }
}
