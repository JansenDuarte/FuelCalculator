using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class History : MonoBehaviour
{

    #region UI_COMPONENTS

    [SerializeField] private GameObject m_contentPivot;
    [SerializeField] private GameObject m_itemPrefab;

    #endregion // UI_COMPONENTS

    //TODO: Make the empty list prefab spawn

    void Start()
    {
        List<Consumption> consumptions = DataBaseConnector.Instance.GetAllConsumptions();

        foreach (Consumption item in consumptions)
        {
            HistoryItem hi = Instantiate(m_itemPrefab.gameObject, m_contentPivot.transform).GetComponent<HistoryItem>();

            hi.Kml.text = item.KmL.ToString();
            hi.FuelType.text = (item.Fuel == 0) ? "Álcool" : "Gasolina";
            hi.Date.text = item.Date;
        }
    }
}