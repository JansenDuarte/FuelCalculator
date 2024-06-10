using UnityEngine;
using TMPro;

public class Calculator : MonoBehaviour
{
    #region UI_COMPONENTS

    [SerializeField] private TMP_InputField m_alcoholPrice;
    [SerializeField] private TMP_InputField m_gasolinePrice;
    [SerializeField] private TMP_Text m_priceDiferenceResult;

    #endregion // UI_COMPONENTS

    #region VARIABLES

    private float m_priceDiferencePercent;

    //TEST
    public float Alcohol_KmL;
    public float Gasoline_KmL;
    public float KmL_diferencePercent;

    #endregion // VARIABLES

    private void Awake()
    {
        KmL_diferencePercent = ((Gasoline_KmL - Alcohol_KmL) * 100f) / Alcohol_KmL;
    }

    public void UI_CalculatePriceDiferencePercent()
    {
        float gasoline = float.Parse(m_gasolinePrice.text);
        float alcohol = float.Parse(m_alcoholPrice.text);
        m_priceDiferencePercent = ((gasoline - alcohol) * 100f) / alcohol;
        string result = string.Format("<b>{0} %</b>", m_priceDiferencePercent);

        m_priceDiferenceResult.text = string.Format("Gasolina está {0} mais caro que o Álcool", result);

        AdviceFuelToUse();
    }

    private void AdviceFuelToUse()
    {
        if (m_priceDiferencePercent < KmL_diferencePercent)
        {
            //TAGED gasoline should be adviced

        }
        else
        {
            //TAGED alcohol shold be adviced
        }
    }
}