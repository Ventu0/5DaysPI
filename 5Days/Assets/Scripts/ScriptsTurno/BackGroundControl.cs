using UnityEngine;

public class BackGroundControl : MonoBehaviour
{
    [SerializeField] Material dayShader;
    void Start()
    {
        float actualHour = DiaENoite.instance.relogioScript.GetCurrentHour(); //pega o horario atual
        float multipleOf24 = GetPercentOf24(); //pega o numero de 1 / 24, que da 0,0416..., usado pra aplicar a cor entre 0 a 1
        SetHour(multipleOf24 * actualHour); //como o multiplo é dividido por 24, eu pego o valor da hora atual e multiplico pelo outro numero, dando a hora atual entre 0 a 1
    }
    #region ShaderDeDiaENoiteDoModoDeCombate
    public void SetHour(float time)
    {
        dayShader.SetFloat("_time", time);
    }
    public float GetPercentOf24()
    {
        return 1f / 24f;
    }
    #endregion
}
