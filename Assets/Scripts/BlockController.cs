using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockController : MonoBehaviour
{ 
    public Vector3 lastPlacedLocation { get; private set; }

    public Vector3 desiredPos { get; private set; }

    [Tooltip("Index for Solution Block to be placed in")]
    public Vector2Int solutionBlockIndex;                              

    [Header("Stripes Details")]
    public List<float> stripeObjectsXpos;

    public StripeController[] stripes;

    public GameObject[] stripeParent;

    public StripeController[,] newStripes;

    public SpriteRenderer[,] stripeSprites;

    private BlockService blockService;

    private int solBoxLength;

    private void Awake()
    {
        stripes = GetComponentsInChildren<StripeController>();
        stripeObjectsXpos = new List<float>(stripes.Length);
    }
    private void Start()
    {
        for (int i = 0; i < stripes.Length; i++)
        {
            stripeObjectsXpos.Add( stripes[i].transform.localPosition.x);
        }
        blockService = BlockService.Instance;

        if (gameObject.layer == 10)                     //all problem blocks will be placed in this layer
        {

            solBoxLength = blockService.solBoxs.Length;

            stripeParent = new GameObject[solBoxLength];
            newStripes = new StripeController[solBoxLength, stripes.Length];
            stripeSprites = new SpriteRenderer[solBoxLength, stripes.Length];


            for (int i = 0; i < solBoxLength; i++)
            {
                stripeParent[i] = new GameObject("Added Stripes"+i);
                stripeParent[i].transform.parent = this.transform;
                stripeParent[i].transform.localPosition = Vector3.zero;
                stripeParent[i].gameObject.SetActive(false);

                for (int j = 0; j < stripes.Length; j++)
                {
                    newStripes[i,j] = Instantiate(blockService.stripePrefab, stripeParent[i].transform);
                    stripeSprites[i, j] = newStripes[i,j].GetComponent<SpriteRenderer>();
                    newStripes[i,j].gameObject.SetActive(false);
                }

            }

        }

        lastPlacedLocation = desiredPos = transform.position;
    }


    //Adding stripe object's x co-ordinate into an array
    public void GetNearestPosition()
    {
        foreach (Vector3 item in blockService.blockPlaceHolderPos)
        {

            if (Mathf.Abs( Vector3.Distance(item, transform.position)) <= .5f)
            {
                desiredPos = item;
            }
        }
    }

    public void SetSolBlockPosition()                       //sets this block at location specified by solution index
    {
        transform.position = blockService.solBlocksPositions[solutionBlockIndex.x,solutionBlockIndex.y];        //using vector2Int fo solution index as it is easier to keep index 
    }

    public void PlaceBlock()                //places block at desired position
    {
        Debug.Log("nearestPos: " + desiredPos);
        if (desiredPos != Vector3.negativeInfinity)
        {
            transform.position = desiredPos;
            lastPlacedLocation = desiredPos;

        }
        blockService.blockPlaceHolderPos.Remove(lastPlacedLocation);            //so that other blocks can't be placed in the same location

    }

}
