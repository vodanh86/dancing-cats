#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
//using System;
//using System;

public class ScenesMenu : EditorWindow
{

    private GUIStyle simpleButtonStyle;
    private GUIStyle greenButtonStyle;
    private Vector2 scrollPosition = Vector2.zero;

    [MenuItem("GameScenes/Show All Scenes #%&L")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ScenesMenu));
    }

    void OnEnable()
    {
        // Устанавливаем минимальный и максимальный размеры окна
        this.minSize = new Vector2(20, 20);
        //this.maxSize = new Vector2(200, 100);
    }

    //[MenuItem("GameScenes/Load Scene 0 %#&0")] public static void LoadScene0() { LoadSceneWithIndex(0); }
    //[MenuItem("GameScenes/Load Scene 1 %#&1")] public static void LoadScene1() { LoadSceneWithIndex(1); }
    //[MenuItem("GameScenes/Load Scene 2 %#&2")] public static void LoadScene2() { LoadSceneWithIndex(2); }
    //[MenuItem("GameScenes/Load Scene 3 %#&3")] public static void LoadScene3() { LoadSceneWithIndex(3); }
    //[MenuItem("GameScenes/Load Scene 4 %#&4")] public static void LoadScene4() { LoadSceneWithIndex(4); }
    //[MenuItem("GameScenes/Load Scene 5 %#&5")] public static void LoadScene5() { LoadSceneWithIndex(5); }
    //[MenuItem("GameScenes/Load Scene 6 %#&6")] public static void LoadScene6() { LoadSceneWithIndex(6); }
    //[MenuItem("GameScenes/Load Scene 7 %#&7")] public static void LoadScene7() { LoadSceneWithIndex(7); }
    //[MenuItem("GameScenes/Load Scene 8 %#&8")] public static void LoadScene8() { LoadSceneWithIndex(8); }
    //[MenuItem("GameScenes/Load Scene 9 %#&9")] public static void LoadScene9() { LoadSceneWithIndex(9); }

    void OnGUI()
    {
        if (simpleButtonStyle == null)
        {
            simpleButtonStyle = new GUIStyle(GUI.skin.button);
            simpleButtonStyle.normal.textColor = Color.white;
            simpleButtonStyle.hover.textColor = Color.white;
            simpleButtonStyle.active.textColor = Color.white;
            simpleButtonStyle.alignment = TextAnchor.MiddleLeft;
        }

        if (greenButtonStyle == null)
        {
            greenButtonStyle = new GUIStyle(GUI.skin.button);
            greenButtonStyle.normal.textColor = Color.green;
            greenButtonStyle.hover.textColor = Color.green;
            greenButtonStyle.active.textColor = Color.green;
            greenButtonStyle.alignment = TextAnchor.MiddleLeft;
        }

        GUILayout.Label("Custom Scene Menu", EditorStyles.boldLabel);
        var scenes = EditorBuildSettings.scenes;


        // Устанавливаем начальную позицию прокрутки
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        for (int i = 0; i < scenes.Length; i++)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenes[i].path);
            GUIStyle style = EditorSceneManager.GetActiveScene().buildIndex == i ? greenButtonStyle : simpleButtonStyle;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(sceneName, style, GUILayout.ExpandWidth(true)))
            {
                LoadSceneWithIndex(i);
            }
            if (GUILayout.Button("P", GUILayout.Width(20)))
            {
                PlaySceneWithIndex(i);
            }
            if (GUILayout.Button("L", GUILayout.Width(20)))
            {
                SelectScene(i);
            }
            GUILayout.EndHorizontal();
        }
        // Заканчиваем прокручиваемую область
        GUILayout.EndScrollView();
    }

    private static string _previousScenePath;

    private static void LoadSceneWithIndex(int index)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(EditorBuildSettings.scenes[index].path);
        }
    }

    private static void PlaySceneWithIndex(int index)
    {
        // Сохраняем путь к текущей сцене
        _previousScenePath = EditorSceneManager.GetActiveScene().path;
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            // Если пользователь согласился сохранить изменения или их не было, загружаем сцену с индексом 0
            EditorSceneManager.OpenScene(EditorBuildSettings.scenes[index].path);
            EditorApplication.isPlaying = true; // Запускаем игру
        }
    }

    //[InitializeOnLoadMethod]
    //private static void Initialize()
    //{
    //    // Подписываемся на событие, которое срабатывает перед выходом из режима игры
    //    EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    //}

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        // Проверяем, вышли ли мы из режима игры
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            // Возвращаемся к предыдущей сцене, если она была сохранена
            if (!string.IsNullOrEmpty(_previousScenePath))
            {
                EditorSceneManager.OpenScene(_previousScenePath);
                _previousScenePath = null; // Очищаем сохраненный путь
                EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            }
        }
    }



    public static void SelectScene(int index)
    {
        if (index < 0 || index >= EditorBuildSettings.scenes.Length)
        {
            Debug.LogError("Invalid scene index.");
            return;
        }

        string scenePath = EditorBuildSettings.scenes[index].path;

        // Load the asset at the specified path and select it
        Object sceneAsset = AssetDatabase.LoadAssetAtPath<Object>(scenePath);
        if (sceneAsset != null)
        {
            Selection.activeObject = sceneAsset;
            EditorGUIUtility.PingObject(sceneAsset);
        }
        else
        {
            Debug.LogError("Scene asset not found at path: " + scenePath);
        }
    }

}
#endif