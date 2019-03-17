using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    Image image;
    void Start()
    {
        //get the Text component
        image = GetComponent<Image>();
        //Call coroutine BlinkText on Start
        StartCoroutine(BlinkImage());
    }

    public IEnumerator BlinkImage()
    {
        //blink it forever. You can set a terminating condition depending upon your requirement
        while (true)
        {

            image.color = new Color(0, 0, 0, 1);

            yield return new WaitForSeconds(.5f);

            image.color = new Color(0, 0, 0, 0);

            yield return new WaitForSeconds(.5f);
        }
    }
}
