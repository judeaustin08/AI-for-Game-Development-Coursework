using static Constraint;

using UnityEditor;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(Constraint))]
public class ConstraintEditor : Editor
{
    SerializedProperty type_prop;

    SerializedProperty comparisonType_prop;
    SerializedProperty boolEvaluationType_prop;

    SerializedProperty distanceThreshold_prop;
    SerializedProperty targetTag_prop;

    SerializedProperty propertyName_prop;
    SerializedProperty floatCompare_prop;

    private void OnEnable()
    {
        type_prop = serializedObject.FindProperty("type");
        comparisonType_prop = serializedObject.FindProperty("comparisonType");
        boolEvaluationType_prop = serializedObject.FindProperty("boolEvaluationType");

        distanceThreshold_prop = serializedObject.FindProperty("distanceThreshold");
        targetTag_prop = serializedObject.FindProperty("targetTag");

        propertyName_prop = serializedObject.FindProperty("propertyName");
        floatCompare_prop = serializedObject.FindProperty("floatCompare");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        PropertyField(type_prop);
        Type type = (Type)type_prop.enumValueIndex;

        switch (type)
        {
            case Type.DISTANCE:
                PropertyField(distanceThreshold_prop);
                PropertyField(comparisonType_prop);
                PropertyField(targetTag_prop);
                break;
            case Type.FLOAT:
                PropertyField(propertyName_prop);
                PropertyField(comparisonType_prop);
                PropertyField(floatCompare_prop);
                break;
            case Type.BOOLEAN:
                PropertyField(propertyName_prop);
                PropertyField(boolEvaluationType_prop);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}