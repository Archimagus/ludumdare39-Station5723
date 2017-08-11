using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
	public string levelToLoad;
	public GameObject loadingPanel;

	void Start () {
        loadingPanel.GetComponent<Image>().CrossFadeAlpha(0, 0, false);
		loadingPanel.SetActive(false);
	}

    public void LoadScene(string sceneToLoad)
    {
        levelToLoad = sceneToLoad;

        StartCoroutine(DisplayLoadingScreen(levelToLoad));
    }

	public IEnumerator DisplayLoadingScreen(string level) {
		loadingPanel.SetActive(true);

        AsyncOperation async = SceneManager.LoadSceneAsync(level);

		while(!async.isDone)
        {			
			yield return null;
		}

        loadingPanel.SetActive(false);
    }


}
