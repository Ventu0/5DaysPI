using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab; //se quiser incluir vários tipos de inimigo, faça depois do protótipo
    [SerializeField] Transform[] spawnSpots;
    public static SpawnEnemies instance;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Spawn(int quantidadeDeInimigos)
    {
       int valorLimitado = Mathf.Clamp(quantidadeDeInimigos, 0, spawnSpots.Length);
       for (int i = 0; i < valorLimitado; i++)
       {
           int randomIndex = Random.Range(0, spawnSpots.Length);
           GameObject enemy = Instantiate(enemyPrefab, spawnSpots[i].position, transform.rotation);
           enemy.transform.SetParent(spawnSpots[i]);
       }
    }
}
