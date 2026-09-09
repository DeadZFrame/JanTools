using Jan.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Jan.Core
{
    public class ObservedValueTrigger: MonoBehaviour
    {
        enum ValueType
        {
            None,
            Int,
            Float,
            Bool
        }
        
        [SerializeField, ValueDropdown(nameof(GetObservedNames)), OnValueChanged(nameof(DetectValueType))]
        private string observedValueName;
        private string[] GetObservedNames => ObservedValueManager.GetAllObservedNames();

        [SerializeField, ShowIf(nameof(_valueType), ValueType.Int)] private int targetValue;
        [SerializeField, ShowIf(nameof(_valueType), ValueType.Float)] private float targetFloatValue;
        //[SerializeField, ShowIf(nameof(_valueType), ValueType.String)] private string targetStringValue;
        [SerializeField, ShowIf(nameof(_valueType), ValueType.Bool)] private bool targetBoolValue;

        [SerializeField] private UnityEvent response;

        private ValueType _valueType;

        void OnEnable()
        {
            var observedValue = ObservedValueManager.Get(observedValueName);

            _valueType = observedValue switch
            {
                ObservedValue<int> => ValueType.Int,
                ObservedValue<float> => ValueType.Float,
                ObservedValue<bool> => ValueType.Bool,
                _ => ValueType.None
            };

            switch (_valueType)
            {
                case ValueType.Int:
                    Debug.Log($"Listening to int observed value for {observedValueName}");
                    observedValue.Listen<int>(OnObservedValueChanged);
                    break;
                case ValueType.Float:
                    observedValue.Listen<float>(OnObservedValueChanged);
                    break;
                case ValueType.Bool:
                    observedValue.Listen<bool>(OnObservedValueChanged);
                    break;
            }
        }

        void OnDisable()
        {
            var observedValue = ObservedValueManager.Get(observedValueName);

            switch (_valueType)
            {
                case ValueType.Int:
                    observedValue.Unlisten<int>(OnObservedValueChanged);
                    break;
                case ValueType.Float:
                    observedValue.Unlisten<float>(OnObservedValueChanged);
                    break;
                case ValueType.Bool:
                    observedValue.Unlisten<bool>(OnObservedValueChanged);
                    break;
            }
        }

        private void OnObservedValueChanged(int newValue)
        {
            Debug.Log($"Observed value changed for {observedValueName}: {newValue}");
            if (newValue == targetValue)
            {
                response?.Invoke();
            }
        }

        private void OnObservedValueChanged(float newValue)
        {
            if (Mathf.Approximately(newValue, targetFloatValue))
            {
                response?.Invoke();
            }
        }

        private void OnObservedValueChanged(bool newValue)
        {
            if (newValue == targetBoolValue)
            {
                response?.Invoke();
            }
        }

        private void DetectValueType()
        {
            var observedValue = ObservedValueManager.Get(observedValueName);

            _valueType = observedValue switch
            {
                ObservedValue<int> => ValueType.Int,
                ObservedValue<float> => ValueType.Float,
                ObservedValue<bool> => ValueType.Bool,
                _ => ValueType.None
            };
        }
    }
}