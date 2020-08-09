using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script öffnet erwünschte Scene und schließt die aktuelle

public class OpenSceneAtButtonPress : MonoBehaviour
{
    public string target;   //Für Ziel , welches durch diesen Butten geöffnet werden soll
    public void OnButtonPress()
    {
        OpenScene();
        Debug.Log("Jetzt wird das Fenster für "+target+" geöffnet werden");
    }
    private void OpenScene()
    {
        SceneManager.LoadScene(target);
    }

}
