using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    [SerializeField]
    private GameObject grabBlockTutorial;
    [SerializeField]
    private GameObject placeBlockTutorial;
    [SerializeField]
    private GameObject continueTutorial;
    [SerializeField]
    private GameObject waitTutorial;
    [SerializeField]
    private GameObject completeTutorial;

    enum TutorialStates { GRAB_BLOCK, PLACE_BLOCK, CONTINUE, WAIT, COMPLETE };
    [SerializeField]
    private TutorialStates currentTutorialState = TutorialStates.GRAB_BLOCK;

    private GameManager gameManager;

    private Stackable[] stackables;

    // Start is called before the first frame update
    void Start()
    {
        stackables = GameObject.FindObjectsOfType<Stackable>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentTutorialState)
        {
            case TutorialStates.GRAB_BLOCK:
                foreach (Stackable stackable in stackables)
                {
                    if (stackable.CheckIfGrabbed())
                    {
                        StartPlaceTutorial();
                        break;
                    }
                }
                break;
            case TutorialStates.PLACE_BLOCK:
                foreach (Stackable stackable in stackables)
                {
                    if (stackable.CheckInBuildZone() && !stackable.CheckIfGrabbed())
                    {
                        StartContinueTutorial();
                        break;
                    }
                }
                break;
            case TutorialStates.CONTINUE:
                if (gameManager.GetCurrentGameState() == GameManager.GameStates.EVALUATE)
                {
                    StartWaitTutorial();
                }
                break;
            case TutorialStates.WAIT:
                if (gameManager.GetCurrentGameState() == GameManager.GameStates.SUCCESS)
                {
                    StartCompletTutorial();
                }
                break;

        }
    }

    public void StartPlaceTutorial()
    {
        grabBlockTutorial.SetActive(false);
        placeBlockTutorial.SetActive(true);
        currentTutorialState = TutorialStates.PLACE_BLOCK;
    }

    public void StartContinueTutorial()
    {
        placeBlockTutorial.SetActive(false);
        continueTutorial.SetActive(true);
        currentTutorialState = TutorialStates.CONTINUE;
    }

    public void StartWaitTutorial()
    {
        continueTutorial.SetActive(false);
        waitTutorial.SetActive(true);
        currentTutorialState = TutorialStates.WAIT;
    }

    public void StartCompletTutorial()
    {
        waitTutorial.SetActive(false);
        completeTutorial.SetActive(true);
        currentTutorialState = TutorialStates.CONTINUE;
    }

    public void EndTutorial()
    {
        Destroy(this.gameObject);
    }
}
