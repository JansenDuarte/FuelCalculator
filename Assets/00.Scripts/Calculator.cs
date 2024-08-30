using UnityEngine;
using TMPro;
using System.Collections;

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

        if (m_gasolinePrice.text.Length == 0 || m_alcoholPrice.text.Length == 0)
        {
            StartCoroutine(ShowInfoText(5f, "Não é possível calcular com um dos campos vazio!"));
            return;
        }

        float gasoline = float.Parse(m_gasolinePrice.text);
        float alcohol = float.Parse(m_alcoholPrice.text);

        m_priceDiferencePercent = ((gasoline - alcohol) * 100f) / alcohol;

        StartCoroutine(AdviseFuelToUse());
    }

    private IEnumerator AdviseFuelToUse()
    {
        if (m_priceDiferencePercent == 0f)
        {
            yield break;
        }

        string formatedToUI = "Gasolina está <b>" + m_priceDiferencePercent + "%</b> mais caro que o Álcool.";


        if (m_alcoholConsumptionPercent != 0f)
        {
            formatedToUI += "\n\nDiferença de consumo baseado em resultados anteriores <b>" + m_consumptionDiferencePercent + "%</b>";
        }


        if (m_priceDiferencePercent < m_consumptionDiferencePercent)
        {
            formatedToUI += "\n\nGasolina é aconcelhado";
        }
        else
        {
            formatedToUI += "\n\nÁlcool é aconcelhado";
        }

        m_priceDiferenceResult.text = formatedToUI;
        yield return new WaitForSeconds(5f);
        m_priceDiferenceResult.text = string.Empty;
        yield break;
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


    private IEnumerator ShowInfoText(float _timer, string _msg = "")
    {
        m_priceDiferenceResult.text = _msg;
        yield return new WaitForSeconds(_timer);

        m_priceDiferenceResult.text = string.Empty;
        yield break;
    }
}