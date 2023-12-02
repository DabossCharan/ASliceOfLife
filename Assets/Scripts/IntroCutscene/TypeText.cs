using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeText : MonoBehaviour
{
    private TextMeshProUGUI tMPro;
    public string completeText;
    public string currentText;
    public int currentIndex;
    

    public float duration;
    private float currentTime = 0;

    private float alpha = 1;
    // Start is called before the first frame update
    void Awake()
    {
        tMPro = GetComponent<TextMeshProUGUI>();
        completeText = completeText.Replace("\\n", "\n");
        StartCoroutine(PrintLetters(completeText));
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        alpha = Mathf.Lerp(1, 0, currentTime / duration);
        tMPro.color = new Color(tMPro.color.r, tMPro.color.g, tMPro.color.b, alpha);
    }

    IEnumerator PrintLetters(string completeText)
    {
        foreach (char c in completeText.ToCharArray())
        {
            yield return new WaitForSeconds(0.05f);
            currentText += c;
            tMPro.text = currentText;
        }
    }
}
