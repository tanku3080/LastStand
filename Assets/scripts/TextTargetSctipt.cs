using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTargetSctipt : MonoBehaviour
{
    [SerializeField, Header("E‰Ÿ‚µ‚½‚ç")]
    float ThetaScale = 0.01f;
    private float Theta = 0f;
    private int Size;
    [Header("Enter‚¨‚µ‚½‚ç")]
    [SerializeField] private LineRenderer m_lineRenderer = null; // ‰~‚ğ•`‰æ‚·‚é‚½‚ß‚Ì LineRenderer
    [SerializeField] private float radius = 0;    // ‰~‚Ì”¼Œa
    [SerializeField] private float lineWidth = 0.1f;    // ‰~‚Ìü‚Ì‘¾‚³
    [SerializeField] private float center_x = 0;    // ‰~‚Ì’†SÀ•W
    [SerializeField] private float center_y = 0;    // ‰~‚Ì’†SÀ•W
    private void Start()
    {
        m_lineRenderer = gameObject.GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            radius /= 100;
            radius -= 1;
            if (radius == 0)
            {
                radius = -0.99f;
            }
            int segments = 380;
            m_lineRenderer.startWidth = lineWidth;
            m_lineRenderer.endWidth = lineWidth;
            m_lineRenderer.positionCount = segments;
            var points = new Vector3[segments];
            for (int i = 0; i < segments; i++)
            {
                var rad = Mathf.Deg2Rad * (i * 380f / segments);
                var x = center_x + Mathf.Sin(rad) * radius;
                var y = center_y + Mathf.Cos(rad) * radius;
                points[i] = new Vector3(x, y, 0);
            }
            m_lineRenderer.SetPositions(points);
        }
        Theta = 0f;
        Size = (int)((1f / ThetaScale) + 1f);
        m_lineRenderer.positionCount = Size;
        for (int i = 0; i < Size; i++)
        {
            Theta += (2.0f * Mathf.PI * ThetaScale);
            float x = radius * Mathf.Cos(Theta);
            float y = radius * Mathf.Sin(Theta);
            m_lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}
