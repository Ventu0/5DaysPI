using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class RecieveInfoManager : MonoBehaviour
{
    public static RecieveInfoManager instance;
    [SerializeField] private List<Aliados> aliadosOriginais = new List<Aliados>();
    [SerializeField] private List<EnemyAI> inimigosOriginais = new List<EnemyAI>();
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
            turnModeManager.inimigos[i].gameObject.SetActive(false);
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
                playerStatus[i].ataques[j].oneTime = false; //para não dar erro de ataque nulo
                aliado.characterStatus.ataques[j].name = playerStatus[i].ataques[j].name; //para nao dar o ataque(copia)
            }
            

        }

        for (int i = 0; i < enemyStatus.Count; i++)
        {
            turnModeManager.inimigos.Add(inimigosOriginais[i]);
            turnModeManager.inimigosPersonagens.Add(inimigosOriginais[i].GetComponent<BasePersonagem>());

            BasePersonagem inimigo = turnModeManager.inimigosPersonagens[i];
            Vector2 inimigoPos = inimigo.transform.position;
            Vector2 shadowOriginalPos = inimigo.shadow.transform.position;

            inimigo.characterStatus = Instantiate(enemyStatus[i]);

            inimigo.GetComponent<SpriteRenderer>().flipX = inimigo.characterStatus.flipX;

            inimigo.transform.position = new Vector2(inimigoPos.x, inimigoPos.y + inimigo.characterStatus.YOffset);
            inimigo.shadow.transform.position = shadowOriginalPos;
            inimigo.shadow.gameObject.SetActive(true);

            inimigo.SetupStatus();

            inimigo.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(0.1f);
        TurnModeManager.instance.FirstAllyAttack();
    }
}