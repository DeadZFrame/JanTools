using UnityEngine;

namespace Jan.Core
{
    public class CursorUtils : Singleton<CursorUtils>
    {
        public Texture2D defaultCursor;
        public Texture2D horizontalHoverCursor;
        public Texture2D verticalHoverCursor;
        public Vector2 cursorHotspot = Vector2.zero;

        public void Start()
        {
            Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
        }
        
        public void SetCursor(Texture2D texture, Vector2 hotspot, CursorMode mode = CursorMode.Auto)
        {
            Cursor.SetCursor(texture, hotspot, mode);
        }

        public void ResetCursor()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}