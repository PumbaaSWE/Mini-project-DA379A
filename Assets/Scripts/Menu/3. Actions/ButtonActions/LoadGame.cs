using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
    [SerializeField]
    private int levelIndex = 1;

    [SerializeField]
    private GameObject loadingScreen;

    [SerializeField]
    private Slider progressBar;

    [SerializeField]
    private TextMeshProUGUI progressText;

    public void LoadLevelScene()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadAsync());
    }

    private IEnumerator LoadAsync()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelIndex);

        while(!loadOperation.isDone)
        {
            float currentProgress = Mathf.Clamp01(loadOperation.progress / 0.9f);

            progressBar.value = currentProgress;

            progressText.text = currentProgress * 100f + "%";

            yield return null;
        }
    }
}
