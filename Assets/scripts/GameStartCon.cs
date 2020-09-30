using UnityEngine;
using UnityEngine.UI;

public class GameStartCon : MonoBehaviour
{
    public AudioSource source;
    public AudioClip se;
    void Start()
    {
        source = this.gameObject.GetComponent<AudioSource>();
        //source.PlayOneShot(se);
    }
}
