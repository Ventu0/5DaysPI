using UnityEngine;
using System.Collections;
public class Vagalumes : MonoBehaviour
{
    [SerializeField] GameObject vagalumesObject;
    [SerializeField] float secondsToSpawn = 3f;
    [SerializeField] float fireflySpeed = 3f;
    float time;
    bool canSpawn = false;

    void Start()
    {
        
    }
    void Update()
    {
        if(canSpawn)
        {
            time += Time.deltaTime;
            if (time >= secondsToSpawn)
            {
                
                time = 0f;
            }
        }
    }
    void OnNight()
    {
        canSpawn = true;
    }
}
