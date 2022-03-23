using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockService : MonoBehaviour
{

    #region Singleton Instance
    private static BlockService instance;
    public static BlockService Instance { get { return instance; } }
    private void SingletonInstantiate()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null || instance != this)
        {
            Destroy(this);
        }
    }
    #endregion

    public List<Vector3> blockPlaceHolderPos;

    private BlockController selectedBlock, previousBlock, lastSelectedBlock;

    public ProblemBoxScript problemBoxScript;

    public List<BlockController> solutionBlocks;

    public BlockController[] movableBlocks; 

    public Transform[] solBoxs;

    private Vector3[,] solBlocksPositions;

    private Vector3 inventoryBlocksStartPos;

    public Vector3[] problemBlockPos;
    public List<float> problemBlockXpos; // position of x positions of problem blocks 

    public Transform movableBlocksStart, movableBlocksParent;

    public float yPosDiff;

    public StripeController stripePrefab;

    Transform[,] stripeParents;

    private void Awake()
    {
        SingletonInstantiate();
        //problemBlocks = new BlockController[3];
        problemBlockPos = new Vector3[3];    // only 3 problem blocks will be present
        problemBlockXpos = new List<float>(3);
        solBlocksPositions = new Vector3[solBoxs.Length,3];
        stripeParents = new Transform[solBoxs.Length, 3];
        movableBlocks = movableBlocksParent.GetComponentsInChildren<BlockController>();

        inventoryBlocksStartPos = movableBlocksStart.position;
        lastSelectedBlock = null;
        previousBlock = null;
    }

    private void Start()
    {
        blockPlaceHolderPos = new List<Vector3>(); 
        for (int i = 0; i < solBoxs.Length; i++)                            //adding locations from solution boxes
        {
            for (int j = 0; j < 3; j++)
            {
                Vector3 solBlocksLocation = new Vector3(problemBlockPos[j].x, solBoxs[i].position.y);
                //all block place holders are slots in which blocks can be placed
                AddBlockPlaceHolderPositions(solBlocksLocation);
                
                solBlocksPositions[i, j] = solBlocksLocation;                        //these locations won't be changed afterwards
            }
            AddingBlockInventoryPos();
        }
    }
    private void AddingBlockInventoryPos()
    {
        for (int i = 0; i < movableBlocks.Length; i++)
        {
            Vector3 _pos = inventoryBlocksStartPos;
            _pos.y -= yPosDiff * i;
            _pos.z = 0;
            movableBlocks[i].transform.position = _pos;
            AddBlockPlaceHolderPositions(_pos);
        }
    }

    public void RefreshInventoryList()
    {
        int q = 0;                  //only used for indexing
        foreach (BlockController item in movableBlocks)
        {
            if(item.transform.position.x == inventoryBlocksStartPos.x)
            {
                Vector3 _pos = inventoryBlocksStartPos;
                _pos.y -= yPosDiff * q;
                _pos.z = 0;
                item.transform.position = _pos;
                q++;
            }
        }
    }
    public void ProblemBlockStripesPosCheck()
    {
        problemBoxScript.solvedBlocksCount = 0;
        for (int i = 0; i < solBoxs.Length; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                BlockController blockController = GetBlockByPosition(solBlocksPositions[i, j]);
                //adding new stripe positions to problem block
                if (blockController)
                {
                    //stripeParents[i, j] = blockController.stripeParent;
                    //stripeParents[i, j].gameObject.SetActive(true);
                    stripeParents[i,j] = problemBoxScript.AddNewStripes(i,j,blockController.stripeObjectsXpos);
                    previousBlock = blockController;
                }
                else
                {
                    try
                    {
                        stripeParents[i,j].gameObject.SetActive(false);
                    }
                    catch(NullReferenceException ex)
                    {
                        Debug.Log("There is no object in the desired index");
                    }
                    //Destroy(stripeParents[i, j]);
                }
            }
        }
    }
    public BlockController GetBlockByPosition(Vector3 pos)
    {
        foreach (BlockController item in movableBlocks)
        {
            if(item.transform.position == pos)
            {
                return item;
            }
        }
        Debug.Log("No block found");
        return null;
    }

    private void Update()
    {
        if (selectedBlock)
        {
            //checking for one frame where left mouse button was down and checking if last location is valid
            if (Input.GetMouseButtonDown(0) && selectedBlock.lastPlacedLocation != Vector3.negativeInfinity)            
            {
                AddBlockPlaceHolderPositions(selectedBlock.lastPlacedLocation);
            }
            MotionofBlocksWithMouseInput();
            selectedBlock.GetNearestPosition();
            Debug.Log("desiredPos: "+selectedBlock.desiredPos);

        }
    }

    private void MotionofBlocksWithMouseInput()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        selectedBlock.transform.position = pos;
    }

    public void AddBlockPlaceHolderPositions(Vector3 pos)
    {
        if(pos != Vector3.negativeInfinity && !blockPlaceHolderPos.Contains(pos))
        {
            blockPlaceHolderPos.Add(pos);

        }
    }
    public void PlaceSelectedBlock()
    {
        selectedBlock.PlaceBlock();



        if (selectedBlock.desiredPos.x < -5f)        //$@
        {
            if (!solutionBlocks.Contains(selectedBlock))
                solutionBlocks.Add(selectedBlock);
        }
        else
        {
            if (solutionBlocks.Contains(selectedBlock))
                solutionBlocks.Remove(selectedBlock);

            RefreshInventoryList();
        }

        SetSelectedBlock(null);    //resetting selected block back to null
    }
    public void SetSelectedBlock(BlockController block)
    {
        
        lastSelectedBlock = selectedBlock;
        selectedBlock = block;
    }

}
