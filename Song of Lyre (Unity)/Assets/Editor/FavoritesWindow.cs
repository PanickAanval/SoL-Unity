using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class FavoritesWindow : EditorWindow
{
    public ResourceCollection resourceCollection;
    private Vector2 scroll;

    private string newCategoryName = "New Category";

    [MenuItem("Window/Favorites")]
    public static void ShowWindow()
    {
        GetWindow<FavoritesWindow>("Favorites");
    }

    private void OnGUI()
    {
        resourceCollection = EditorGUILayout.ObjectField(resourceCollection, typeof(ResourceCollection), false) as ResourceCollection;
        if (resourceCollection == null) { return; }

        scroll = EditorGUILayout.BeginScrollView(scroll);
        List<ResourceList> categories = new List<ResourceList>(resourceCollection.resources);
        foreach (ResourceList category in categories)
        {

            EditorGUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            category.isFoldout = EditorGUILayout.Foldout(category.isFoldout, category.name, true);
            GUILayout.FlexibleSpace(); // Pushes the X to the right
            if (GUILayout.Button("X"))
            {
                resourceCollection.resources.Remove(category);
                GUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                break;
            }
            GUILayout.EndHorizontal();
            Rect foldoutRect = GUILayoutUtility.GetLastRect();
            HandleCategoryDrop(category.name, foldoutRect);

            if (category.isFoldout)
            {
                List<Object> favorites = category.favorites;
                for (int i = 0; i < favorites.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    favorites[i] = EditorGUILayout.ObjectField(favorites[i], typeof(Object), false);

                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        favorites.RemoveAt(i);
                        break;
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField($"Drag assets here to add them (adds to '{newCategoryName}'):");
        Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drop here");

        HandleDragAndDrop(dropArea, newCategoryName);

        EditorGUILayout.Space();
        DrawCategoryCreationUI();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(resourceCollection);
        }
    }

    private void DrawCategoryCreationUI()
    {
        EditorGUILayout.BeginHorizontal();
        newCategoryName = EditorGUILayout.TextField(newCategoryName);

        if (GUILayout.Button("Add Category"))
        {
            if (resourceCollection.GetResourceListByCategory(newCategoryName) == null)
            {
                resourceCollection.AddCategory(newCategoryName);
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private void HandleDragAndDrop(Rect dropArea, string targetCategory)
    {
        Event evt = Event.current;
        if ((evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform) && dropArea.Contains(evt.mousePosition))
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();

                if (!resourceCollection.resources.Exists(x => x.name == targetCategory))
                {
                    resourceCollection.AddCategory(targetCategory);
                }

                foreach (Object draggedObject in DragAndDrop.objectReferences)
                {
                    string path = AssetDatabase.GetAssetPath(draggedObject);

                    if (AssetDatabase.IsValidFolder(path))
                    {
                        string[] assetGUIDs = AssetDatabase.FindAssets("", new[] { path });

                        foreach (string guid in assetGUIDs)
                        {
                            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                            Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

                            if (asset != null && !resourceCollection.GetResourceListByCategory(targetCategory).favorites.Contains(asset))
                            {
                                resourceCollection.GetResourceListByCategory(targetCategory).favorites.Add(asset);
                            }
                        }
                    }
                    else
                    {
                        if (!resourceCollection.GetResourceListByCategory(targetCategory).favorites.Contains(draggedObject))
                        {
                            resourceCollection.GetResourceListByCategory(targetCategory).favorites.Add(draggedObject);
                        }
                    }
                }
            }

            evt.Use();
        }
    }

    private void HandleCategoryDrop(string category, Rect categoryRect)
    {
        Event evt = Event.current;
        if ((evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform) && categoryRect.Contains(evt.mousePosition))
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                HandleDragAndDrop(categoryRect, category);
            }

            evt.Use();
        }
    }
}