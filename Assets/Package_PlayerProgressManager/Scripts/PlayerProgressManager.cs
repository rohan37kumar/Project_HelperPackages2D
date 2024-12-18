using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerProgress
{
    public static class PlayerProgressManager
    {
        //private const string TotalLevelsCompletedKey = "MainLevelsCompleted";
        //private const string TotalSubLevelsCompletedKey = "SubLevelsCompleted";
        private const string MainLevelKeyPrefix = "MainLevel_";
        private const string SubLevelKeyPrefix = "SubLevel_";
        private const string UsernameKey = "Username";
        private const string LastLoginKey = "LastLogin";
        private const string LatestLoginKey = "LatestLogin";
        private const string ExperienceKey = "Xp_";
        private const string CoinsKey = "Coins_";
        private const string GemsKey = "Gems_";
        private const string lastClaimedRewardKey = "Reward_Claim_DateTime";
        private const string DailyRewardIndexKey = "DailyRewardIndex";

        #region Username

        /// <summary>
        /// Set the username for the player.
        /// </summary>
        public static void SetUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                Debug.LogWarning("Username cannot be empty or null.");
                return;
            }

            PlayerPrefs.SetString(UsernameKey, username);
            PlayerPrefs.Save();
            Debug.Log($"Username set to: {username}");
        }

        /// <summary>
        /// Get the username of the player.
        /// </summary>
        public static string GetUsername()
        {
            return PlayerPrefs.GetString(UsernameKey, "Guest");
        }

        /// <summary>
        /// Reset the username to default.
        /// </summary>
        public static void ResetUsername()
        {
            PlayerPrefs.DeleteKey(UsernameKey);
            PlayerPrefs.Save();
            Debug.Log("Username reset to default.");
        }

        #endregion

        #region MainLevel

        /// <summary>
        /// Call this method when a main level is completed.
        /// </summary>
        public static void MainLevelCompleted(string mainLevelID)
        {
            string mainLevelKey = MainLevelKeyPrefix + mainLevelID;
            PlayerPrefs.SetInt(mainLevelKey, 1);
            PlayerPrefs.Save();
        }
        /// <summary>
        /// Check if a specific main level is completed.
        /// </summary>
        public static bool HasCompletedMainLevel(string mainLevelID)
        {
            string mainLevelKey = MainLevelKeyPrefix + mainLevelID;
            return PlayerPrefs.GetInt(mainLevelKey, 0) == 1;
        }

        #endregion

        #region Sub Levels

        /// <summary>
        /// Call this method when a sub-level is completed.
        /// </summary>
        public static void CompleteSubLevel(string subLevelID)
        {
            string subLevelKey = SubLevelKeyPrefix + subLevelID;
            PlayerPrefs.SetInt(subLevelKey, 1);
            PlayerPrefs.Save();
        }
        /// <summary>
        /// Check if a specific sub-level is completed.
        /// </summary>
        public static bool HasCompletedSubLevel(string subLevelID)
        {
            string subLevelKey = SubLevelKeyPrefix + subLevelID;
            return PlayerPrefs.GetInt(subLevelKey, 0) == 1;
        }

        #endregion

        #region Login Tracking
        /// <summary>
        /// Call this method to save the login timestamps.
        /// </summary>

        public static void UpdateLoginTime()
        {
            string currentLoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (PlayerPrefs.HasKey(LatestLoginKey))
            {
                string previousLogin = PlayerPrefs.GetString(LatestLoginKey);
                PlayerPrefs.SetString(LastLoginKey, previousLogin);
            }

            PlayerPrefs.SetString(LatestLoginKey, currentLoginTime);

            PlayerPrefs.Save();
        }

        /// <summary>
        /// Get the last login time (before the most recent login).
        /// </summary>
        public static string GetLastLoginTime()
        {
            return PlayerPrefs.GetString(LastLoginKey, "No previous login recorded.");
        }

        /// <summary>
        /// Get the latest login time (most recent login).
        /// </summary>
        public static string GetLatestLoginTime()
        {
            return PlayerPrefs.GetString(LatestLoginKey, "No login recorded yet.");
        }

        #endregion

        #region Coins tracking

        public static void UpdateCoins(int coinsCount)
        {
            PlayerPrefs.SetInt(CoinsKey, coinsCount);
            PlayerPrefs.Save();
        }

        public static int GetCoinsCount()
        {
            return PlayerPrefs.GetInt(CoinsKey);
        }

        #endregion

        #region Gems tracking

        public static void UpdateGems(int gemsCount)
        {
            PlayerPrefs.SetInt(GemsKey, gemsCount);
            PlayerPrefs.Save();
        }

        public static int GetGemsCount()
        {
            return PlayerPrefs.GetInt(GemsKey);
        }

        #endregion

        #region Exp tracking

        public static void UpdateExperience(int Xp)
        {
            PlayerPrefs.SetInt(ExperienceKey, Xp);
            PlayerPrefs.Save();
        }

        public static int GetExperience()
        {
            return PlayerPrefs.GetInt(ExperienceKey);
        }

        #endregion

        #region Reward Tracking

        public static void SetLastRewardClaimedDateTime(DateTime dateTime)
        {
            PlayerPrefs.SetString(lastClaimedRewardKey, dateTime.ToString("o"));
        }

        public static string GetLastRewardClaimedDateTime()
        {
            return PlayerPrefs.GetString(lastClaimedRewardKey);
        }

        public static void SetDailyRewardIndex(int index)
        {
            PlayerPrefs.SetInt(DailyRewardIndexKey, index);
            PlayerPrefs.Save();
        }

        public static int GetDailyRewardIndex()
        {
            return PlayerPrefs.GetInt(DailyRewardIndexKey, 0);
        }

        public static bool HasLastRewardClaimedDateTime()
        {
            return PlayerPrefs.HasKey(lastClaimedRewardKey);
        }


        #endregion
    }
}
