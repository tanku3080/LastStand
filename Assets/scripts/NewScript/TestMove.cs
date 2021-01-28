using UnityEngine;

public class TestMove : MonoBehaviour
{
    Rigidbody Rd = null;
    Animator Anime = null;
    float playerSpeed = 2f;
    void Start()
    {
        Rd = gameObject.GetComponent<Rigidbody>();
        Anime = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Vertical") * playerSpeed;

        //if (x != 0)
        //{
        //    Rd.velocity = new Vector3(0, 0,x);
        //    Anime.SetBool("Move",true);
        //}
        //else
        //{
        //    Anime.SetBool("Move",false);
        //}

        if (x != 0)
        {
            Rd.velocity = new Vector3(0, 0, x);
            Anime.SetBool("Move", true);
        }
        else
        {
            Anime.SetBool("Move",false);
        }
    }

    void TypeSet()
    {

    }
}
