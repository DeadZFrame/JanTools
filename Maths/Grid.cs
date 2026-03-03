using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Jan.Maths
{
    [Serializable]
    public class JanGrid : MonoBehaviour
    {
        public Vector3Int Matrices => matrices;
        [SerializeField] private Vector3Int matrices = Vector3Int.one;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Vector3 rotationOffset;
        [SerializeField] private Vector3 gridCellSpacing = new Vector3(0.2f, 0.2f, 0.2f);
        
        // For backward compatibility
        public float gridCellSize
        {
            get => gridCellSpacing.x;
            set => gridCellSpacing = new Vector3(value, value, value);
        }
        
        public readonly  List<Node> Nodes = new List<Node>();

        public void CreateGrid()
        {
            Nodes.Clear();
            
            for (int y = 0; y < matrices.y; y++)
            {
                for (int z = 0; z < matrices.z; z++)
                {
                    for (int x = 0; x < matrices.x; x++)
                    {
                        var nodePosition = transform.position + offset + 
                                           new Vector3(x * gridCellSpacing.x, y * gridCellSpacing.y, z * gridCellSpacing.z);
                        // Apply additional rotation from the SerializeField
                        nodePosition = nodePosition.RotatePositionAroundPivot(transform.position, transform.rotation);
                        Node node = new Node 
                        { 
                            Position = nodePosition,
                            LocalRotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationOffset)
                        };
                        Nodes.Add(node);
                    }
                }
            }
        }

        public void UpdateMatrices(Vector3Int newMatrices)
        {
            matrices = newMatrices;
        }
        
        public void CreateGrid<T>(bool autoCalc = false) where T : Node, new()
        {
            if(autoCalc) CreateGridFromBounds();
            
            Nodes.Clear();
            
            for (int y = 0; y < matrices.y; y++)
            {
                for (int z = 0; z < matrices.z; z++)
                {
                    for (int x = 0; x < matrices.x; x++)
                    {
                        var nodePosition = transform.position + offset + 
                                           new Vector3(x * gridCellSpacing.x, y * gridCellSpacing.y, z * gridCellSpacing.z);
                        // Apply additional rotation from the SerializeField, then transform rotation
                        nodePosition = nodePosition.RotatePositionAroundPivot(transform.position, transform.rotation);
                        T node = new T 
                        { 
                            Position = nodePosition, 
                            LocalRotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationOffset)
                        };
                        Nodes.Add(node);
                    }
                }
            }
        }
        
        public void UpdateGridFromPosition()
        {
            var index = 0;
            {
                for (int y = 0; y < matrices.y; y++)
                {
                    for (int z = 0; z < matrices.z; z++)
                    {
                        for (int x = 0; x < matrices.x; x++)
                        {
                            var nodePosition = transform.position + offset + 
                                               new Vector3(x * gridCellSpacing.x, y * gridCellSpacing.y, z * gridCellSpacing.z);
                            // Apply additional rotation from the SerializeField, then transform rotation
                            nodePosition = nodePosition.RotatePositionAroundPivot(transform.position, transform.rotation);
                            Nodes[index].Position = nodePosition;
                            Nodes[index].LocalRotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationOffset);
                            index++;
                        }
                    }
                }
            }
        }
        
        [Button]
        private void CreateGridFromBounds()
        {
            Bounds bounds;

            // Try to get Renderer bounds first, then Collider bounds if Renderer is absent
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                bounds = renderer.bounds;
            }
            else
            {
                Collider collider = GetComponent<Collider>();
                if (collider != null)
                {
                    bounds = collider.bounds;
                }
                else
                {
                    Debug.LogWarning("No Renderer or Collider found on the target object.");
                    return;
                }
            }

            // Calculate grid dimensions based on bounds and cell size
            Vector3 gridSize = new Vector3(
                bounds.size.x / gridCellSpacing.x,
                bounds.size.y / gridCellSpacing.y,
                bounds.size.z / gridCellSpacing.z);
                
            int gridX = Mathf.CeilToInt(gridSize.x) - 1;
            int gridY = Mathf.CeilToInt(gridSize.y);
            int gridZ = Mathf.CeilToInt(gridSize.z) - 1;

            // Generate grid cells based on grid dimensions and bounds center
            Vector3 pos = new Vector3(
                (gridX - 1) * gridCellSpacing.x / 2,
                (gridY - 1) * gridCellSpacing.y / 2,
                (gridZ - 1) * gridCellSpacing.z / 2);

            matrices.x = gridX;
            matrices.y = gridY;
            matrices.z = gridZ;
            offset.x = - pos.x;
            offset.z = - pos.z;
            //offset.y = pos.y; // Set Manually
        }

#if UNITY_EDITOR
        
        private void OnDrawGizmosSelected()
        {
            var index = 0;
            // Store the original gizmos matrix
            Matrix4x4 originalMatrix = Gizmos.matrix;
            // Create the rotation that needs to be applied
            Quaternion rotation = transform.rotation;

            if (Application.isPlaying)
            {
                foreach (var node in Nodes)
                {
                    // Set the matrix with the correct position and rotation
                    Gizmos.matrix = Matrix4x4.TRS(node.Position, rotation, Vector3.one);
                    Gizmos.DrawWireCube(Vector3.zero, gridCellSpacing);
                    Handles.Label(node.Position, index.ToString());
                    index++;
                }
            }
            else
            {
                for (int y = 0; y < matrices.y; y++)
                {
                    for (int z = 0; z < matrices.z; z++)
                    {
                        for (int x = 0; x < matrices.x; x++)
                        {
                            var cellPosition = transform.position + offset + 
                                               new Vector3(x * gridCellSpacing.x, y * gridCellSpacing.y, z * gridCellSpacing.z);
                            var pos = cellPosition.RotatePositionAroundPivot(transform.position, rotation);
                            
                            // Set the matrix with the correct position and rotation
                            Gizmos.matrix = Matrix4x4.TRS(pos, rotation, Vector3.one);
                            Gizmos.DrawWireCube(Vector3.zero, gridCellSpacing);
                            Handles.Label(pos, index.ToString());
                            index++;
                        }
                    }
                }
            }
            
            // Restore the original gizmos matrix
            Gizmos.matrix = originalMatrix;
        }
        
#endif
        
    }

    public class Node
    {
        public Vector3 Position;
        public Quaternion LocalRotation;
    }
}