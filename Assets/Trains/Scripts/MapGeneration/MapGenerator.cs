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
        public int numberOfMountains;
        public int numberOfLakes;
        public int numberOfDeers;
        public int numberOfBlizzards;
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
        [SerializeField]
        private GameObject testMark;

        private int[,] map;
        private int[,] resourceMap;
        private bool[,] canResourceMap;

        private List<Vector2Int> borderPositions;
        private GameObject[,] mapTiles;

        private Vector2Int spawnPoint;
        private Vector2Int endPoint;
        public List<GameObject> endPointNeighbours;

        public GameObject spawnObject;
        public GameObject endObject;

        private GameObject player;
        private float tileSize;

        public Action action;

        private void Start()
        {
            tileSize = tiles[(int)Tiles.deers].transform.localScale.x;
            GenerateMap();
        }
        private void Update()
        {
            if(InputManager.Data.clear)
            {
                ClearMap();
                GenerateMap();
            }
                
        }
        public void GenerateMap()
        {
            // defining map
            PrepareMaps();
            BoxMethod();
            FindBorders(false);

            GenerateMountains();
            FindBorders(true);

            GeneratingTerrain();
            GenerateSpawndAndEnd();
            

            // spawning objects
            SpawnTiles();
            SpawnResources();
            SpawnPassengers();
            SpawnPlayer();
            action?.Invoke();
        }

        public void ClearMap()
        {
            foreach (Transform tile in transform)
            {
                Destroy(tile.gameObject);
            }

            Destroy(player);
        }

        
        private void PrepareMaps()
        {
            map = new int[mapSize, mapSize];
            resourceMap = new int[mapSize, mapSize];
            canResourceMap = new bool[mapSize, mapSize];

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    map[i, j] = (int)Tiles.none;
                    resourceMap[i, j] = (int)Resources.none;
                    canResourceMap[i, j] = false;
                }
            }
        }

        private void BoxMethod()
        {
            SetTile(mapSize / 2, mapSize / 2, Tiles.basic);

            DrawAnyBox(mapSize / 2, mapSize / 2, 9);
            int boxes = numberOfBoxes;
            while (boxes > 0)
            {
                int x, y;
                (x, y) = GetRandomPoint();
                DrawBox5(x, y);
                boxes--;
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
                    }

                }
            }

            GenerateDeers();
            GenerateIce();
            GenerateBlizzards();
        }

        private void GenerateMountains()
        {
            int mountains = numberOfMountains;
            while (mountains > 0)
            {
                int x, y;
                (x, y) = GetRandomPoint();
                int walkerRounds = Random.Range(15, 25);
                SetMountain(x, y, walkerRounds);
                mountains--;
            }
        }

        private void SetMountain(int x, int y, int walkerRounds)
        {
            // drunk walker generation
            Vector2Int walker = new Vector2Int(x, y);
            Direction randomDirection;

            while (walkerRounds > 0)
            {
                randomDirection = (Direction)Random.Range(0, 4);
                if(randomDirection == Direction.up)
                {
                    if (walker.y + 1 < mapSize)
                        walker.y += 1;

                }
                if (randomDirection == Direction.down)
                {
                    if (walker.y - 1 >= 0)
                        walker.y -= 1;
                }
                if (randomDirection == Direction.left)
                {
                    if (walker.x - 1 >= 0)
                        walker.x -= 1;
                }
                if (randomDirection == Direction.right)
                {
                    if (walker.x + 1 >= 0)
                        walker.x += 1;
                }

                SetTile(walker.x, walker.y, Tiles.none);
                walkerRounds--;
            }


        }

        private enum Direction
        {
            up,
            right,
            down,
            left
        }

        private void GenerateDeers()
        {
            int deers = Random.Range(numberOfDeers - 2, numberOfDeers + 4);
            while (deers > 0)
            {
                int x, y;
                (x, y) = GetRandomPoint();
                if(map[x, y] != (int)Tiles.border)
                {
                    map[x, y] = (int)Tiles.deers;
                    deers--;
                }
                   
               
            }
        }

        private void GenerateBlizzards()
        {
            int blizzards = Random.Range(numberOfBlizzards - 2, numberOfBlizzards + 2);
            while (blizzards > 0)
            {
                int x, y;
                (x, y) = GetRandomPoint();
                if (map[x, y] != (int)Tiles.border)
                {
                    map[x, y] = (int)Tiles.blizzard;
                    blizzards--;
                }


            }
        }

        private void GenerateIce()
        {
            int lakes = numberOfLakes;
            while(lakes > 0)
            {
                int x, y;
                (x, y) = GetRandomPoint();
                int randomBoxes = Random.Range(3, 15);
                GenerateLake(x, y, randomBoxes);
                lakes--;
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
                        Vector3 angle = new Vector3(0,0,0);
                        //Debug.Log(map[i, j]);
                        if(map[i,j] == (int)Tiles.end || map[i,j] == (int)Tiles.spawn)
                        {

                            if ((int)i - 1 >= 0 && map[i- 1, j] != (int)Tiles.border && map[i - 1, j] != (int)Tiles.none)
                            {
                                angle = new Vector3(0, -90f, 0);
                            }
                            if (i + 1 < mapSize && map[i + 1, j] != (int)Tiles.border && map[i + 1, j] != (int)Tiles.none)
                            {
                                angle = new Vector3(0, 90f, 0);
                            }
                            if (i - 1 >= 0 && map[i, j - 1] != (int)Tiles.border && map[i, j - 1] != (int)Tiles.none)
                            {
                                angle = new Vector3(0, 180f, 0);
                            }
                        }
                        else angle = new Vector3(0, Random.Range(0, 5) * 90f, 0);

                        GameObject tile = Instantiate(tiles[map[i,j]], new Vector3(
                            tiles[(int)Tiles.basic].transform.localScale.x * i,
                            0,
                            tiles[(int)Tiles.basic].transform.localScale.z * j),
                            Quaternion.Euler(angle));
                        tile.transform.SetParent(this.transform);
                        tile.name = (((Tiles)map[i, j]).ToString());
                        mapTiles[i, j] = tile;

                        if (map[i, j] == (int)Tiles.spawn) spawnObject = tile;
                        if (map[i, j] == (int)Tiles.end) endObject = tile;
                    }

                }
            }
            endPointNeighbours = new List<GameObject>();
            for (int i = endPoint.x - 10; i <= endPoint.x + 10; i++)
            {
                for (int j = endPoint.y - 10; j <= endPoint.y + 10; j++)
                {
                    if (i >= 0 && i < mapSize && j >= 0 && j < mapSize)
                    {
                        if (map[i, j] == (int)Tiles.border)
                            endPointNeighbours.Add(mapTiles[i, j]);
                    }
                }
            }
        }

        public void TurnOffCollidersOfEndPointNeighbours()
        {
            foreach (GameObject item in endPointNeighbours)
            {
                item.GetComponentInChildren<BoxCollider>().isTrigger = true;
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
            player = Instantiate(playerPrefab, new Vector3(spawnPoint.x * tileSize, 2.9f, spawnPoint.y * tileSize),
                Quaternion.Euler(angle));
            //player.transform.LookAt(new Vector3(mapSize / 2 * tileSize, 0, mapSize / 2 * tileSize));
        }

        private void SpawnResources()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j] == (int)Tiles.basic || map[i,j] == (int)Tiles.ice)
                    {
                        if(i-1 >= 0 && i+1 < mapSize && j - 1 >= 0 && j + 1 < mapSize)
                        {
                            if (map[i - 1, j] != (int)Tiles.border && map[i + 1, j] != (int)Tiles.border && map[i, j - 1] != (int)Tiles.border && map[i, j + 1] != (int)Tiles.border)
                            {
                                canResourceMap[i, j] = true;
                                //Instantiate(testMark, new Vector3(i * tileSize, 1, j * tileSize), Quaternion.identity);
                            }
                        }
                        
                    }

                }
            }

            int coals = numberOfCoals;
            while(coals > 0)
            {
                int x, y;
                do
                {
                    (x, y) = GetRandomPoint();

                } while (!canResourceMap[x, y]);
                resourceMap[x, y] = (int)Resources.coal;
                canResourceMap[x, y] = false;
                GameObject coal = Instantiate(resources[(int)Resources.coal], new Vector3(x * tileSize, resources[(int)Resources.coal].transform.position.y, y * tileSize), Quaternion.identity);
                coal.transform.SetParent(mapTiles[x, y].transform);
                coals--;

            }
            int steel = numberOfSteel;
            while (steel > 0)
            {
                int x, y;
                do
                {
                    (x, y) = GetRandomPoint();

                } while (!canResourceMap[x, y]);
                resourceMap[x, y] = (int)Resources.steel;
                canResourceMap[x, y] = false;
                GameObject steelObject = Instantiate(resources[(int)Resources.steel], new Vector3(x * tileSize, 1.5f, y * tileSize), Quaternion.identity);
                steelObject.transform.SetParent(mapTiles[x, y].transform);
                steel--;

            }
            /*
            while (numberOfCoals > 0)
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
            */
        }

        private bool CheckResourceSpawnCondition(int x, int y)
        {
            return ((map[x, y] != (int)Tiles.border && map[x, y] != (int)Tiles.forest && map[x, y] != (int)Tiles.end && map[x, y] != (int)Tiles.spawn) && resourceMap[x, y] != (int)Resources.none);
        }

        private void SpawnPassengers()
        {
            int passengers = Random.Range(numberOfPassengers-1, numberOfPassengers+2);
            numberOfPassengers = passengers;

            while (passengers > 0)
            {
                int x, y;
                do
                {
                    (x, y) = GetRandomPoint();
                    //Debug.Log("TILE: " + map[x, y]);

                } while (!canResourceMap[x, y]);
                resourceMap[x, y] = (int)Resources.passenger;
                canResourceMap[x, y] = false;
                GameObject passenger = Instantiate(resources[(int)Resources.passenger], new Vector3(x * tileSize, resources[(int)Resources.passenger].transform.position.y, y * tileSize), Quaternion.identity);
                passenger.transform.SetParent(mapTiles[x, y].transform);
                passengers--;
            }
            /*

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
            */
        }

        private void GenerateSpawndAndEnd()
        {
            float distance = 1000;
            int errorCounter = 0;

            do
            {
                spawnPoint = borderPositions[Random.Range(0, borderPositions.Count)];
                errorCounter++;
                if(errorCounter > 10000)
                {
                    Debug.Log("DIDNT FIND PROPER SPAWN POINT");
                    break;
                }
            } while (CheckSpawnPointCondition(spawnPoint));
            
          
            endPoint = borderPositions[Random.Range(0, borderPositions.Count)];

            while (Vector2.Distance(spawnPoint, endPoint) * tileSize <= distance)
            {
                errorCounter++;
                if (errorCounter > 10000)
                {
                    Debug.Log("DIDNT FIND END POINT FAR ENOUGH FROM SPAWN");
                    break;
                }

                //borderPositions.Remove(endPoint);
                endPoint = borderPositions[Random.Range(0, borderPositions.Count)];
            }             

            map[spawnPoint.x,spawnPoint.y] = (int)Tiles.spawn;
            map[endPoint.x, endPoint.y] = (int)Tiles.end;


            /*
            int iterations = 5;
            if(map[endPoint.x-1, endPoint.y] == (int)Tiles.none)
            {
                while(iterations > 0)
                {
                    Instantiate(tiles[(int)Tiles.end], new Vector3(endPoint.x * tileSize -tileSize*iterations, 0, endPoint.y * tileSize), Quaternion.identity);
                    iterations--;
                }
            }
            if (map[endPoint.x + 1, endPoint.y] == (int)Tiles.none)
            {
                while (iterations > 0)
                {
                    Instantiate(tiles[(int)Tiles.end], new Vector3(endPoint.x * tileSize + tileSize * iterations, 0, endPoint.y * tileSize), Quaternion.identity);
                    iterations--;
                }
            }
            if (map[(int)endPoint.x, (int)endPoint.y - 1] == (int)Tiles.none)
            {
                while (iterations > 0)
                {
                    Instantiate(tiles[(int)Tiles.end], new Vector3(endPoint.x * tileSize, 0, endPoint.y * tileSize - tileSize * iterations), Quaternion.identity);
                    iterations--;
                }
            }
            if (map[(int)endPoint.x, (int)endPoint.y + 1] == (int)Tiles.none)

            {
                while (iterations > 0)                {
                    Instantiate(tiles[(int)Tiles.end], new Vector3(endPoint.x * tileSize, 0, endPoint.y * tileSize + tileSize * iterations), Quaternion.identity);
                    iterations--;
                }
            }*/
        }

        private bool CheckSpawnPointCondition(Vector2Int spawnPoint)
        {
            int neighbours = 0;
            for (int i = spawnPoint.x-1; i <= spawnPoint.x+1; i++)
            {
                for (int j = spawnPoint.y-1; j <= spawnPoint.y +1; j++)
                {
                    if(i >= 0 && i < mapSize && j >= 0 && j < mapSize)
                    {
                        if (map[i, j] == (int)Tiles.border)
                            neighbours++;
                    }
                  
                }
            }

            if (neighbours <= 3) return false;
            else return true;


        }

        private void DrawLakeBox(int x, int y, List<Vector2Int> lakeTiles)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && i < mapSize && j >= 0 && j < mapSize)
                    {
                        if (map[i, j] != (int)Tiles.none)
                        {
                            SetTile(i, j, Tiles.ice);
                            lakeTiles.Add(new Vector2Int(i, j));
                        }
                            
                    }

                }
            }
        }
        private void DrawBox5(int x, int y)
        {
            for (int i = x-2; i <= x+2; i++)
            {
                for (int j = y-2; j <= y+2; j++)
                {
                    if (i >= 0 && i < mapSize && j >= 0 && j < mapSize)
                    {
                        SetTile(i, j, Tiles.basic);
                    }
                }
            }
        }

        private void DrawAnyBox(int x, int y, int boxSize)
        {
            int size = boxSize / 2;
            for (int i = x - size; i <= x + size; i++)
            {
                for (int j = y - size; j <= y + size; j++)
                {
                    if (i >= 0 && i < mapSize && j >= 0 && j < mapSize)
                    {
                        SetTile(i, j, Tiles.basic);
                    }
                }
            }
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

            return (x, y);
        }

        private void FindBorders(bool areMountains)
        {
            if(!areMountains)
                borderPositions = new List<Vector2Int>();

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
            if (!areMountains)
            {
                for (int i = 0; i < mapSize; i++)
                {
                    for (int j = 0; j < mapSize; j++)
                    {
                        if (map[i, j] == (int)Tiles.border)
                        {
                            if (!CheckIfCorner(i, j))
                                borderPositions.Add(new Vector2Int(i, j));
                        }

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
            deers,
            blizzard
            
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