using UnityEngine;

public class BackGroundControl : MonoBehaviour
{
    [SerializeField] Material dayShader;
    void Start()
    {
        
    }
    public void SetHour(float time)
    {
        dayShader.SetFloat("_time", time);
    }
    public float GetPercentOf24()
    {
        return 1f / 24f;
    }
}
