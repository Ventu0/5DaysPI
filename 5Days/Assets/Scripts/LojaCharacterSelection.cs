using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
public class LojaCharacterSelection : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField] LojaCharacterInstance[] characters;
    [SerializeField] PersonagensNaLoja[] charactersInfo;

    [Header("Config")]
    [SerializeField] GameObject selectionMenu;
    [SerializeField] GameObject buyMenu;
    [SerializeField] RectTransform[] fixedPositions;
    [SerializeField] Color[] buttonColors;

    [Header("Read-Only")]
    [SerializeField] LojaAnimSequence animSequence;
    [SerializeField] int selected;
    [SerializeField] int lastSelected;
    [SerializeField] bool canRecieveInput = true;
    public bool canMove = true;
    public static LojaCharacterSelection instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        SetActive(false);
        animSequence = GetComponent<LojaAnimSequence>();
        lastSelected = selected;
        SelectedButtonSetOnClick(selected, true); //adicionar um primeiro botao pra nao ficar estranho e bugado

        TrocarCoresDeTodos();
        characters[0].Comprado(false); //garante que o primeiro personagem ja comece comprado
    }
    public void Setup(List<bool> unlocked)
    {
        bool isNull = unlocked == null;
        print(isNull);
        for (int i = 0; i < charactersInfo.Length; i++)
        {
            print(i);
            characters[i].Setup(charactersInfo[i], isNull ? false : unlocked[i]); //se nao tiver pre salvado, colocar como false
        }
    }
    public void SetActive(bool active)
    {
        selectionMenu.SetActive(active);
        canMove = active;

        Player.instance.SetGamePauseManual(active);
    }
    void Update()
    {
        if (!canMove)
            return;

        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Horizontal"))
        {
            MexerTodos(horizontal);
        }
    }

    #region Movimento do menu
    void MexerTodos(int sentido)
    {
        ChecarSePodeReceberInput();
        if (!canRecieveInput)
            return;
        canRecieveInput = false;

        if (0 >= selected && sentido == -1 || selected == characters.Length - 1 && sentido == 1)
        {
            print("Chegou no limite");
            return;
        }

        lastSelected = selected;
        SelectedButtonSetOnClick(lastSelected, false);

        selected = Mathf.Clamp(selected + sentido, 0, characters.Length - 1);

        SelectedButtonSetOnClick(selected, true); //adiciona os listeners pro novo selecionado

        sentido = -sentido; //inverte o sentido, pois os personagems se mexem na direcao contraria ao input
        for (int i = 0; i < characters.Length; i++)
        {
            int interval = ChecarSeONumeroEstaNoIntervalo(i);
            interval = Mathf.Clamp(interval, 0, fixedPositions.Length - 1);

            LojaCharacterInstance character = characters[i];
            RectTransform characterTransform = character.rect;

            bool isOnInterval = interval > 2 ? false : true;
            RectTransform fixedPos = fixedPositions[interval];

            TrocarCoresDeTodos();
            int sentidoInstance = ChecarSePrecisaInverter(sentido, characterTransform.anchoredPosition.x);
            character.Move(fixedPos.anchoredPosition * sentidoInstance, fixedPos.localScale, 0.3f, isOnInterval);
        }
    }
    void TrocarCoresDeTodos()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            int interval = ChecarSeONumeroEstaNoIntervalo(i);
            interval = Mathf.Clamp(interval, 0, fixedPositions.Length - 1);

            characters[i].TrocarCor(i == selected, ButtonColor(interval));
        }
    }
    void ChecarSePodeReceberInput()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (!characters[i].gameObject.activeSelf) continue;
            if (characters[i].HasEndedCoroutine() != null)
            {
                canRecieveInput = false;
                return;
            }
        }
        canRecieveInput = true;
    }
    #endregion

    #region OnClick
    void FadeToAlphaDisabled(bool invert = false) //faz o fade out do personagem que nao esta selecionado
    {
        for (int i = 0; i < characters.Length; i++)
        {
            bool isSelected = i == selected;
            if (isSelected) continue; //pula o que ta selecionado, pra otimizar e nao deixar transparente

            characters[i].StartChangeAlpha(animSequence, 0.25f, invert);
        }
        LojaShowCharacterInfo info = LojaShowCharacterInfo.instance;

        buyMenu.SetActive(true);
        StartCoroutine(animSequence.FadeAlpha(a => info.ChangeAlpha(a), 0.35f, b => buyMenu.SetActive(b), !invert));
    }
    void SelectedButtonSetOnClick(int whichCharacter, bool addListener)
    {
        whichCharacter = Mathf.Clamp(whichCharacter, 0, characters.Length - 1);

        LojaCharacterInstance character = characters[whichCharacter];
        UnityEvent onClick = character.button.onClick;
        if (!addListener)
        {
            animSequence.onAnimEnd.RemoveAllListeners();
            onClick.RemoveAllListeners(); //remove todos os listeners pra evitar bugs
            return;
        }

        UnityAction sequence = () => animSequence.StartSequence(character.rect, false);
        Action<UnityAction> action = addListener ? onClick.AddListener : onClick.RemoveListener; //decide o que vai fazer (remover ou adicionar)
        Action<UnityAction> onAnimEnd = addListener ? animSequence.onAnimEnd.AddListener : animSequence.onAnimEnd.RemoveListener;

        print("colocando actions");
        action(() => character.button.enabled = false);
        action(() => canMove = false);
        action(() => LojaShowCharacterInfo.instance.ApplyInfo(character.personagem));
        action(() => FadeToAlphaDisabled());
        onAnimEnd(() => character.button.enabled = true);
        onAnimEnd(() => print(character.button.enabled));
        action(sequence);
        action(OnSelected);
    }
    void OnSelected()
    {
        UnityEvent onClick = characters[selected].button.onClick;
        print("selected");
        onClick.RemoveAllListeners();
        onClick.AddListener(DeselectCurrent);
    }
    public void DeselectCurrent()
    {
        LojaCharacterInstance character = characters[selected];

        character.button.onClick.RemoveAllListeners();
        character.button.enabled = false;

        ReturnToSelection();
        FadeToAlphaDisabled(true);
    }
    void ReturnToSelection()
    {
        animSequence.onAnimEnd.AddListener(() => canMove = true);
        animSequence.onAnimEnd.AddListener(() => characters[selected].button.enabled = true);
        animSequence.afterAnimReset.AddListener(() => SelectedButtonSetOnClick(selected, true));

        RectTransform rect = characters[selected].rect;

        animSequence.StartSequence(rect, true);
    }
    #endregion
    #region Functions With Return
    int ChecarSePrecisaInverter(int sentido, float positionEmRelacao0)
    {
        float tolerancia = 0.5f; //faco isso porque o float pode não ser exatamente 0, então coloco esse nivel de tolerancia
        //porque no modo janela tava dando certo, e quando fui pro fullscreen tava dando coordenadas levemente erradas, fazendo dar erro

        if (positionEmRelacao0 > tolerancia)
            return 1;
        else if (positionEmRelacao0 < -tolerancia)
            return -1;
        else
        {
            return sentido == -1 ? -1 : 1;
        }
    }
    Color ButtonColor(int distanceInInterval)
    {
        distanceInInterval = Mathf.Clamp(distanceInInterval, 0, buttonColors.Length - 1);
        return buttonColors[distanceInInterval];
    }
    int ChecarSeONumeroEstaNoIntervalo(int i)
    {
        return Mathf.Abs(i - selected);
    }
    public LojaCharacterInstance CurrentSelected()
    {
        return characters[selected];
    }
    public List<bool> CheckWhichAreUnlocked()
    {
        List<bool> unlocked = new List<bool>();
        for(int i = 0; i < characters.Length; i++)
        {
            unlocked.Add(characters[i].unlocked);
        }
        return unlocked;
    }
    #endregion
}
