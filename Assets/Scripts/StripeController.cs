using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripeController : MonoBehaviour
{
    private BlockController blockController;

    // Start is called before the first frame update
    void Start()
    {
        blockController = GetComponentInParent<BlockController>();
        //blockController.AddStripeXpos(transform);
    }

}
