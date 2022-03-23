using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private BlockService blockService;

    private BlockController[] problemBlocks, movableBlocks;

    private int problemBlockTotalStripes;

    private bool onlyOnce = true;

    public ProblemBoxScript problemBoxScript;

    public Transform movableBlocksParent;

    // Start is called before the first frame update
    void Start()
    {
        blockService = BlockService.Instance;
        problemBlocks = problemBoxScript.transform.GetComponentsInChildren<BlockController>();
        movableBlocks = movableBlocksParent.transform.GetComponentsInChildren<BlockController>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < problemBlocks.Length; i++)
        {
            if(problemBlocks[i].addedStripes && onlyOnce)
            {
                problemBlockTotalStripes += problemBlocks[i].stripes.Length;

                if(i == problemBlocks.Length - 1)                           //resetting when in last block and added stripes
                {
                    onlyOnce = false;
                }
            }

        }
    }

    public void ResetLevel()
    {
        problemBlockTotalStripes = 0;
        onlyOnce = true;
    }
}
