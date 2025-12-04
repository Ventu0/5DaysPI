using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class QuestController : MonoBehaviour
{
    [Header("Obrigatório")]
    public GameObject menu;
    [SerializeField] GameObject canva;
    [SerializeField] TextMeshProUGUI questText;
    [SerializeField] Image retangulo;
    [SerializeField] TextMeshProUGUI closedText;

    [Header("Configurações da animação")]
    [SerializeField] float duration;
    [SerializeField] float waitTime;

    bool isOnRoutine = false;
    bool canActive = true;
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

    }
    public void SetAllActive(bool allActive)
    {
        canActive = allActive;
        menu.SetActive(allActive ? !allActive : allActive);
        closedText.gameObject.SetActive(allActive);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && canActive)
        {
            SetActive(menu.activeSelf);
        }
    }
    void SetActive(bool active)
    {
        menu.SetActive(!active);
        closedText.gameObject.SetActive(active);
    }
    #region OpçõesDeSetQuest
    public string GetActiveQuest()
    {
        return questText.text;
    }
    public void JustSetQuest(string text)
    {
        print("setando text");
        questText.text = text;
    }
    #endregion
    public void SetQuestWithAnimation(string text)
    {
        RectTransform transform = retangulo.GetComponent<RectTransform>();
        Vector2 size = questText.GetPreferredValues(questText.text);
        transform.sizeDelta = new Vector2(size.x + 10, transform.sizeDelta.y);

        if (isOnRoutine) return;
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
        isOnRoutine = true;
        SetQuestWithAnimation(text); // Atualiza o tamanho do retângulo com o novo texto
        yield return new WaitForSeconds(halfWaitTime);
        while (iterador > 0)
        {
            iterador -= Time.deltaTime;
            retangulo.fillAmount = Mathf.Clamp01(iterador);
            yield return null;
        }
        isOnRoutine = false;
    }
}
