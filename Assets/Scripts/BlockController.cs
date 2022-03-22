using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockController : MonoBehaviour
{ 
    public float[] stripeObjectsXpos;

    private int index =0;

    //private float canvasScaleFactor;

    private RectTransform rectTransform;
    private BlockService blockService;

    public StripeController[] stripes;

    public Vector3 lastPlacedLocation { get; private set; }

    public Vector3 desiredPos;

    public bool addedStripes =false;

    private void Awake()
    {
        stripes = GetComponentsInChildren<StripeController>();
        stripeObjectsXpos = new float[stripes.Length];
    }
    private void Start()
    {
        blockService = BlockService.Instance;
        //lastPlacedLocation = Vector3.negativeInfinity;              //using as null value. As I can't use origin (as it is also a position).
        lastPlacedLocation = desiredPos = transform.position;
    }
    private void OnEnable()
    {
        for (int i = 0; i < stripes.Length; i++)
        {
            stripeObjectsXpos[i] = stripes[i].transform.localPosition.x;
        }

        //rectTransform = GetComponent<RectTransform>();
        //canvasScaleFactor = blockService.mainCanvas.scaleFactor;
        index = 0;

/*        if (gameObject.layer == 9)       //problem box layer
        {
            blockService.problemBlocks.Add(this);
        }
        else
        {
            blockService.solutionBlocks.Add(this);
        }
*/
    }

    //Adding stripe object's x co-ordinate into an array(local postion)
    public void AddStripeXpos(Transform stripe)
    {
        stripeObjectsXpos[index] = stripe.localPosition.x;
        index++;
    }
    public void GetNearestPosition()
    {
        foreach (Vector3 item in blockService.blockPlaceHolderPos)
        {
            Debug.Log("Dist :" + item + " :-" + Mathf.Abs(Vector3.Distance(item, transform.position)));

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
        if(desiredPos.x > -5f)        //$@
        {
            BlockController block = blockService.GetBlockByPosition(desiredPos);
            if (!blockService.solutionBlocks.Contains(block))
                blockService.solutionBlocks.Add(block);
        }
        else
        {
            blockService.RefreshInventoryList();
        }
            blockService.blockPlaceHolderPos.Remove(lastPlacedLocation);            //so that other blocks can't be placed in the same location
    }

/*    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvasScaleFactor;
        //throw new System.NotImplementedException();
    }
*/
    /*    private void OnDisable()
        {
            if (gameObject.layer == 9)       //problem box layer
            {
                blockService.problemBlocks.Remove(this);
            }
            else
            {
                blockService.solutionBlocks.Remove(this);
            }

        }
    */
}
