using UnityEngine;
using UnityEngine.UI;
using UnityEditor.EventSystems;
using UnityEngine.SceneManagement;
/// <summary>
/// マネージャークラスが無い場合に動的に生成するスクリプト
/// </summary>
public class SceneCreate : MonoBehaviour
{
    GameObject ManagersObj = null;
    GameObject UIobj = null;
    private void Awake()
    {
        //シーン名を取得。今はテストシーンだけの判定にするので意味のない条件式を書く
        //raycasttargetの切替も
        if (SceneManager.GetActiveScene().name == "vvvv")
        {
            var v = GameObject.Find("Uis");
            if (v == null)
            {
                Debug.LogWarning("Uisが無いので生成するよ");
                UIobj = new GameObject("Uis");
                var c = new GameObject("Canvas");
                var s = new GameObject("Slider");
                var b = new GameObject("Background");
                var f = new GameObject("Fill Area");
                var ff = new GameObject("Fill");

                c.transform.parent = UIobj.transform;
                c.gameObject.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                c.gameObject.AddComponent<CanvasScaler>();
                c.gameObject.AddComponent<GraphicRaycaster>();

                s.gameObject.layer = 5;
                s.transform.parent = c.transform;
                s.AddComponent<RectTransform>();
                RectTransform anchor = s.gameObject.GetComponent<RectTransform>();
                anchor.anchorMin = new Vector2(0, 0);
                anchor.anchorMax = new Vector2(1, 1);
                anchor.localPosition = new Vector3(287,434,0);
                anchor.localScale = new Vector3(2,2,1);
                s.AddComponent<Slider>();
                Slider slider = s.gameObject.GetComponent<Slider>();
                slider.interactable = false;
                slider.value = 100;//これは後々変更

                b.gameObject.layer = 5;
                b.transform.parent = s.transform;
                b.AddComponent<RectTransform>();
                RectTransform back = b.gameObject.GetComponent<RectTransform>();
                back.anchorMin = new Vector2(0, 0);
                back.anchorMax = new Vector2(1, 1);
                b.AddComponent<Image>().color = Color.red;

                f.gameObject.layer = 5;
                f.transform.parent = s.transform;
                f.AddComponent<RectTransform>();
                RectTransform full = b.gameObject.GetComponent<RectTransform>();
                full.anchorMin = new Vector2(0, 0);
                full.anchorMax = new Vector2(1, 1);

                ff.gameObject.layer = 5;
                ff.transform.parent = f.transform;
                ff.AddComponent<RectTransform>();
                ff.AddComponent<Image>().color = Color.green;
            }
            else UIobj = v;
        }
        var o = GameObject.Find("Managers");
        if (o == null)
        {
            Debug.LogWarning("Managersが無いので生成します");
            ManagersObj = new GameObject("Managers");
            var c = new GameObject("Canvas");
            c.transform.parent = ManagersObj.transform;
            //以下ManagerObjにアタッチ
            ManagersObj.AddComponent<GameManager>();
            ManagersObj.AddComponent<TurnManager>();
            ManagersObj.AddComponent<SceneFadeManager>();
            ManagersObj.AddComponent<DieManager>();
            ManagersObj.AddComponent<AudioSource>();
            //以下Canvasオブジェクトにアタッチ
            c.gameObject.layer = 5;
            c.gameObject.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            c.gameObject.GetComponent<Canvas>().sortingOrder = 100;
            c.gameObject.AddComponent<CanvasScaler>();
            c.gameObject.AddComponent<GraphicRaycaster>();
            c.gameObject.AddComponent<CanvasGroup>().alpha = 1;

            var image = new GameObject("Image");
            image.transform.parent = c.transform;
            image.gameObject.layer = 5;
            image.gameObject.AddComponent<RectTransform>();
            RectTransform anchor = image.GetComponent<RectTransform>();
            anchor.anchorMin = new Vector2(0,0);
            anchor.anchorMax = new Vector2(1, 1);
            anchor.localPosition = Vector3.zero;
            Color col = new Color(255,255,255);
            image.gameObject.AddComponent<Image>().color = col;
        }
        else ManagersObj = o;

    }

    private void Start()
    {
        if (GameManager.Instance.tankChengeObj == null)
        {
            GameManager.Instance.tankChengeObj = GameObject.Find("TankChengeButton");
        }
    }
}
