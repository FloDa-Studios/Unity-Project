using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerOnButton : MonoBehaviour
{
    public float targettime;
    ReserachTimes times = new ReserachTimes();
    public Text text;

    void Start()
    {
        targettime = times.Testreserach;
    }

    public void OnButtonClick()
    {
        StartCoroutine(Counter());
    }
    
    void Update()
    {
        if(targettime < 0)
        {
            StopCoroutine(Counter());
            text.text = "Fertig";
        }
    }

    IEnumerator Counter()
    {
        while (true)
        {
            targettime -= 1;
            text.text = targettime.ToString("0.00");
            yield return new WaitForSeconds(1);
        }
    }
}
