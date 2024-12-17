using RewardSystem;
using System.Collections.Generic;
using UnityEngine;

namespace RewardSystem
{
    /// <summary>
    /// Manages all rewards, including level-based rewards and daily login rewards.
    /// </summary>
    public class RewardManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance to ensure only one RewardManager exists.
        /// </summary>
        public static RewardManager Instance;

        [Header("Reward Configuration")]
        [Tooltip("Configuration data for level and daily rewards.")]
        public RewardConfig rewardConfig;

        /// <summary>
        /// Tracks levels whose rewards have already been claimed to prevent duplicate claims.
        /// </summary>
        private HashSet<int> claimedLevels = new HashSet<int>();

        /// <summary>
        /// Tracks the current day for daily login rewards.
        /// </summary>
        private int dailyLoginDay = 0;

        /// <summary>
        /// Ensures a singleton instance of RewardManager persists across scenes.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Make this object persistent across scenes.
            }
            else
            {
                Destroy(gameObject); // Destroy duplicate instances.
            }
        }

        #region Level Rewards

        /// <summary>
        /// Retrieves the reward for a specific level.
        /// </summary>
        /// <param name="level">The level for which to fetch the reward.</param>
        /// <returns>The reward for the specified level, or null if no reward exists.</returns>
        public Reward GetLevelReward(int level)
        {
            foreach (var lr in rewardConfig.levelRewards)
            {
                if (lr.level == level)
                    return lr.reward;
            }
            return null; // Return null if no reward is configured for the specified level.
        }

        /// <summary>
        /// Claims the reward for a specified level if it hasn't been claimed already.
        /// </summary>
        /// <param name="level">The level for which to claim the reward.</param>
        /// <returns>True if the reward is successfully claimed, false otherwise.</returns>
        public bool ClaimLevelReward(int level)
        {
            // Check if the reward has already been claimed.
            if (claimedLevels.Contains(level))
            {
                Debug.Log($"Level {level} reward already claimed.");
                return false;
            }

            // Fetch the reward for the specified level.
            Reward reward = GetLevelReward(level);
            if (reward != null)
            {
                // Deliver the reward to the player and mark it as claimed.
                GiveReward(reward);
                claimedLevels.Add(level);
                Debug.Log($"Level {level} Reward Claimed: {reward}");
                return true;
            }

            Debug.Log($"No reward found for Level {level}.");
            return false; // No reward exists for the specified level.
        }

        #endregion

        #region Daily Rewards

        /// <summary>
        /// Retrieves the daily reward for the current login day.
        /// </summary>
        /// <returns>The daily reward, or null if no more rewards are available.</returns>
        public Reward GetDailyReward()
        {
            // Check if there are still daily rewards left in the reward configuration.
            if (dailyLoginDay < rewardConfig.dailyRewards.Count)
                return rewardConfig.dailyRewards[dailyLoginDay];

            Debug.Log("No more daily rewards available.");
            return null; // No rewards left for the remaining days.
        }

        /// <summary>
        /// Claims the daily reward for the current login day.
        /// </summary>
        public void ClaimDailyReward()
        {
            // Retrieve the daily reward for the current day.
            Reward reward = GetDailyReward();
            if (reward != null)
            {
                // Deliver the reward to the player and increment the day counter.
                GiveReward(reward);
                dailyLoginDay++;
                Debug.Log($"Daily Reward Claimed: {reward}");
            }
            else
            {
                Debug.Log("No daily reward to claim.");
            }
        }

        #endregion

        #region Reward Delivery

        /// <summary>
        /// Handles the delivery of rewards to the player based on the reward type.
        /// </summary>
        /// <param name="reward">The reward to be given to the player.</param>
        private void GiveReward(Reward reward)
        {
            // Switch based on the type of reward and log the reward details.
            switch (reward.type)
            {
                case Reward.RewardType.Coins:
                    Debug.Log($"Player received {reward.quantity} Coins.");
                    break;
                case Reward.RewardType.Gems:
                    Debug.Log($"Player received {reward.quantity} Gems.");
                    break;
                case Reward.RewardType.Item:
                    Debug.Log($"Player received Item: {reward.rewardID}");
                    break;
                case Reward.RewardType.Experience:
                    Debug.Log($"Player gained {reward.quantity} XP.");
                    break;
                case Reward.RewardType.Custom:
                    Debug.Log($"Custom reward received: {reward.customData}");
                    break;
            }
        }

        #endregion
    }
}
