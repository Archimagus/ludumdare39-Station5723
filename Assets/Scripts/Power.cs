using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Power : MonoBehaviour
{
    [SerializeField]
    private bool _powerIsDepleting = false;

    [SerializeField]
    private float timeInMinutesToComplete = 5;

    private float _timeToCompleteInSeconds;
    private float _remainingTime;

    public bool gameOver = false;

    [SerializeField]
    private Image _powerIndicator;

    void Start ()
    {
        _timeToCompleteInSeconds = 60 * timeInMinutesToComplete;
        _remainingTime = _timeToCompleteInSeconds;
    }

	void Update ()
    {
        if(_powerIsDepleting)
        {
            if(_remainingTime > 0)
            {
                _remainingTime -= Time.deltaTime;

                float t = _remainingTime / _timeToCompleteInSeconds;
                _powerIndicator.fillAmount = t;
            }
            else
            {
                if(!gameOver)
                {
                    GameManager.gameplayUIManager.ShowLostPanel();
                    gameOver = true;
                }
            }
        }
	}

    /**
     * This is used to enable
     */ 
    public void SetPowerIsDepleting(bool isDepleting)
    {
        this._powerIsDepleting = isDepleting;
    }

    public void GameWon()
    {
        _powerIsDepleting = false;
        gameOver = true;

        GameManager.gameplayUIManager.ShowWonPanel();
    }

}
