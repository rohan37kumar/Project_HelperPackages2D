using System.Collections.Generic;
using UnityEngine;


namespace RewardSystem
{
    [CreateAssetMenu(fileName = "NewRewardConfig", menuName = "Reward System/Reward Configuration")]
    public class RewardConfig : ScriptableObject
    {
        [Header("Level Rewards")]
        public List<LevelReward> levelRewards;

        [Header("Daily Rewards")]
        public List<Reward> dailyRewards; // List of daily rewards (ordered by day)

        [Header("Weekly Rewards")]
        public List<Reward> weeklyRewards; // List of daily rewards (ordered by day)

        [System.Serializable]
        public class LevelReward
        {
            public int level;
            public Reward reward; // Reference to the Reward ScriptableObject
        }
    }
}