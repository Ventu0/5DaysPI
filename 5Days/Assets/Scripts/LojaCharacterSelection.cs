using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LojaCharacterSelection : MonoBehaviour
{
    [SerializeField] LojaCharacterInstance[] characters;
    [SerializeField] PersonagensNaLoja[] charactersInfo;
    [SerializeField] RectTransform[] fixedPositions;
    [SerializeField] int selected;

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
        if(0 >= selected && sentido == -1 || selected == characters.Length - 1 && sentido == 1)
        {
            print("nao mexe");
            return;
        }

        selected = Mathf.Clamp(selected + sentido, 0, characters.Length - 1);
        sentido = -sentido;
        for (int i = 0; i < characters.Length; i++)
        {
           int interval = ChecarSeONumeroEstaNoIntervalo(i);
           interval = Mathf.Clamp(interval, 0, fixedPositions.Length - 1);
           LojaCharacterInstance character = characters[i];
          
           bool isOnInterval = interval > 2 ? false : true;
           character.gameObject.SetActive(isOnInterval);
           print("eu: " + characters[i].name + ", tenho o numero: " + ChecarSeONumeroEstaNoIntervalo(i));

           
           RectTransform fixedPos = fixedPositions[interval];
           RectTransform characterTransform = character.GetComponent<RectTransform>();

            int sentidoInstance = ChecarSePrecisaInverter(sentido, characterTransform.anchoredPosition.x);
           

                //int invertXIfPositive = 1;
                //if(sentido == -1 && characterTransform.anchoredPosition.x > 0)
                //     invertXIfPositive = -1;
                // else if(sentido == -1 && characterTransform.anchoredPosition.x < 0)
                //     invertXIfPositive = 1;

            StartCoroutine(character.Mexer(fixedPos.anchoredPosition * sentidoInstance, fixedPos.localScale, 0.2f));
        }
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
                return -1;
            else if (positionEmRelacao0 < 0)
                return 1;
            else
                return -1;
        }
    }
    int ChecarSeONumeroEstaNoIntervalo(int i)
    {
        return Mathf.Abs(i - selected);
    }
}
