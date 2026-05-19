using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class PaintScriptVis : MonoBehaviour
{
    public GameObject target;
    RenderTexture targetTexture;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);

        targetTexture = target.GetComponent<Paintable>().GetMask();
        if (targetTexture == null)
        {
            Debug.Log("TARGET TEXTURE NULL");
        }
        GetComponent<Renderer>().material.SetTexture("_BaseMap", targetTexture);


        // yield return new WaitForSeconds(3.0f);
        // PaintManager.Instance.Paint(target.GetComponent<Paintable>(), new(0.0f, 0.0f, 1.0f));
        // targetTexture = target.GetComponent<Paintable>().GetMask();
        // if (targetTexture == null)
        // {
        //     Debug.Log("TARGET TEXTURE NULL");
        // }
        // GetComponent<Renderer>().material.SetTexture("_BaseMap", targetTexture);


        yield return null;
    }
}
