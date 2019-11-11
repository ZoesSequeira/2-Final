using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    ControllerScript myGameController;
    public int myX;
    public int myY;
    // Start is called before the first frame update
    void Start()
    {
        myGameController = GameObject.Find("ControllerObject").GetComponent<ControllerScript>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        myGameController.ProcessClick(gameObject, myX, myY);
    }
}
