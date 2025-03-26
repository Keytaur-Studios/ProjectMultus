using UnityEngine;

public class ExampleUIScript : MonoBehaviour
{
    private SimpleText simpleText;
    private int count;

    private void Start()
    {
        simpleText = GetComponent<SimpleText>();
        count = 0;
    }

    private void Update()
    {
        count++;
        if (simpleText != null)
        {
            simpleText.UpdateText((count / 100).ToString());
        }
    }
}
