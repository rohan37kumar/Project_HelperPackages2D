using PlayerProgress;
using RewardSystem;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        public static event Action<int, Reward> OnRewardClaimed;

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

        public Reward FetchLevelReward(int level)
        {
            Reward reward = rewardConfig.levelRewards[level].reward;

            return reward;
        }

        public void ClaimLevelReward(int level)
        {
            if (IsLevelRewardClaimed(level))
            {
                Debug.Log("Reward for this level is claimed");
                return; 
            }
            Reward reward = FetchLevelReward(level); //fetch the reward
            switch(reward.type) //Claim it to player depending on type
            {
                case Reward.RewardType.Coins: PlayerProgressManager.UpdateCoins(reward.quantity); break;
                case Reward.RewardType.Gems: PlayerProgressManager.UpdateGems(reward.quantity); break;
                case Reward.RewardType.Experience: PlayerProgressManager.UpdateExperience(reward.quantity); break;
            }
            claimedLevels.Add(level);

            OnRewardClaimed?.Invoke(level, reward);
            
        }

        public bool IsLevelRewardClaimed(int level)
        {
            //check if reward is available
            return claimedLevels.Contains(level); 
        }

        public bool IsRewardAvailable()
        {
            //This function should be able to check the availability of any type of reward
            return false;
        }


    }
}
