using UnityEngine;
using System.Collections;
using UnityEditor.Tilemaps;
using System.Collections.Generic;
using TMPro;
public class Vagalumes : MonoBehaviour
{
    [SerializeField] GameObject vagalumesObject;
    [SerializeField] float secondsToSpawn = 3f;
    [SerializeField] float fireflySpeed = 3f;
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
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
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
