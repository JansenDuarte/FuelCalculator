using UnityEngine;
using TMPro;

public class Options_Controler : MonoBehaviour
{
    [Space, Header("Options"), Space]
    [SerializeField] private TMP_InputField m_avgCount;

    [Space, Header("Internal Panels"), Space]
    [SerializeField] private GameObject m_mainPanel;
    [SerializeField] private GameObject m_sidePanel;

    [Space, Header("Canvases"), Space]
    [SerializeField] private Canvas[] m_canvases;
    [SerializeField] private int m_lastOpenCanvasIdx;

    private void Start()
    {
        m_avgCount.text = GameManager.Instance.AvgCount.ToString();

        //Find the open canvas in the scene and place it as the open canvas
        for (int i = 0; i < m_canvases.Length; i++)
        {
            if (m_canvases[i].enabled)
            {
                m_lastOpenCanvasIdx = i;
            }
        }
    }

    public void UI_ChangeVisibleCanvas(int _canvasIdx)
    {
        m_canvases[m_lastOpenCanvasIdx].enabled = false;
        m_canvases[_canvasIdx].enabled = true;

        m_lastOpenCanvasIdx = _canvasIdx;
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

        m_canvases[m_lastOpenCanvasIdx].enabled = false;

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
