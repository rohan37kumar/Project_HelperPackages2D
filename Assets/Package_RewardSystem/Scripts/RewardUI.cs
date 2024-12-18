using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RewardSystem;

public class RewardUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Button levelRewardButton;
    public Button dailyRewardButton;
    public Button resetButton;
    public InputField levelInput;

    public TextMeshProUGUI rewardDetailsText; // Text to display reward info (requires TextMeshPro)

    private void Start()
    {
        levelRewardButton.onClick.AddListener(ClaimLevelReward);
        dailyRewardButton.onClick.AddListener(ClaimDailyReward);
        resetButton.onClick.AddListener(ResetReward);
        UpdateRewardDisplay(""); // Clear the text initially
    }

    private void OnEnable()
    {
        RewardManager.OnRewardClaimed += HandleClaimedReward;
        RewardManager.OnDailyRewardClaimed += HandleDailyReward;
    }

    private void HandleDailyReward(int day, Reward reward)
    {
        if (reward != null)
        {
            UpdateRewardDisplay($"Day {day} Reward: {reward.type} x {reward.quantity}");
        }
        else
        {
            UpdateRewardDisplay($"Reward already claimed");
        }
        
    }

    private void HandleClaimedReward(int level, Reward reward)
    {
        if (reward != null)
        {
            //RewardManager.Instance.ClaimLevelReward(level);
            UpdateRewardDisplay($"Level {level} Reward: {reward.type} x {reward.quantity}");
        }
        else
        {
            UpdateRewardDisplay($"No reward found for Level {level}");
        }
        //throw new System.NotImplementedException();
    }

    void ClaimLevelReward()
    {
        
        if (int.TryParse(levelInput.text, out int level))
        {
            RewardManager.Instance.ClaimLevelReward(level);
        }
 
    }

    void ClaimDailyReward()
    {
        RewardManager.Instance.ClaimDailyReward();
    }

    void UpdateRewardDisplay(string message)
    {
        rewardDetailsText.text = message;
    }

    void ResetReward()
    {
        RewardManager.Instance.ResetAllRewards();
        UpdateRewardDisplay("Rewards have been reset.");
    }
}
