using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerScript : MonoBehaviour
{
    public GameObject cubePrefab;
    Vector3 cubePosition;
    public static GameObject airplane;
    public static GameObject activeAirplane;
    public static GameObject[,] cubeGrid;
    int gridX = 16;
    int gridY = 9;
    int airplaneX;
    int airplaneY;
    public static int depotX = 15;
    public static int depotY = 0;
    public static int maxCargo = 90;
    public static int currentCargo = 0;
    int score = 0;
    float turnTimer = 1.5f; //indicates when to take a turn
    float turnLength = 1.5f;
    int turnCount = 1;
    int cargoLoad = 10;
    int startX = 0;
    int startY = 8;
    public Text ScoreText;
    int moveY = 0;
    int moveX = 0;
    int[] destination; //will track destination X,Y.  0 will be x, 1 will be y
    // Start is called before the first frame update
    void CollectCargo()
    {
        if (airplaneX == startX && airplaneY == startY)
        {
            currentCargo += cargoLoad;
            currentCargo = Mathf.Min(currentCargo, maxCargo); //if cargo is over max, reduces cargo to max
        }
    }
    void DeliverCargo()
    {
        if (airplaneX == depotX && airplaneY == depotY)
        {
            score += currentCargo;
            currentCargo = 0;
        }
    }
        void TakeaTurn()
    {
        turnCount++;
        print("turn " + turnCount);
        ChartCourse();
        MoveAirplane();
        CollectCargo();
        DeliverCargo();
        turnTimer += turnLength;
        ScoreText.text = "Score: " + score + "                                                                          Cargo: " + currentCargo;
    }
    void ChartCourse()
    {
        if (airplaneY<destination[1] && airplaneY < gridY - 1)
        {
            moveY = 1;
          
        }
        else if (airplaneY>destination[1] && airplaneY > 0)
        {
            moveY = -1;
            
        }
        if (airplaneX>destination[0] && airplaneX > 0)
        {
            moveX = -1;
            
        }
        else if (airplaneX<destination[0] && airplaneX < gridX - 1)
        {
            moveX = 1;
            
        }
    }
    public void MoveAirplane()
    {
        if (activeAirplane != null)
        {
            if (airplaneX == depotX && airplaneY == depotY)
            { activeAirplane.GetComponent<Renderer>().material.color = Color.black; } //turns depot black
            else
            { activeAirplane.GetComponent<Renderer>().material.color = Color.white; }//turns sky white
            activeAirplane.transform.localScale = new Vector3(1f, 1f, 1f);
            airplaneX += moveX;
            airplaneY += moveY;
            activeAirplane = cubeGrid[airplaneX, airplaneY];
            activeAirplane.GetComponent<Renderer>().material.color=Color.red;
            activeAirplane.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            moveX = 0; //reset movement
            moveY = 0; //reset movement
        }
    }
    public void ProcessClick(GameObject clickedCube, int X, int Y) {
        if (airplane == clickedCube)
        {
            airplane.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            activeAirplane = clickedCube;
            airplane = null;
        }
        else if (activeAirplane == clickedCube)
        {
            activeAirplane.transform.localScale = new Vector3(1f, 1f, 1f);
            airplane = clickedCube;
            activeAirplane = null;
        }
        else if (activeAirplane != null) //set destination to coordinates of clicked cube
        {
            destination[0] = X;
            destination[1] = Y;
        }
    }
    void Start()
    {
        airplaneX = startX;
        airplaneY = startY;
        cubeGrid = new GameObject[gridX, gridY];
        destination = new int[2];
        destination[0] = startX;
        destination[1] = startY;
        for (int y = 0; y <gridY; y ++)
        {
            for (int x = 0; x < gridX; x++)
            {
                cubePosition = new Vector3(x*2, y*2, 0);
                cubeGrid[x,y] = Instantiate(cubePrefab, cubePosition, Quaternion.identity);
                cubeGrid[x, y].GetComponent<CubeController>().myX = x;
                cubeGrid[x, y].GetComponent<CubeController>().myY = y;
            }
        }
        cubeGrid[airplaneX, airplaneY].GetComponent<Renderer>().material.color = Color.red;
        airplane = cubeGrid[airplaneX, airplaneY];
        cubeGrid[depotX, depotY].GetComponent<Renderer>().material.color = Color.black;
        print("turn " + turnCount);
        ScoreText.text = "Score: " + score + "                                                                          Cargo: " + currentCargo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > turnTimer)
        {
            TakeaTurn();  
        }
    }
}
