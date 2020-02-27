using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool paused = false;
    private bool despawned = false;
    private Transform currentParentTile;
    // Start is called before the first frame update
    void Start()
    {
        currentParentTile = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetCurrentParentTile(Transform tile)
    {
        Spawn();
        currentParentTile = tile;
    }

    private void FixedUpdate()
    {
        if (!despawned && !paused)
        {
            transform.localPosition = new Vector3(currentParentTile.localPosition.x, currentParentTile.localPosition.y + 2.5f, currentParentTile.localPosition.z);
        }
       
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
