using UnityEngine;

public class DebuggerCollider : MonoBehaviour
{

    private BoxCollider2D boxCollider;
    private LineRenderer lineRenderer;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 5;
        lineRenderer.loop = true;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    void Update()
    {
        if (boxCollider != null)
        {
            Vector2 size = boxCollider.size;
            Vector2 offset = boxCollider.offset;
            Vector2 pos = (Vector2)transform.position + offset;

            Vector3[] points = new Vector3[5];
            points[0] = new Vector3(pos.x - size.x / 2, pos.y + size.y / 2, 0);
            points[1] = new Vector3(pos.x + size.x / 2, pos.y + size.y / 2, 0);
            points[2] = new Vector3(pos.x + size.x / 2, pos.y - size.y / 2, 0);
            points[3] = new Vector3(pos.x - size.x / 2, pos.y - size.y / 2, 0);
            points[4] = points[0]; // Boucle fermée

            lineRenderer.SetPositions(points);
        }
    }
}
