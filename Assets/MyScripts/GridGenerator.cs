using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject blockPrefab2;
    public GameObject basePrefab;
    public GameObject coinPrefab;
    public GameObject holePrefab;
    public Player currentPlayer;
    public Transform baseParent;
    public Color obstacleColor;
    public int gridX;
    public int gridZ;
    public int obstacleNumber = 3;
    public int coinsToProgress = 5;
    public float waitUpTime = 1f;
    public float waitDownTime = 1f;
    public bool LoadColliders = true;
    public float blockAnimationTime = 1f;
    
    // Start is called before the first frame update
    private List<GameObject> gridTiles;
    private List<GameObject> raisedTiles;
    private List<int> raisedTileIDs;

    private GameObject coin;
    private GameObject checkpoint;

    private float width = 0;
    private float blockOffset;
    private float halfWidth;

    private bool checkPointspawned = false;
    private bool coinSpawned = false;
    private bool moving = false;
    private bool gameActive = false;
    private bool paused = false;
    private void Awake()
    {
        raisedTiles = new List<GameObject>();
        width = gridX * blockPrefab.transform.localScale.x;
        blockOffset = blockPrefab.transform.localScale.x / 2;
        halfWidth = width / 2;
    }

    private void Start()
    {
        RegenGrid();
        InitializeCoin();
        InitializeProgressor();
    }

    private void Update()
    {
        if (!moving&&!paused)
        {
            StartCoroutine(MoveBlock());
        }
        if (gameActive)
        {
            if (!coinSpawned)
            {
                int score = currentPlayer.GetScore();

                if(score<coinsToProgress)
                {
                    SpawnCoin();
                }
                else if (!checkPointspawned)
                {
                    DespawnCoin();
                    SpawnCheckpoint();
                }
            }
        }
        else if (!gameActive)
        {
            if (coinSpawned)
            {
                DespawnCoin();
            }
        }
        
    }

    public void InitializeCoin()
    {
        GameObject coinHolder = new GameObject();
        coinHolder.transform.parent = transform;
        coinHolder.name = "Coins";
        coinHolder.transform.localPosition = Vector3.zero;
        coin = Instantiate(coinPrefab, Vector3.zero, transform.rotation);
        coin.transform.parent = coinHolder.transform;
        DespawnCoin();
        coin.tag = "Coin";
        coin.name = "Coin";
        //SpawnCoin();
    }

    public void InitializeProgressor()
    {
        GameObject checkPointHolder = new GameObject();
        checkPointHolder.transform.parent = transform;
        checkPointHolder.name = "Checkpoint";
        checkPointHolder.transform.localPosition = Vector3.zero;
        checkpoint = Instantiate(holePrefab, Vector3.zero, transform.rotation);
        checkpoint.transform.parent = checkPointHolder.transform;
        checkpoint.transform.localPosition = new Vector3(0, 0, -100);
        checkpoint.name = "Checkpoint";
        checkpoint.tag = "Checkpoint";
    }
    public float GetGridWidth()
    {
        return width;
    }

    public float GetTileOffset()
    {
        return blockOffset;
    }

    public void SetCoinSpawned(bool b)
    {
        coinSpawned = b;
    }

    public bool GetCoinSpawned()
    {
        return coinSpawned;
    }

    private IEnumerator MoveUp(Transform block)
    {
        moving = true;
        float tTotal = blockAnimationTime;
        float tMovement = 0f;
        Vector3 downPosition = block.transform.localPosition;
        Vector3 upPosition = downPosition + new Vector3(0f, 5f, 0f);
        Renderer rend = block.GetComponent<Renderer>();
        Color originalColor = rend.material.color;

        while (tMovement < tTotal)
        {
            while (paused)
            {
                yield return null;
            }
            tMovement += Time.deltaTime;
            block.transform.localPosition = Vector3.Lerp(downPosition, upPosition, tMovement / tTotal);
            float lerp = Mathf.PingPong(tMovement, blockAnimationTime) / blockAnimationTime;
            rend.material.color = Color.Lerp(originalColor, obstacleColor, lerp);
            yield return null;
        }

        yield return new WaitForSeconds(waitUpTime);

        StartCoroutine(MoveDown(block.transform, originalColor));
    }

    private IEnumerator MoveDown(Transform block, Color originalColor)
    {
        moving = true;
        float tTotal = blockAnimationTime;
        float tMovement = 0f;
        Vector3 upPosition = block.transform.localPosition;
        Vector3 downPosition = upPosition - new Vector3(0f, 5f, 0f);
        Renderer rend = block.GetComponent<Renderer>();

        while (tMovement < tTotal)
        {
            while (paused)
            {
                yield return null;
            }
            
            tMovement += Time.deltaTime;
            block.transform.localPosition = Vector3.Lerp(upPosition, downPosition, tMovement / tTotal);
            float lerp = Mathf.PingPong(tMovement, blockAnimationTime) / blockAnimationTime;
            rend.material.color = Color.Lerp(obstacleColor, originalColor, lerp);
            yield return null;
        }

        yield return new WaitForSeconds(waitDownTime);

        raisedTileIDs.Clear();
        raisedTiles.Clear();
        moving = false;
    }

    private IEnumerator MoveBlock()
    {
        while (paused)
        {
            yield return null;
        }
        float tTotal = blockAnimationTime;

        raisedTileIDs = GenerateUniqueRandom(obstacleNumber, gridTiles.Count);

        foreach (int tileID in raisedTileIDs)
        {
            raisedTiles.Add(gridTiles[tileID]);
        }

        foreach (GameObject currentBlock in raisedTiles)
        {
            StartCoroutine(MoveUp(currentBlock.transform));
        }
        yield return null;
    }

    private void RegenGrid()
    {
        gridTiles = new List<GameObject>();

        Vector3 newPos = new Vector3(-halfWidth + blockOffset, 0, -halfWidth + blockOffset);
        transform.position = newPos;

        GameObject gridHolder = Instantiate(basePrefab, Vector3.zero, basePrefab.transform.rotation);
        gridHolder.transform.parent = transform;
        gridHolder.name = "Grid";
        gridHolder.transform.localPosition = Vector3.zero;

        if (blockPrefab != null)
        {
            bool block1 = false;
            float tileSize = blockPrefab.transform.localScale.x;
            for (int x = 0; x < gridX; x++)
            {
                block1 = !block1;
                for (int z = 0; z < gridZ; z++)
                {
                    GameObject tile;
                    if (block1)
                    {
                        tile = Instantiate(blockPrefab, Vector3.zero, blockPrefab.transform.rotation);
                    }
                    else
                    {
                        tile = Instantiate(blockPrefab2, Vector3.zero, blockPrefab2.transform.rotation);
                    }
                    block1 = !block1;
                    // Instantiate tile instance
                    tile.name = (x + "," + z);
                    //tile.transform.parent = transform;
                    tile.transform.parent = gridHolder.transform;
                    tile.transform.localPosition = new Vector3(x * tileSize, 0, z * tileSize);
                    tile.tag = "tile";
                    tile.AddComponent<Rigidbody>();
                    tile.GetComponent<Rigidbody>().isKinematic = true;
                    tile.GetComponent<Rigidbody>().useGravity = false;
                    gridTiles.Add(tile.gameObject);
                }
            }
        }
        else
        {
            Debug.Log("ERROR: Block prefab not set, grid not generated.");
        }
        Debug.Log(gridX + "," + gridZ + " grid generated.");
        Debug.Log("gridtiles: " + gridTiles.Count);

        //Instantiates the base object to match the size of the generate grid.
        if (basePrefab != null)
        {
            GameObject baseTile = Instantiate(basePrefab, Vector3.zero, basePrefab.transform.rotation);
            baseTile.transform.parent = baseParent;
            baseTile.transform.localScale = new Vector3(width + 2, blockPrefab.transform.localScale.y, width + 2);
            baseTile.transform.localPosition = new Vector3(0, -1, 0);
            baseTile.name = "BaseTile";

            if (LoadColliders)
            {
                int colliderThickness = 2;
                GameObject northCollider = new GameObject("NorthCollider");
                northCollider.transform.parent = baseTile.transform;
                northCollider.transform.localPosition = new Vector3(0f, 0f, 0.5f);
                BoxCollider nCol = northCollider.AddComponent<BoxCollider>();
                nCol.size = new Vector3(width + 2, 40, colliderThickness);

                GameObject southCollider = new GameObject("SouthCollider");
                southCollider.transform.parent = baseTile.transform;
                southCollider.transform.localPosition = new Vector3(0f, 0f, -0.5f);
                BoxCollider sCol = southCollider.AddComponent<BoxCollider>();
                sCol.size = new Vector3(width + 2, 40, colliderThickness);

                GameObject eastCollider = new GameObject("EastCollider");
                eastCollider.transform.parent = baseTile.transform;
                eastCollider.transform.localPosition = new Vector3(0.5f, 0f, 0f);
                BoxCollider eCol = eastCollider.AddComponent<BoxCollider>();
                eCol.size = new Vector3(colliderThickness, 40, width + 2);

                GameObject westCollider = new GameObject("WestCollider");
                westCollider.transform.parent = baseTile.transform;
                westCollider.transform.localPosition = new Vector3(-0.5f, 0f, 0f);
                BoxCollider wCol = westCollider.AddComponent<BoxCollider>();
                wCol.size = new Vector3(colliderThickness, 40f, width + 2);

                GameObject topCollider = new GameObject("TopCollider");
                topCollider.transform.parent = baseTile.transform;
                topCollider.transform.localScale = new Vector3(1f, 1f, 1f);
                topCollider.transform.localPosition = new Vector3(0f, 4f, 0f);
                topCollider.tag = "TopCol";
                BoxCollider tCol = topCollider.AddComponent<BoxCollider>();
                tCol.size = new Vector3(1f, 3f, 1f);
                tCol.isTrigger = true;


                GameObject bottomCollider = new GameObject("BottomCollider");
                bottomCollider.transform.parent = baseTile.transform;
                bottomCollider.transform.localScale = new Vector3(1f, 1f, 1f);
                bottomCollider.transform.localPosition = new Vector3(0f, -10f, 00f);
                bottomCollider.tag = "BottomCol";
                BoxCollider bCol = bottomCollider.AddComponent<BoxCollider>();
                bCol.size = new Vector3(1f, 3f, 1f);
                tCol.isTrigger = true;
            }
        }
        else
        {
            Debug.Log("ERROR: Base prefab not loaded");
        }
        //Instantiate bounds for box colliders
    }

    private void SpawnCoin()
    {
        Transform tile = gridTiles[Random.Range(0, gridTiles.Count)].transform;
        coin.GetComponent<Coin>().SetCurrentParent(tile);
        coinSpawned = true;
        Debug.Log("Coin Spawned");
    }

    private void DespawnCoin()
    {
        coin.GetComponent<Coin>().Despawn();
        coinSpawned = false;
    }

    private void SpawnCheckpoint()
    {
        Transform tile = gridTiles[Random.Range(0, gridTiles.Count)].transform;
        checkpoint.GetComponent<Checkpoint>().SetCurrentParentTile(tile);
        checkPointspawned = true;
        Debug.Log("Checkpoint spawned");
    }

    private void DespawnCheckpoint()
    {
        checkpoint.GetComponent<Checkpoint>().Despawn();
    }

    private List<int> GenerateUniqueRandom(int numValues, int max)
    {
        List<int> uniques = new List<int>();
        for (int i = 0; i < numValues; i++)
        {
            int safetycheck = 0;
            int currentNum = Random.Range(0, max);
            while (uniques.Contains(currentNum) && safetycheck < 10)
            {
                currentNum = Random.Range(0, max);
                safetycheck++;
            }
            uniques.Add(currentNum);
        }
        return uniques;
    }

    public void SetGameActive(bool b)
    {
        gameActive = b;
    }
    public bool GetGameActive()
    {
        return gameActive;
    }
    public void SetGamePaused(bool b)
    {
        paused = b;
        coin.GetComponent<Coin>().SetPaused(b);
    }
    public bool GetGamePaused()
    {
        return paused;
    }

    public void GameOver()
    {
        DespawnCoin();
        DespawnCheckpoint();
    }
}