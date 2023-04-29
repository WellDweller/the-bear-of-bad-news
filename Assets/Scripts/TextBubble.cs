using System.Collections;
using System.Text;
using UnityEngine;

public class TextBubble : MonoBehaviour
{
    [SerializeField] float delayBetweenCharacters;
    [SerializeField, TextArea] string text;
    [SerializeField] TMPro.TextMeshProUGUI textMesh;

    void Awake()
    {
        textMesh.text = "";
        StartCoroutine(Typewrite(text));
    }

    IEnumerator Typewrite(string text)
    {
        StringBuilder sb = new(text.Length);

        for (int i = 0; i < text.Length; i++)
        {
            sb.Append(text[i]);
            textMesh.text = sb.ToString();
            yield return new WaitForSeconds(delayBetweenCharacters);
        }
    }
}
