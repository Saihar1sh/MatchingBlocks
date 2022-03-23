using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockController : MonoBehaviour
{ 
    public List<float> stripeObjectsXpos;

    private int index =0;

    //private float canvasScaleFactor;

    private RectTransform rectTransform;
    private BlockService blockService;

    public StripeController[] stripes;

    public StripeController[,] newStripes;

    public Vector3 lastPlacedLocation { get; private set; }

    public Vector3 desiredPos { get; private set; }

    public bool addedStripes =false;

    public SpriteRenderer[,] stripeSprites;

    private int solBoxLength;

    public GameObject[] stripeParent;

    private void Awake()
    {
        stripes = GetComponentsInChildren<StripeController>();
        stripeObjectsXpos = new List<float>(stripes.Length);
    }
    private void Start()
    {
        addedStripes = false;
        for (int i = 0; i < stripes.Length; i++)
        {
            stripeObjectsXpos.Add( stripes[i].transform.localPosition.x);
        }
        blockService = BlockService.Instance;
        solBoxLength = blockService.solBoxs.Length;

        stripeParent = new GameObject[solBoxLength];
        newStripes = new StripeController[solBoxLength, stripes.Length];
        stripeSprites = new SpriteRenderer[solBoxLength, stripes.Length];

        if (gameObject.layer == 10)                     //problem blocks
        {
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



        //lastPlacedLocation = Vector3.negativeInfinity;              //using as null value. As I can't use origin (as it is also a position).
        lastPlacedLocation = desiredPos = transform.position;
    }


    //Adding stripe object's x co-ordinate into an array(local postion)
    public void GetNearestPosition()
    {
        foreach (Vector3 item in blockService.blockPlaceHolderPos)
        {
            //Debug.Log("Dist :" + item + " :-" + Mathf.Abs(Vector3.Distance(item, transform.position)));

            if (Mathf.Abs( Vector3.Distance(item, transform.position)) <= .5f)
            {
                //selectedBlock.PlaceBlock(item);
                //blockPlaceHolderPos.Remove(item);
                desiredPos = item;
            }
            

        }
    }


    public void PlaceBlock()
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
