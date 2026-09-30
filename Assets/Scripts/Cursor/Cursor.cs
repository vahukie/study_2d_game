using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorNormal;
    [SerializeField] private Texture2D cursorShoot;
    [SerializeField] private Texture2D cursorReload;

    private Vector2 hotSpot = new Vector2(16, 48);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.SetCursor(cursorNormal, hotSpot, CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorShoot, hotSpot, CursorMode.Auto);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorNormal, hotSpot, CursorMode.Auto);
        }
        if(Input.GetMouseButtonDown(1))
        {
            Cursor.SetCursor(cursorReload, hotSpot, CursorMode.Auto);
        }
        else if(Input.GetMouseButtonUp(1))
        {
            Cursor.SetCursor(cursorNormal, hotSpot, CursorMode.Auto);
        }
    }

}
