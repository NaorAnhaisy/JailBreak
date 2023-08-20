using UnityEngine;

public class GunManager : MonoBehaviour
{
    // Static reference to the instance
    private static GunManager _instance;

    // Public property to access the instance
    public static GunManager Instance
    {
        get
        {
            // If the instance doesn't exist, try to find it in the scene
            if (_instance == null)
            {
                _instance = FindObjectOfType<GunManager>();

                // If it's still null, create a new GameObject to host the Singleton script
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GunManager");
                    _instance = singletonObject.AddComponent<GunManager>();
                }
            }
            return _instance;
        }
        set { _instance = value; }
    }

    // The gameObject that belongs to the Singleton instance
    private GameObject gunGameObject;

    // Public property to access and modify the gameObject
    public GameObject GunGameObject
    {
        get { return gunGameObject; }
        set { gunGameObject = value; }
    }

    // Ensure the Singleton instance isn't destroyed on scene changes
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Example method to use the Singleton instance
    public void DoSomethingWithGameObject()
    {
        if (gunGameObject != null)
        {
            // Perform actions on myGameObject
            Debug.Log("Doing something with the gameObject!");
        }
        else
        {
            Debug.Log("No gameObject assigned.");
        }
    }
}
