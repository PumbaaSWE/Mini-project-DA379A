using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMenu : ButtonAction
{
    [SerializeField]
    Menu menu;

    public override void Action()
    {
        transform.root.GetComponent<MenuManager>().SetMenu(menu);
    }
}
