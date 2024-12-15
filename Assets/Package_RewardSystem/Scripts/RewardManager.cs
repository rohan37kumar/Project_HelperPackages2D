using System;
using System.Collections.Generic;
using UnityEngine;

namespace RewardSystem
{
    public class RewardManager : MonoBehaviour
    {
        [Header("Rewards")]
        [SerializeField] private List<Reward> rewards; // All available rewards in the game

        // Keys for PlayerPrefs
        private const string LastDailyClaimKey = "RS_LastDailyRewardClaim";
        private const string LastWeeklyClaimKey = "RS_LastWeeklyRewardClaim";

        public event Action<Reward> OnRewardClaimed; // Event to notify when a reward is claimed

        /// <summary>
        /// Checks if a daily reward can be claimed.
        /// </summary>
        public bool IsDailyRewardAvailable()
        {
            string lastClaimDate = PlayerPrefs.GetString(LastDailyClaimKey, "");
            if (string.IsNullOrEmpty(lastClaimDate)) return true;

            DateTime lastClaim = DateTime.Parse(lastClaimDate);
            return DateTime.Now.Date > lastClaim.Date;
        }

        /// <summary>
        /// Checks if a weekly reward can be claimed.
        /// </summary>
        public bool IsWeeklyRewardAvailable()
        {
            string lastClaimDate = PlayerPrefs.GetString(LastWeeklyClaimKey, "");
            if (string.IsNullOrEmpty(lastClaimDate)) return true;

            DateTime lastClaim = DateTime.Parse(lastClaimDate);
            return (DateTime.Now - lastClaim).Days >= 7;
        }

        /// <summary>
        /// Claims a daily reward if available.
        /// </summary>
        public bool ClaimDailyReward()
        {
            if (!IsDailyRewardAvailable()) return false;

            Reward dailyReward = rewards.Find(r => r.type == Reward.RewardType.Daily);
            if (dailyReward != null)
            {
                GrantReward(dailyReward);
                PlayerPrefs.SetString(LastDailyClaimKey, DateTime.Now.ToString());
                PlayerPrefs.Save();
            }
            return true;
        }

        /// <summary>
        /// Claims a weekly reward if available.
        /// </summary>
        public bool ClaimWeeklyReward()
        {
            if (!IsWeeklyRewardAvailable()) return false;

            Reward weeklyReward = rewards.Find(r => r.type == Reward.RewardType.Weekly);
            if (weeklyReward != null)
            {
                GrantReward(weeklyReward);
                PlayerPrefs.SetString(LastWeeklyClaimKey, DateTime.Now.ToString());
                PlayerPrefs.Save();
            }
            return true;
        }

        /// <summary>
        /// Grants a reward to the player.
        /// </summary>
        private void GrantReward(Reward reward)
        {
            // Notify the game about the granted reward
            OnRewardClaimed?.Invoke(reward);
            Debug.Log($"Reward granted: {reward.rewardName} ({reward.value})");
        }
    }
}
