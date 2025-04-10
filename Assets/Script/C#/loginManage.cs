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
    public Text messageText;

    public void Login()
    {
        Debug.Log("Logining.....");
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

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Raw JSON: " + jsonResponse);
                UserResponse response = JsonUtility.FromJson<UserResponse>(jsonResponse);

                if (response.status == "success")
                {
                    PlayerPrefs.SetInt("Diamond", response.diamond);
                    PlayerPrefs.SetInt("Heart", response.heart);
                    PlayerPrefs.SetString("Username", response.user);
                    SceneManager.LoadScene("Lobby");

                    Debug.Log("Login Success, Welcome user : " + response.user + " " + "Diamond : " + response.diamond + " " + "Heart : " + response.heart);
                    messageText.text = "Login Success Welcome: " + response.user;
                }
                else
                {
                    messageText.text = "Login Failed: " + response.message;
                }
            }
        }
    }

    [System.Serializable]
    private class UserResponse
    {
        public string status;
        public string user;
        public int diamond;
        public int heart;
        public string message;
    }
}
