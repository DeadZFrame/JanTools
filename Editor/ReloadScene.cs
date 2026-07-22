using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class ReloadSceneEditor
{
    // This adds a menu item and binds it to the hotkey Ctrl + R (Windows) or Cmd + R (Mac)
    [MenuItem("Tools/Reload Scene %q")]
    public static void ReloadActiveScene()
    {
        // Get the active scene path
        string currentScenePath = SceneManager.GetActiveScene().path;

        if (!string.IsNullOrEmpty(currentScenePath))
        {
            // Open the scene again, which forces a reload from disk
            EditorSceneManager.OpenScene(currentScenePath);
        }
    }
}
