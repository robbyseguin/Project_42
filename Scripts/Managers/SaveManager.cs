using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Managers;
using UI.Menu.MainMenu;
using UnityEngine;

public static class SaveManager
{
    private static readonly string _filePath = Application.persistentDataPath + "/save.data";
    private static GameData _saveData;

    private static string _username;

    public static void SaveGame(string username = "")
    {
        _saveData ??= new GameData();
        
        _saveData.Username = username is "" ? _username : username;
        _saveData.Statistics = StatisticsManager.Instance._sessionStatistics;
        
        FileStream dataStream = new FileStream(_filePath, FileMode.Create);

        BinaryFormatter converter = new BinaryFormatter();
        converter.Serialize(dataStream, _saveData);

        dataStream.Close();

        if(GameManager.Instance.Score > 0)
            LeaderboardMenu.SetLeaderboardEntry(_saveData.Username, GameManager.Instance.Score);
    }

    public static void LoadGame()
    {
        if(File.Exists(_filePath))
        {
            FileStream dataStream = new FileStream(_filePath, FileMode.Open);

            BinaryFormatter converter = new BinaryFormatter();
            _saveData = converter.Deserialize(dataStream) as GameData;

            dataStream.Close();

            StatisticsManager.Instance._sessionStatistics = _saveData.Statistics;
            LocalizationManager.Instance.LoadLocalization();
            _username = _saveData.Username;
        }
        else
        {
            _saveData = new GameData();
        }
    }
    
    [System.Serializable]
    private class GameData
    {
        public string Username;
        public int[] Statistics;
    }
}
