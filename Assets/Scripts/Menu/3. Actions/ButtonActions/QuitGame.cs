using UnityEngine;

public class QuitGame : ButtonAction
{
    public override void Action()
    {
        //Player application terminates on Windows, Mac and Linux.
        //Check platform because this crashes the game if called in for example WebGL

        if (Application.platform == RuntimePlatform.LinuxPlayer ||
            Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Application.Quit();
        }

        if (Application.platform == RuntimePlatform.LinuxEditor ||
            Application.platform == RuntimePlatform.OSXEditor ||
            Application.platform == RuntimePlatform.WindowsEditor)
        {
            Debug.Log("Not quitting the game, because you're playing in the editor!");
        }
    }
}
