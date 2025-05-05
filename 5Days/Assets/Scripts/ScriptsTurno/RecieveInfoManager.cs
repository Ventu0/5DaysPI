using UnityEngine;
using System.Collections.Generic;
using System.Collections;
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
        turnModeManager.aliadosPersonagens.Clear();
    }
    public void SetupCharacters(List<CharacterStatusGeneric> playerStatus, List<CharacterStatusGeneric> enemiesStatus)
    {
        StartCoroutine(Setup(playerStatus, enemiesStatus));
    }
    IEnumerator Setup(List<CharacterStatusGeneric> playerStatus, List<CharacterStatusGeneric> enemiesStatus)
    {
        TurnModeManager turnModeManager = TurnModeManager.instance;

        for (int i = 0; i < playerStatus.Count; i++)
        {
            turnModeManager.aliados.Add(aliadosOriginais[i]);
            turnModeManager.aliadosPersonagens.Add(aliadosOriginais[i].GetComponent<BasePersonagem>());

            turnModeManager.aliadosPersonagens[i].characterStatus = playerStatus[i];
            turnModeManager.aliadosPersonagens[i].SetupStatus();

            turnModeManager.aliados[i].gameObject.SetActive(true);
        }
        SpawnEnemies.instance.Spawn(enemiesStatus.Count, enemiesStatus);
        yield return new WaitForSeconds(0.1f);
        TurnModeManager.instance.FirstAllyAttack();
    }
}