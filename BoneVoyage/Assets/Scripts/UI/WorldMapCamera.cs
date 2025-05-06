using UnityEngine;
using UnityEngine.UI;

public class WorldMapCamera : MonoBehaviour
{
    public GameObject player;
    public Image playerImageUI;
    public Sprite mageImage;
    public Sprite archerImage;
    public Sprite knightImage;
    public Sprite barbarianImage;
    public Vector3 offset;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = GetOffset();
        if (player != null)
        {
            switch (player.name)
            {
                case "MageWorldMap(Clone)":
                    playerImageUI.sprite = mageImage;
                    break;
                case "ArcherWorldMap(Clone)":
                    playerImageUI.sprite = archerImage;
                    break;
                case "KnightWorldMap(Clone)":
                    playerImageUI.sprite = knightImage;
                    break;
                case "BarbarianWorldMap(Clone)":
                    playerImageUI.sprite = barbarianImage;
                    break;
            }
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.transform.position + offset;
            transform.LookAt(player.transform);
        }
    }

    public Vector3 GetOffset()
    {
        return transform.position - player.transform.position;
    }
}