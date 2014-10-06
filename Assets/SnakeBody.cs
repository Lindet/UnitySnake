using System.Linq;
using System.Runtime.Versioning;
using Assets;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;


public class SnakeBody : MonoBehaviour
{
    private GlobalVar globals = GlobalVar.Instance;

	public static UnityEngine.Object Prefab;
    public static UnityEngine.Object HeadPrefab;
	public int Head;
	float speed = 10f;
	public KeyCode MoveUp;
	public KeyCode MoveDown;
	public KeyCode MoveLeft;
	public KeyCode MoveRight;

    private bool _haveKey;
    private int _deadAnimationTimer;
    private SpriteRenderer DeadScreen;

	float timer;
    // Use this for initialization
	void StartGame ()
	{
		using (var sw = new StreamWriter("GameLog.txt", true)) {
            sw.WriteLine("BottomWall : " + globals.bottomWall + ". RightWall : " + globals.rightWall + ". TopWall : " + globals.topWall + ". LeftWall : " + globals.leftWall);
				}

		Head = 3;
		timer = 0f;
		Prefab = Resources.Load ("SnakePart");
	    HeadPrefab = Resources.Load("SnakeHead");
        globals.SnakeBody = new List<GameObject>();

		for (int i = 0; i < 6; i++) {

            globals.SnakeBody.Add(CreateObject(globals.SnakeBody.Count));
				}	
        RotateHead(1, Head);
        DeadScreen = GameObject.Find("DeadScreen").GetComponent<SpriteRenderer>();
	    DeadScreen.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (globals.InTheMenu) return;
        if (globals.SnakeBody == null) StartGame();
	    if (globals.SnakeIsDead) return;

	        if (_haveKey) return;
	        if (Input.GetKey(MoveUp) && Head != 2)
	        {
	            RotateHead(Head, 4);
	            Head = 4;
	            _haveKey = true;
	            return;
	        }
	        if (Input.GetKey(MoveDown) && Head != 4)
	        {
	            RotateHead(Head, 2);
	            Head = 2;
	            _haveKey = true;
	            return;
	        }
	        if (Input.GetKey(MoveLeft) && Head != 1)
	        {
	            RotateHead(Head, 3);
	            Head = 3;
	            _haveKey = true;
	            return;
	        }
	        if (Input.GetKey(MoveRight) && Head != 3)
	        {
	            RotateHead(Head, 1);
	            Head = 1;
	            _haveKey = true;
	        }
	    
	}

	void FixedUpdate()
	{
        if (globals.InTheMenu) return;
	    if (globals.SnakeIsDead)
	    {
            if (_deadAnimationTimer == 0) DeadScreen.enabled = true;
            if (_deadAnimationTimer % 10 > 5)
            {
                using (var sw = new StreamWriter("GameLog.txt", true))
                {
                    sw.WriteLine("DeadTimer : " + _deadAnimationTimer);
                }
                foreach (var item in globals.SnakeBody)
                {
                    item.renderer.material.color = Color.red;
                }
                _deadAnimationTimer--;
            }
            else if (_deadAnimationTimer % 10 <= 5)
            {
                using (var sw = new StreamWriter("GameLog.txt", true))
                {
                    sw.WriteLine("DeadTimer : " + _deadAnimationTimer);
                }
                foreach (var item in globals.SnakeBody)
                {
                    item.renderer.material.color = Color.white;
                }
                _deadAnimationTimer--;
            }
            return;
	    }
	    var nextHeadPos = new Vector3();
	    switch (Head)
	    {
            case 1:
                nextHeadPos = new Vector3(Mathf.Round(globals.SnakeBody[0].transform.position.x * 10 + 1f) / 10, globals.SnakeBody[0].transform.position.y);
                break;
            case 2:
                nextHeadPos = new Vector3(globals.SnakeBody[0].transform.position.x, Mathf.Round(globals.SnakeBody[0].transform.position.y * 10 - 1f) / 10);
                break;
            case 3:
                nextHeadPos = new Vector3(Mathf.Round(globals.SnakeBody[0].transform.position.x * 10 - 1f) / 10, globals.SnakeBody[0].transform.position.y);
                break;
            case 4:
                nextHeadPos = new Vector3(globals.SnakeBody[0].transform.position.x, Mathf.Round(globals.SnakeBody[0].transform.position.y * 10 + 1f) / 10);
                break;
	    }
        for (var i = 1; i < globals.SnakeBody.Count - 1; i++)
        {
            if (CheckIntersection(globals.SnakeBody[i].transform.position, nextHeadPos))
            {
                globals.SnakeIsDead = true;
                _deadAnimationTimer = 100;
                return;
            }
        }
		if (Mathf.Approximately(speed,timer) && !globals.SnakeIsDead) {

			Log ();
            for (int i = globals.SnakeBody.Count - 1; i > 0; i--)
            {
                if (i == globals.SnakeBody.Count - 1)
                {
                    if (globals.Foods.Any(z => CheckIntersection(z.transform.position, globals.SnakeBody[i].transform.position)))
                    {

                        globals.SnakeBody.Add(CreateObject(globals.SnakeBody[i]));
                        Destroy(
                            globals.Foods.First(
                                z => CheckIntersection(z.transform.position, globals.SnakeBody[i].transform.position)));
                        globals.Foods.Remove(
                            globals.Foods.First(z => CheckIntersection(z.transform.position, globals.SnakeBody[i].transform.position)));
                    }
                }
                globals.SnakeBody[i].transform.position = globals.SnakeBody[i - 1].transform.position;
			}
		    

		    if (Head == 1) {
                if (Mathf.Approximately( Mathf.Round(globals.SnakeBody[0].transform.position.x*10 + 1f)/10, globals.rightWall))
                {
                    globals.SnakeBody[0].transform.position = new Vector3(Mathf.Round(globals.leftWall*10 + 1f)/10, globals.SnakeBody[0].transform.position.y);
				} else {
                    globals.SnakeBody[0].transform.position = new Vector3(Mathf.Round(globals.SnakeBody[0].transform.position.x*10 + 1f)/10, globals.SnakeBody[0].transform.position.y);
				}
			}
			if (Head == 2) {
                if (Mathf.Approximately(Mathf.Round(globals.SnakeBody[0].transform.position.y*10 - 1f)/10, globals.bottomWall))
                {
                    globals.SnakeBody[0].transform.position = new Vector3(globals.SnakeBody[0].transform.position.x, Mathf.Round(globals.topWall*10 - 1f)/10);
				} else {
                    globals.SnakeBody[0].transform.position = new Vector3(globals.SnakeBody[0].transform.position.x, Mathf.Round(globals.SnakeBody[0].transform.position.y*10 - 1f)/10);
				}
			}
			if (Head == 3) {
                if (Mathf.Approximately(Mathf.Round(globals.SnakeBody[0].transform.position.x*10 - 1f)/10, globals.leftWall))
                {
                    globals.SnakeBody[0].transform.position = new Vector3(Mathf.Round(globals.rightWall*10 - 1f)/10, globals.SnakeBody[0].transform.position.y);
				} else {
                    globals.SnakeBody[0].transform.position = new Vector3(Mathf.Round(globals.SnakeBody[0].transform.position.x*10 - 1f)/10, globals.SnakeBody[0].transform.position.y);
				}
			}
			if (Head == 4) {
                if (Mathf.Approximately(Mathf.Round(globals.SnakeBody[0].transform.position.y*10 + 1f)/10, globals.topWall))
                {
                    globals.SnakeBody[0].transform.position = new Vector3(globals.SnakeBody[0].transform.position.x, Mathf.Round(globals.bottomWall*10 + 1f)/10);
				} else {
                    globals.SnakeBody[0].transform.position = new Vector3(globals.SnakeBody[0].transform.position.x, Mathf.Round(globals.SnakeBody[0].transform.position.y*10 + 1f)/10);
				}
			}
			timer = 0;
		    _haveKey = false;

            if (globals.Foods.Any(z => CheckIntersection(z.transform.position, globals.SnakeBody[0].transform.position)))
            {
                globals.NeedNewFood = true;
                globals.PlayerScore++;
            }
		}
		else timer += 1;	
	}
	
	public GameObject CreateObject(int size)
	{
		var newObj = size == 0 ? Instantiate(HeadPrefab) as GameObject : Instantiate(Prefab) as GameObject;
		newObj.transform.position = new Vector3(size*0.1f,0);

        return newObj;
    }

    public GameObject CreateObject(GameObject snakePart)
    {
        var newObj = Instantiate(Prefab) as GameObject;
        newObj.transform.position = snakePart.transform.position;

        return newObj;
    }

	public void Log()
	{
		using (var sw = new StreamWriter("GameLog.txt", true))
		{
			sw.WriteLine(DateTime.Now + ":");
			var i =0;
            foreach (var snakepart in globals.SnakeBody)
			{
				sw.WriteLine("[" + i + "] : x - " + snakepart.transform.position.x + " , y - " + snakepart.transform.position.y);
				i++;
			}
		}
	}

    public bool CheckIntersection(Vector3 obj, Vector3 snakePart)
    {
        return Mathf.Approximately(obj.x, snakePart.x) && Mathf.Approximately(obj.y, snakePart.y);
    }

    public void RotateHead(int prevHead, int curHead)
    {

        if (curHead > prevHead)
        {
            globals.SnakeBody[0].transform.Rotate(globals.SnakeBody[0].transform.forward * ((curHead - prevHead) * -90));
          
        }
        else
        {
            globals.SnakeBody[0].transform.Rotate(globals.SnakeBody[0].transform.forward * ((prevHead - curHead) * 90));
          
        }
    }
}
