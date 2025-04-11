using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class signupManage : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public InputField conPasswordInput;

    public GameObject panalSignUP;
    public GameObject panalAlertMSG;
    public Text messageText;

    void Awake()
    {
        if (!panalSignUP.activeSelf)
        {
            panalSignUP.SetActive(true);
        }

        if (panalAlertMSG.activeSelf)
        {
            panalAlertMSG.SetActive(false);
        }
    }

    public void Signup()
    {
        StartCoroutine(SignupCoroutine(usernameInput.text, passwordInput.text));
    }

    public void OK()
    {
        if (panalAlertMSG.activeSelf)
        {
            panalSignUP.SetActive(true);
            panalAlertMSG.SetActive(false);
            passwordInput.text = "";
            conPasswordInput.text = "";
        }
    }

    IEnumerator SignupCoroutine(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        if (passwordInput.text == conPasswordInput.text)
        {
            using (UnityWebRequest www = UnityWebRequest.Post("https://test-piggy.codedefeat.com/worktest/dev04/register.php", form))
            {

                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Sign uping....");
                    string json = www.downloadHandler.text;
                    // Debug.Log("Raw JSON: " + json);
                    SigupResponse response = JsonUtility.FromJson<SigupResponse>(json);

                    if (response.status == "success")
                    {
                        Debug.Log("สมัครสมาชิกสำเร็จ");
                        panalSignUP.SetActive(false);
                        panalAlertMSG.SetActive(true);
                        messageText.text = response.message;
                    }
                    else
                    {
                        Debug.Log("สมัครสมาชิกไม่สำเร็จ");
                        panalSignUP.SetActive(false);
                        panalAlertMSG.SetActive(true);
                        messageText.text = response.message;
                    }
                }
            }
        }
        else
        {
            panalSignUP.SetActive(false);
            panalAlertMSG.SetActive(true);
            Debug.Log("ยืนยันรหัสผ่านไม่ถูกต้อง กรุณากรอกใหม่อีกครั้ง");
            messageText.text = "Confirm your password is incorrect. Please enter it again.";
        }
    }

    [System.Serializable]
    private class SigupResponse
    {
        public string status;
        public string message;
    }
}
