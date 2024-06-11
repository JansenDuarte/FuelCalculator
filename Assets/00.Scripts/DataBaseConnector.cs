using UnityEngine;
using Mono.Data.Sqlite;
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
            DontDestroyOnLoad(gameObject);
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion // SIMPLE_SINGLETON



    IDbConnection _connection;
    IDbCommand _command;
    IDataReader _reader;




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

    public void SaveFuelConsumption(float _volume, float _kilometer, int _fuelType)
    {
        if (_kilometer <= 0f)
        {
            SaveOnlyFuel(_volume);
            return;
        }

        float KmPerL = _kilometer / _volume;

        Connect();

        _command.CommandText = string.Format(CommandCodex.INSERT_CONSUMPTION, KmPerL.ToString(CultureInfo.InvariantCulture), _fuelType.ToString());
        _command.ExecuteNonQuery();

        _command.CommandText = CommandCodex.RESET_FUEL_REFILL;
        _command.ExecuteNonQuery();

        CloseConnection();
    }

    private void SaveOnlyFuel(float _volume)
    {
        Connect();

        _command.CommandText = CommandCodex.UPDATE_FUEL_REFIL + _volume;
        _command.ExecuteNonQuery();

        CloseConnection();
    }


    public void GetAverageConsumption(out float _alcohol, out float _gasoline)
    {
        _alcohol = 0f;
        _gasoline = 0f;
        int avgIdx = 0;

        Connect();

        _command.CommandText = string.Format(CommandCodex.SELECT_CONSUMPTION_ALCOHOL, 5);
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

        _command.CommandText = string.Format(CommandCodex.SELECT_CONSUMPTION_GASOLINE, 5);
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








    private void Connect()
    {
        _connection = new SqliteConnection(CommandCodex.DB_NAME);
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
        public static readonly string DB_NAME = "URI=file:" + Application.dataPath + "/99.DataBase/" + "InternalDataBase.db";

        public const string INSERT_CONSUMPTION = "INSERT INTO FuelConsumption (KmL, Fuel) VALUES ({0} , {1})";
        public const string RESET_FUEL_REFILL = "UPDATE FuelRefill SET Volume = -1.0";
        public const string UPDATE_FUEL_REFIL = "UPDATE FuelRefill SET Volume = ";
        public const string SELECT_ALL_FUEL_REFILL = "SELECT * FROM FuelRefill";
        public const string SELECT_CONSUMPTION_ALCOHOL = "SELECT Kml FROM FuelConsumption WHERE Fuel = 0 ORDER BY ID DESC LIMIT {0}";
        public const string SELECT_CONSUMPTION_GASOLINE = "SELECT Kml FROM FuelConsumption WHERE Fuel = 1 ORDER BY ID DESC LIMIT {0}";
    }

}