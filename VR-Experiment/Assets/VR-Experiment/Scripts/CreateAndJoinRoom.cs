using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateAndJoinRoom : MonoBehaviour
{
    public void CreateRoom()
    {
        SceneManager.LoadScene("Testscene_Robin");
    }

    public void JoinRoom()
    {
        SceneManager.LoadScene("Testscene_Robin");
    }
}
