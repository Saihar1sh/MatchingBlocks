using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemBoxScript : MonoBehaviour
{
    private BlockController[] blockControllers;

    [Tooltip("0- black, 1- green, 2- white")]
    public Sprite[] fixingLines;            //0- black, 1- green, 2- white

    public StripeController stripePrefab;

    private BlockService blockService;

    GameObject[] stripesParent;

    private void Awake()
    {
        blockService = BlockService.Instance;
        blockControllers = new BlockController[3];
        stripesParent = new GameObject[3];
        blockControllers = GetComponentsInChildren<BlockController>();
        blockService.AssignProblemBlocks(blockControllers);
        //sending x positions of blocks in problemBox
        for (int i = 0; i < blockControllers.Length; i++)
        {
            blockService.problemBlockXPos[i] = blockControllers[i].transform.position.x;
            //StripeParentObjectInstantiation(i);
        }

    }

    private void StripeParentObjectInstantiation(int i)
    {
    }

    public void AddNewStripes(int problemBlockIndex, Transform strParent, float[] xPositions)
    {
        strParent.parent = blockControllers[problemBlockIndex].transform;
        foreach (float item in xPositions)
        {
            StripeController stripe = Instantiate(stripePrefab, strParent);
            Vector3 strPos = stripe.transform.localPosition;
            strPos.x = item;
            stripe.transform.localPosition = strPos;
        }
    }

}
