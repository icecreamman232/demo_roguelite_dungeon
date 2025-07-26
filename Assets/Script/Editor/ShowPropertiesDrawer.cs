using UnityEditor;
using UnityEngine;

namespace SGGames.Script.EditorExtensions
{
    [CustomPropertyDrawer(typeof(ShowPropertiesAttribute))]
    public class ShowPropertiesDrawer : PropertyDrawer
    {
        private bool isExpanded = false; // Tracks foldout state

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float currentY = position.y;

            // Draw the ScriptableObject field
            Rect objectFieldRect = new Rect(position.x, currentY, position.width, lineHeight);
            EditorGUI.PropertyField(objectFieldRect, property, label);
            currentY += lineHeight + spacing;

            // Check if the property is a valid ScriptableObject
            if (property.propertyType == SerializedPropertyType.ObjectReference &&
                property.objectReferenceValue != null &&
                property.objectReferenceValue is ScriptableObject)
            {
                // Draw the foldout toggle
                Rect foldoutRect = new Rect(position.x, currentY, position.width, lineHeight);
                isExpanded = EditorGUI.Foldout(foldoutRect, isExpanded, "Properties", true);
                currentY += lineHeight + spacing;

                // If expanded, draw the properties of the ScriptableObject
                if (isExpanded)
                {
                    EditorGUI.indentLevel++;
                    SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
                    serializedObject.Update(); // Sync serialized data

                    SerializedProperty iterator = serializedObject.GetIterator();
                    iterator.NextVisible(true); // Skip the script reference

                    // Iterate through all visible properties
                    while (iterator.NextVisible(false))
                    {
                        float propertyHeight = EditorGUI.GetPropertyHeight(iterator, true);
                        Rect propertyRect = new Rect(position.x, currentY, position.width, propertyHeight);
                        EditorGUI.PropertyField(propertyRect, iterator, true);
                        currentY += propertyHeight + spacing;
                    }

                    serializedObject.ApplyModifiedProperties(); // Apply changes
                    EditorGUI.indentLevel--;
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = EditorGUIUtility.singleLineHeight; // Height for object field
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            // Add height for foldout if the property is a valid ScriptableObject
            if (property.propertyType == SerializedPropertyType.ObjectReference &&
                property.objectReferenceValue != null &&
                property.objectReferenceValue is ScriptableObject)
            {
                totalHeight += EditorGUIUtility.singleLineHeight + spacing;

                // If expanded, add height for nested properties
                if (isExpanded)
                {
                    SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
                    SerializedProperty iterator = serializedObject.GetIterator();
                    iterator.NextVisible(true); // Skip script reference

                    while (iterator.NextVisible(false))
                    {
                        totalHeight += EditorGUI.GetPropertyHeight(iterator, true) + spacing;
                    }
                }
            }

            return totalHeight;
        }
    }
}
