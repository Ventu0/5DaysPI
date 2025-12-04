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
            if (playerStatus[i].isDead)
            {
                BasePersonagem personagem = aliadosOriginais[i].GetComponent<BasePersonagem>();
                personagem.characterStatus = Instantiate(playerStatus[i]);
                turnModeManager.aliadosPersonagensPersistentes.Add(personagem);
                continue;
            }

            BasePersonagem aliadoPersonagem = aliadosOriginais[i].GetComponent<BasePersonagem>();
            turnModeManager.aliados.Add(aliadosOriginais[i]);

            aliadoPersonagem.shadow.gameObject.SetActive(true);

            aliadoPersonagem.characterStatus = Instantiate(playerStatus[i]);

            aliadoPersonagem.SetupStatus();
            aliadoPersonagem.gameObject.SetActive(true);
            for(int j = 0; j < enemyStatus.Count; j++)
            {
                aliadoPersonagem.characterStatus.ataques[j] = Instantiate(playerStatus[i].ataques[j]);
                playerStatus[i].ataques[j].oneTime = false; //para não dar erro de ataque nulo
                aliadoPersonagem.characterStatus.ataques[j].name = playerStatus[i].ataques[j].name; //para nao dar o ataque(copia)
            }
            turnModeManager.aliadosPersonagensPersistentes.Add(aliadoPersonagem); //adiciona os personagens no persistente (pra prevalecer o isDead)
            turnModeManager.aliadosPersonagens.Add(aliadoPersonagem);
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

            Vector2 newEnemyPos = new Vector2(inimigoPos.x, inimigoPos.y + inimigo.characterStatus.YOffset);
            inimigo.transform.position = newEnemyPos;
            inimigo.originalPos = newEnemyPos;

            inimigo.shadow.transform.position = shadowOriginalPos;
            inimigo.shadow.gameObject.SetActive(true);

            inimigo.SetupStatus();

            inimigo.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(0.1f);
        TurnModeManager.instance.FirstAllyAttack();
    }
}