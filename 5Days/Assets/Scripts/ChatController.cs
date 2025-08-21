using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ChatController : MonoBehaviour
{
    [SerializeField] GameObject dontDestroyObject;
    [SerializeField] GameObject chatMenu;
    [SerializeField] GameObject portraitFundo;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI dialogueText;

    [Header("Opcional")]
    [SerializeField] AudioSource talkVoice;
    [SerializeField] Button yesBTN;
    [SerializeField] Button noBTN;

    [Header("Configurações para aparecer texto de clicar E")]
    [SerializeField] GameObject pressButtonText;
    [SerializeField] float timer;
    [SerializeField] float timeToTextAppear = 5f;
    [SerializeField] bool canShowText = false;
    string line;
    BounceEffect bounceEffect;
    public Coroutine falasRoutine;
    public static ChatController instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(dontDestroyObject);
        }
        else
        {
            Destroy(dontDestroyObject);
        }
    }
    private void Start()
    {
        bounceEffect = BounceEffect.instance;
        ShowYesOrNoButtons(false);
        pressButtonText.SetActive(false);
        chatMenu.SetActive(false);

    }
    private void Update()
    {
        if(canShowText) timer += Time.deltaTime;
        else timer = 0;
        if(timer >= timeToTextAppear)
        {
            pressButtonText.SetActive(true);
            timer = 0;
            canShowText = false;
        }
    }
    public void StartDialogue(Sprite sprite, string fala)
    {
        line = fala;
        chatMenu.SetActive(true);
        timer = 0;
        canShowText = true;
        pressButtonText.SetActive(false);
        bounceEffect.isOnRoutine = false;
        //SÓ PARA MOSTRAR: posso fazer isso: portraitFundo.SetActive(sprite == null);
        if (sprite == null)
        {
            portraitFundo.SetActive(false);
        }
        else
        {
            portrait.sprite = sprite;
            portraitFundo.SetActive(true);
            //if (portrait.sprite != sprite) //se for um novo sprite
            StartCoroutine(bounceEffect?.Bounce(portrait.transform, 0.25f, 5)); //efeito de pulinho
        }

        dialogueText.text = "";
        if(falasRoutine != null)
        {
            ResetText();
            return;
        }
        else 
            falasRoutine = StartCoroutine(EscreverFalas(fala));
    }
    public void ResetText()
    {
        StopAllCoroutines();
        dialogueText.text = "";
        dialogueText.text = line;
        falasRoutine = null;
    }
    #region ChooseBTN
    public void SetYesNoFunctions(NPC npc)
    {
        yesBTN.onClick.AddListener(() => npc.ChooseQuestion(true));
        noBTN.onClick.AddListener(() => npc.ChooseQuestion(false));
    }
    public void ShowYesOrNoButtons(bool show)
    {
        yesBTN.gameObject.SetActive(show);
        noBTN.gameObject.SetActive(show);
    }
    #endregion
    public void CloseDialogue()
    {
        StopAllCoroutines();
        canShowText = false;
        pressButtonText.SetActive(false);
        chatMenu.SetActive(false);
        bounceEffect.isOnRoutine = false;
        dialogueText.text = "";
        portrait.sprite = null;
    }
    IEnumerator EscreverFalas(string fala)
    {
        char[] caracteres = fala.ToCharArray();
        for (int i = 0; i < caracteres.Length; i++)
        {
            float pitch = UnityEngine.Random.Range(1, 3);
            talkVoice.pitch = pitch;
            talkVoice?.Play();
            dialogueText.text += caracteres[i];
            yield return new WaitForSeconds(0.05f);
            talkVoice?.Stop();
        }
        falasRoutine = null;
    }
}
