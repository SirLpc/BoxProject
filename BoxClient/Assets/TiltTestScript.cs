using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using System;
using System.Timers;

public class TiltTestScript : MonoBehaviour
{
    float msg = 0f;

	void Update ()
    {
        msg = CrossPlatformInputManager.GetAxis("Mouse X");
        
	}

    void OnGUI()
    {
        timeInterval = double.Parse(GUILayout.TextField(timeInterval.ToString()));

        //GUILayout.Label("                     InputMousePos : " + msg.ToString());
        if (GUILayout.Button("Load"))
        {
            //SceneManager.LoadScene(3);
            //Application.LoadLevel(3);
            TestTimer();
        }

    }

    private double timeInterval;
    private Timer time = new Timer();

    private void TestTimer()
    {
        time.Elapsed += TimerElaspsed;
        time.Enabled = true;
        time.Interval = timeInterval;
    }

    private void TimerElaspsed(object sender, ElapsedEventArgs e)
    {
        Debug.Log("timer fired");
        time.Elapsed -= TimerElaspsed;
        time.Enabled = false;
    }
}
