using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripeController : MonoBehaviour
{   
    //this class is mostly created for referencing
    
    private BlockController blockController;

    // Start is called before the first frame update
    void Start()
    {
        blockController = GetComponentInParent<BlockController>();
    }

}
