using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : ButtonAction
{
    [SerializeField]
    private int levelIndex;

    public override void Action()
    {
        SceneManager.LoadScene(levelIndex);
    }
}
