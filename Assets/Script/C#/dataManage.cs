using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class dataManage : MonoBehaviour
{
    private int id;
    public Text diamondText;
    public int diamond = 100;
    public int currentDiamond;
    public Text messageText;

    public Image heartBarFill;
    public int maxHeart = 100;
    public int currentHeart;
    private string statusIn;
    public GameObject panalAlertMSG;

    void Start()
    {
        string Username = PlayerPrefs.GetString("Username");
        Debug.Log("Welcome: " + Username);
        id = PlayerPrefs.GetInt("Id");
        currentDiamond = PlayerPrefs.GetInt("Diamond");
        currentHeart = PlayerPrefs.GetInt("Heart");
        panalAlertMSG.SetActive(false);
    }

    void Update()
    {
        diamondText.text = PlayerPrefs.GetInt("Diamond").ToString();
        if (currentDiamond < 0)
        {
            currentDiamond = 0;
        }
    }

    void UpdateHeartBar()
    {
        float target = (float)currentHeart / maxHeart;
        heartBarFill.fillAmount = Mathf.Lerp(heartBarFill.fillAmount, target, Time.deltaTime * 5f);
    }

    public void SetHeart(int value)
    {
        currentHeart = Mathf.Clamp(value, 0, maxHeart);
        UpdateHeartBar();
    }

    public void addDiamonds()
    {
        if (currentDiamond < 10000)
        {
            currentDiamond += diamond;
            Debug.Log("Check current diamond: " + currentDiamond);
            StartCoroutine(AddDiamondCoroutine(id, currentDiamond));
        }
        else if (currentDiamond > 10000)
        {
            currentDiamond = 10000;
            panalAlertMSG.SetActive(false);
            messageText.text = "Your bag is full, can't add any diamond!";
        }
    }

    public void OK()
    {
        if (statusIn == "logout")
        {
            panalAlertMSG.SetActive(false);
            statusIn = "";
            SceneManager.LoadScene("Login Scene");
            messageText.text = "";
        }
        else
        {
            panalAlertMSG.SetActive(false);
            messageText.text = "";
            statusIn = "";
        }
    }

    public void Logout()
    {
        StartCoroutine(LogoutCoroutine());
    }

    IEnumerator LogoutCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("diamond", currentDiamond);
        form.AddField("heart", currentHeart);

        using (UnityWebRequest www = UnityWebRequest.Post("https://test-piggy.codedefeat.com/worktest/dev04/logout.php", form))
        {
            yield return www.SendWebRequest();
            string dataResponse = www.downloadHandler.text;
            // Debug.Log("Raw DATA: " + dataResponse);
            DataResponse data = JsonUtility.FromJson<DataResponse>(dataResponse);
            if (data.status == "success")
            {
                statusIn = "logout";
                Debug.Log("Logout Success.");
                messageText.text = "Logout Success.";
                panalAlertMSG.SetActive(true);
            }
            else
            {
                Debug.Log("Can't logout because : " + data.message);
                messageText.text = "Can't logout because : " + data.message;
                panalAlertMSG.SetActive(true);
            }
        }

    }

    IEnumerator AddDiamondCoroutine(int id, int currentDiamond)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("diamond", currentDiamond);

        using (UnityWebRequest www = UnityWebRequest.Post("https://test-piggy.codedefeat.com/worktest/dev04/update_data.php", form))
        {
            yield return www.SendWebRequest();
            string dataResponse = www.downloadHandler.text;
            // Debug.Log("Raw DATA: " + dataResponse);
            DataResponse data = JsonUtility.FromJson<DataResponse>(dataResponse);
            if (data.status == "success")
            {
                statusIn = "success";
                PlayerPrefs.SetInt("Diamond", data.diamond);
                Debug.Log("Add 100 diamonds success.");
                messageText.text = "Add 100 diamonds success.";
            }
            else
            {
                Debug.Log("Can't add 100 diamonds because: " + data.message);
                messageText.text = "Can't add 100 diamonds because: " + data.message;
            }
            panalAlertMSG.SetActive(true);
        }
    }

    [System.Serializable]
    private class DataResponse
    {
        public string status;
        public int diamond;
        public int heart;
        public string message;
    }
}

