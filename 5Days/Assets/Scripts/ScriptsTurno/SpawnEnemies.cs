using UnityEngine;
using System.Collections.Generic;
public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab; //se quiser incluir vários tipos de inimigo, faça depois do protótipo
    [SerializeField] Transform[] spawnSpots;
    public static SpawnEnemies instance;
    private void Awake()
    {
        if (instance == null)
        {
            print("SpawnEnemies instance criado");
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Spawn(int quantidadeDeInimigos, List<CharacterStatusGeneric> enemyStatus)
    {
        print("Spawn ativado");
       int valorLimitado = Mathf.Clamp(quantidadeDeInimigos, 0, spawnSpots.Length);
       for (int i = 0; i < valorLimitado; i++)
       {
           int randomIndex = Random.Range(0, spawnSpots.Length);
           GameObject enemy = Instantiate(enemyPrefab, spawnSpots[i].position, transform.rotation);
           enemy.transform.SetParent(spawnSpots[i]);
           enemy.GetComponent<BasePersonagem>().characterStatus = Instantiate(enemyStatus[i]);
           SetInstancesToTurnMode(enemy);   
       }
    }
    void SetInstancesToTurnMode(GameObject enemy)
    {
        print("mandando instancias para o modo de turno script yay");
        TurnModeManager turnModeManager = TurnModeManager.instance;
        turnModeManager.inimigos.Add(enemy.GetComponent<EnemyAI>());
        turnModeManager.inimigosPersonagens.Add(enemy.GetComponent<BasePersonagem>());
        enemy.GetComponent<BasePersonagem>().SetupStatus();
    }
}
