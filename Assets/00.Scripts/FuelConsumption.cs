using UnityEngine;
using TMPro;
using System.Globalization;

public class FuelConsumption : MonoBehaviour
{
    #region UI_COMPONENTS

    [SerializeField] private TMP_InputField m_fuelVolume;
    [SerializeField] private TMP_InputField m_kilometer;
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

        if (kilometer > 0f)
        {
            m_info.text = "Média de " + kilometer / volume + "Km/l";
        }
    }
}
