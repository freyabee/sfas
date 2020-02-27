using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotateSpeed = 5f;
    private Transform currentParentTile;
    private bool paused= false;
    private bool despawned = false;
    // Start is called before the first frame update
    private void Start()
    {
        currentParentTile = transform;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void FixedUpdate()
    {
        if(!paused && !despawned)
        {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.Self);
            transform.localPosition = new Vector3(currentParentTile.localPosition.x, currentParentTile.localPosition.y + 5f, currentParentTile.localPosition.z);
        }
        
    }

    public void SetCurrentParent(Transform tile)
    {
        Spawn();
        currentParentTile = tile;
    }

    public void SetPaused(bool b)
    {
        paused = b;
    }
    public void Despawn()
    {
        despawned = true;
        transform.localPosition = new Vector3(0, 0, -100);
    }

    public void Spawn()
    {
        despawned = false;
    }
}