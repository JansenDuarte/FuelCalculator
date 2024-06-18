using UnityEngine;
using TMPro;
using System.Globalization;
using System.Collections;

public class FuelConsumption : MonoBehaviour
{
    #region UI_COMPONENTS

    [SerializeField] private TMP_InputField m_fuelVolume;
    [SerializeField] private TMP_InputField m_kilometer;
    [SerializeField] private TMP_Dropdown m_fuelType;
    [SerializeField] private TMP_Text m_info;

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
        if (m_fuelVolume.text == string.Empty)
        {
            return;
        }

        float volume = float.Parse(m_fuelVolume.text, CultureInfo.InvariantCulture);
        float.TryParse(m_kilometer.text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float kilometer);

        DataBaseConnector.Instance.SaveFuelConsumption(volume, kilometer, m_fuelType.value);

        if (kilometer > 0f)
        {
            StartCoroutine(ShowInfoText(5f, "Média de " + kilometer / volume + "Km/l"));
        }
        else
        {
            StartCoroutine(ShowInfoText(2f, "Reabastecimento salvo!"));
        }
    }

    private IEnumerator ShowInfoText(float _timer, string _msg = "")
    {
        m_info.text = _msg;
        yield return new WaitForSeconds(_timer);

        m_info.text = string.Empty;
        yield break;
    }
}
