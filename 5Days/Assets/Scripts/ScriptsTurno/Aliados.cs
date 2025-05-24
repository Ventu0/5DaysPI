using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Aliados : MonoBehaviour
{
    [SerializeField] public List<Attack> ataques;
    public bool isDefending;
    public GameObject shield;
    void Awake()
    {
        shield.SetActive(false);
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
