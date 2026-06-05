using System;
using System.IO;
using System.Text.Json;

namespace Asteroid_Run
{
    public class SaveData
    {
        public int HighScore { get; set; }
        public int HighDistance { get; set; }
    }

    public static class SaveManager
    {
        private static readonly string filePath = "savegame.json";

        public static SaveData Load()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    return JsonSerializer.Deserialize<SaveData>(json);
                }
                catch
                {
                    return new SaveData { HighScore = 0, HighDistance = 0 };
                }
            }

            return new SaveData { HighScore = 0, HighDistance = 0 };
        }

        public static void Save(SaveData data)
        {
            try
            {
                string json = JsonSerializer.Serialize(data);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении: " + ex.Message);
            }
        }
    }
}
