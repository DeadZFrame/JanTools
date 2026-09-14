using UnityEngine;

namespace Jan.Navigation
{
    public class NavmeshItem : MonoBehaviour
    {
        [field: SerializeField] public NavmeshAreas NavmeshArea { get; private set; }
    }
}