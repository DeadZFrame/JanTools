using UnityEngine;

public static class MaterialExtensions
{
    public static void ChangeColor(this Renderer renderer, Color color)
    {
        if(renderer != null)
        {
            var propertyBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_BaseColor", color);
            renderer.SetPropertyBlock(propertyBlock);
        }
        else
        {
            Debug.LogError("Renderer is null. Cannot change color.");
        }
    }
}
