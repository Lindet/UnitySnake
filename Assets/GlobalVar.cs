using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    class GlobalVar
    {
        static readonly GlobalVar _instance = new GlobalVar();

        public static GlobalVar Instance
        {
            get { return _instance; }
        }

        // Переменные класса

        public List<GameObject> SnakeBody;
        public List<GameObject> Foods = new List<GameObject>();


        public bool SnakeIsDead;
        public bool NeedNewFood;
        public int PlayerScore = 0;
        public bool FirstMenu = true;
        public bool InTheMenu = true;
        public bool AccMenu = false;

        public float bottomWall, topWall, leftWall, rightWall, scoreTopWall;

        public Camera MainCam;

        private GlobalVar()
        {  
        }

        public void CreateWalls()
        {
            bottomWall = Mathf.Round(MainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).y * 10) / 10;
            topWall = Mathf.Round(MainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y * 10 - 3f) / 10;
            scoreTopWall = Mathf.Round(MainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y * 10) / 10;
            leftWall = Mathf.Round(MainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x * 10) / 10;
            rightWall = Mathf.Round(MainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x * 10) / 10;
        }

        public void NewGame()
        {
            foreach (var parts in SnakeBody)
            {
                UnityEngine.Object.Destroy(parts);
            }
            SnakeBody = null;
            foreach (var food in Foods)
            {
                UnityEngine.Object.Destroy(food);
            }
            Foods = new List<GameObject>();
            SnakeIsDead = false;
            NeedNewFood = true;
            PlayerScore = 0;
        }
    }
}
