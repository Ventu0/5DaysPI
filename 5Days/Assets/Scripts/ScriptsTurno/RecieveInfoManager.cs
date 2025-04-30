using UnityEngine;
using System.Collections.Generic;
public class RecieveInfoManager : MonoBehaviour
{
    public static RecieveInfoManager instance;
    private List<Aliados> aliadosOriginais = new List<Aliados>();
    private void Awake()
    {
        TurnModeManager turnModeManager = TurnModeManager.instance;
        if (instance == null) instance = this;

        for (int i = 0; i < turnModeManager.aliados.Count; i++)
        {
            aliadosOriginais.Add(turnModeManager.aliados[i]);
            turnModeManager.aliados[i].gameObject.SetActive(false);
        }
        turnModeManager.aliados.Clear();
    }
    public void SetupCharacters(List<CharacterStatusGeneric> playerStatus, List<CharacterStatusGeneric> enemiesStatus)
    {
        TurnModeManager turnModeManager = TurnModeManager.instance;
        for (int i = 0; i < playerStatus.Count; i++)
        {
            turnModeManager.aliados.Add(aliadosOriginais[i]);
            turnModeManager.aliados[i].gameObject.SetActive(true);
            turnModeManager.aliadosPersonagens[i].characterStatus = playerStatus[i];
            turnModeManager.aliadosPersonagens[i].SetupStatus();
        }
        
        for(int i = 0; i < enemiesStatus.Count; i++)
        {
            SpawnEnemies.instance.Spawn(enemiesStatus.Count, enemiesStatus);
            turnModeManager.inimigosPersonagens[i].characterStatus = enemiesStatus[i];
            turnModeManager.inimigosPersonagens[i].SetupStatus();

        }
    }
}
