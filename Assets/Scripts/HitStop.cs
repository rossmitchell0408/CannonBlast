using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    /****************Singleton*****************/
    public static HitStop Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }
    /*******************************************/

    bool waiting = false;

    public void Stop(float duration = 0.2f, float speed = 0.1f)
    {
        if (waiting)
        {
            return;
        }
        StartCoroutine(Wait(duration, speed));
    }

    IEnumerator Wait(float duration, float speed)
    {
        waiting = true;
        Time.timeScale = speed;
        yield return new WaitForSecondsRealtime(duration);
        waiting = false;
        Time.timeScale = 1f;
    }
}
