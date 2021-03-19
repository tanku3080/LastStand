using UnityEngine;

public class God : MonoBehaviour
{
    GameObject objKeep = null;
    // Start is called before the first frame update
    void Start()
    {
        
        if (objKeep == null)
        {
            objKeep = (GameObject)Instantiate(Resources.Load("Prefab/Managers"), gameObject.transform.parent);
        }
    }
}
