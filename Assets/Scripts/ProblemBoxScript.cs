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
        blockService = BlockService.Instance;
        blockControllers = new BlockController[3];
        solBoxes = blockService.solBoxs;
        strParents = new Transform[solBoxes.Length, 3];
        blockControllers = GetComponentsInChildren<BlockController>();
        solvedProblemBlocks = new List<BlockController>(3);
        //blockService.AssignProblemBlocks(blockControllers);
        //sending x positions of blocks in problemBox
        for (int i = 0; i < blockControllers.Length; i++)
        {
            blockService.problemBlockPos[i] = blockControllers[i].transform.position;
            //StripeParentObjectInstantiation(i);
        }

        solvedBlocksCount = 0;

    }
    public Transform AddNewStripes(int solBoxIndex,int problemBlockIndex, List<float> xPositions)
    {
        blockControllers[problemBlockIndex].addedStripes = false;

        for (int i = 0; i < blockControllers[problemBlockIndex].stripes.Length; i++)
        {
             blockControllers[problemBlockIndex].newStripes[solBoxIndex, i].gameObject.SetActive(false);
        }


        strParents[solBoxIndex,problemBlockIndex] = blockControllers[problemBlockIndex].stripeParent[solBoxIndex].transform;

        Vector3 pos = strParents[solBoxIndex, problemBlockIndex].localPosition;
        pos.y = 0;
        pos.z = -1;
        strParents[solBoxIndex, problemBlockIndex].localPosition = pos;
        strParents[solBoxIndex, problemBlockIndex].gameObject.SetActive(true);
        for (int i = 0; i < xPositions.Capacity; i++)
        {
            blockControllers[problemBlockIndex].newStripes[solBoxIndex,i].gameObject.SetActive(true);
            StripeController stripe = blockControllers[problemBlockIndex].newStripes[solBoxIndex, i];
            Vector3 strPos = stripe.transform.localPosition;
            strPos.x = xPositions[i];

            strPos.y = 0;
            stripe.transform.localPosition = strPos;
            Debug.Log("sdh" + stripe.transform.localPosition);

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
        blockControllers[problemBlockIndex].addedStripes = true;
        return strParents[solBoxIndex, problemBlockIndex];
    }

}
