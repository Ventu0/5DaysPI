using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QuestController : MonoBehaviour
{
    [SerializeField] GameObject canva;
    [SerializeField] TextMeshProUGUI questText;
    [SerializeField] Image retangulo;
    [SerializeField] float padding;
    public static QuestController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(canva);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
    }

    [ContextMenu("SeQuestText")]
    public void SetQuestText(string text)
    {
        if (questText != null)
        {
            questText.text = text;
            float textWidth = questText.preferredWidth;
            RectTransform rectTransform = retangulo.GetComponent<RectTransform>();
            Vector2 size = questText.GetPreferredValues();
            rectTransform.sizeDelta = size;
        }
    }
}
