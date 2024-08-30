using UnityEngine;
using TMPro;

public class InitialSetup : MonoBehaviour
{
    #region UI_ELEMENTS

    [SerializeField] private TMP_InputField m_alcohool;
    [SerializeField] private TMP_InputField m_gasoline;

    #endregion // UI_ELEMENTS  


    public void UI_Save()
    {
        if (m_alcohool.text == string.Empty || m_gasoline.text == string.Empty)
        {
            //TODO: This should show a message
            return;
        }

        float alcohol = float.Parse(m_alcohool.text);
        float gasoline = float.Parse(m_gasoline.text);

        DataBaseConnector.Instance.SaveKnownConsumptions(alcohol, gasoline);
        GameManager.Instance.ChangeScene(SceneCodex.MAIN);
    }

    public void UI_Skip()
    {
        GameManager.Instance.ChangeScene(SceneCodex.MAIN);
    }
}