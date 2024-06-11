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

    //DEBUG
    [Header("DEBUG")]
    [SerializeField] private float m_priceDiferencePercent;
    [SerializeField] private float m_alcoholConsumptionPercent;
    [SerializeField] private float m_gasolineConsumptionPercent;
    [SerializeField] private float m_consumptionDiferencePercent;
    //DEBUG

    #endregion // VARIABLES

    private void Start()
    {
        CalculateAvg();
    }

    private void CalculateAvg()
    {
        DataBaseConnector.Instance.GetAverageConsumption(out m_alcoholConsumptionPercent, out m_gasolineConsumptionPercent);
        m_consumptionDiferencePercent = (m_alcoholConsumptionPercent != 0f) ? ((m_gasolineConsumptionPercent - m_alcoholConsumptionPercent) * 100f) / m_alcoholConsumptionPercent : 0f;
    }

    public void UI_CalculatePriceDiferencePercent()
    {
        float gasoline = float.Parse(m_gasolinePrice.text);
        float alcohol = float.Parse(m_alcoholPrice.text);
        m_priceDiferencePercent = ((gasoline - alcohol) * 100f) / alcohol;
        string result = string.Format("<b>{0}%</b>", m_priceDiferencePercent);
        string consumption = (m_consumptionDiferencePercent != 0f) ? string.Format("\n\nDiferença de consumo baseado em resultados anteriores <b>{0}%</b>", m_consumptionDiferencePercent) : "";

        m_priceDiferenceResult.text = string.Format("Gasolina está {0} mais caro que o Álcool.{1}", result, consumption);

        AdviseFuelToUse();
    }

    private void AdviseFuelToUse()
    {
        if (m_consumptionDiferencePercent == 0f)
        {
            return;
        }

        if (m_priceDiferencePercent < m_consumptionDiferencePercent)
        {
            m_priceDiferenceResult.text += "\n\nGasolina é aconcelhado";
        }
        else
        {
            m_priceDiferenceResult.text += "\n\nÁlcool é aconcelhado";
        }
    }
}