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


    public ProblemBoxScript problemBoxScript;

    public Transform movableBlocksStart, movableBlocksParent;

    public StripeController stripePrefab;

    public float yPosDiff;


    public BlockController[] movableBlocks; 

    public Transform[] solBoxs;
    
    public Vector3[] problemBlockPos;
    
    
    public List<float> problemBlockXpos; // position of x positions of problem blocks 
    
    public List<BlockController> solutionBlocks;
    
    public List<Vector3> blockPlaceHolderPos;
    
    public Vector3[,] solBlocksPositions { get; private set; }

    private Vector3 inventoryBlocksStartPos;
    
    private BlockController selectedBlock;

    private Transform[,] stripeParents;




    private void Awake()
    {
        SingletonInstantiate();
        problemBlockPos = new Vector3[3];    // only 3 problem blocks will be present
        problemBlockXpos = new List<float>(3);
        solBlocksPositions = new Vector3[solBoxs.Length,3];
        stripeParents = new Transform[solBoxs.Length, 3];
        movableBlocks = movableBlocksParent.GetComponentsInChildren<BlockController>();

        inventoryBlocksStartPos = movableBlocksStart.position;
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
                
                solBlocksPositions[i, j] = solBlocksLocation;                        //these locations shouldn't be changed afterwards
            }
            AddingBlockInventoryPos();
        }
    }
    private void AddingBlockInventoryPos()                                      //making inventory blocks place in order 
    {
        for (int i = 0; i < movableBlocks.Length; i++)
        {
            Vector3 _pos = inventoryBlocksStartPos;
            _pos.y -= yPosDiff * i;
            _pos.z = 0;
            movableBlocks[i].transform.position = _pos;
        }
    }

    public void RefreshInventoryList()                                      //placing all inventory blocks in order after a block is place in this list
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
                AddBlockPlaceHolderPositions(_pos);                     //adding these positions to list of positions where blocks can be placed in.
                q++;
            }
        }
    }

    //adds stripes according to movable block placed under the specific problem block
    public void ProblemBlockAddStripes()                           
    {
        problemBoxScript.solvedBlocksCount = 0;                     //resetting count for checking solved stripes
        for (int i = 0; i < solBoxs.Length; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                BlockController blockController = GetBlockByPosition(solBlocksPositions[i, j]);
                //adding new stripe positions to problem block
                if (blockController)
                {
                    stripeParents[i,j] = problemBoxScript.AddNewStripes(i,j,blockController.stripeObjectsXpos);
                }
                else                //checks if the block is removed from the solution boxes and removes the specific stripes if present.
                {
                    try
                    {
                        stripeParents[i,j].gameObject.SetActive(false);
                    }
                    catch(NullReferenceException ex)
                    {
                        Debug.Log("There is no block in the desired index");
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (selectedBlock)
        {
            //checking for one frame where left mouse button was down and checking if last location is valid
            if (Input.GetMouseButtonDown(0) && selectedBlock.lastPlacedLocation != Vector3.negativeInfinity)                    
            {
                //using negative infinity for positions null check

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
        //using negative infinity for positions null check
        if(pos != Vector3.negativeInfinity && !blockPlaceHolderPos.Contains(pos))
        {
            blockPlaceHolderPos.Add(pos);

        }
    }
    public void PlaceSelectedBlock()
    {
        if (selectedBlock != null)
        {
            selectedBlock.PlaceBlock();


            if (selectedBlock.desiredPos.x < -5f)       
            {
                if (!solutionBlocks.Contains(selectedBlock))
                    solutionBlocks.Add(selectedBlock);
            }
            else
            {
                if (solutionBlocks.Contains(selectedBlock))
                    solutionBlocks.Remove(selectedBlock);

                RefreshInventoryList();                 //refreshes the positions in list when another block is placed
            }
        }
        SetSelectedBlock(null);    //resetting selected block back to null
    }
    public void SetSelectedBlock(BlockController block)
    {
        
        selectedBlock = block;
    }



    public BlockController GetBlockByPosition(Vector3 pos)
    {
        foreach (BlockController item in movableBlocks)
        {
            if (item.transform.position == pos)
            {
                return item;
            }
        }
        Debug.Log("No block found");
        return null;
    }

}
