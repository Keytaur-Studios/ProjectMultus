using UnityEngine;

public class Crane : MonoBehaviour
{
    [Header("Pieces")]
    public GameObject chain;
    public GameObject magnet;

    public void Down()
    {
        chain.transform.localScale += new Vector3(0, .1f, 0);
        magnet.transform.position = new Vector3(magnet.transform.position.x, magnet.transform.position.y -.1f, magnet.transform.position.z);
    }

    public void Up()
    {
        if (chain.transform.localScale.y <= 1)
            return;
        chain.transform.localScale += new Vector3(0, -.1f, 0);
        magnet.transform.position = new Vector3(magnet.transform.position.x, magnet.transform.position.y + .1f, magnet.transform.position.z);
    }

    public void Left()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.y - 5f, 0);
    }

    public void Right()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.y + 5, 0);
    }
}
