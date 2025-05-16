using System.Collections;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public GameObject characterPrefab;
    public GameObject characterWorldMapPrefab;
    public Animator animator;
    public GameObject gameManager;

    public AudioSource audioSource;
    public AudioClip selectedSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        StartCoroutine(playSound());
        gameManager.GetComponent<GameManagerScript>().player = characterPrefab;
        gameManager.GetComponent<GameManagerScript>().playerWorldMap = characterWorldMapPrefab;
    }

    public IEnumerator playSound()
    {
        audioSource.PlayOneShot(selectedSound);
        yield return new WaitForSeconds(selectedSound.length);
    }
}
