using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region SIMPLE_SINGLETON
    private static GameManager s_instance;
    public static GameManager Instance { get { return s_instance; } }
    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion // SIMPLE_SINGLETON

    public int AvgCount;
    public bool FirstExecution = true;

    private void Start()
    {
        if (PlayerPrefs.HasKey(PrefKeys.FIRST_EXECUTION))
        {
            FirstExecution = false;
            AvgCount = PlayerPrefs.GetInt(PrefKeys.AVG_COUNT);
        }
        else
        {
            // INFO: FIRST_EXECUTION has a magic number, but it only needs to be present
            PlayerPrefs.SetInt(PrefKeys.FIRST_EXECUTION, 0);
            PlayerPrefs.SetInt(PrefKeys.AVG_COUNT, AvgCount);
        }
    }

    public void ChangeAvgCount(int _value)
    {
        AvgCount = _value;
        PlayerPrefs.SetInt(PrefKeys.AVG_COUNT, AvgCount);
    }

    public void ChangeScene(int _sceneIndex)
    {
        SceneManager.LoadScene(_sceneIndex);
    }

    public void ChangeScene()
    {
        if (FirstExecution)
        {
            SceneManager.LoadScene(SceneCodex.INITIAL_SETUP);
        }
        else
        {
            SceneManager.LoadScene(SceneCodex.MAIN);
        }
    }



    private static class PrefKeys
    {
        public const string FIRST_EXECUTION = "FirstExecution";
        public const string AVG_COUNT = "AverageCount";
    }
}
