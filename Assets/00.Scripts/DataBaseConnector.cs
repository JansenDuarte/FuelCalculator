using System.Data;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class DataBaseConnector : MonoBehaviour
{
    #region SIMPLE_SINGLETON
    private static DataBaseConnector s_instance;
    public static DataBaseConnector Instance { get { return s_instance; } }
    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(InitialSetup());
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion // SIMPLE_SINGLETON


    [SerializeField] private TMP_Text m_log;

    IDbConnection _connection;
    IDbCommand _command;
    IDataReader _reader;


    private IEnumerator InitialSetup()
    {
        if (!File.Exists(Application.persistentDataPath + "/InternalDataBase.db"))
        {
            m_log.text += "\nVerifying DataBase presence...";
            yield return new WaitForSeconds(1f);

            m_log.text += "\nDataBase not present! Preparing to write it to " + Application.persistentDataPath;
            yield return new WaitForSeconds(1f);

            double opInitialTime = Time.realtimeSinceStartupAsDouble;
            double maxLoadTimeS = 600;

            m_log.text += "\nPreparing to download DataBase...";
            yield return new WaitForSeconds(1f);
            UnityWebRequest loadDb = UnityWebRequest.Get(Application.streamingAssetsPath + "/InternalDataBase.db");
            loadDb.SendWebRequest();

            while (!loadDb.isDone)
            {
                if (Time.realtimeSinceStartupAsDouble - opInitialTime >= maxLoadTimeS)
                {
                    m_log.text += "\nDataBase file could not be loaded! Terminating application!";
                    yield return new WaitForSeconds(1f);
                    Application.Quit();
                    break;
                }
                yield return new WaitForEndOfFrame();
            }

            m_log.text += "\nWriting Database to file location: " + Application.persistentDataPath + "/InternalDataBase.db";
            yield return new WaitForSeconds(1f);
            File.WriteAllBytes(Application.persistentDataPath + "/InternalDataBase.db", loadDb.downloadHandler.data);
        }

        //StartCoroutine(FixDb());
        m_log.text += "\nDataBase loaded correctly! Resuming application...";
        yield return new WaitForSeconds(1f);
        GameManager.Instance.ChangeScene();
        yield break;
    }


    //HACK  Remove this after correcting the db on my phone
    private IEnumerator FixDb()
    {
        GetAllConsumptions(out List<Consumption> data);

        Connect();

        int i = 0;
        foreach (Consumption c in data)
        {
            if (c.KmL > 1f)
                continue;

            float correct = c.KmL * 100f;
            _command.CommandText = "update FuelConsumption set Kml = " + correct.ToString(CultureInfo.InvariantCulture) + " where ID = " + c.Id;
            _command.ExecuteNonQuery();
            i++;
        }

        m_log.text += "\nCorrection finished! " + i + " values corrected";
        CloseConnection();
        yield return new WaitForSeconds(5f);
        GameManager.Instance.ChangeScene();
        yield break;
    }


    public float GetFuelRefill()
    {
        float value = -1f;

        Connect();

        _command.CommandText = CommandCodex.SELECT_ALL_FUEL_REFILL;

        _reader = _command.ExecuteReader();

        while (_reader.Read())
        {
            value = _reader.GetFloat(1);
        }

        _reader.Close();
        CloseConnection();

        return value;
    }

    public void SaveKnownConsumptions(float _alcoholKmL, float _gasolineKmL)
    {
        Connect();

        _command.CommandText = string.Format(CommandCodex.INSERT_CONSUMPTION_WITHOUT_DATE, _alcoholKmL, 0);
        _command.CommandText += string.Format(CommandCodex.INSERT_CONSUMPTION_WITHOUT_DATE, _gasolineKmL, 1);
        _command.ExecuteNonQuery();

        CloseConnection();
    }

    public int SaveFuelConsumption(float _kml, int _fuelType)
    {
        Connect();

        _command.CommandText = string.Format(CommandCodex.INSERT_CONSUMPTION, _kml.ToString(CultureInfo.InvariantCulture), _fuelType.ToString());
        int result = _command.ExecuteNonQuery();

        CloseConnection();

        InvokeConsumptionsUpdated();

        return result;
    }


    public int SaveRefill(float _volume)
    {
        Connect();

        _command.CommandText = CommandCodex.UPDATE_FUEL_REFIL + _volume.ToString(CultureInfo.InvariantCulture);
        int result = _command.ExecuteNonQuery();

        CloseConnection();

        return result;
    }


    public void GetAverageConsumption(out float _alcohol, out float _gasoline)
    {
        _alcohol = 0f;
        _gasoline = 0f;
        int avgIdx = 0;

        Connect();

        _command.CommandText = string.Format(CommandCodex.SELECT_CONSUMPTION_ALCOHOL, GameManager.Instance.AvgCount);
        _reader = _command.ExecuteReader();
        while (_reader.Read())
        {
            _alcohol += _reader.GetFloat(0);
            avgIdx++;
        }
        if (avgIdx != 0)
        {
            _alcohol /= avgIdx;
        }
        _reader.Close();

        avgIdx = 0;

        _command.CommandText = string.Format(CommandCodex.SELECT_CONSUMPTION_GASOLINE, GameManager.Instance.AvgCount);
        _reader = _command.ExecuteReader();
        while (_reader.Read())
        {
            _gasoline += _reader.GetFloat(0);
            avgIdx++;
        }
        if (avgIdx != 0)
        {
            _gasoline /= avgIdx;
        }

        CloseConnection();
    }


    public void GetAllConsumptions(out List<Consumption> _con)
    {
        _con = new List<Consumption>();

        Connect();

        _command.CommandText = CommandCodex.SELECT_ALL_CONSUMPTION;
        _reader = _command.ExecuteReader();

        while (_reader.Read())
        {
            Consumption con = new()
            {
                Id = _reader.GetInt32(0),
                KmL = _reader.GetFloat(1),
                Fuel = _reader.GetInt32(2),
                Date = _reader.GetString(3)
            };
            _con.Add(con);
        }

        CloseConnection();

    }


    private void InvokeConsumptionsUpdated()
    {
        GameManager.Instance.ConsumptionsUpdated.Invoke();
    }








    private void Connect()
    {
        _connection = new SqliteConnection("URI=file:" + CommandCodex.DB_PATH);
        _connection.Open();

        _command = _connection.CreateCommand();
    }

    private void CloseConnection()
    {
        _reader?.Close();
        _command?.Dispose();
        _connection?.Close();
    }

    private static class CommandCodex
    {
#if UNITY_EDITOR
        public static readonly string DB_PATH = Application.dataPath + "/StreamingAssets/" + "InternalDataBase.db";
#else 
        public static readonly string DB_PATH = Application.persistentDataPath + "/InternalDataBase.db";
#endif

        public const string INSERT_CONSUMPTION = "INSERT INTO FuelConsumption (KmL, Fuel, Date) VALUES ({0} , {1}, date());";
        /// <summary>
        /// Insert Kilometer per litre and fuel type. Inserts 'Date' as a NULL string
        /// </summary>
        public const string INSERT_CONSUMPTION_WITHOUT_DATE = "INSERT INTO FuelConsumption (KmL, Fuel, Date) VALUES ({0} , {1}, \"NULL\");";
        public const string RESET_FUEL_REFILL = "UPDATE FuelRefill SET Volume = -1.0;";
        public const string UPDATE_FUEL_REFIL = "UPDATE FuelRefill SET Volume = ";
        public const string SELECT_ALL_FUEL_REFILL = "SELECT * FROM FuelRefill";
        public const string SELECT_CONSUMPTION_ALCOHOL = "SELECT Kml FROM FuelConsumption WHERE Fuel = 0 ORDER BY ID DESC LIMIT {0}";
        public const string SELECT_CONSUMPTION_GASOLINE = "SELECT Kml FROM FuelConsumption WHERE Fuel = 1 ORDER BY ID DESC LIMIT {0}";
        public const string SELECT_ALL_CONSUMPTION = "SELECT * FROM FuelConsumption ORDER BY ID DESC";
    }
}