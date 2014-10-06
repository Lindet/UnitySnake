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
    public GUISkin _skin;
    public Texture2D MenuBack;
    public KeyCode Escape;
    

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

	}

    void StartNewGame()
    {   
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

        if(Input.GetKeyDown(Escape))
        {
            globals.InTheMenu = true;
            
        }
	
	}

    void OnGUI()
    {
        GUI.skin = _skin;
        if (globals.InTheMenu)
        {            
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), MenuBack);
            var ButtonsMargin = Screen.height / 18;

            if (globals.FirstMenu)
            {
                GUI.enabled = true;
                GUIStyle style = new GUIStyle { fontSize = 25, normal = { textColor = Color.grey }, alignment = TextAnchor.MiddleCenter };
                if (GUI.Button(new Rect((Screen.width / 2) - 100, (Screen.height / 6) + 20f, 200, 30), new GUIContent("Start New Game", "Starts a new game.")))
                {
                    globals.FirstMenu = false;
                    globals.InTheMenu = false;
                    StartNewGame();
                }
                GUI.enabled = false;
                if (GUI.Button(new Rect((Screen.width / 2) - 100f, (Screen.height / 6) + 20f + ButtonsMargin, 200f, 30f), new GUIContent("Challenges", "This function will be enabled in one of the next updates.")))
                { }
                GUI.enabled = false;
                if (GUI.Button(new Rect((Screen.width / 2) - 100f, (Screen.height / 6) + 20f + (ButtonsMargin * 2), 200f, 30f), new GUIContent("Leaderboard", "This function will be enabled in one of the next updates.")))
                { }
                GUI.enabled = false;
                if (GUI.Button(new Rect((Screen.width / 2) - 100f, (Screen.height / 6) + 20f + (ButtonsMargin * 3), 200f, 30f), new GUIContent("Preferences", "This function will be enabled in one of the next updates.")))
                { }
                GUI.enabled = false;
                if (GUI.Button(new Rect((Screen.width / 2) - 100f, (Screen.height / 6) + 20f + (ButtonsMargin * 4), 200f, 30f), new GUIContent("Skin Settings", "This function will be enabled in one of the next updates.")))
                { }
                GUI.enabled = false;
                if (GUI.Button(new Rect((Screen.width / 2) - 100f, (Screen.height / 6) + 20f + (ButtonsMargin * 7), 200f, 30f), new GUIContent("Credits", "This function will be enabled in one of the next updates.")))
                { }
                GUI.enabled = true;
                if (GUI.Button(new Rect((Screen.width / 2) - 100f, (Screen.height / 6) + 20f + (ButtonsMargin * 8), 200f, 30f), new GUIContent("Exit", "Click here to exit the game.")))
                {
                    Application.Quit();
                }
                if (GUI.Button(new Rect(Screen.width - 70f, Screen.height - 50f, 60, 40), new GUIContent("Acc", "Click here to open the Account Information Menu.")))
                {
                    globals.AccMenu = globals.AccMenu ? false : true;
                }

                if (globals.AccMenu)
                {
                    GUI.Box(new Rect(Screen.width - 150f, Screen.height - 208f, 150f, 170f),"");
                    GUI.enabled = false;
                    if (GUI.Button(new Rect(Screen.width - 150f, Screen.height - 208f, 145f, 26f), new GUIContent("Sign in", "Sign into the LindetGame system.")))
                    { }
                    GUI.enabled = false;
                    if (GUI.Button(new Rect(Screen.width - 150f, Screen.height - 208f + (24f) + 2f, 145f, 24f), new GUIContent("My results", "Click here to check your results.")))
                    { }
                    GUI.enabled = false;
                    if (GUI.Button(new Rect(Screen.width - 150f, Screen.height - 208f + (24f * 2) + 2f, 145f, 24f), new GUIContent("Account Settings", "Here you can change your account name, account picture etc.")))
                    { }
                    GUI.enabled = false;
                    if (GUI.Button(new Rect(Screen.width - 150f, Screen.height - 208f + (24f * 3) + 2f, 145f, 24f), new GUIContent("Talent tree", "You can choose your talents here.")))
                    { }
                    GUI.enabled = false;
                    if (GUI.Button(new Rect(Screen.width - 150f, Screen.height - 208f + (24f * 4) + 2f, 145f, 24f), new GUIContent("Achievements", "Click here to see how many achievments you still need to earn.")))
                    { }
                    GUI.enabled = false;
                    if (GUI.Button(new Rect(Screen.width - 150f, Screen.height - 208f + (24f * 5) + 2f, 145f, 24f), new GUIContent("Sign out", "Sign out of the LindetGame system.")))
                    { }
                }

                GUI.Label(new Rect(0, Screen.height - 50, Screen.width, 40), GUI.tooltip, style);

            }
            else
            {
                GUI.enabled = true;
                GUIStyle style = new GUIStyle { fontSize = 25, normal = { textColor = Color.grey }, alignment = TextAnchor.MiddleCenter };
                if (GUI.Button(new Rect((Screen.width / 2) - 100f, (Screen.height / 6) + 20f, 200f, 30f), new GUIContent("Continue", "Return to your game.")))
                {
                    globals.InTheMenu = false;
                }
                if (GUI.Button(new Rect((Screen.width / 2) - 100f, (Screen.height / 6) + 20f + ButtonsMargin, 200f, 30f), new GUIContent("Start New Game", "Starts a new game.")))
                {
                    globals.NewGame();
                    globals.InTheMenu = false;                    
                    StartNewGame();
                }

                GUI.enabled = false;
                if (GUI.Button(new Rect((Screen.width / 2) - 100f, (Screen.height / 6) + 20f + (ButtonsMargin * 2), 200f, 30f), new GUIContent("Leaderboard", "This function will be enabled in one of the next updates.")))
                { }
                GUI.enabled = false;
                if (GUI.Button(new Rect((Screen.width / 2) - 100f, (Screen.height / 6) + 20f + (ButtonsMargin * 3), 200f, 30f), new GUIContent("Preferences", "This function will be enabled in one of the next updates.")))
                { }
                GUI.enabled = true;
                if (GUI.Button(new Rect((Screen.width / 2) - 100f, (Screen.height / 6) + 20f + (ButtonsMargin * 7), 200f, 30f), new GUIContent("Exit to the Main Menu", "Exit to the Main Menu. Current game will not be saved.")))
                {
                    globals.NewGame();
                    globals.FirstMenu = true;
                    globals.InTheMenu = true;
                }
                GUI.enabled = true;
                if (GUI.Button(new Rect((Screen.width / 2) - 100f, (Screen.height / 6) + 20f + (ButtonsMargin * 8), 200f, 30f), new GUIContent("Exit", "Click here to exit the game.")))
                {
                    Application.Quit();
                }
                if (GUI.Button(new Rect(Screen.width - 70f, Screen.height - 50f, 60, 40), new GUIContent("Acc", "Click here to open the Account Information Menu.")))
                {
                    globals.AccMenu = globals.AccMenu ? false : true;
                }

                if (globals.AccMenu)
                {
                    GUI.Box(new Rect(Screen.width - 150f, Screen.height - 208f, 150f, 170f), "");
                    GUI.enabled = false;
                    if (GUI.Button(new Rect(Screen.width - 150f, Screen.height - 208f, 145f, 26f), new GUIContent("Sign in", "Sign into the LindetGame system.")))
                    { }
                    GUI.enabled = false;
                    if (GUI.Button(new Rect(Screen.width - 150f, Screen.height - 208f + (24f) + 2f, 145f, 24f), new GUIContent("My results", "Click here to check your results.")))
                    { }
                    GUI.enabled = false;
                    if (GUI.Button(new Rect(Screen.width - 150f, Screen.height - 208f + (24f * 2) + 2f, 145f, 24f), new GUIContent("Account Settings", "Here you can change your account name, account picture etc.")))
                    { }
                    GUI.enabled = false;
                    if (GUI.Button(new Rect(Screen.width - 150f, Screen.height - 208f + (24f * 3) + 2f, 145f, 24f), new GUIContent("Talent tree", "You can choose your talents here.")))
                    { }
                    GUI.enabled = false;
                    if (GUI.Button(new Rect(Screen.width - 150f, Screen.height - 208f + (24f * 4) + 2f, 145f, 24f), new GUIContent("Achievements", "Click here to see how many achievments you still need to earn.")))
                    { }
                    GUI.enabled = false;
                    if (GUI.Button(new Rect(Screen.width - 150f, Screen.height - 208f + (24f * 5) + 2f, 145f, 24f), new GUIContent("Sign out", "Sign out of the LindetGame system.")))
                    { }
                }
                GUI.Label(new Rect(0, Screen.height - 50, Screen.width, 40f), GUI.tooltip, style);
            }
        }
        else
        {
            var leftSide = (globals.leftWall * 10 + 2f) / 10;
            var topSide = globals.bottomWall;
            var style = new GUIStyle { fontSize = 45, normal = { textColor = Color.red } };
            GUI.Label(new Rect(globals.MainCam.WorldToScreenPoint(new Vector3(leftSide, 0f, 0f)).x, globals.MainCam.WorldToScreenPoint(new Vector3(0f, topSide, 0f)).y, 3f, 1f), "Score : " + globals.PlayerScore, style);
        }
    }

    void FixedUpdate()
    {
        if (globals.InTheMenu) return;
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