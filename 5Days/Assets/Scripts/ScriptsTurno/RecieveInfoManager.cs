using UnityEngine;
using System.Collections.Generic;
public class RecieveInfoManager : MonoBehaviour
{
    public delegate void OnStartBattle();
    public OnStartBattle onStartBattle;
    public static RecieveInfoManager instance;
    private void Awake()
    {
        TurnModeManager turnModeManager = TurnModeManager.instance;
        if (instance == null) instance = this;

        for (int i = 0; i < turnModeManager.aliados.Count; i++)
        {
            turnModeManager.aliados[i].gameObject.SetActive(false);
        }

    }
    private void Start()
    {
        TurnModeManager turnModeManager = TurnModeManager.instance;
        print("RecieveInfoManager started");
        //for (int i = 0; i < TurnModeInfo.alliesToLoad.Count; i++)
        //{
        //    print("tem aliado");
        //    turnModeManager.aliados[i].gameObject.SetActive(true);
        //    turnModeManager.aliadosPersonagens[i].characterStatus = TurnModeInfo.alliesToLoad[i];
        //}

        //SpawnEnemies.instance.Spawn(TurnModeInfo.enemiesToLoad.Count);

        //for (int i = 0; i < TurnModeInfo.enemiesToLoad.Count; i++)
        //{
        //    turnModeManager.inimigosPersonagens[i].characterStatus = TurnModeInfo.enemiesToLoad[i];
        //}
    }
    public void SetupCharacters(List<CharacterStatusGeneric> playerStatus, List<CharacterStatusGeneric> enemiesStatus)
    {
        TurnModeManager turnModeManager = TurnModeManager.instance;
        for (int i = 0; i < playerStatus.Count; i++)
        {
            turnModeManager.aliados[i].gameObject.SetActive(true);
            turnModeManager.aliadosPersonagens[i].characterStatus = playerStatus[i];
            turnModeManager.aliadosPersonagens[i].SetupStatus();
        }
        
        for(int i = 0; i < enemiesStatus.Count; i++)
        {
            print("enemiesStatus: " + enemiesStatus[i] + " Quantidade de rotaçoes: " + i + " Tamanho da Lista: " + enemiesStatus.Count);
            SpawnEnemies.instance.Spawn(enemiesStatus.Count, enemiesStatus);
            turnModeManager.inimigosPersonagens[i].characterStatus = enemiesStatus[i];
            turnModeManager.inimigosPersonagens[i].SetupStatus();

        }
    }
}
