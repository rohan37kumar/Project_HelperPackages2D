using UnityEngine;
using UnityEngine.UI;

namespace RewardSystem
{
    public class RewardUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RewardManager rewardManager;
        [SerializeField] private Button dailyRewardButton;
        [SerializeField] private Button weeklyRewardButton;
        [SerializeField] private Text dailyRewardStatusText;
        [SerializeField] private Text weeklyRewardStatusText;

        private void Start()
        {
            // Attach button listeners
            dailyRewardButton.onClick.AddListener(() =>
            {
                if (rewardManager.ClaimDailyReward())
                {
                    UpdateUI();
                }
            });

            weeklyRewardButton.onClick.AddListener(() =>
            {
                if (rewardManager.ClaimWeeklyReward())
                {
                    UpdateUI();
                }
            });

            UpdateUI();
        }

        private void UpdateUI()
        {
            // Update daily reward button and text
            dailyRewardButton.interactable = rewardManager.IsDailyRewardAvailable();
            dailyRewardStatusText.text = rewardManager.IsDailyRewardAvailable()
                ? "Daily reward available!"
                : "Daily reward already claimed.";

            // Update weekly reward button and text
            weeklyRewardButton.interactable = rewardManager.IsWeeklyRewardAvailable();
            weeklyRewardStatusText.text = rewardManager.IsWeeklyRewardAvailable()
                ? "Weekly reward available!"
                : "Weekly reward already claimed.";
        }
    }
}
