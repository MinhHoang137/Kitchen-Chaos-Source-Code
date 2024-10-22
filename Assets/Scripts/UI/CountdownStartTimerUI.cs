using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownStartTimerUI : MonoBehaviour
{
    private const string ONE_SECOND = "OneSecond";
    [SerializeField] private TextMeshProUGUI countdownTimer;
    [SerializeField] private Animator animator;
    private int previousTimerInt = 0;
    private int lastTimerInt;
    // Start is called before the first frame update
    void Start()
    {
		GameManager.Instance.OnStateChange += GameManager_OnStateChange;
        Hide();
    }

	private void GameManager_OnStateChange(object sender, System.EventArgs e)
	{
        if (GameManager.Instance.IsCountdownToStart())
        {
            Show();
        }
        else
        {
            Hide();
        }
	}
	private void Update()
	{
		if (lastTimerInt != previousTimerInt)
		{
			previousTimerInt = lastTimerInt;
			animator.SetTrigger(ONE_SECOND);
		}
		lastTimerInt = Mathf.CeilToInt(GameManager.Instance.GetCountdownToStartTimer());
		countdownTimer.text = lastTimerInt.ToString();
	}
    private void Show()
    {
        gameObject.SetActive(true);
        countdownTimer.gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
