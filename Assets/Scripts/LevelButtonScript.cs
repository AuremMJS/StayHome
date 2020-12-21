using ColorSnipersU.MonoBehaviors.MainGameScene;
using ColorSnipersU.ScriptableObjects;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonScript : MonoBehaviour
{
    public GameObject PrevButton;
    public GameObject NextButton;
    public GameObject main_Camera;
    public TextMeshPro LevelNo;
    public Image AdventureTitle;

    public GameObject[] LitStars;
    public GameObject[] UnlitStars;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        if (GameTicketManager.Instance.PlayGamePopUp.activeSelf || GameErrorDisplayManager.Instance.ErrorDisplaying || GameTicketManager.Instance.LoadingScreen.activeSelf)
            return;
        int levelNo = int.Parse(LevelNo.text.Split(' ')[1]);
        LevelBase level = LevelsSO.instance.Levels[levelNo - 1];
        if (level != null)
        {
            if (GameStarManager.Instance.TotalStars >= level.StarsToUnlock && !level.Locked)
            {
                GameTicketManager.Instance.SelectedLevel = levelNo;
                GameTicketManager.Instance.ShowPlayGamePopUp(true);
            }
            else
            {
                if (GameStarManager.Instance.TotalStars >= level.StarsToUnlock)
                    GameErrorDisplayManager.Instance.DisplayError("Collect atleast one Star in previous Level to unlock this Level");
                else
                    GameErrorDisplayManager.Instance.DisplayError("Collect " + level.StarsToUnlock + " Stars in previous Levels to unlock this Level");
            }
        }
        else
        {
            Debug.LogError(String.Format("Level {0} SO not configured properly", levelNo));
        }
    }

    public void OnMouseDown()
    {
        Vector3 mPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
        //var rayOrigin = Camera.main.ScreenToWorldPoint(mPosition);
        //RaycastHit hitInfo;
        //Physics.Raycast(new Vector3(rayOrigin.x, rayOrigin.y, rayOrigin.z), Camera.main.transform.forward, out hitInfo);
        //if (hitInfo.collider != null && hitInfo.collider.CompareTag("LevelWall") && !GameTicketManager.Instance.PlayGamePopUp.activeSelf)
        if (!GameTicketManager.Instance.PlayGamePopUp.activeSelf)
        {
            OnClick();
            return;
        }
    }

    public void OnMouseDrag()
    {

    }

    public void NextButtonClicked()
    {
        if (main_Camera.transform.position.x < 5)
            main_Camera.transform.Translate(6, 0, 0);
        if (main_Camera.transform.position.x >= 12)
            NextButton.SetActive(false);
        PrevButton.SetActive(true);
    }
    public void PrevButtonClicked()
    {
        if (main_Camera.transform.position.x > 0)
            main_Camera.transform.Translate(-6, 0, 0);
        if (main_Camera.transform.position.x <= 0)
            PrevButton.SetActive(false);
        NextButton.SetActive(true);
    }
}
