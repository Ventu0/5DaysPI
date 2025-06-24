using UnityEngine;
using TMPro;
public class MainText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mainText;
    public static MainText instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        mainText.gameObject.SetActive(false);
    }
    public void SetText(string text, Color textColor, float textDuration = 1f)
    {
        mainText.gameObject.SetActive(true);
        mainText.text = text;
        mainText.color = textColor;
        Invoke("EndText", textDuration);
    }
    void EndText()
    {
        mainText.text = "";
        mainText.gameObject.SetActive(false);
    }
}
