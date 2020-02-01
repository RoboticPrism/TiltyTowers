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

    [SerializeField]
    private GameObject buildArea;

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
                    }
                }
                break;
            case TutorialStates.PLACE_BLOCK:
                // TODO: Check how close to buildArea
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
    }

    public void StartContinueTutorial()
    {
        placeBlockTutorial.SetActive(false);
        continueTutorial.SetActive(true);
    }

    public void StartWaitTutorial()
    {
        continueTutorial.SetActive(false);
        waitTutorial.SetActive(true);
    }

    public void StartCompletTutorial()
    {
        continueTutorial.SetActive(false);
        completeTutorial.SetActive(true);
    }

    public void EndTutorial()
    {
        Destroy(this.gameObject);
    }
}
