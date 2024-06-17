using UnityEngine;
using TMPro;

public class Options_Controler : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private TMP_InputField m_avgCount;

    [Header("Internal Panels")]
    [SerializeField] private GameObject m_mainPanel;
    [SerializeField] private GameObject m_sidePanel;

    [Header("Canvases")]
    [SerializeField] private Canvas[] m_canvases;
    [SerializeField] private int m_lastOpenCanvasIdx;

    private void Start()
    {
        m_avgCount.text = GameManager.Instance.AvgCount.ToString();
    }

    public void UI_OpenCalculator()
    {
        //FIXME: this is shit!!
        m_canvases[0].enabled = true;
        m_canvases[1].enabled = false;
    }

    public void UI_OpenConsumption()
    {
        //FIXME: this is shit!!
        m_canvases[0].enabled = false;
        m_canvases[1].enabled = true;
    }


    public void UI_ChangeAvgCont()
    {
        if (int.TryParse(m_avgCount.text, out int value))
        {
            GameManager.Instance.ChangeAvgCount(value);
        }
    }

    public void UI_OpenOptions()
    {
        m_avgCount.text = GameManager.Instance.AvgCount.ToString();

        for (int i = 0; i < m_canvases.Length; i++)
        {
            if (m_canvases[i].enabled)
            {
                m_lastOpenCanvasIdx = i;
                m_canvases[i].enabled = false;
            }
        }

        m_mainPanel.SetActive(true);
        m_sidePanel.SetActive(false);
    }

    public void UI_CloseOptions()
    {
        m_mainPanel.SetActive(false);
        m_sidePanel.SetActive(true);

        m_canvases[m_lastOpenCanvasIdx].enabled = true;
    }
}
