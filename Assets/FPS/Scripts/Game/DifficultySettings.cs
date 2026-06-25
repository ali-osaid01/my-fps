using UnityEngine;

namespace Unity.FPS.Game
{
    public enum GameDifficulty
    {
        Easy,
        Medium,
        Hard,
    }

    public static class DifficultySettings
    {
        const string k_PlayerPrefsKey = "SelectedDifficulty";

        public static GameDifficulty SelectedDifficulty { get; private set; } = GameDifficulty.Medium;

        static DifficultySettings()
        {
            SelectedDifficulty = LoadDifficulty();
        }

        public static void SetDifficulty(GameDifficulty difficulty)
        {
            SelectedDifficulty = difficulty;
            PlayerPrefs.SetInt(k_PlayerPrefsKey, (int)difficulty);
            PlayerPrefs.Save();
        }

        static GameDifficulty LoadDifficulty()
        {
            int savedValue = PlayerPrefs.GetInt(k_PlayerPrefsKey, (int)GameDifficulty.Medium);

            if (savedValue < (int)GameDifficulty.Easy || savedValue > (int)GameDifficulty.Hard)
                return GameDifficulty.Medium;

            return (GameDifficulty)savedValue;
        }
    }
}
