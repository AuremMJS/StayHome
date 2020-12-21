using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameErrorDisplayManager : MonoBehaviour
{
    public bool ErrorDisplaying;
    public GameObject ErrorDisplay;
    public Text ErrorText;
    public static GameErrorDisplayManager Instance;
    // Use this for initialization
    void Start()
    {
        Instance = this;
        ErrorDisplaying = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayError(string errorMessage)
    {
        ErrorText.text = errorMessage;
        ErrorDisplaying = true;
        StartCoroutine(DisplayErrorCoroutine());
    }

    private IEnumerator DisplayErrorCoroutine()
    {
        ErrorDisplay.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        ErrorDisplay.SetActive(false);
        ErrorDisplaying = false;

    }
}
