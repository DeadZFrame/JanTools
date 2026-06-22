// using UnityEngine;
// using UnityEditor;
// using System.IO;
// using Jan.Core;

// namespace Jan.Editor
// {
//     [CustomEditor(typeof(SoundLibrary))]
//     public class EnumEditor : Editor
//     {
//         public override void OnInspectorGUI()
//         {
//             // Draw standard inspector fields
//             DrawDefaultInspector();

//             SoundLibrary script = (SoundLibrary)target;

//             GUILayout.Space(15);
//             GUILayout.Label("Add New Enum Flag", EditorStyles.boldLabel);
            
//             // Input text field in inspector
//             script.newFlagName = EditorGUILayout.TextField("New Flag Name", script.newFlagName);

//             if (GUILayout.Button("Append to Enum"))
//             {
//                 if (!string.IsNullOrEmpty(script.newFlagName))
//                 {
//                     AddNewEnumEntry(script.newFlagName.Trim());
//                     script.newFlagName = ""; // Clear string input field
//                 }
//             }
//         }

//         private void AddNewEnumEntry(string newEntry)
//         {
//             // Find the absolute path to your file
//             string filePath = "Assets/GameData.cs"; 
            
//             if (!File.Exists(filePath))
//             {
//                 Debug.LogError("Enum file not found at path: " + filePath);
//                 return;
//             }

//             string fileContent = File.ReadAllText(filePath);

//             // Locates the closing bracket of your target enum block
//             int enumEndIndex = fileContent.IndexOf("FirstQuest,"); // Points near target block
//             if (enumEndIndex == -1) return;

//             // Clean way to insert text cleanly before the closing bracket of the enum block
//             string searchString = "FirstQuest,";
//             string insertString = $"\n    {newEntry},";
            
//             string updatedContent = fileContent.Replace(searchString, searchString + insertString);

//             File.WriteAllText(filePath, updatedContent);
            
//             // Force Unity editor to read asset changes and compile
//             AssetDatabase.ImportAsset(filePath); 
//         }
//     }
// }
