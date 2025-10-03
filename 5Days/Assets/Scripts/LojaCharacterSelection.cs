using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LojaCharacterSelection : MonoBehaviour
{
    [SerializeField] LojaCharacterInstance[] characters;
    [SerializeField] PersonagensNaLoja[] charactersInfo;
    [SerializeField] RectTransform[] fixedPositions;
    [SerializeField] Color[] buttonColors;
    [SerializeField] int selected;
    [SerializeField] bool canRecieveInput = true;
    public bool canMove = true;
    void Start()
    {
        LojaCharacterInstance character = characters[selected];
        character.button.onClick.AddListener(FadeToAlphaDisabled);
        TrocarCoresDeTodos();
        Setup();
    }
    void Setup()
    {
        for(int i = 0; i < charactersInfo.Length; i++)
        {
            characters[i].Setup(charactersInfo[i]);
        }
    }
    void Update()
    {
        if(!canMove)
            return;

        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Horizontal"))
        {  
            MexerTodos(horizontal);
        }
    }
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
        selected = Mathf.Clamp(selected + sentido, 0, characters.Length - 1);
        characters[selected].button.onClick.AddListener(FadeToAlphaDisabled);

        sentido = -sentido; //inverte o sentido, pois os personagems se mexem na direcao contraria ao input
        for (int i = 0; i < characters.Length; i++)
        {
            int interval = ChecarSeONumeroEstaNoIntervalo(i);
            interval = Mathf.Clamp(interval, 0, fixedPositions.Length - 1);

            LojaCharacterInstance character = characters[i];
          
            bool isOnInterval = interval > 2 ? false : true;
            RectTransform fixedPos = fixedPositions[interval];
           
            RectTransform characterTransform = character.GetComponent<RectTransform>();
            TrocarCoresDeTodos();
            int sentidoInstance = ChecarSePrecisaInverter(sentido, characterTransform.anchoredPosition.x);
            character.Move(fixedPos.anchoredPosition * sentidoInstance, fixedPos.localScale, 0.3f, isOnInterval);
        }
    }
    void TrocarCoresDeTodos()
    {
        for(int i = 0; i < characters.Length; i++)
        {
            int interval = ChecarSeONumeroEstaNoIntervalo(i);
            interval = Mathf.Clamp(interval, 0, fixedPositions.Length - 1);

            characters[i].TrocarCor(i == selected, ButtonColor(interval));
        }
    }
    void ChecarSePodeReceberInput()
    {
        for(int i = 0; i < characters.Length; i++)
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
    void FadeToAlphaDisabled() //faz o fade out do personagem que nao esta selecionado
    {
        print("clicado");
        for (int i = 0; i < characters.Length; i++)
        {
            bool isSelected = i == selected;
            if (isSelected) continue; //pula o que ta selecionado, pra otimizar e nao deixar transparente
            StartCoroutine(characters[i].FadeAlpha(0.1f));
        }
        canMove = false;
    }
    #region Functions With Return
    int ChecarSePrecisaInverter(int sentido, float positionEmRelacao0)
    {
        int positionIn0 = 0;

        if (positionEmRelacao0 > 0)
            return 1;
        else if (positionEmRelacao0 < 0)
            return -1;
        else
            return positionIn0 = sentido == -1 ? -1 : 1;
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
    #endregion
}
