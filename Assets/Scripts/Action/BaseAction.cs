using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


class AddActionWindow : EditorWindow
{
    private List<Type> _allActions;
    private ActionControlWindow _actionControlWindow;

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AddActionWindow));
    }

    void OnGUI()
    {
        if (_actionControlWindow == null)
        {
            _actionControlWindow = GetWindow(typeof(ActionControlWindow)) as ActionControlWindow;

            if (_actionControlWindow == null)
            {
                GUILayout.Label("Can't Find actionControlWindow!!");
                return;
            }
        }

        if( _allActions == null)
        {
            _allActions = new List<Type>();
            foreach (var fileName in Directory.GetFiles(Application.dataPath + "/Scripts/Action/", "*.cs"))
            {
                Type type = Type.GetType(Path.GetFileNameWithoutExtension(fileName));
                if( type == null ||
                    !type.IsClass || type.IsAbstract)
                {
                    continue;
                }

                _allActions.Add(type);
            }
        }
        GUILayout.BeginHorizontal();


        if ( !_allActions.IsEmpty())
        {
            const int widthSize = 100;
            int countByHorizontal = (int)position.width / widthSize;
            int currWidthCount = 0;

            foreach ( var actionType in _allActions)
            {
                if( currWidthCount >= countByHorizontal)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    currWidthCount = 0;
                }

                if ( GUILayout.Button( actionType.Name, GUILayout.Height(50), GUILayout.Width(100)))
                {
                    _actionControlWindow.AddActionToLastWindow(actionType);
                }
                ++currWidthCount;
            }
        }
        GUILayout.EndHorizontal();
    }
}
class ActionControlWindow : EditorWindow
{
    private CustomAnimationController _lastSellectedController = null;

    [MenuItem("Window/My Window")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ActionControlWindow));
    }

    public void AddActionToLastWindow( Type type)
    {
        if( _lastSellectedController != null)
        {
            _lastSellectedController.ActionList.Add( Activator.CreateInstance(type) as BaseAction);
        }
        Repaint();
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.ExpandHeight(true);

        var gameobject = Selection.activeGameObject;
        if ( gameobject != null)
        {
            GUILayout.Label(gameobject.name, EditorStyles.boldLabel);

            var customAnimationContrller = gameobject.GetComponent<CustomAnimationController>();
            if(customAnimationContrller != null &&
               customAnimationContrller.ActionList != null)
            {
                _lastSellectedController = customAnimationContrller;
                for ( int i = 0; i < customAnimationContrller.ActionList.Count; ++i )
                {
                    var action = customAnimationContrller.ActionList[i];

                    GUILayout.Label(action.GetType().ToString(), EditorStyles.boldLabel);
                    if (GUILayout.Button("Remove Action", GUILayout.Width(100)))
                    {
                        if( customAnimationContrller.ActionList.Remove(action) )
                        {
                            --i;
                            continue;
                        }
                    }
                    try
                    {
                        foreach (var property in action.GetType().GetProperties())
                        {
                            var propertyValue = property.GetValue(action, null);
                            if (propertyValue is float)
                            {
                                EditorGUILayout.FloatField(property.Name.ToString(), (float)propertyValue);
                            }
                        }
                    }catch(Exception ex)
                    {
                        Debug.Log(ex.Message);
                    }
                }

                GUILayout.BeginHorizontal();
                GUILayout.ExpandWidth(true);
                if (GUILayout.Button("Add Action"))
                {
                    AddActionWindow.ShowWindow();
                }
                GUILayout.ExpandWidth(false);
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.ExpandHeight(false);
        GUILayout.EndVertical();
    }
}

[System.Serializable]
public abstract class BaseAction : MonoBehaviour{

    [SerializeField]
    protected class AnimationInfo
    {
        public int AnimIndex { get; set; }
        public AnimationClip Clip { get; set; }

        public AnimationInfo()
        {
            AnimIndex = -1;
            Clip = null;
        }
    }

    [SerializeField]
    public string ResourcePath { get; set; }
    [SerializeField]
    public CustomAnimationController customAnimController;
    protected EnumArray<AnimationInfo> _animationInfos;

    protected abstract void LoadResource();


    public void RegisterAnimation()
    {
        if (_animationInfos == null)
        {
            Debug.LogError(string.Format("Couldn't register animation info, because animation info is null: {0}", this.GetType().Name));
            return;
        }

        for ( int n =0; n < _animationInfos.Length; ++n)
        {
            AnimationClip clip = _animationInfos[n].Clip;
            if( clip == null)
            {
                Debug.LogError( string.Format( "Couldn't register animation clip, because clip is null : {0}", this.GetType().Name ));
                continue;
            }
            _animationInfos[n].AnimIndex = customAnimController.AddClip(clip);
        }
    }

    public void UnregisterAnimation()
    {
        for (int n = 0; n < _animationInfos.Length; ++n)
        {
            int index = _animationInfos[n].AnimIndex;
            if (index == -1)
            {
                Debug.LogError(string.Format("Couldn't unregister animation clip, because index is -1 : {0}", this.GetType().Name));
                continue;
            }

            customAnimController.RemoveClip(index);
        }
    }
}
