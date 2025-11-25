using UnityEngine;

public class CombatBackground : MonoBehaviour
{
    [SerializeField] Sprite background;
    public static CombatBackground instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }
    public void ChangeBackGround()
    {
        print("trocando background");
        BackGroundControl.instance.ChangeMainBackGround(background);
    }
}
