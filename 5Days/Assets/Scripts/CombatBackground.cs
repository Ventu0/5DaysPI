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
        if (BackGroundControl.instance == null) print("nulooo porra");
        BackGroundControl.instance.ChangeMainBackGround(background);
    }
}
