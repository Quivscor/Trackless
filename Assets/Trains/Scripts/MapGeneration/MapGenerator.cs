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
        public int numberOfLakes;
        public int numberOfDeers;
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
        private GameObject[,] mapTiles;

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
            GeneratingTerrain();
            GenerateSpawndAndEnd();
            

            // spawning objects
            SpawnTiles();
            SpawnResources();
            SpawnPassengers();
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
                        if (Random.Range(0, 100) > 90)
                            map[i, j] = (int)Tiles.forest;
                        /*
                        if (Random.Range(0, 100) > 95)
                            map[i, j] = (int)Tiles.ice;
                            */
                    }

                }
            }
            GenerateDeers();
            GenerateIce();
        }

        private void GenerateDeers()
        {
            int deers = Random.Range(numberOfDeers - 2, numberOfDeers + 4);
            while (deers > 0)
            {
                int x, y;
                (x, y) = GetRandomPoint();
                map[x, y] = (int)Tiles.deers;
                deers--;
            }
        }

        private void GenerateIce()
        {
            while(numberOfLakes > 0)
            {
                int x, y;
                (x, y) = GetRandomPoint();
                int randomBoxes = Random.Range(3, 15);
                GenerateLake(x, y, randomBoxes);
                numberOfLakes--;
            }
        }

        private void GenerateLake(int x, int y, int randomBoxes)
        {
            List<Vector2Int> lakeTiles = new List<Vector2Int>();
            while(randomBoxes > 0)
            {
                DrawLakeBox(x, y, lakeTiles);
                x = lakeTiles[Random.Range(0, lakeTiles.Count)].x;
                y = lakeTiles[Random.Range(0, lakeTiles.Count)].y;
                randomBoxes--;
            }
        }

        private void SpawnTiles()
        {
            mapTiles = new GameObject[mapSize, mapSize];
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j] != (int)Tiles.none)
                    {
                        //Debug.Log(map[i, j]);

                        Vector3 angle = new Vector3(0, Random.Range(0, 5) * 90f, 0);

                        GameObject tile = Instantiate(tiles[map[i,j]], new Vector3(
                            tiles[(int)Tiles.basic].transform.localScale.x * i,
                            0,
                            tiles[(int)Tiles.basic].transform.localScale.z * j),
                            Quaternion.Euler(angle));
                        tile.transform.SetParent(this.transform);
                        tile.name = (((Tiles)map[i, j]).ToString());
                        mapTiles[i, j] = tile;
                    }

                }
            }
        }

        private void SpawnPlayer()
        {
            Vector3 angle = new Vector3(0,0,0);
          
            if((int)spawnPoint.x - 1 >= 0 && map[(int)spawnPoint.x - 1, (int)spawnPoint.y] != (int)Tiles.border && map[(int)spawnPoint.x - 1, (int)spawnPoint.y] != (int)Tiles.none)
            {
                angle = new Vector3(0, -90f, 0);
            }
            if ((int)spawnPoint.x + 1 < mapSize && map[(int)spawnPoint.x + 1, (int)spawnPoint.y] != (int)Tiles.border && map[(int)spawnPoint.x + 1, (int)spawnPoint.y] != (int)Tiles.none)
            {
                angle = new Vector3(0, 90f, 0);
            }
            if ((int)spawnPoint.y - 1 >= 0 && map[(int)spawnPoint.x, (int)spawnPoint.y - 1] != (int)Tiles.border && map[(int)spawnPoint.x, (int)spawnPoint.y - 1] != (int)Tiles.none)
            {
                angle = new Vector3(0, 180f, 0);
            }

            //player = Instantiate(playerPrefab, new Vector3(mapSize / 2 * tileSize, 3.5f, mapSize/2*tileSize), Quaternion.identity);
            player = Instantiate(playerPrefab, new Vector3(spawnPoint.x * tileSize, 3.5f, spawnPoint.y * tileSize),
                Quaternion.Euler(angle));
            //player.transform.LookAt(new Vector3(mapSize / 2 * tileSize, 0, mapSize / 2 * tileSize));
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
                while (CheckResourceSpawnCondition(x, y));
                resourceMap[x, y] = (int)Resources.coal;
                GameObject coal = Instantiate(resources[(int)Resources.coal], new Vector3(x * tileSize, resources[(int)Resources.coal].transform.position.y, y * tileSize), Quaternion.identity);
                coal.transform.SetParent(mapTiles[x, y].transform);
                numberOfCoals--;

            }

            while (numberOfSteel > 0)
            {
                int x, y;
                do
                {
                    (x, y) = GetRandomPoint();
                }
                while (CheckResourceSpawnCondition(x, y));
                resourceMap[x, y] = (int)Resources.steel;
                Instantiate(resources[(int)Resources.steel], new Vector3(x * tileSize, 1.5f, y * tileSize), Quaternion.identity);
                numberOfSteel--;
            }
        }

        private bool CheckResourceSpawnCondition(int x, int y)
        {
            return ((map[x, y] != (int)Tiles.border && map[x, y] != (int)Tiles.forest && map[x, y] != (int)Tiles.end && map[x, y] != (int)Tiles.spawn) && resourceMap[x, y] != (int)Resources.none);
        }

        private void SpawnPassengers()
        {
            numberOfPassengers = Random.Range(3, 6);
            int passengers = numberOfPassengers;
            while (passengers > 0)
            {
                int x, y;
                do
                {
                    (x, y) = GetRandomPoint();
                    Debug.Log("TILE: " + map[x, y]);
                }
                while (CheckResourceSpawnCondition(x, y));
                resourceMap[x, y] = (int)Resources.passenger;
                GameObject passenger = Instantiate(resources[(int)Resources.passenger], new Vector3(x * tileSize, resources[(int)Resources.passenger].transform.position.y, y * tileSize), Quaternion.identity);
                passenger.transform.SetParent(mapTiles[x, y].transform);
                passengers--;
            }
        }

        private void GenerateSpawndAndEnd()
        {
            float distance = 1000;

            spawnPoint = borderPositions[Random.Range(0,borderPositions.Count)];
            endPoint = borderPositions[Random.Range(0, borderPositions.Count)];

            int errorCounter = 0;

            while (Vector2.Distance(spawnPoint, endPoint) * tileSize <= distance)
            {
                errorCounter++;
                if (errorCounter > 10000) break;
                //borderPositions.Remove(endPoint);
                endPoint = borderPositions[Random.Range(0, borderPositions.Count)];
            }
            if (errorCounter > 10000)
                Debug.Log("DIDNT FIND END POINT FAR ENOUGH FROM SPAWN");

            map[(int)spawnPoint.x, (int)spawnPoint.y] = (int)Tiles.spawn;
            map[(int)endPoint.x, (int)endPoint.y] = (int)Tiles.end;
        }

        private void DrawLakeBox(int x, int y, List<Vector2Int> lakeTiles)
        {
            // drawing box 3x3 in room boundaries

            SetTile(x - 1, y, Tiles.ice);
            SetTile(x + 1, y, Tiles.ice);
            SetTile(x, y - 1, Tiles.ice);
            SetTile(x, y + 1, Tiles.ice);
            SetTile(x - 1, y - 1, Tiles.ice);
            SetTile(x + 1, y + 1, Tiles.ice);
            SetTile(x + 1, y - 1, Tiles.ice);
            SetTile(x - 1, y + 1, Tiles.ice);


            lakeTiles.Add(new Vector2Int(x - 1, y));
            lakeTiles.Add(new Vector2Int(x + 1, y));
            lakeTiles.Add(new Vector2Int(x, y - 1));
            lakeTiles.Add(new Vector2Int(x, y + 1));
            lakeTiles.Add(new Vector2Int(x - 1, y - 1));
            lakeTiles.Add(new Vector2Int(x + 1, y + 1));
            lakeTiles.Add(new Vector2Int(x - 1, y + 1));
            lakeTiles.Add(new Vector2Int(x + 1, y - 1));

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
                if(map[x,y] != (int)Tiles.border)
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

            //Debug.Log("RANDOM POINT: " + map[x, y]);
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
                        }
                    }
                    else if(map[i, j] != (int)Tiles.none && (i == 0 || i == mapSize - 1 || j == 0 || j == mapSize - 1))
                    {
                        map[i, j] = (int)Tiles.border;
                       
                    }

                }
            }

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j] == (int)Tiles.border)
                    {
                        if (!CheckIfCorner(i, j))
                            borderPositions.Add(new Vector2(i, j));
                    }

                }
            }


        }

        private bool CheckIfCorner(int i, int j)
        {
            if(i - 1 >= 0 && j + 1 < mapSize)
                if (map[i - 1, j] == (int)Tiles.border && map[i, j + 1] == (int)Tiles.border) return true;

            if (i + 1 < mapSize && j + 1 < mapSize)
                if (map[i + 1, j] == (int)Tiles.border && map[i, j + 1] == (int)Tiles.border) return true;

            if (i + 1 < mapSize && j - 1 >= 0)
                if (map[i + 1, j] == (int)Tiles.border && map[i, j - 1] == (int)Tiles.border) return true;

            if (i - 1 >= 0 && j - 1 >= 0)
                if (map[i - 1, j] == (int)Tiles.border && map[i, j - 1] == (int)Tiles.border) return true;

            return false;
        }

        public enum Tiles
        {
            none,
            border,
            basic,
            forest,
            spawn,
            end,
            ice,
            deers
            
        }

        public enum Resources
        {
            none,
            coal,
            steel,
            passenger
        }
    }

}