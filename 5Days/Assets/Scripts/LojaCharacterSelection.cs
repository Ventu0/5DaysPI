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
    [SerializeField] bool canMoveAll = true;
    void Start()
    {
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
        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Horizontal"))
        {  
            MexerTodos(horizontal);
        }
    }
    void MexerTodos(int sentido)
    {
        ChecarSePodeReceberInput();
        if (!canMoveAll)
            return;

        if (0 >= selected && sentido == -1 || selected == characters.Length - 1 && sentido == 1)
        {
            print("nao mexe");
            return;
        }

        canMoveAll = false;
        selected = Mathf.Clamp(selected + sentido, 0, characters.Length - 1);
        sentido = -sentido;
        for (int i = 0; i < characters.Length; i++)
        {
           int interval = ChecarSeONumeroEstaNoIntervalo(i);
           interval = Mathf.Clamp(interval, 0, fixedPositions.Length - 1);
           LojaCharacterInstance character = characters[i];
          
           bool isOnInterval = interval > 2 ? false : true;
           
           RectTransform fixedPos = fixedPositions[interval];
           RectTransform characterTransform = character.GetComponent<RectTransform>();

            int sentidoInstance = ChecarSePrecisaInverter(sentido, characterTransform.anchoredPosition.x);
            character.TrocarCor(i == selected, ButtonColor(interval));
            character.Move(fixedPos.anchoredPosition * sentidoInstance, fixedPos.localScale, 0.1f, isOnInterval);
        }
    }
    void ChecarSePodeReceberInput()
    {
        for(int i = 0; i < characters.Length; i++)
        {
            if (!characters[i].gameObject.activeSelf) continue;
            if (characters[i].HasEndedCoroutine() != null)
            {
                canMoveAll = false;
                return;
            }
        }
        canMoveAll = true;
    }
    int ChecarSePrecisaInverter(int sentido, float positionEmRelacao0)
    {
        if(sentido == -1)
        {
            if (positionEmRelacao0 > 0)
                return 1;
            else if (positionEmRelacao0 < 0)
                return -1;
            else
                return -1;
        }
        else
        {
            if (positionEmRelacao0 > 0)
                return 1;
            else if (positionEmRelacao0 < 0)
                return -1;
            else
                return 1;
        }
    }
    Color ButtonColor(int distanceInInterval)
    {
        print("Distance: " + distanceInInterval);
        distanceInInterval = Mathf.Clamp(distanceInInterval, 0, buttonColors.Length - 1);
        return buttonColors[distanceInInterval];
    }
    int ChecarSeONumeroEstaNoIntervalo(int i)
    {
        return Mathf.Abs(i - selected);
    }
}
