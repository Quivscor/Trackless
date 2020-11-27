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

        private GameObject player;
        private float tileSize;

        public Action action;

        private void Start()
        {
            tileSize = tiles[1].transform.localScale.x;
            GenerateMap();
        }

        public void GenerateMap()
        {
            // defining map
            PrepareMaps();
            BoxMethod();
            FindBorders();
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
            SetTile(mapSize / 2, mapSize / 2, Tiles.normal);
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
            
        }

        private void SpawnTiles()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j] != (int)Tiles.none)
                    {
                        Debug.Log(map[i, j]);

                        GameObject tile = Instantiate(tiles[map[i,j]], new Vector3(
                            tiles[0].transform.localScale.x * i,
                            0,
                            tiles[0].transform.localScale.z * j),
                            Quaternion.identity);
                        tile.transform.SetParent(this.transform);
                    }

                }
            }
        }

        private void SpawnPlayer()
        {
            player = Instantiate(playerPrefab, new Vector3(mapSize / 2 * tileSize, 1.5f, mapSize/2*tileSize), Quaternion.identity);
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

        private void DrawBox(int x, int y)
        {
            // drawing box 3x3 in room boundaries

            SetTile(x - 1, y, Tiles.normal);
            SetTile(x + 1, y, Tiles.normal);
            SetTile(x, y - 1, Tiles.normal);
            SetTile(x, y + 1, Tiles.normal);
            SetTile(x - 1, y - 1, Tiles.normal);
            SetTile(x + 1, y + 1, Tiles.normal);
            SetTile(x + 1, y - 1, Tiles.normal);
            SetTile(x - 1, y + 1, Tiles.normal);

        }
        private void DrawBox5(int x, int y)
        {
            // drawing box 3x3 in room boundaries

            SetTile(x - 1, y, Tiles.normal);
            SetTile(x + 1, y, Tiles.normal);
            SetTile(x, y - 1, Tiles.normal);
            SetTile(x, y + 1, Tiles.normal);
            SetTile(x - 1, y - 1, Tiles.normal);
            SetTile(x + 1, y + 1, Tiles.normal);
            SetTile(x + 1, y - 1, Tiles.normal);
            SetTile(x - 1, y + 1, Tiles.normal);

            SetTile(x - 2, y, Tiles.normal);
            SetTile(x + 2, y, Tiles.normal);
            SetTile(x, y - 2, Tiles.normal);
            SetTile(x, y + 2, Tiles.normal);
            SetTile(x - 2, y - 2, Tiles.normal);
            SetTile(x + 2, y + 2, Tiles.normal);
            SetTile(x + 2, y - 2, Tiles.normal);
            SetTile(x - 2, y + 2, Tiles.normal);

            SetTile(x - 2, y - 1, Tiles.normal);
            SetTile(x - 2, y + 1, Tiles.normal);
            SetTile(x + 2, y - 1, Tiles.normal);
            SetTile(x + 2, y + 1, Tiles.normal);

            SetTile(x - 1, y - 2, Tiles.normal);
            SetTile(x + 1, y - 2, Tiles.normal);
            SetTile(x - 1, y + 2, Tiles.normal);
            SetTile(x + 1, y + 2, Tiles.normal);

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
        }

        public enum Tiles
        {
            none,
            border,
            normal,
            
        }

        public enum Resources
        {
            none,
            coal,
            steel
        }
    }

}