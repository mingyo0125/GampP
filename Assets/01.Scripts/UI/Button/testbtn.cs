using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class testbtn : MonoBehaviour
{
    public void asdASDASD()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("DemoScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
