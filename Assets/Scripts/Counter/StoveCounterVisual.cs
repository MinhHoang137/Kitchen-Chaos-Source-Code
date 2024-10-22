using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    private const string WARNING = "Warning";
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particleGameObject;
    [SerializeField] private Animator progressBarAnimator;
    [SerializeField] private Animator warningAnimator;

    private bool warning;
    private float warningSoundDuration = 0.2f;
    private float warningSoundTimer;
    private StoveCounter.State state;
	// Start is called before the first frame update
	void Start()
    {
		stoveCounter.OnStateChange += StoveCounter_OnStateChange;
		stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        warningAnimator.gameObject.SetActive(false);
    }

	private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEvenArgs e)
	{
        warning = (state == StoveCounter.State.Fried && e.progressNormalized >= 0.5f);
        warningAnimator.SetBool(WARNING, warning);
        progressBarAnimator.SetBool(WARNING, warning);
		warningAnimator.gameObject.SetActive(warning);
	}

	private void StoveCounter_OnStateChange(object sender, StoveCounter.OnStateChangeEventArgs e)
	{
        bool showVisual = (e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried);
        stoveOnGameObject.SetActive(showVisual);
        particleGameObject.SetActive(showVisual);
        state = e.state;
	}
	private void Update()
	{
		if (warning)
        {
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer < 0)
            {
                warningSoundTimer = warningSoundDuration;
                SFXManager.Instance.PlayWarningSound(transform.position);
            }
        }
	}
}
