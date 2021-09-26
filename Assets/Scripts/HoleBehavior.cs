using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HoleBehavior : MonoBehaviour
{
    private CancellationTokenSource cts;
    public Vector3 TargetScale;
    public float Duration;
    public bool Goal = false;

    void Start()
    {
        cts = new CancellationTokenSource();
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!Goal)
            {
                StartCoroutine(Rescale(TargetScale, Duration, other, cts.Token));
                StartCoroutine(MoveInside(transform.position, Duration, other, cts.Token));
            }
            else
            {
                StartCoroutine(Rescale(new Vector3(5, 5, 5), Duration, other, cts.Token));
                StartCoroutine(MoveInside(transform.position, Duration, other, cts.Token));
                other.attachedRigidbody.isKinematic = false;
                Debug.Log("YEET");
            }

        }
    }

    private IEnumerator Rescale(Vector3 targetValue, float duration, Collider2D other, CancellationToken token)
    {
        Vector3 startValue = other.transform.localScale;
        var x = startValue.x;
        var y = startValue.y;
        var z = startValue.z;


        float time = 0;

        while (time < duration)
        {
            token.ThrowIfCancellationRequested();

            var newX = Mathf.Lerp(x, targetValue.x, time / duration);
            var newY = Mathf.Lerp(y, targetValue.y, time / duration);
            var newZ = Mathf.Lerp(z, targetValue.z, time / duration);

            other.transform.localScale = new Vector3(newX, newY, newZ);

            time += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene("SampleScene");

        other.transform.localScale = targetValue;
    }

    private IEnumerator MoveInside(Vector3 targetValue, float duration, Collider2D other, CancellationToken token)
    {
        Vector3 startValue = other.transform.position;
        var x = startValue.x;
        var y = startValue.y;
        var z = startValue.z;


        float time = 0;

        while (time < duration)
        {
            token.ThrowIfCancellationRequested();

            var newX = Mathf.Lerp(x, targetValue.x, time / duration);
            var newY = Mathf.Lerp(y, targetValue.y, time / duration);
            var newZ = Mathf.Lerp(z, targetValue.z, time / duration);

            other.transform.position = new Vector3(newX, newY, newZ);

            time += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene("SampleScene");

        other.transform.position = targetValue;
    }
}
