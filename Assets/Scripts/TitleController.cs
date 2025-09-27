using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    private bool firstpuch = false;
    public void onClickStartButton()
    {
        if (!firstpuch)
        {
            SceneManager.LoadScene("SampleScene");
            firstpuch = true;
        }
    }
}
