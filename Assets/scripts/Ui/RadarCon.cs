using UnityEngine;
using UnityEngine.UI;
public class RadarCon : MonoBehaviour
{
    public Image image;
    float time = 0;
    float speed;

    private void Update()
    {
        if (TurnManager.Instance.playerTurn && gameObject.activeSelf)
        {
            float pos = Vector3.Distance(TurnManager.Instance.nowPayer.transform.position,TurnManager.Instance.nearEnemy.transform.position);
            if (pos < 500) speed = 0.5f;
            if (pos < 300) speed = 1f;
            if (pos < 100) speed = 3f;
            if (pos < 50) speed = 3.5f;
            if (pos <= 0)speed = 4f;
            image.color = GetAlphaColor(image.color, speed);
        }
    }

    Color GetAlphaColor(Color color,float spd)
    {
        if (color.a == 255f)
        {
            Debug.Log("risetto");
            GameManager.Instance.source.PlayOneShot(GameManager.Instance.RadarSfx);
        }
        else
        {
            time += Time.deltaTime * 5.0f * spd;
            color.a = Mathf.Sin(time) * 0.5f + 0.5f;
        }
        return color;
    }
}
