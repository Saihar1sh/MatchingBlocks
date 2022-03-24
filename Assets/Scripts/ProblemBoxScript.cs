using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProblemBoxScript : MonoBehaviour
{
    private BlockController[] blockControllers;

    private List<BlockController> solvedProblemBlocks;

    [Tooltip("0- black, 1- green, 2- white")]
    public Sprite[] fixingLines;            //0- black, 1- green, 2- white

    private BlockService blockService;
    private Transform[,] strParents;

    private Transform[] solBoxes;

    public int solvedBlocksCount;

    private void Awake()
    {
        #region Init
        blockService = BlockService.Instance;
        blockControllers = new BlockController[3];
        solBoxes = blockService.solBoxs;
        strParents = new Transform[solBoxes.Length, 3];
        blockControllers = GetComponentsInChildren<BlockController>();
        solvedProblemBlocks = new List<BlockController>(3);
        #endregion

        //sending x positions of blocks in problemBox
        for (int i = 0; i < blockControllers.Length; i++)
        {
            blockService.problemBlockPos[i] = blockControllers[i].transform.position;
        }

        solvedBlocksCount = 0;

    }
    public Transform AddNewStripes(int solBoxIndex,int problemBlockIndex, List<float> xPositions)
    {
        for (int i = 0; i < blockControllers[problemBlockIndex].stripes.Length; i++)
        {
            //disabling all stripes and only necessary stripes will be enabled
             blockControllers[problemBlockIndex].newStripes[solBoxIndex, i].gameObject.SetActive(false);
        }

        //assigning stripe parent's transform
        strParents[solBoxIndex,problemBlockIndex] = blockControllers[problemBlockIndex].stripeParent[solBoxIndex].transform;

        Vector3 pos = strParents[solBoxIndex, problemBlockIndex].localPosition;
        pos.y = 0;
        pos.z = -1;
        strParents[solBoxIndex, problemBlockIndex].localPosition = pos;
        strParents[solBoxIndex, problemBlockIndex].gameObject.SetActive(true);

        for (int i = 0; i < xPositions.Capacity; i++)
        {
            blockControllers[problemBlockIndex].newStripes[solBoxIndex, i].gameObject.SetActive(true);
            StripeController stripe = blockControllers[problemBlockIndex].newStripes[solBoxIndex, i];
            Vector3 strPos = stripe.transform.localPosition;
            strPos.x = xPositions[i];

            strPos.y = 0;
            stripe.transform.localPosition = strPos;

            //stripes postions will be compared to their specific problem block and assigns appropriate color
            BlockStripesCheck(solBoxIndex, problemBlockIndex, i, stripe);           

        }
        return strParents[solBoxIndex, problemBlockIndex];
    }

    private void BlockStripesCheck(int solBoxIndex, int problemBlockIndex, int i, StripeController stripe)
    {
        //checking if block's stripe location matches with problemBlock's stripe location
        if (blockControllers[problemBlockIndex].stripeObjectsXpos.Contains(stripe.transform.localPosition.x))
        {                                                                                                   //color assigning
            blockControllers[problemBlockIndex].stripeSprites[solBoxIndex, i].sprite = fixingLines[1];               //green line
            blockControllers[problemBlockIndex].stripeSprites[solBoxIndex, i].color = Color.white;
            solvedBlocksCount++;
        }
        else
        {
            blockControllers[problemBlockIndex].stripeSprites[solBoxIndex, i].color = Color.red;               //red line
            if (solvedProblemBlocks.Contains(blockControllers[problemBlockIndex]))
            {
                solvedProblemBlocks.Remove(blockControllers[problemBlockIndex]);

            }

        }
    }
}
