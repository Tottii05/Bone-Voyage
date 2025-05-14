using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Vector2 clickPosition = Vector2.zero;

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Cursor.SetCursor(cursorTexture, clickPosition, CursorMode.Auto);
    }
}
