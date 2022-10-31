using LiteNinja.DI.attributes;
using UnityEditor;
using UnityEngine;

namespace LiteNinja.DI.Editor
{
    [CustomPropertyDrawer(typeof(ImplementsAttribute))]
    public class ImplementsAttributeDrawer : PropertyDrawer
    {
        const int paddingHeight = 8, marginHeight = 6;
        float baseHeight = 0;

        ImplementsAttribute implementsAttribute => (ImplementsAttribute)attribute;

        private bool IsValid(SerializedProperty property, out SerializedProperty invalid)
        {
            invalid = property;
            if (!property.isArray)
            {
                if (property.objectReferenceValue is GameObject go)
                {
                    property.objectReferenceValue = go.GetComponent(implementsAttribute.InterfaceType);
                }

                return property.objectReferenceValue == null ||
                       implementsAttribute.InterfaceType.IsInstanceOfType(property.objectReferenceValue);
            }

            for (var i = 0; i < property.arraySize; ++i)
            {
                var arrElement = property.GetArrayElementAtIndex(i);
                if (arrElement.objectReferenceValue is GameObject go)
                {
                    arrElement.objectReferenceValue = go.GetComponent(implementsAttribute.InterfaceType);
                }

                if (arrElement.objectReferenceValue == null ||
                    implementsAttribute.InterfaceType.IsInstanceOfType(arrElement.objectReferenceValue)) continue;
                invalid = arrElement;
                return false;
            }

            return true;
        }

        private string GetHelpBoxText(SerializedProperty property)
        {
            return
                $"Object {property.objectReferenceValue.name} of type {property.objectReferenceValue.GetType()} does not implement {implementsAttribute.InterfaceType}";
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var boxPos = position;
            position.height = baseHeight;
            EditorGUI.ObjectField(position, property,
                new GUIContent($"{property.displayName} ({implementsAttribute.InterfaceType.Name})",
                    $"{property.displayName} must implement {implementsAttribute.InterfaceType.Name}"));

            if (!IsValid(property, out var invalid))
            {
                boxPos.height -= baseHeight + marginHeight * 2;
                boxPos.y += baseHeight + marginHeight;
                EditorGUI.HelpBox(boxPos, GetHelpBoxText(invalid), MessageType.Error);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            baseHeight = base.GetPropertyHeight(property, label);

            if (IsValid(property, out var invalid)) return baseHeight;
            const float minHeight = paddingHeight * 5;
            var content = new GUIContent(GetHelpBoxText(invalid));
            var style = GUI.skin.GetStyle("helpbox");
            var height = style.CalcHeight(content, EditorGUIUtility.currentViewWidth);
            height += marginHeight * 2;
            return height > minHeight ? height + baseHeight : minHeight + baseHeight;

        }
    }
}