using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.VisualScripting;
public class QuestController : MonoBehaviour
{
    [Header("Obrigatório")]
    public GameObject menu;
    [SerializeField] GameObject canva;
    [SerializeField] TextMeshProUGUI questText;
    [SerializeField] Image retangulo;

    [Header("Configurações da animação")]
    [SerializeField] float duration;
    [SerializeField] float waitTime;

    public static QuestController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(canva);
            return;
        }

        instance = this;
        DontDestroyOnLoad(canva);
    }
    void Start()
    {
        //QuestController[] uiParaDeletar = FindObjectsByType<QuestController>(FindObjectsSortMode.None);
        //foreach(QuestController ui in uiParaDeletar)
        //{
        //    GameObject uiGameObject = GetComponentInParent<GameObject>();
        //    if (ui != this)
        //    {
        //        Destroy(uiGameObject);
        //    }
        //}
    }
    public void SetQuestText(string text)
    {
        RectTransform transform = retangulo.GetComponent<RectTransform>();
        Vector2 size = questText.GetPreferredValues(text);
        transform.sizeDelta = new Vector2(size.x + 60, transform.sizeDelta.y);
        StartCoroutine(ChangeQuest(text));
    }
    public IEnumerator ChangeQuest(string text)
    {
        float iterador = 0;
        float halfWaitTime = waitTime / 2;
        while(iterador < 1)
        {
            iterador += Time.deltaTime;
            retangulo.fillAmount = Mathf.Clamp01(iterador);
            yield return null;
        }

        yield return new WaitForSeconds(halfWaitTime);
        questText.text = text;
        yield return new WaitForSeconds(halfWaitTime);
        while (iterador > 0)
        {
            iterador -= Time.deltaTime;
            retangulo.fillAmount = Mathf.Clamp01(iterador);
            yield return null;
        }
    }
}
