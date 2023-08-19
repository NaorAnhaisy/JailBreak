using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMissionScript : MonoBehaviour
{
    public Button start;
    public int nextSceneNumber;

    // Start is called before the first frame update
    void Start()
    {
        start.enabled = true;
    }

    public void PlayLevel()
    {
        LifeBarManager.Instance.gameObject.SetActive(false); // Temporarily deactivate the LifeBarManager
        if (!(nextSceneNumber == 0 || nextSceneNumber == 2 || nextSceneNumber == 4 || nextSceneNumber == 6))
        {
            LifeBarManager.Instance.gameObject.SetActive(true);
        }
        SceneManager.LoadScene(nextSceneNumber);
    }

    public void LoadNextScene()
    {
        // Instead of directly loading the next scene, use this method
        // to handle scene transitions while maintaining the LifeBarManager instance
        LifeBarManager.Instance.gameObject.SetActive(false); // Temporarily deactivate the LifeBarManager
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
        SceneManager.LoadScene(nextSceneNumber);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!(scene.buildIndex == 0 || scene.buildIndex == 2 || scene.buildIndex == 4 || scene.buildIndex == 6))
        {
            LifeBarManager.Instance.gameObject.SetActive(true);
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
