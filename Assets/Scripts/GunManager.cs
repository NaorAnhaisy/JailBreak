using UnityEngine;
using UnityEngine.SceneManagement;

public class GunManager : MonoBehaviour
{
    private static GunManager instance;

    public static GunManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GunManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("GunManager");
                    instance = obj.AddComponent<GunManager>();
                }
            }
            return instance;
        }
    }

    public GameObject selectedGun = null;

    private void Start()
    {
        UpdateSelectedGun(null); // Initialize the life bar color
    }

    public void UpdateSelectedGun(GameObject newSelectedGun)
    {
        this.selectedGun = newSelectedGun;
    }

    private void Awake()
    {
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
