using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public ProblemBoxScript problemBoxScript;

    public Transform movableBlocksParent;

    [SerializeField]
    private Button skipLevelBtn, startBtn;

    [SerializeField]
    private Image startPanel, levelCompletedPanel;

    private BlockService blockService;

    private BlockController[] problemBlocks, movableBlocks;

    private int problemBlockTotalStripes;

    private int currentSceneIndex;


    private void Awake()
    {
        skipLevelBtn.onClick.AddListener(SkipLevel);
    }

    // Start is called before the first frame update
    void Start()
    {
        blockService = BlockService.Instance;

        problemBlocks = problemBoxScript.transform.GetComponentsInChildren<BlockController>();
        movableBlocks = movableBlocksParent.transform.GetComponentsInChildren<BlockController>();

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        startPanel.gameObject.SetActive(false);         //just to make sure it is disabled
        if (currentSceneIndex == 0)
        {
            startPanel.gameObject.SetActive(true);
            startBtn.onClick.AddListener(StartButtonFunc);

        }
        levelCompletedPanel.gameObject.SetActive(false);

        //using many invokes will be a problem. Should check for alternatives while integrating into bigger game
        Invoke("ProblemBlockStripesCounting", .1f);             //giving time for stripes instantiation
    }

    // Update is called once per frame
    void Update()
    {
        if (problemBlockTotalStripes == problemBoxScript.solvedBlocksCount && problemBlockTotalStripes != 0)
        {
            Debug.Log("Next Level");
            LevelComplete();
        }
        Debug.Log("problem total:" + problemBlockTotalStripes);
        Debug.Log("problem solved total:" + problemBoxScript.solvedBlocksCount);
    }

    private void ProblemBlockStripesCounting()
    {
        for (int i = 0; i < problemBlocks.Length; i++)
        {
            problemBlockTotalStripes += problemBlocks[i].stripes.Length;

        }
        Debug.Log("Completed");
    }

    public void StartButtonFunc()
    {
        startPanel.gameObject.SetActive(false);

    }

    public void LevelComplete()
    {
        levelCompletedPanel.gameObject.SetActive(true);
        Invoke("LoadNextScene", 1f);
    }

    private void LoadNextScene()
    {
        if (currentSceneIndex != 2)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);

        }
    }

    public void SkipLevel()
    {
        for (int i = 0; i < movableBlocks.Length; i++)
        {
            movableBlocks[i].SetSolBlockPosition();

        }
        //LevelComplete();
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }
}
