using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class Vagalumes : MonoBehaviour
{
    [SerializeField] GameObject vagalumesObject;
    [SerializeField] float secondsToSpawn = 3f;
    [SerializeField] float fireflySpeed = 3f;
    [Header("Vetores aleatorios")]
    [Range(-1,1)]
    [SerializeField] float randomXmin;

    [Range(-1, 1)]
    [SerializeField] float randomXmax;
    [Space]
    [Range(-1, 1)]
    [SerializeField] float randomYmin;

    [Range(-1, 1)]
    [SerializeField] float randomYmax;
    [Space]
    [SerializeField] int poolSize = 10;
    Queue<GameObject> fireflyQueue = new Queue<GameObject>();
    float time;
    bool canSpawn = false;

    void Start()
    {
        for(int i = 0; i < poolSize; i++)
        {
            GameObject firefly = Instantiate(vagalumesObject, transform);
            firefly.SetActive(false);
            firefly.transform.position = transform.position;
            fireflyQueue.Enqueue(firefly);
        }
        canSpawn = true;
    }
    void Update()
    {
        if(canSpawn)
        {
            time += Time.deltaTime;
            if (time >= secondsToSpawn)
            {
                StartCoroutine(SpawnFirefly());
                time = 0f;
            }
        }
    }
    IEnumerator SpawnFirefly()
    {
        GameObject firefly = fireflyQueue.Dequeue();
        SpriteRenderer renderer = firefly.GetComponent<SpriteRenderer>();
        float x = Random.Range(randomXmin, randomXmax);
        float y = Random.Range(randomYmin, randomYmax);
        Vector2 direction = new Vector2(x, y);
        firefly.SetActive(true);

        if(x > 0f)
            renderer.flipX = true;
        else if (x < 0f)
            renderer.flipX = false;

       float speed = Random.Range(fireflySpeed + 0f, fireflySpeed + 0.5f);
       firefly.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * speed;
       yield return new WaitForSeconds(10f);
       firefly.SetActive(false);
       fireflyQueue.Enqueue(firefly);
    }
    void OnNight()
    {
        canSpawn = true;
    }
}
