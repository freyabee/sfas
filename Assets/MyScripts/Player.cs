using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public CameraFollower camFollow;
    private Rigidbody rb;
    public GridGenerator grid;
    public float CubeSpeed = 10f;
    public float CubeGravity = 10f;
    private float gridWidth;
    private float offset;
    private int score;
    bool spawned = false;
    bool paused = false;
    bool falling = false;
    private void Awake()
    {
        score = 0;
    }

    // Start is called before the first frame update
    private void Start()
    {

        gridWidth = grid.GetGridWidth();
        offset = grid.GetTileOffset();
        rb = GetComponent<Rigidbody>();
        
    }

    public void RespawnPlayer()
    {
        //Respawn player above first square
        camFollow.StartFollowingPlayer();
        ResetToBeginning();
        spawned = true;
    }
    public void DespawnPlayer()
    {
        //Despawn Player behind camera
        transform.localPosition = new Vector3(0f, 0f, -100f);
        spawned = false;
    }

    // Update is called once per frame
    private void Update()
    {
        
        if (grid.GetGameActive()&&!grid.GetGamePaused()&&!falling)
        {
            if (!spawned)
            {
                RespawnPlayer();
            }
            else
            {
                if (rb)
                {
                    if (Input.GetKey(KeyCode.W))
                    {
                        rb.velocity = Vector3.forward * CubeSpeed;
                    }

                    if (Input.GetKey(KeyCode.S))
                    {
                        rb.velocity = Vector3.back * CubeSpeed;
                    }

                    if (Input.GetKey(KeyCode.A))
                    {
                        rb.velocity = Vector3.left * CubeSpeed;
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        rb.velocity = Vector3.right * CubeSpeed;
                    }
                }
                if (transform.localPosition.y > 5)
                {
                    rb.velocity += Vector3.down * CubeGravity;
                }
            }   
        }
        else if(!grid.GetGameActive()&& spawned)
        {
            DespawnPlayer();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with:" + other.name);
        if (other.CompareTag("TopCol"))
        {
            rb.velocity = Vector3.zero;
            if (!falling)
            {
                GameOver();
            }
            IsFalling(false);
            
        }

        if (other.CompareTag("Coin"))
        {
            grid.SetCoinSpawned(false);
            score++;
            Debug.Log("Score: " + score);
        }
        if (other.CompareTag("Checkpoint"))
        {
            Debug.Log("Checkpoint touched");
            transform.localPosition = new Vector3(other.transform.parent.transform.localPosition.x+grid.transform.localPosition.x, transform.localPosition.y, other.transform.parent.transform.localPosition.z+grid.transform.localPosition.z);
            rb.velocity = Vector3.zero;
            IsFalling(true);
        }
        if (other.CompareTag("BottomCol"))
        {
            transform.localPosition = new Vector3(-(grid.GetGridWidth() / 2) + offset, 30f, -(grid.GetGridWidth() / 2) + offset);
        }
    }

    private void GameOver()
    {
        DespawnPlayer();
        grid.GameOver();
        //ResetToBeginning();
    }
    private void ResetToBeginning()
    {
        transform.localPosition = new Vector3(-(grid.GetGridWidth() / 2) + offset, 15f, -(grid.GetGridWidth() / 2) + offset);
        StartCoroutine(ResetPosition());
    }
    private IEnumerator ResetPosition()
    {
        while (transform.localPosition.y > 6)
        {
            rb.velocity = Vector3.down * CubeGravity;
            yield return null;
        }
    }
    public int GetScore()
    {
        return score;
    }
    private void IsFalling(bool fall)
    {
        if (fall)
        {
            falling = true;
            camFollow.StartFollowingPlayer();
            gameObject.GetComponent<SphereCollider>().enabled = false;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
        else
        {
            falling = false;
            camFollow.StopFollowingPlayer();
            gameObject.GetComponent<SphereCollider>().enabled = true;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
    }

}