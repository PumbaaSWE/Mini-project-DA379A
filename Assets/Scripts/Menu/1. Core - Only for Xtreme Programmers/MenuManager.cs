using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    GraphicRaycaster graphicRaycaster;
    PointerEventData pointerEventData;
    List<RaycastResult> raycastResults;

    [SerializeField]
    Menu currentMenu;

    private void Awake()
    {
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        pointerEventData = new PointerEventData(EventSystem.current);
        raycastResults = new List<RaycastResult>();

        if (!currentMenu)
        {
            var menus = GetComponentsInChildren<Menu>();

            if (menus.Length > 0)
                SetMenu(menus[0]);
            else
                Debug.LogError("MenuManager can't find any menus");
        }
    }

    private void Update()
    {
        var mouseX = Input.GetAxisRaw("Mouse X");
        var mouseY = Input.GetAxisRaw("Mouse Y");

        if (/*mouseX + mouseY != 0f*/true)
        {
            var foundSelect = false;
            pointerEventData.position = Input.mousePosition;
            raycastResults.Clear();
            graphicRaycaster.Raycast(pointerEventData, raycastResults);

            foreach (var result in raycastResults)
            {
                var t = result.gameObject.transform;
                while (t != null)
                {
                    if (t.TryGetComponent(out MenuSelectable selectable))
                    {
                        foundSelect = true;
                        Select(selectable);
                        break;
                    }

                    t = t.transform.parent;
                }
            }

            if (!foundSelect)
                Select(null);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            Navigate(Vector2.left);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            Navigate(Vector2.right);
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            Navigate(Vector2.up);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            Navigate(Vector2.down);

        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            Confirm(Input.mousePosition);
    }

    public void Select(MenuSelectable selectable)
    {
        currentMenu.Select(selectable);
    }

    public void Navigate(Vector2 direction)
    {
        currentMenu.Navigate(direction);
    }

    public void Confirm(Vector3 mousePos)
    {
        currentMenu.Confirm(mousePos);
    }

    public void SetMenu(Menu menu)
    {
        if (currentMenu)
            currentMenu.Disable();

        currentMenu = menu;
        currentMenu.Enable();
    }
}
