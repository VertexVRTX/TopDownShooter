using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCrosshair;
    public Texture2D enemyCrosshair;

    private Vector2 hotSpot;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        hotSpot = new Vector2(defaultCrosshair.width / 2f, defaultCrosshair.height / 2f);

        Cursor.SetCursor(defaultCrosshair, hotSpot, CursorMode.Auto);
    }

    void Update()
    {
        if (Time.timeScale == 0f || EventSystem.current.IsPointerOverGameObject())
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Cursor.SetCursor(enemyCrosshair, hotSpot, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(defaultCrosshair, hotSpot, CursorMode.Auto);
            }
        }
        else
        {
            Cursor.SetCursor(defaultCrosshair, hotSpot, CursorMode.Auto);
        }
    }
}
