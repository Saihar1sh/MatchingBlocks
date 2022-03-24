using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsHandler : MonoBehaviour
{
    #region Singleton Instance
    private static InputsHandler instance;
    public static InputsHandler Instance { get { return instance; } }
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

    BlockService blockService;

    public bool isDragging { get; private set; } = false;

    private void Awake()
    {
        SingletonInstantiate();

    }
    // Start is called before the first frame update
    void Start()
    {
        blockService = BlockService.Instance;    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit2D.transform.gameObject.layer != 8)
            {
                return;
            }
            else if(hit2D.transform.gameObject.layer == 8)                  //all movable blocks are in layer 8
            {
                if (hit2D.transform.gameObject.GetComponent<BlockController>())
                {
                    blockService.SetSelectedBlock(hit2D.transform.gameObject.GetComponent<BlockController>());

                }
                else
                {
                    Debug.LogWarning("No block controller attached. Please change the gameobject layer if not using");
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            blockService.PlaceSelectedBlock();
            blockService.ProblemBlockAddStripes();

        }
    }
}
