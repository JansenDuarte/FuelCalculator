using UnityEngine;
using TMPro;

public class FuelConsumption : MonoBehaviour
{
    #region UI_COMPONENTS

    [SerializeField] private TMP_InputField m_fuelVolume;
    [SerializeField] private TMP_InputField m_kilometer;
    [SerializeField] private TMP_Dropdown m_fuelType;

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

        float volume = float.Parse(m_fuelVolume.text);
        float.TryParse(m_kilometer.text, out float kilometer);
        DataBaseConnector.Instance.SaveFuelConsumption(volume, kilometer, m_fuelType.value);

        if (kilometer > 0f)
        {
            //TODO mostra a caceta do consumo médio
        }
    }
}
