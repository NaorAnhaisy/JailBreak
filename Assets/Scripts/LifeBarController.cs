using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LifeBarController : MonoBehaviour
{
    public Image fillImage;
    public Text lifeText;

    public int life = 100;
    public Color fullColor = Color.green;
    public Color emptyColor = Color.red;

    private void Start()
    {
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
            SceneManager.LoadScene(6);
        }
    }
}
