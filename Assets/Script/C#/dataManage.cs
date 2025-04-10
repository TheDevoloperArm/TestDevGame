using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class dataManage : MonoBehaviour
{
    public Text diamondText;

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
}
