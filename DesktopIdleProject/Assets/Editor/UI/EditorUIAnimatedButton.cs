using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(UIAnimatedButton))]
[CanEditMultipleObjects]
public class EditorUIAnimatedButton : ButtonEditor
{
    private SerializedProperty s_timeSingleFrame;
    private SerializedProperty s_spriteList;

    protected override void OnEnable()
    {
        base.OnEnable();

        s_timeSingleFrame = serializedObject.FindProperty("timeSingleFrame");
        s_spriteList = serializedObject.FindProperty("spriteList");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();

        EditorGUILayout.PropertyField(s_timeSingleFrame);
        EditorGUILayout.PropertyField(s_spriteList);

        serializedObject.ApplyModifiedProperties();
    }
}
