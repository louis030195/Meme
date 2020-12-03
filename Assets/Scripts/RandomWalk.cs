using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomWalk : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(S());
    }

    private IEnumerator S()
    {
        var n = 1_000_000;
        var x = Enumerable.Repeat(0, n).ToArray();
        var y = Enumerable.Repeat(0, n).ToArray();
        foreach (var i in Enumerable.Range(1, n))
        {
            var v = Random.Range(1, 4);
            switch (v)
            {
                case 1:
                    x[i] = x[i - 1] + 1;
                    y[i] = y[i - 1];
                    break;
                case 2:
                    x[i] = x[i - 1] - 1;
                    y[i] = y[i - 1];
                    break;
                case 3:
                    x[i] = x[i - 1];
                    y[i] = y[i - 1] + 1;
                    break;
                default:
                    x[i] = x[i - 1];
                    y[i] = y[i - 1] - 1;
                    break;
            }

            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = new Vector3(x[i], y[i], 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
