using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class loginManage : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public GameObject palnalLogin;
    public GameObject panalAlertMSG;
    public Text messageText;
    private string statusIn;

    void Awake()
    {
        if (!palnalLogin.activeSelf)
        {
            palnalLogin.SetActive(true);
        }

        if (panalAlertMSG.activeSelf)
        {
            panalAlertMSG.SetActive(false);
        }
    }

    public void OK()
    {
        if (statusIn == "success")
        {
            SceneManager.LoadScene("Lobby");
            palnalLogin.SetActive(true);
            panalAlertMSG.SetActive(false);
            passwordInput.text = "";
            statusIn = "";
        }
        else
        {
            palnalLogin.SetActive(true);
            panalAlertMSG.SetActive(false);
            passwordInput.text = "";
            statusIn = "";
        }
    }

    public void SignUp()
    {
        SceneManager.LoadScene("Register Scene");
    }

    public void Login()
    {
        StartCoroutine(LoginCoroutine(usernameInput.text, passwordInput.text));
    }

    IEnumerator LoginCoroutine(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post("https://test-piggy.codedefeat.com/worktest/dev04/login.php", form))
        {
            yield return www.SendWebRequest();
            Debug.Log("Logining.....");

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Raw JSON: " + jsonResponse);
                UserResponse response = JsonUtility.FromJson<UserResponse>(jsonResponse);

                if (response.status == "success")
                {
                    statusIn = response.status;
                    PlayerPrefs.SetInt("Id", response.id);
                    PlayerPrefs.SetInt("Diamond", response.diamond);
                    PlayerPrefs.SetInt("Heart", response.heart);
                    PlayerPrefs.SetString("Username", response.user);

                    Debug.Log("Login Success, Welcome user : " + response.user + " " + "Diamond : " + response.diamond + " " + "Heart : " + response.heart);
                    messageText.text = "Login Success Welcome : " + response.user;

                    palnalLogin.SetActive(false);
                    panalAlertMSG.SetActive(true);
                }
                else
                {
                    statusIn = response.status;
                    messageText.text = "Login Failed : " + response.message;
                    palnalLogin.SetActive(false);
                    panalAlertMSG.SetActive(true);
                }
            }
        }
    }

    [System.Serializable]
    private class UserResponse
    {
        public int id;
        public string status;
        public string user;
        public int diamond;
        public int heart;
        public string message;
    }
}
