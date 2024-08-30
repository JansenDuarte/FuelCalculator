using UnityEngine;
using TMPro;
using System.Globalization;
using System.Collections;

public class FuelConsumption : MonoBehaviour
{
    #region UI_COMPONENTS

    [Space, Header("Panels"), Space]
    [SerializeField] private GameObject m_mainPanel;
    [SerializeField] private GameObject m_addPanel;

    [Space, Header("Main Panel Components"), Space]
    [SerializeField] private TMP_InputField m_fuelVolume;
    [SerializeField] private TMP_InputField m_kilometer;
    [SerializeField] private TMP_Dropdown m_mainFuelType;
    [SerializeField] private TMP_Text m_info;

    [Space, Header("Add Panel Components"), Space]
    [SerializeField] private TMP_InputField m_knownConsumption;
    [SerializeField] private TMP_Dropdown m_addFuelType;

    #endregion // UI_COMPONENTS

    private void Start()
    {
        float lastRefill = DataBaseConnector.Instance.GetFuelRefill();

        if (lastRefill > 0f)
        {
            m_fuelVolume.text = lastRefill.ToString();
        }
    }

    public void UI_SaveFuelRefill()
    {
        if (m_fuelVolume.text.Length == 0)
        {
            StartCoroutine(ShowInfoText(5f, "Alimente as caixas acima com informações antes de tentar calcular ou salvar o reabastecimento"));
            return;
        }

        float volume = float.Parse(m_fuelVolume.text);
        float kilometer = (m_kilometer.text.Length == 0) ? 0f : float.Parse(m_kilometer.text);

        float KmPerL = 0f;
        if (volume > 0f)
        {
            KmPerL = kilometer / volume;
        }


        if (kilometer > 0f)
        {
            DataBaseConnector.Instance.SaveFuelConsumption(KmPerL, m_mainFuelType.value);
            StartCoroutine(ShowInfoText(5f, "Média de " + KmPerL + "Km/l"));
        }
        else
        {
            DataBaseConnector.Instance.SaveRefill(volume);
            StartCoroutine(ShowInfoText(2f, "Reabastecimento salvo!"));
        }
    }

    public void UI_AddKnownConsumption()
    {
        if (m_knownConsumption.text.Length == 0)
        {
            StartCoroutine(ShowInfoText(5f, "Não é possível adicionar um consumo vazio"));
            return;
        }

        float kml = float.Parse(m_knownConsumption.text);

        DataBaseConnector.Instance.SaveFuelConsumption(kml, m_addFuelType.value);
    }

    public void UI_ChangeLayout()
    {
        m_mainPanel.SetActive(!m_mainPanel.activeSelf);
        m_addPanel.SetActive(!m_addPanel.activeSelf);
    }

    private IEnumerator ShowInfoText(float _timer, string _msg = "")
    {
        m_info.text = _msg;
        yield return new WaitForSeconds(_timer);

        m_info.text = string.Empty;
        yield break;
    }
}
