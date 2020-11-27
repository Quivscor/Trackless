using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TracklessGenerator
{
    public class MapGenerator : MonoBehaviour
    {
        [Header("Generator options")]
        public int mapSize;
        public int numberOfBoxes;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject[] tiles;
        [SerializeField]
        private GameObject playerPrefab;

        private int[,] map;
        private GameObject player;
        private float tileSize;

        private void Start()
        {
            tileSize = tiles[0].transform.localScale.x;
            GenerateMap();
        }

        public void GenerateMap()
        {
            PrepareMap();
            BoxMethod();
            FindBorders();
            SpawnTiles();
            SpawnPlayer();
        }

        private void PrepareMap()
        {
            map = new int[mapSize, mapSize];

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    map[i, j] = (int)Tiles.none;
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

        private void SpawnTiles()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j] != (int)Tiles.none)
                    {
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
            normal,
            border
        }
    }

}