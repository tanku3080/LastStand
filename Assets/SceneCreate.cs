using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// マネージャークラスが無い場合に動的に生成するスクリプト
/// </summary>
public class SceneCreate : MonoBehaviour
{
    GameObject ManagersObj = null;
    private void Awake()
    {
        var o = GameObject.Find("Managers");
        if (o == null)
        {
            Debug.LogWarning("Managersが無いので生成します");
            ManagersObj = new GameObject("Managers");
            var c = new GameObject("Canvas");
            c.transform.parent = ManagersObj.transform;
            //以下ManagerObjにアタッチ
            ManagersObj.AddComponent<NewGameManager>();
            ManagersObj.AddComponent<PlayerManager>();
            ManagersObj.AddComponent<EnemyManager>();
            ManagersObj.AddComponent<SceneFadeManager>();
            ManagersObj.AddComponent<DieManager>();
            ManagersObj.AddComponent<AudioSource>();
            //以下Canvasオブジェクトにアタッチ
            c.gameObject.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            c.gameObject.AddComponent<CanvasScaler>();
            c.gameObject.AddComponent<GraphicRaycaster>();
            c.gameObject.AddComponent<CanvasGroup>().alpha = 1;

            var image = new GameObject("Image");
            image.transform.parent = c.transform;
            image.gameObject.AddComponent<RectTransform>();
            RectTransform anchor = image.GetComponent<RectTransform>();
            anchor.anchorMin = new Vector2(0,0);
            anchor.anchorMax = new Vector2(1, 1);
            anchor.localPosition = Vector3.zero;
            Color col = new Color(255,255,255);
            image.gameObject.AddComponent<Image>().color = col;
            //new GameObject("Image").transform.parent = ManagersObj.transform.GetChild(0).GetChild(0);
        }
        else ManagersObj = o;
    }
}
