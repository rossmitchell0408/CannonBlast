using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /****************Singleton*****************/
    public static CameraController Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }
    /*******************************************/

    bool isShaking = false;

    public void Shake(float duration = 0.1f, float maxDistance = 0.2f)
    {
        if (isShaking)
        {
            return;
        }
        StartCoroutine(RunShake(duration, maxDistance));
    }

    IEnumerator RunShake(float duration, float maxDistance)
    {
        isShaking = true;
        float timeElapsed = 0f;
        Vector3 originalPosition = transform.position;

        while (timeElapsed < duration)
        {
            float x = Random.Range(-maxDistance, maxDistance);
            float y = Random.Range(-maxDistance, maxDistance);

            //transform.rotation = new Quaternion(transform.localRotation.x, transform.localRotation.y, x, transform.localRotation.w);
            transform.position = new Vector3(x, y, originalPosition.z);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPosition;
        isShaking = false;
    }
}
