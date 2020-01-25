using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    private Rigidbody rb;
    public GridGenerator grid;
    public float CubeSpeed = 10f;
    public float CubeGravity = 10f;
    private float gridWidth;
    private float offset;
    private int score;

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
        transform.localPosition = new Vector3(-(grid.GetGridWidth() / 2) + offset, 5.9f, -(grid.GetGridWidth() / 2) + offset);
    }

    // Update is called once per frame
    private void Update()
    {
        //Only allow input if the cube has reached the destination.
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with:" + other.name);
        if (other.CompareTag("TopCol"))
        {
            rb.velocity = Vector3.zero;
            GameOver();
        }

        if (other.CompareTag("Coin"))
        {
            grid.SetCoinSpawned(false);
            score++;
            Debug.Log("Score: " + score);
        }
    }

    private void GameOver()
    {
        ResetToBeginning();
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
}