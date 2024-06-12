using UnityEngine;

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

    public int AvgCount = 5;

    private void Start()
    {
        if (PlayerPrefs.HasKey(PrefKeys.AVG_COUNT))
        {
            AvgCount = PlayerPrefs.GetInt(PrefKeys.AVG_COUNT);
        }
        else
        {
            PlayerPrefs.SetInt(PrefKeys.AVG_COUNT, AvgCount);
        }
    }

    public void ChangeAvgCount(int _value)
    {
        AvgCount = _value;
        PlayerPrefs.SetInt(PrefKeys.AVG_COUNT, AvgCount);
    }

    private static class PrefKeys
    {
        public const string AVG_COUNT = "AverageCount";
    }
}
