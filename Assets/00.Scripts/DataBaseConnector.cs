using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.Data;
using System.Globalization;

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
            PrepareDbURL();
            DontDestroyOnLoad(gameObject);
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion // SIMPLE_SINGLETON

    const string DB_NAME = "InternalDataBase.db";

    private string _dbURL;
    IDbConnection _connection;
    IDbCommand _command;

    private void PrepareDbURL()
    {
        _dbURL = "URI=file:" + Application.dataPath + "/99.DataBase/" + DB_NAME;
    }




    public float GetFuelRefill()
    {
        float value = -1f;

        Connect();

        _command = _connection.CreateCommand();
        _command.CommandText = "SELECT * FROM FuelRefill";

        IDataReader reader = _command.ExecuteReader();

        while (reader.Read())
        {
            value = reader.GetFloat(1);
        }

        reader.Close();
        CloseConnection();

        return value;
    }

    public void SaveFuelConsumption(float _volume, float _kilometer, int _fuelType)
    {
        if (_kilometer <= 0f)
        {
            SaveOnlyFuel(_volume);
            return;
        }

        float KmPerL = _kilometer / _volume;

        Connect();

        _command = _connection.CreateCommand();
        _command.CommandText = string.Format("INSERT INTO FuelConsumption (KmL, Fuel) VALUES ({0} , {1})", KmPerL.ToString(CultureInfo.InvariantCulture), _fuelType.ToString());
        Debug.Log(_command.CommandText);
        _command.ExecuteNonQuery();

        _command.CommandText = "UPDATE FuelRefill SET Volume = -1.0";
        _command.ExecuteNonQuery();

        CloseConnection();

    }

    private void SaveOnlyFuel(float _volume)
    {
        Connect();

        _command = _connection.CreateCommand();
        _command.CommandText = "UPDATE FuelRefill SET Volume = " + _volume;

        _command.ExecuteNonQuery();

        CloseConnection();
    }

    private void Connect()
    {
        _connection = new SqliteConnection(_dbURL);
        _connection.Open();
    }

    private void CloseConnection()
    {
        if (_connection != null)
        {
            _connection.Close();
        }
    }


}
