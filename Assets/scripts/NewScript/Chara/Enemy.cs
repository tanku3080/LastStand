using UnityEngine;

public class Enemy : EnemyBase
{
    private void Start()
    {
        Rd = gameObject.GetComponent<Rigidbody>();
        Renderer = gameObject.GetComponent<MeshRenderer>();
        Anime = gameObject.GetComponent<Animator>();
        Trans = gameObject.GetComponent<Transform>();
    }

    /// <summary>
    /// 一番近い敵のオブジェクトを帰す
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="name">探すオブジェクトのlayer名</param>
    /// <returns></returns>
    GameObject NearPlayer(GameObject obj, string name = "Player")
    {
        float keepPos = 0;
        float nearDistance = 0;
        GameObject[] allP = FindObjectsOfType<GameObject>();
        GameObject target = null;
        int layerNum = LayerMask.NameToLayer(name);
        foreach (var item in allP)
        {
            if (item.layer == layerNum)
            {
                keepPos = Vector3.Distance(item.transform.position, obj.transform.position);
                if (nearDistance == 0 || nearDistance > keepPos)
                {
                    nearDistance = keepPos;
                    target = item;
                }
            }

        }
        return target;
    }
}
