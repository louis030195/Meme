using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Fractal : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        // StartCoroutine(DrawCircle(0, 0, 500));
        // StartCoroutine(DrawCircle2(500, 500, 500));
        StartCoroutine(DrawCircle3(500, 500, 500));
    }

    private IEnumerator DrawCircle(float x, float y, float radius)
    {
        yield return new WaitForSeconds(0.01f);
        Ellipse(new GameObject(radius.ToString(CultureInfo.InvariantCulture)), radius, 1, Color.red, Color.blue);
        if (radius > 2) {
            radius *= 0.75f;
            StartCoroutine(DrawCircle(0, 0, radius));
        }
    }
    
    private IEnumerator DrawCircle2(float x, float y, float radius)
    {
        yield return new WaitForSeconds(0.01f);
        var go = new GameObject();
        go.transform.position = new Vector3(x, y, 0);
        Ellipse(go, radius, 1, Color.red, Color.blue);
        if (radius > 2) {
            StartCoroutine(DrawCircle2(x + radius/2, y, radius/2));
            StartCoroutine(DrawCircle2(x - radius/2, y, radius/2));
        }
    }
    
    private IEnumerator DrawCircle3(float x, float y, float radius) {
        yield return new WaitForSeconds(0.01f);
        var go = new GameObject();
        go.transform.position = new Vector3(x, y, 0);
        Ellipse(go, radius, 1, Color.red, Color.blue);
        if(radius > 8) {
            StartCoroutine(DrawCircle3(x + radius/2, y, radius/2));
            StartCoroutine(DrawCircle3(x - radius/2, y, radius/2));
            StartCoroutine(DrawCircle3(x, y + radius/2, radius/2));
            StartCoroutine(DrawCircle3(x, y - radius/2, radius/2));
        }
    }
    
    private const int NumberOfSegments = 360;
    private void Ellipse(GameObject go, float radius, float lineWidth, Color startColor, Color endColor)
    {
        var circle = go.AddComponent<LineRenderer>();
        circle.material = new Material(Shader.Find("Unlit/Texture"));;
        circle.useWorldSpace = false;
        circle.startWidth = lineWidth;
        circle.endWidth = lineWidth;
        circle.endColor = endColor;
        circle.startColor = startColor;
        circle.positionCount = NumberOfSegments + 1;
        var points = new Vector3[NumberOfSegments + 1];

        for (var i = 0; i < NumberOfSegments + 1; i++)
        {
            var rad = Mathf.Deg2Rad * i;
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }
        circle.SetPositions(points);
    }
}
