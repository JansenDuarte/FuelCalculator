using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class History : MonoBehaviour
{

    #region UI_COMPONENTS

    [SerializeField] private GameObject m_contentPivot;
    [SerializeField] private GameObject m_itemPrefab;
    [SerializeField] private GameObject m_emptyListMsg;
    [SerializeField] private GameObject m_listFilter;

    #endregion // UI_COMPONENTS


    private void Start()
    {
        PopulateHistoryList();
    }

    private void PopulateHistoryList()
    {
        DataBaseConnector.Instance.GetAllConsumptions(out List<Consumption> consumptions);

        if (consumptions.Count == 0)
        {
            m_listFilter.SetActive(false);
            return;
        }

        m_emptyListMsg.SetActive(false);
        m_listFilter.SetActive(true);

        foreach (Consumption item in consumptions)
        {
            HistoryItem hi = Instantiate(m_itemPrefab, m_contentPivot.transform).GetComponent<HistoryItem>();

            hi.Kml.text = item.KmL.ToString();
            hi.FuelType.text = (item.Fuel == 0) ? "Álcool" : "Gasolina";
            hi.Date.text = (item.Date == "NULL") ? "N/A" : item.Date;
        }
    }
}