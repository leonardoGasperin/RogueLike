using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;



    public class BoardManager : MonoBehaviour
    {
        [Serializable]
        public class Count
        {
            public int minimum;
            public int maximum;

            public Count(int min, int max)
            {
                minimum = min;
                maximum = max;
            }
        }

        //64bits boards
        public int columns = 8;
        public int rows = 8;
        //counts for all and itens and soon more things (min,max)
        public Count wallCount = new Count(5, 9);
        public Count foodCount = new Count(1, 5);
        //spawns
        public GameObject exit;
        public GameObject[] floorTiles;
        public GameObject[] wallTiles;
        public GameObject[] foodTiles;
        public GameObject[] enemyTiles;
        public GameObject[] outerWallTiles;

        private Transform boardHolder;//hierarchy clear
        private List<Vector3> gridPositions = new List<Vector3>();//all possible positions (64)

        /*funtions*/

        void InitialiseList()
        {
            gridPositions.Clear();

            //make a grid positions list as Vector3
            for (int x = 1; x < columns - 1; x++)
            {
                for (int y = 1; y < rows - 1; y++)
                {
                    gridPositions.Add(new Vector3(x, y, 0.0f));
                }
            }
        }

        void BoardSetup()
        {
            boardHolder = new GameObject("Board").transform;

            for (int x = -1; x < columns + 1; x++)
            {
                for (int y = -1; y < rows + 1; y++)
                {
                    //floor
                    GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                    //outWall
                    if (x == -1 || x == columns || y == -1 || y == rows)
                        toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                    GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                    instance.transform.SetParent(boardHolder);
                }
            }
        }

        Vector3 RandomPosition()
        {
            int randomIndex = Random.Range(0, gridPositions.Count);
            Vector3 randomPosition = gridPositions[randomIndex];
            gridPositions.RemoveAt(randomIndex);//remove the object on the same place

            return randomPosition;
        }

        void LayoutObjectRandom(GameObject[] tileArray, int minimum, int maximum)
        {
            int objectCount = Random.Range(minimum, maximum + 1);//plus on each level the number of enemy and walls

            //put on board
            for (int i = 0; i < objectCount; i++)
            {
                Vector3 randomPosition = RandomPosition();
                GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
                Instantiate(tileChoice, randomPosition, Quaternion.identity);
            }
        }

        public void SetupLevel(int level)
        {
            BoardSetup();
            InitialiseList();
            LayoutObjectRandom(wallTiles, wallCount.minimum, wallCount.maximum);
            LayoutObjectRandom(foodTiles, foodCount.minimum, foodCount.maximum);
            int enemyCount = (int)Mathf.Log(level, 2.0f);//upper maximum of enemy per lvl
            LayoutObjectRandom(enemyTiles, enemyCount, enemyCount);
            Instantiate(exit, new Vector3(columns - 1, rows - 1, 0.0f), Quaternion.identity);
        }
    }