using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked");
    }

    public void OnOptionButtonClicked()
    {
        Debug.Log("Options button clicked");
    }

    public void OnHighScoreClicked()
    {
        Debug.Log("High score clicked");
    }
}