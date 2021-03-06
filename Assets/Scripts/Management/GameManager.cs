﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private BoxCollider outOfBoundsCatcher;

    [SerializeField]
    private BoxCollider heightRequirementCollider;

    [SerializeField]
    private Timer timer;

    [SerializeField]
    private Stackable[] stackables;

    public enum GameStates { BUILD, EVALUATE, SUCCESS, FAILURE };
    [SerializeField]
    private GameStates currentState = GameStates.BUILD;

    [SerializeField]
    private float maxTimeSeconds = 5f;

    [SerializeField]
    private float currentTimerSeconds = 5f;

    [SerializeField]
    private VRButton resetPlunger;

    [SerializeField]
    private LevelLoader levelLoader;

    [SerializeField]
    private NextLevelButton nextLevelButton;

    [SerializeField]
    private MusicManager musicManager;

    [SerializeField]
    private BuildingBase buildingBase;

    [SerializeField]
    private float explosionStrenght = 10f;

    [SerializeField]
    private float explosionRadius = 5f;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        ResetConnections();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameStates.BUILD:
                // Check if any blocks are out of bounds or currently being held
                bool isReadyToEvaluate = true;
                foreach (Stackable stackable in stackables)
                {
                    if (stackable.CheckIfGrabbed())
                    {
                        isReadyToEvaluate = false;
                        break;
                    }
                    if (!stackable.CheckInBounds())
                    {
                        isReadyToEvaluate = false;
                        break;
                    }
                    if (!stackable.CheckInBuildZone())
                    {
                        isReadyToEvaluate = false;
                        break;
                    }
                }
                if (!isReadyToEvaluate)
                {
                    break;
                }
                // Otherwise its time to evaluate if the building will actually hold up
                ToEvaluateState();
                break;
            case GameStates.EVALUATE:
                currentTimerSeconds -= Time.deltaTime;
                if (currentTimerSeconds <= 0f)
                {
                    ToSuccessState();
                    break;
                }
                // Check if any blocks have fallen out of bounds or if the player grabbed one
                foreach (Stackable stackable in stackables)
                {
                    if (stackable.CheckIfGrabbed())
                    {
                        ToBuildState();
                        break;
                    } 
                    if (!stackable.CheckInBounds())
                    {
                        ToFailedState();
                        break;
                    }
                    if (!stackable.CheckInBuildZone())
                    {
                        ToFailedState();
                        break;
                    }
                }
                break;
        }
    }

    public GameStates GetCurrentGameState()
    {
        return currentState;
    }

    void ToBuildState()
    {
        currentTimerSeconds = maxTimeSeconds;
        timer.StopTimer();
        currentState = GameStates.BUILD;
        musicManager.PlayGameMusic();
    }

    void ToEvaluateState()
    {
        currentTimerSeconds = maxTimeSeconds;
        timer.StartTimer();
        currentState = GameStates.EVALUATE;
        musicManager.PlayEvalMusic();
    }

    void ToSuccessState()
    {
        foreach (Stackable stackable in stackables)
        {
            stackable.MakeStatic();
        }
        currentTimerSeconds = 0f;
        timer.StopTimer();
        currentState = GameStates.SUCCESS;
        buildingBase.CreateSuccess();
        nextLevelButton.gameObject.SetActive(true);
        musicManager.PlayWinMusic();
    }

    void ToFailedState()
    {
        currentState = GameStates.FAILURE;
        timer.StopTimer();
        musicManager.PlayFailMusic();
    }

    public void StartReset()
    {
        foreach (Stackable stackable in stackables)
        {
            stackable.GetComponent<Rigidbody>().AddExplosionForce(explosionStrenght, buildingBase.GetExplosionPosition().position, explosionRadius, 3f);
        }
        currentState = GameStates.BUILD;
        timer.StopTimer();
        buildingBase.CreateExplosion();
        StartCoroutine("ResetCoroutine");
    }

    void Reset()
    {
        foreach (Stackable stackable in stackables)
        {
            stackable.Reset();
        }
        resetPlunger.Reset();
        ToBuildState();
    }

    IEnumerator ResetCoroutine()
    {
        float delaySeconds = 2;
        while (delaySeconds > 0f)
        {
            delaySeconds -= Time.deltaTime;
            yield return null;
        }
        Reset();
    }

    public void ResetConnections()
    {
        stackables = GameObject.FindObjectsOfType<Stackable>();
        outOfBoundsCatcher = GameObject.FindObjectOfType<OutOfBoundsCatcher>().GetComponent<BoxCollider>();
        timer = GameObject.FindObjectOfType<Timer>();
        resetPlunger = GameObject.FindObjectOfType<VRButton>();
        buildingBase = GameObject.FindObjectOfType<BuildingBase>();
        musicManager = GameObject.FindObjectOfType<MusicManager>();
        // weird hack to find inactive objects
        nextLevelButton = Resources.FindObjectsOfTypeAll<NextLevelButton>()[0];
        ToBuildState();
    }

    public float GetMaxTimeSeconds()
    {
        return maxTimeSeconds;
    }

    public float GetCurrentTimeSeconds()
    {
        return currentTimerSeconds;
    }
}
