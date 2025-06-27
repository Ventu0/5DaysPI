using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class RecieveInfoManager : MonoBehaviour
{
    public static RecieveInfoManager instance;
    private List<Aliados> aliadosOriginais = new List<Aliados>();
    private List<EnemyAI> inimigosOriginais = new List<EnemyAI>();
    private void Awake()
    {
        TurnModeManager turnModeManager = TurnModeManager.instance;
        if (instance == null) instance = this;

        for (int i = 0; i < turnModeManager.aliados.Count; i++)
        {
            aliadosOriginais.Add(turnModeManager.aliados[i]);
            turnModeManager.aliados[i].gameObject.SetActive(false);
        }
        for(int i = 0; i < turnModeManager.inimigos.Count; i++)
        {
            inimigosOriginais.Add(turnModeManager.inimigos[i]);
            turnModeManager.inimigos[i].GetComponent<BasePersonagem>().gameObject.SetActive(false);
        }
        turnModeManager.aliados.Clear();
        turnModeManager.aliadosPersonagens.Clear();
        turnModeManager.inimigos.Clear();
        turnModeManager.inimigosPersonagens.Clear();
    }
    public void SetupCharacters(List<CharacterStatusGeneric> playerStatus, List<CharacterStatusGeneric> enemiesStatus)
    {
        StartCoroutine(Setup(playerStatus, enemiesStatus));
    }
    IEnumerator Setup(List<CharacterStatusGeneric> playerStatus, List<CharacterStatusGeneric> enemyStatus)
    {
        TurnModeManager turnModeManager = TurnModeManager.instance;
        

        for (int i = 0; i < playerStatus.Count; i++)
        {
            turnModeManager.aliados.Add(aliadosOriginais[i]);
            turnModeManager.aliadosPersonagens.Add(aliadosOriginais[i].GetComponent<BasePersonagem>());
            turnModeManager.aliadosPersonagensPersistentes.Add(aliadosOriginais[i].GetComponent<BasePersonagem>());

            BasePersonagem aliado = turnModeManager.aliadosPersonagens[i];

            aliado.shadow.gameObject.SetActive(true);
            aliado.characterStatus = Instantiate(playerStatus[i]);
            
            
            aliado.SetupStatus();

            aliado.gameObject.SetActive(true);
            for(int j = 0; j < enemyStatus.Count; j++)
            {
                aliado.characterStatus.ataques[j] = Instantiate(playerStatus[i].ataques[j]);
                aliado.characterStatus.ataques[j].name = playerStatus[i].ataques[j].name;
            }
        }

        for (int i = 0; i < enemyStatus.Count; i++)
        {
            turnModeManager.inimigos.Add(inimigosOriginais[i]);
            turnModeManager.inimigosPersonagens.Add(inimigosOriginais[i].GetComponent<BasePersonagem>());
            BasePersonagem inimigo = turnModeManager.inimigosPersonagens[i];

            inimigo.characterStatus = Instantiate(enemyStatus[i]);

            if (inimigo.characterStatus.isEnemy)
                inimigo.GetComponent<SpriteRenderer>().flipX = true;

            inimigo.shadow.gameObject.SetActive(true);

            inimigo.SetupStatus();

            inimigo.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(0.1f);
        TurnModeManager.instance.FirstAllyAttack();
    }
}