using Sirenix.OdinInspector;
using UnityEngine;

public class GizmoDebugger : MonoBehaviour
{
    public enum GizmoType
    {
        Sphere,
        Wire_Sphere,
        Cube,
        Wire_Cube,
        Line,
        Ray,
        Icon
    }

    public enum DrawMode
    {
        Always,
        Selected,
        Runtime
    }

    [Header("Gizmo Settings")]
    public GizmoType type = GizmoType.Wire_Sphere;
    public DrawMode drawMode = DrawMode.Always;
    public Color gizmoColor = Color.yellow;
    public Vector3 offset = Vector3.zero;
    public string label = "";
    
    [Header("Size Settings")]
    public float radius = 1f;
    public Vector3 size = Vector3.one;
    
    [Header("Line Settings")]
    [ShowIf("type", GizmoType.Line)]
    public Vector3 direction = Vector3.forward;
    [ShowIf("type", GizmoType.Line)]
    public float length = 1f;
    
    [Header("Icon Settings")]
    [ShowIf("type", GizmoType.Icon)]
    public string iconName = "Light Gizmo";
    [ShowIf("type", GizmoType.Icon)]
    public bool allowScaling = true;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (drawMode != DrawMode.Always && drawMode != DrawMode.Runtime) return;
        DrawGizmo();
    }

 
    private void OnDrawGizmosSelected()
    {
        if (drawMode != DrawMode.Selected) return;
        DrawGizmo();

       
        // Optional: Add handles for easier size manipulation in scene view
        {
            if (!UnityEditor.Selection.Contains(gameObject)) return;

            UnityEditor.Handles.color = gizmoColor;
            
            switch (type)
            {
                case GizmoType.Sphere:
                case GizmoType.Wire_Sphere:
                    radius = UnityEditor.Handles.RadiusHandle(
                        Quaternion.identity, 
                        transform.position, 
                        radius);
                    break;
                    
                case GizmoType.Line:
                case GizmoType.Ray:
                    UnityEditor.Handles.DrawLine(
                        transform.position, 
                        transform.position + direction.normalized * length);
                    break;
            }
        }

        if (!string.IsNullOrEmpty(label))
        {
            UnityEditor.Handles.Label(transform.position + offset, label);
        }
    }


    private void DrawGizmo()
    {
        Gizmos.color = gizmoColor;

        switch (type)
        {
            case GizmoType.Sphere:
                Gizmos.DrawSphere(transform.position + offset, radius);
                break;
                
            case GizmoType.Wire_Sphere:
                Gizmos.DrawWireSphere(transform.position + offset, radius);
                break;
                
            case GizmoType.Cube:
                Gizmos.DrawCube(transform.position + offset, size);
                break;
                
            case GizmoType.Wire_Cube:
                Gizmos.DrawWireCube(transform.position + offset, size);
                break;
                
            case GizmoType.Line:
                Gizmos.DrawLine(transform.position + offset, transform.position + offset + direction.normalized * length);
                break;
                
            case GizmoType.Ray:
                Gizmos.DrawRay(transform.position + offset, direction.normalized * length);
                break;
                
            case GizmoType.Icon:
                Gizmos.DrawIcon(transform.position + offset, iconName, allowScaling);
                break;
        }

        if (!string.IsNullOrEmpty(label))
        {
            GUIStyle labelStyle = new GUIStyle
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold,
            };
            UnityEditor.Handles.Label(transform.position + offset, label, labelStyle);
        }
    }

#endif
}
