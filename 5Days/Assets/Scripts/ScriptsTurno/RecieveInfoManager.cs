using UnityEngine;

public class RecieveInfoManager : MonoBehaviour
{
    public static RecieveInfoManager instance;
    private void Awake()
    {
        TurnModeManager turnModeManager = TurnModeManager.instance;
        if (instance == null) instance = this;

        for (int i = 0; i < turnModeManager.aliados.Count; i++)
        {
            turnModeManager.aliados[i].gameObject.SetActive(false);
        }

    }
    private void Start()
    {
        TurnModeManager turnModeManager = TurnModeManager.instance;
        if (TurnModeInfo.enemiesToLoad.Count == 0) print("Nulo");
        if (TurnModeInfo.alliesToLoad.Count == 0) print(" aliado Nulo");
        print("RecieveInfoManager started");
        for (int i = 0; i < TurnModeInfo.alliesToLoad.Count; i++)
        {
            print("tem aliado");
            turnModeManager.aliados[i].gameObject.SetActive(true);
            turnModeManager.aliadosPersonagens[i].characterStatus = TurnModeInfo.alliesToLoad[i];
        }

        SpawnEnemies.instance.Spawn(TurnModeInfo.enemiesToLoad.Count);

        for (int i = 0; i < TurnModeInfo.enemiesToLoad.Count; i++)
        {
            turnModeManager.inimigosPersonagens[i].characterStatus = TurnModeInfo.enemiesToLoad[i];
        }





    }
}
