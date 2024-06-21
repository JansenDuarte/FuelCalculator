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
    private float m_alcoholConsumptionPercent;
    private float m_gasolineConsumptionPercent;
    private float m_consumptionDiferencePercent;

    #endregion // VARIABLES

    private void Start()
    {
        CalculateAvg();
    }



    public void UI_CalculatePriceDiferencePercent()
    {
        CalculateAvg();

        float gasoline = float.Parse(m_gasolinePrice.text);
        float alcohol = float.Parse(m_alcoholPrice.text);

        m_priceDiferencePercent = ((gasoline - alcohol) * 100f) / alcohol;

        AdviseFuelToUse();
    }

    private void AdviseFuelToUse()
    {
        if (m_consumptionDiferencePercent == 0f)
        {
            return;
        }

        string result = "<b>" + m_priceDiferencePercent + "%</b>";
        string consumption = (m_consumptionDiferencePercent != 0f) ? string.Format("\n\nDiferença de consumo baseado em resultados anteriores <b>{0}%</b>", m_consumptionDiferencePercent) : "";

        m_priceDiferenceResult.text = string.Format("Gasolina está {0} mais caro que o Álcool.{1}", result, consumption);

        if (m_priceDiferencePercent < m_consumptionDiferencePercent)
        {
            m_priceDiferenceResult.text += "\n\nGasolina é aconcelhado";
        }
        else
        {
            m_priceDiferenceResult.text += "\n\nÁlcool é aconcelhado";
        }
    }

    private void CalculateAvg()
    {
        DataBaseConnector.Instance.GetAverageConsumption(out m_alcoholConsumptionPercent, out m_gasolineConsumptionPercent);
        if (m_alcoholConsumptionPercent != 0f && m_gasolineConsumptionPercent != 0f)
        {
            m_consumptionDiferencePercent = ((m_gasolineConsumptionPercent - m_alcoholConsumptionPercent) * 100f) / m_alcoholConsumptionPercent;
        }
        else
        {
            //Not enough info, can't make an acessment
            m_consumptionDiferencePercent = 0f;
        }
    }

    //FIXME: the UI message needs to be separated, it'll be easier to debug and maintain
}