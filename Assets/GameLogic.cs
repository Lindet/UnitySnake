using System;
using System.IO;
using System.Runtime.InteropServices;
using Assets;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


public enum FoodTypes{
	White, Green, Blue, Orange, Red, Yellow, Rainbow, Pink
}

public class GameLogic : MonoBehaviour
{
	private List<Food> FoodElement = new List<Food>();
    private GlobalVar globals;
	public static Object Prefab = Resources.Load("SnakePart");
    

    public Camera MainCam;

    public GameObject LeftBorder;
    public GameObject TopBorder;
    public GameObject RightBorder;
    public GameObject BottomBorder;
    public GameObject ScoreTopBorder;


    public static Object TopBottomBorder = Resources.Load("TopBottomBorder");
    public static Object LeftRightBorder = Resources.Load("LeftRightBorderSmall");

    // Use this for initialization
	void Start ()
	{
	    globals = GlobalVar.Instance;
	    globals.MainCam = MainCam;
        globals.CreateWalls();

        LeftBorder = Instantiate(LeftRightBorder) as GameObject;
        TopBorder = Instantiate(TopBottomBorder) as GameObject;
        ScoreTopBorder = Instantiate(TopBottomBorder) as GameObject;

        RightBorder = Instantiate(LeftRightBorder) as GameObject;
        BottomBorder = Instantiate(TopBottomBorder) as GameObject;

        LeftBorder.transform.position = new Vector3(globals.leftWall + 0.02f, 0);
        ScoreTopBorder.transform.position = new Vector3(0, globals.scoreTopWall);
        TopBorder.transform.position = new Vector3(0, globals.topWall);
        RightBorder.transform.position = new Vector3(globals.rightWall - 0.02f, 0);
        BottomBorder.transform.position = new Vector3(0, globals.bottomWall);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        var leftSide = (globals.leftWall*10 + 2f)/10;
        var topSide = globals.bottomWall;
        var style = new GUIStyle {fontSize = 45, normal = {textColor = Color.red}};
        GUI.Label(new Rect(globals.MainCam.WorldToScreenPoint(new Vector3(leftSide, 0f, 0f)).x, globals.MainCam.WorldToScreenPoint(new Vector3(0f, topSide, 0f)).y, 3f, 1f), "Score : " + globals.PlayerScore, style);
    }

    void FixedUpdate()
    {
        if(!globals.NeedNewFood && globals.Foods.Count > 0) return;
        
	    var tempFood = new Food(FoodTypes.White, GetCoordinates());

        globals.Foods.Add(Instantiate(Prefab) as GameObject);
        globals.Foods[globals.Foods.Count-1].transform.position = tempFood.Place;

        globals.NeedNewFood = false;

    }

	private Vector2 GetCoordinates()
	{
		var rnd = new Random ();
		var vector = new Vector2();
		bool freePlace = false;
		float x, y;

		while (!freePlace) {
            x = Mathf.Round(Random.Range(globals.leftWall + 0.1f, globals.rightWall - 0.1f)*10)/10;
            y = Mathf.Round(Random.Range(globals.topWall - 0.1f, globals.bottomWall + 0.1f)*10)/10;
			freePlace = Check(x,y);
		    if (freePlace) vector = new Vector3(x, y);

		}
		return vector;
	}

	private bool Check(float x, float y)
	{
        for (int i = 0; i < globals.SnakeBody.Count; i++)
		{
            if (Mathf.Approximately(globals.SnakeBody[i].transform.position.x, x) && Mathf.Approximately(globals.SnakeBody[i].transform.position.y, y)) return false;
		}

	    foreach (var items in globals.Foods)
	    {
	        if (Mathf.Approximately(x, items.transform.position.x) && Mathf.Approximately(y, items.transform.position.y))
	            return false;
	    }
		return true;
	}
}


public class Food
{
	public Vector2 Place;
	public FoodTypes Type;

	public Food(FoodTypes type, Vector3 vector)
	{
		Type = type;
		Place = vector;
	}
}