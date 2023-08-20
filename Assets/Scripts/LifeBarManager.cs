using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LifeBarManager : MonoBehaviour
{
    private static LifeBarManager instance;

    public static LifeBarManager Instance
    {
        get
        {
            Debug.Log("Here!!");
            if (instance == null)
            {
                instance = FindObjectOfType<LifeBarManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("LifeBarManager");
                    instance = obj.AddComponent<LifeBarManager>();
                }
            }
            return instance;
        }
    }

    public Image fillImage;
    public Text lifeText;

    public int life = 100;
    public Color fullColor = Color.green;
    public Color emptyColor = Color.red;

    private void Start()
    {
        Debug.Log("ehre!!");
        UpdateLife(0); // Initialize the life bar color
    }

    public void UpdateLife(int amount)
    {
        life += amount;
        life = Mathf.Clamp(life, 0, 100);
        fillImage.fillAmount = life / 100f;
        lifeText.text = life.ToString() + "/100";

        // Change the color based on remaining life
        fillImage.color = Color.Lerp(emptyColor, fullColor, life / 100f);

        if (life == 0)
        {
            // Unlock cursor
            Cursor.lockState = CursorLockMode.None;

            // hide life bar
            LifeBarManager.Instance.gameObject.SetActive(false);

            // Save the current scene number
            int currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt("LastFailedScene", currentSceneNumber);

            // Load the failed scene
            SceneManager.LoadScene(7);
        }
    }

    public void ResetLife()
    {
        life = 0;
        this.UpdateLife(100);
    }

    private void Awake()
    {
        // Get the current scene's build index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (!(currentSceneIndex == 2 || currentSceneIndex == 4 || currentSceneIndex == 6))
        {
            // Deactivate the canvas if the scene is not 0, 2, 4, or 6
            gameObject.SetActive(false);
            return;
        }

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
