using System.Collections.Generic;
using UnityEngine;

public class History : MonoBehaviour
{

    #region UI_COMPONENTS

    [SerializeField] private GameObject m_contentPivot;
    [SerializeField] private GameObject m_itemPrefab;
    [SerializeField] private GameObject m_emptyListMsg;
    [SerializeField] private GameObject m_listFilter;

    #endregion // UI_COMPONENTS

    private List<HistoryItem> historyItems = new List<HistoryItem>();


    private void Start()
    {
        GameManager.Instance.ConsumptionsUpdated.AddListener(PopulateHistoryList);

        PopulateHistoryList();
    }

    public void PopulateHistoryList()
    {
        DataBaseConnector.Instance.GetAllConsumptions(out List<Consumption> consumptions);

        if (consumptions.Count == 0)
        {
            m_listFilter.SetActive(false);
            return;
        }


        m_emptyListMsg.SetActive(false);
        m_listFilter.SetActive(true);

        int diff = consumptions.Count - historyItems.Count;
        while (diff > 0)
        {
            HistoryItem hi = Instantiate(m_itemPrefab, m_contentPivot.transform).GetComponent<HistoryItem>();
            historyItems.Add(hi);
            diff--;
        }

        ModifyItemsTexts(ref consumptions);
    }

    private void ModifyItemsTexts(ref List<Consumption> _info)
    {
        for (int i = 0; i < historyItems.Count; i++)
        {
            historyItems[i].Kml.text = _info[i].KmL.ToString();
            historyItems[i].FuelType.text = (_info[i].Fuel == 0) ? "Álcool" : "Gasolina";
            historyItems[i].Date.text = (_info[i].Date == "NULL") ? "N/A" : _info[i].Date;
        }
    }
}