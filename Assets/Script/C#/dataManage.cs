using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class dataManage : MonoBehaviour
{
    public Text diamondText;
    public int diamond = 100;
    public int heartCount = 100;
    public Text messageText;

    void Start()
    {
        string Username = PlayerPrefs.GetString("Username");
        Debug.Log("Welcome: " + Username);
    }
    // Update is called once per frame
    void Update()
    {
        diamondText.text = PlayerPrefs.GetInt("Diamond").ToString();
    }

    public void addDiamonds()
    {
        StartCoroutine(AddDiamondCoroutine(diamond));
    }

    IEnumerator AddDiamondCoroutine(int diamond)
    {
        WWWForm form = new WWWForm();
        form.AddField("diamond", diamond);

        using (UnityWebRequest www = UnityWebRequest.Post("https://test-piggy.codedefeat.com/worktest/dev04/update_data.php", form))
        {
            yield return www.SendWebRequest();
            string dataResponse = www.downloadHandler.text;
            Debug.Log("Raw DATA: " + dataResponse);
            DataResponse data = JsonUtility.FromJson<DataResponse>(dataResponse);
            if (data.status == "success")
            {
                Debug.Log("Add 100 diamonds success.");
            }
            else
            {
                Debug.Log("Can't add 100 diamonds because: " + data.message);
            }
        }
    }

    [System.Serializable]
    private class DataResponse
    {
        public string status;
        public string message;
    }
}
