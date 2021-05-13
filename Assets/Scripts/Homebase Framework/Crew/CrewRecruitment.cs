using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity;
using UnityEngine.Internal;
using TMPro;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using System;

public class CrewRecruitment : MonoBehaviour
{
    public int amount;

    private CrewMan man;
    private int value;  
    private Color color = new Color();
    private List<CrewMan> crewMen = new List<CrewMan>();

    // Start is called before the first frame update
    void Start()
    {
        GameObject CrewWindow = GameObject.Find("Content");
        GameObject TierImage = GameObject.Find("TierImage");
        GameObject SpacerImage = GameObject.Find("SpacerImage");
        GameObject ButtonRecruit = GameObject.Find("ButtonRecruit");

        CrewManagement.Instance.Initialize();
        System.Random random = new System.Random();

        CrewWindow.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, amount*100);

        for (int i = 0; i < amount; i++)
        {
            int index1 = random.Next(CrewManagement.Instance.firstNames.Count);
            int index2 = random.Next(CrewManagement.Instance.surNames.Count);
            float skill = UnityEngine.Random.Range(0.00f, 0.80f);
            skill = (float)System.Math.Round(skill, 2);
            man = new CrewMan(CrewManagement.Instance.firstNames[index1] + " " + CrewManagement.Instance.surNames[index2], CrewManagement.Instance.crewMen.Count, skill, UnityEngine.Random.Range(0,3));
            crewMen.Add(man);
            int randomNumber = UnityEngine.Random.Range(0, 5);
            if (man.crewManSkill > 0.70)
            {
                value = 4;
                color = Color.yellow;
            }
            else if (man.crewManSkill > 0.60)
            {
                value = 3;
                color = Color.magenta;
            }
            else if (man.crewManSkill > 0.30)
            {
                value = 2;
                color = Color.green;
            }
            else if (man.crewManSkill > 0.1)
            {
                value = 1;
                color = Color.blue;
            }
            else if (man.crewManSkill > 0.0)
            {
                value = 0;
                color = Color.grey;
            }

            GameObject newTierImage = Instantiate<GameObject>(GameObject.Find("TierImage" + value));
            newTierImage.transform.SetParent(CrewWindow.transform);
            newTierImage.transform.localPosition = new Vector3(61, -60 - i * 100, 0);
            newTierImage.name = "TierImage" + i;
            TextMeshProUGUI tmp = newTierImage.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = man.crewManSkill.ToString();

            GameObject newSpacerImage = Instantiate<GameObject>(SpacerImage);
            newSpacerImage.transform.SetParent(CrewWindow.transform);
            newSpacerImage.transform.localPosition = new Vector3(256, -60 - i * 100, 0);
            newSpacerImage.name = "newSpacerImage" + i;

            tmp = (TextMeshProUGUI)newSpacerImage.GetComponentsInChildren<TextMeshProUGUI>().GetValue(0);
            tmp.text = "" + man.crewManName;
            tmp = (TextMeshProUGUI)newSpacerImage.GetComponentsInChildren<TextMeshProUGUI>().GetValue(1);
            tmp.text = "" + man.crewManJob;

            GameObject newButtonRecruit = Instantiate<GameObject>(ButtonRecruit);
            newButtonRecruit.transform.SetParent(CrewWindow.transform);
            newButtonRecruit.transform.localPosition = new Vector3(456, -60 - i * 100, 0);
            newButtonRecruit.name = "newButtonRecruit" + i;
        }

        CrewManagement.Instance.AddCrewMan(man);
    }  

    public void ButtonRecruitClick()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        string resultString = Regex.Match(name, @"\d+").Value;
        int number = Int32.Parse(resultString);
        CrewManagement.Instance.SaveCrewMan(crewMen[number]);
    }
    


}
