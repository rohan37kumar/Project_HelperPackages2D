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

        private int dailyRewardIndex;

        public static event Action<int, Reward> OnRewardClaimed;
        public static event Action<int, Reward> OnDailyRewardClaimed;

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

        private void Start()
        {
            dailyRewardIndex = PlayerProgressManager.GetDailyRewardIndex();
        }


        public void DeliverRewardToPlayer(Reward reward)
        {
            switch (reward.type) //Claim it to player depending on type
            {
                case Reward.RewardType.Coins: PlayerProgressManager.UpdateCoins(reward.quantity); break;
                case Reward.RewardType.Gems: PlayerProgressManager.UpdateGems(reward.quantity); break;
                case Reward.RewardType.Experience: PlayerProgressManager.UpdateExperience(reward.quantity); break;
            }
        }

        #region Level Rewards
        public Reward FetchLevelReward(int level)
        {
            return rewardConfig.levelRewards[level].reward; 
        }

        public void ClaimLevelReward(int level)
        {
            if (IsLevelRewardClaimed(level))
            {
                Debug.Log("Reward for this level is claimed");
                return; 
            }
            Reward reward = FetchLevelReward(level); //fetch the reward
            DeliverRewardToPlayer(reward);
            claimedLevels.Add(level);
            OnRewardClaimed?.Invoke(level, reward);
            
        }

        public bool IsLevelRewardClaimed(int level)
        {
            //check if reward is already claimed
            return claimedLevels.Contains(level); 
        }

        #endregion

        #region DailyRewards

        public Reward FetchDailyReward(int index)
        {
            int loopedIndex = index % rewardConfig.dailyRewards.Count; // Loop back to start
            return rewardConfig.dailyRewards[loopedIndex];
        }


        public void ClaimDailyReward()
        {
            if (!IsDailyRewardAvailable())
            {
                Debug.Log("Reward for this Day is claimed");
                return; 
            }
            Reward reward = FetchDailyReward(dailyRewardIndex);
            DeliverRewardToPlayer(reward);
            PlayerProgressManager.SetLastRewardClaimedDateTime(DateTime.Now);
            PlayerProgressManager.SetDailyRewardIndex(++dailyRewardIndex);
            OnDailyRewardClaimed?.Invoke(dailyRewardIndex, reward);
        }



        public bool IsDailyRewardAvailable()
        {
            if (!PlayerProgressManager.HasLastRewardClaimedDateTime())
            {
                // First time login or no prior claim
                return true;
            }

            DateTime currentDateTime = DateTime.Now;
            DateTime lastRewardClaimedDateTime = DateTime.Parse(PlayerProgressManager.GetLastRewardClaimedDateTime());

            Double elapsedTime = (currentDateTime - lastRewardClaimedDateTime).TotalDays;

            if (elapsedTime >= rewardConfig.rewardDelay)
            {
                
                return true;
            }

            return false;
        }

        #endregion


        public void ResetAllRewards()
        {
            // Reset PlayerPrefs
            PlayerPrefs.DeleteAll();

            // Reset runtime variables
            dailyRewardIndex = 0;
            claimedLevels.Clear();

            Debug.Log("All rewards and progress have been reset.");
        }


    }
}
