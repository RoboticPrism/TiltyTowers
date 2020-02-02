using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private BoxCollider outOfBoundsCatcher;

    [SerializeField]
    private BoxCollider heightRequirementCollider;

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

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        stackables = GameObject.FindObjectsOfType<Stackable>();
        resetPlunger = GameObject.FindObjectOfType<VRButton>();
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
        currentState = GameStates.BUILD;
    }

    void ToEvaluateState()
    {
        currentTimerSeconds = maxTimeSeconds;
        currentState = GameStates.EVALUATE;
    }

    void ToSuccessState()
    {
        foreach (Stackable stackable in stackables)
        {
            stackable.MakeStatic();
        }
        currentTimerSeconds = 0f;
        currentState = GameStates.SUCCESS;
        levelLoader.NextScene();
    }

    void ToFailedState()
    {
        currentState = GameStates.FAILURE;
    }

    public void Reset()
    {
        Debug.Log("Reset");
        foreach (Stackable stackable in stackables)
        {
            stackable.Reset();
        }
        resetPlunger.Reset();
    }

    public void ResetConnections()
    {
        stackables = GameObject.FindObjectsOfType<Stackable>();
        resetPlunger = GameObject.FindObjectOfType<VRButton>();
    }
}
