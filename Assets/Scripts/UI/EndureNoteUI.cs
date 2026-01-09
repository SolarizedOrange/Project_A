using UnityEngine;
using UnityEngine.UI;

public class EndureNoteUI : MonoBehaviour
{
    float startTime;
    public float StartTime {get {return startTime;}}
    
    float endTime;
    public float EndTime {get {return endTime;}}
    float DisplayTime = 1.0f;
    Material mat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RawImage img = GetComponent<RawImage>();
        mat = Instantiate(img.material);
        img.material = mat;
    }

    void OnDestroy()
    {
        if (mat != null)
        {
            Destroy(mat);
        }
    }

    public void Init(EndureNote note)
    {
        startTime = note.StartTime;
        endTime = note.StartTime + Mathf.Max(note.Duration, 0.05f);
        if (note.Target != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(note.Target.position + note.ScreenOffset);
        }
        Debug.DrawLine(
            transform.position - Camera.main.transform.up * 0.5f,
            transform.position + Camera.main.transform.up * 0.5f,
            Color.cyan,
            5.0f
        );
        Debug.DrawLine(
            transform.position - Camera.main.transform.right * 0.5f,
            transform.position + Camera.main.transform.right * 0.5f,
            Color.cyan,
            5.0f
        );
    }

    public void Hit()
    {
        mat.SetColor("_Color", new Color(0.5f, 0.5f, 0.5f, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        mat.SetFloat("_StartTime", 1.0f - Mathf.Clamp01((startTime - Time.time) / DisplayTime));
        mat.SetFloat("_EndTime", 1.0f - Mathf.Clamp01((endTime - Time.time) / DisplayTime));
    }
}
