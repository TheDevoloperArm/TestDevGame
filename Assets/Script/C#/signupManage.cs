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

    public Text messageText;

    public void Signup()
    {
        StartCoroutine(SignupCoroutine(usernameInput.text, passwordInput.text));
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
                    Debug.Log("Raw JSON: " + json);
                    SigupResponse response = JsonUtility.FromJson<SigupResponse>(json);

                    if (response.status == "success")
                    {
                        messageText.text = "สมัครสมาชิกสำเร็จแล้ว";
                    }
                }
            }
        }
        else
        {
            Debug.Log("ยืนยันรหัสผ่านไม่ถูกต้อง กรุณากรอกใหม่อีกครั้ง");
            Debug.Log(passwordInput.text);
            Debug.Log(conPasswordInput.text);
            messageText.text = "ยืนยันรหัสผ่านไม่ถูกต้อง กรุณากรอกใหม่อีกครั้ง";
        }
    }

    [System.Serializable]
    private class SigupResponse
    {
        public string status;
        public string message;
    }
}
