using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace TracklessGenerator
{
    public class MapGenerator : MonoBehaviour
    {
        [Header("Generator options")]
        public int mapSize;
        public int numberOfBoxes;
        public int numberOfCoals;
        public int numberOfSteel;
        public int numberOfPassengers;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject[] tiles;
        [SerializeField]
        private GameObject[] resources;
        [SerializeField]
        private GameObject playerPrefab;

        private int[,] map;
        private int[,] resourceMap;
        private List<Vector2> borderPositions;

        private Vector2 spawnPoint;
        private Vector2 endPoint;
        private GameObject player;
        private float tileSize;

        public Action action;

        private void Start()
        {
            tileSize = tiles[(int)Tiles.basic].transform.localScale.x;
            GenerateMap();
        }

        public void GenerateMap()
        {
            // defining map
            PrepareMaps();
            BoxMethod();
            FindBorders();
            GenerateSpawndAndEnd();
            GeneratingTerrain();

            // spawning objects
            SpawnTiles();
            SpawnResources();
            SpawnPlayer();
            action?.Invoke();
        }

        
        private void PrepareMaps()
        {
            map = new int[mapSize, mapSize];
            resourceMap = new int[mapSize, mapSize];

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    map[i, j] = (int)Tiles.none;
                    resourceMap[i, j] = (int)Resources.none;
                }
            }
        }

        private void BoxMethod()
        {
            SetTile(mapSize / 2, mapSize / 2, Tiles.basic);
            DrawBox5(mapSize / 2, mapSize / 2);
            while (numberOfBoxes > 0)
            {
                int x, y;
                (x, y) = GetRandomPoint();
                DrawBox5(x, y);
                numberOfBoxes--;
            }

        }
        private void GeneratingTerrain()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j] == (int)Tiles.basic)
                    {
                        if (Random.Range(0, 100) > 95)
                            map[i, j] = (int)Tiles.forest;
                    }

                }
            }
        }

        private void SpawnTiles()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j] != (int)Tiles.none)
                    {
                        //Debug.Log(map[i, j]);

                        GameObject tile = Instantiate(tiles[map[i,j]], new Vector3(
                            tiles[(int)Tiles.basic].transform.localScale.x * i,
                            0,
                            tiles[(int)Tiles.basic].transform.localScale.z * j),
                            Quaternion.identity);
                        tile.transform.SetParent(this.transform);
                    }

                }
            }
        }

        private void SpawnPlayer()
        {
            //player = Instantiate(playerPrefab, new Vector3(mapSize / 2 * tileSize, 3.5f, mapSize/2*tileSize), Quaternion.identity);
            player = Instantiate(playerPrefab, new Vector3(spawnPoint.x * tileSize, 3.5f, spawnPoint.y * tileSize), Quaternion.identity);
        }

        private void SpawnResources()
        {

            while(numberOfCoals > 0)
            {
                int x, y;
                do
                {
                    (x, y) = GetRandomPoint();
                }
                while (resourceMap[x, y] != (int)Resources.none);

                resourceMap[x, y] = (int)Resources.coal;
                Instantiate(resources[(int)Resources.coal], new Vector3(x * tileSize, 1.5f, y * tileSize), Quaternion.identity);
                numberOfCoals--;
            }

            while (numberOfSteel > 0)
            {
                int x, y;
                do
                {
                    (x, y) = GetRandomPoint();
                }
                while (resourceMap[x, y] != (int)Resources.none);

                resourceMap[x, y] = (int)Resources.steel;
                Instantiate(resources[(int)Resources.steel], new Vector3(x * tileSize, 1.5f, y * tileSize), Quaternion.identity);
                numberOfSteel--;
            }
        }

        private void GenerateSpawndAndEnd()
        {
            spawnPoint = borderPositions[Random.Range(0,borderPositions.Count)];
            float distance = 1000;
            Vector2 endPoint = borderPositions[Random.Range(0, borderPositions.Count)];
            int errorCounter = 0;
            while (Vector2.Distance(spawnPoint, endPoint) < distance)
            {
                errorCounter++;
                if (errorCounter > 10000) break;
                endPoint = borderPositions[Random.Range(0, borderPositions.Count)];
            }
            if (errorCounter > 10000)
                Debug.Log("DIDNT FIND END POINT FAR ENOUGH FROM SPAWN");
            map[(int)spawnPoint.x, (int)spawnPoint.y] = (int)Tiles.spawn;
            map[(int)endPoint.x, (int)endPoint.y] = (int)Tiles.end;
        }

        private void DrawBox(int x, int y)
        {
            // drawing box 3x3 in room boundaries

            SetTile(x - 1, y, Tiles.basic);
            SetTile(x + 1, y, Tiles.basic);
            SetTile(x, y - 1, Tiles.basic);
            SetTile(x, y + 1, Tiles.basic);
            SetTile(x - 1, y - 1, Tiles.basic);
            SetTile(x + 1, y + 1, Tiles.basic);
            SetTile(x + 1, y - 1, Tiles.basic);
            SetTile(x - 1, y + 1, Tiles.basic);

        }
        private void DrawBox5(int x, int y)
        {
            // drawing box 3x3 in room boundaries

            SetTile(x - 1, y, Tiles.basic);
            SetTile(x + 1, y, Tiles.basic);
            SetTile(x, y - 1, Tiles.basic);
            SetTile(x, y + 1, Tiles.basic);
            SetTile(x - 1, y - 1, Tiles.basic);
            SetTile(x + 1, y + 1, Tiles.basic);
            SetTile(x + 1, y - 1, Tiles.basic);
            SetTile(x - 1, y + 1, Tiles.basic);

            SetTile(x - 2, y, Tiles.basic);
            SetTile(x + 2, y, Tiles.basic);
            SetTile(x, y - 2, Tiles.basic);
            SetTile(x, y + 2, Tiles.basic);
            SetTile(x - 2, y - 2, Tiles.basic);
            SetTile(x + 2, y + 2, Tiles.basic);
            SetTile(x + 2, y - 2, Tiles.basic);
            SetTile(x - 2, y + 2, Tiles.basic);

            SetTile(x - 2, y - 1, Tiles.basic);
            SetTile(x - 2, y + 1, Tiles.basic);
            SetTile(x + 2, y - 1, Tiles.basic);
            SetTile(x + 2, y + 1, Tiles.basic);

            SetTile(x - 1, y - 2, Tiles.basic);
            SetTile(x + 1, y - 2, Tiles.basic);
            SetTile(x - 1, y + 2, Tiles.basic);
            SetTile(x + 1, y + 2, Tiles.basic);

        }

        private void SetTile(int x, int y, Tiles tile)
        {
            if (x < mapSize && x >= 0 && y < mapSize && y >= 0)
            {
                map[x, y] = (int)tile;
            }
        }

        private (int, int) GetRandomPoint()
        {
            int x, y;

            do
            {
                x = Random.Range(0, mapSize);
                y = Random.Range(0, mapSize);
            } while (map[x, y] == (int)Tiles.none);

            return (x, y);

        }

        private void FindBorders()
        {
            borderPositions = new List<Vector2>();

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j] != (int)Tiles.none && (i - 1) >= 0 && (i + 1) < mapSize && (j - 1) >= 0 && (j + 1) < mapSize)
                    {
                        if(map[i-1,j] == (int)Tiles.none || map[i + 1, j] == (int)Tiles.none || map[i, j + 1] == (int)Tiles.none || map[i, j - 1] == (int)Tiles.none)
                        {
                            map[i, j] = (int)Tiles.border;
                            borderPositions.Add(new Vector2(i, j));
                        }
                    }
                    else if(map[i, j] != (int)Tiles.none && (i == 0 || i == mapSize - 1 || j == 0 || j == mapSize - 1))
                    {
                        map[i, j] = (int)Tiles.border;
                        borderPositions.Add(new Vector2(i, j));
                    }

                }
            }
        }

        public enum Tiles
        {
            none,
            border,
            basic,
            forest,
            spawn,
            end
            
        }

        public enum Resources
        {
            none,
            coal,
            steel
        }
    }

}