using UnityEngine;

public class GameExitManager : MonoBehaviour {

    public static GameExitManager Instance;

    public GameObject ExitConfirmationPopUp;

	// Use this for initialization
	void Start () {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            HandleExit();
        }
    }

    public void ShowExitConfirmation()
    {
        ExitConfirmationPopUp.SetActive(true);
        HandleExit();
    }

    private void HandleExit()
    {
        GameStarManager.Instance.ExitGame();
        GameTicketManager.Instance.ExitGame();
        GemScript.Instance.ExitGame();
        StoreObjectPurchaseController.Instance.ExitGame();
        GameNotificationManager.Instance.ExitGame();
    }

    public void ExitConfirmed()
    {
        ExitConfirmationPopUp.SetActive(false);
        Application.Quit();
    }

    public void ExitCancelled()
    {
        ExitConfirmationPopUp.SetActive(false);
        GameNotificationManager.Instance.CancelAllNotification();
    }
}
