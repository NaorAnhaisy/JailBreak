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
        SceneManager.LoadScene(nextSceneNumber);
    }
}
