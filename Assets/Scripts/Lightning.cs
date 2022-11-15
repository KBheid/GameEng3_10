using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public Light lightning;
    public float maxInterval;
    public int numFlashes;
    private float _nextInterval;
    private float _curInterval;

    // Start is called before the first frame update
    void Start()
    {
        _nextInterval = Random.Range(3, maxInterval);
    }

    // Update is called once per frame
    void Update()
    {
        _curInterval += Time.deltaTime;
        if (_curInterval >= _nextInterval)
		{
            _nextInterval = Random.Range(0, maxInterval);
            _curInterval = 0;
            StartCoroutine(nameof(LightningEffect));
        }
    }

    IEnumerator LightningEffect()
	{
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Random.Range(-30,30), transform.rotation.eulerAngles.z);
        for (int i=0; i<Random.Range(1, numFlashes); i++)
		{
            RenderSettings.fog = false;
            lightning.enabled = true;
            yield return new WaitForSeconds(Random.Range(0.15f, 0.35f));
            lightning.enabled = false;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));

            lightning.enabled = false;
            RenderSettings.fog = true;
        }
    }
}
