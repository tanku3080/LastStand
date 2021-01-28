using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : PlayerBase
{
    private void Start()
    {
        Rd = gameObject.GetComponent<Rigidbody>();
        Rd.mass = 10;
        Renderer = gameObject.GetComponent<MeshRenderer>();
        Anime = gameObject.GetComponent<Animator>();
    }
    //大雑把に八方向に移動できる。ロボットは二方向にしたほうが良い？
    private void Update()
    {
        var y = Input.GetAxis("Vertical") * playerSpeed;
        if (y != 0)
        {
            Rd.velocity = new Vector3(0, 0, y);
            Anime.SetFloat("Move",y);
        }
        //Rd.velocity = Vector3.zero;
    }

    void Die()
    {
        PlayerDie(Renderer);
    }
}
