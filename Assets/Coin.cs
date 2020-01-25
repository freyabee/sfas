using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotateSpeed = 5f;
    private bool isRotating;
    private Transform currentParentTile;

    // Start is called before the first frame update
    private void Start()
    {
        isRotating = true;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.Self);
        transform.localPosition = new Vector3(currentParentTile.localPosition.x, currentParentTile.localPosition.y + 5f, currentParentTile.localPosition.z);
    }

    public void SetCurrentParent(Transform tile)
    {
        currentParentTile = tile;
    }
}