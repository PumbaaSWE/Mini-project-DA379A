using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    MenuElement[] elements;
    MenuSelectable selected;
    
    MenuSelectable[] selectables
    {
        get
        {
            var list = new List<MenuSelectable>();
            foreach (var element in elements)
                if (element is MenuSelectable)
                    list.Add((MenuSelectable)element);

            return list.ToArray();
        }
    }
    
    public void Enable()
    {
        gameObject.SetActive(true);

        if (elements is null)
        {
            elements = GetComponentsInChildren<MenuElement>();

            foreach (var element in elements)
                element.Initiate();
        }

        //if (selectables.Length > 0)
        //    Select(selectables[0]);
    }

    public void Disable()
    {
        if (selected)
            selected.Deselect();

        selected = null;

        gameObject.SetActive(false);
    }

    public void Select(MenuSelectable selectable)
    {
        if (selectable == selected)
            return;

        if (selected)
            selected.Deselect();

        selected = selectable;

        if (selected)
            selected.Select();
    }

    public void Navigate(Vector2 direction)
    {
        if (selected)
        {
            MenuSelectable selectable = selected.Navigate(direction);

            if (selectable)
                Select(selectable);
        }
        else
        {
            if (selectables.Length > 0)
                Select(selectables[0]);
        }
    }

    public void Confirm(Vector3 mousePos)
    {
        if (selected)
            selected.Confirm(mousePos);
    }
}
