using UnityEditor;
using UnityEngine;

public class CreateLevelLine : EditorWindow
{
    private AnimationCurve curve = new();
    private LevelLine _levelLine;
    private string AssetType = "t:LevelLine";
    private float _expDoNextLevel = 100;
    private int _maxLevel = 5;
    private int indexSelected;

    [MenuItem("Tools/Create Level Line")] // объявляем что этот метод можно вывести через панель Window - пункт называется GameTest
    public static void ShowWindow() // он должен быть объявлен статичным 
    {
        EditorWindow.GetWindow(typeof(CreateLevelLine)); // мы создаем либо открываем его если оно открыто (окно типа нашего класса )
    }

    public void OnGUI()
    {
        var AssetList = AssetDatabase.FindAssets(AssetType);

        if (AssetList == null) { return; }

        string[] listName = new string[AssetList.Length];
        for (int i = 0; i < AssetList.Length; i++)
        {
            LevelLine Asset = AssetDatabase.LoadAssetAtPath<LevelLine>(AssetDatabase.GUIDToAssetPath(AssetList[i]));
            listName[i] = Asset.name;
        }

        GUILayout.Label("Enter AnimationCurve", EditorStyles.boldLabel);
        GUILayout.Space(20);


        var newSelectedBinding = EditorGUILayout.Popup(AssetType, indexSelected, listName);
        if (newSelectedBinding != indexSelected)
        {
            _levelLine = AssetDatabase.LoadAssetAtPath<LevelLine>(AssetDatabase.GUIDToAssetPath(AssetList[newSelectedBinding]));
            indexSelected = newSelectedBinding;
        }

        if (_levelLine == null)
        {
            _levelLine = AssetDatabase.LoadAssetAtPath<LevelLine>(AssetDatabase.GUIDToAssetPath(AssetList[indexSelected]));
        }

        GUILayout.Space(100);

        _maxLevel = EditorGUILayout.IntField("MaxLevel", _maxLevel);
        _expDoNextLevel = EditorGUILayout.FloatField("ExpDoNextLevel", _expDoNextLevel);
        curve = EditorGUILayout.CurveField("LevelLine", curve);

        if (GUILayout.Button("Create Level Line"))
        {
            curve.ClearKeys();

            for (int i = 0; i < _maxLevel; i++)
            {
                curve.AddKey(i * _expDoNextLevel, i + 1);
            }
            _levelLine.SetLevelLine(curve);
        }
    }
}
