using UnityEngine;
using TMPro;

public class NpcScript : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] lines = {
        "Hi there! Welcome to the world!",
        "You can pick up the candy corn... maybe.",
        "Go ahead — test and see!"
    };

    public float delay = 2f;

    void Start()
    {
        StartCoroutine(DisplayIntro());
    }

    System.Collections.IEnumerator DisplayIntro()
    {
        if (textDisplay == null) yield break; 

        textDisplay.text = "";

        foreach (string line in lines)
        {
            if (textDisplay == null) yield break; 
            textDisplay.text = line;
            yield return new WaitForSeconds(delay);
        }
    }
}
