/*
 * Author:      AndySun
 * Date:        2015-07-08
 * Description: 通过界面操作XML文件进行读写操作
 *              1.输入要读取的文件名，点击“Load File”按钮，从XML文件中读取信息并在Inspector界面中显示
 *              ２.输入要写入的文件名，点击“Save File”按钮，将更改后的操作信息写入xml文件。
 * ChangeLog:
 *          2015-07-23
 *          Added:
 *              1.添加物体对象的激活与隐藏控制
 *       2015-07-20:
 *          Added:
 *              1.添加物体运动的速度和精度控制
 */
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(SetOperOrder))]
public class SetOperOrderEditor : Editor
{
    SerializedProperty PropXMLName;
    SerializedProperty PropXMLSaveName;

    void OnEnable()
    {
        PropXMLName = serializedObject.FindProperty("fileName");
        PropXMLSaveName = serializedObject.FindProperty("saveName");
    }

    public override void OnInspectorGUI()
    {
        Vector4 v4GUIColor = GUI.contentColor;

        serializedObject.Update();

        SetOperOrder order = target as SetOperOrder;

        EditorGUILayout.HelpBox("读取XML文件,未设置状态检测", MessageType.Info);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        PropXMLName.stringValue = EditorGUILayout.TextField(new GUIContent("Load XML File:", "当前操作的XML文件名称"), PropXMLName.stringValue);
        if (GUILayout.Button(new GUIContent("Load File")))
        {
            order.OperOrder = XMLRW.ReadXML(PropXMLName.stringValue, order.transform);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        PropXMLSaveName.stringValue = EditorGUILayout.TextField(new GUIContent("Save XML File:"), PropXMLSaveName.stringValue);
        if (GUILayout.Button(new GUIContent("Save File")))
        {
            XMLRW.WriteXML(order.OperOrder, PropXMLSaveName.stringValue);
        }
        EditorGUILayout.EndHorizontal();
        if (EditorGUI.EndChangeCheck())
        {
            order.fileName = PropXMLName.stringValue;
            order.saveName = PropXMLSaveName.stringValue;
        }
        if (GUILayout.Button(new GUIContent("Fold All Option")))
        {
            for (int node = 0; node < order.OperOrder.Count; node++)
            {
                order.OperOrder[node].bFold = false;
            }
        }
        EditorGUILayout.Space();

        int nUp = -1;
        int nDown = -1;
        int nAdd = -1;
        int nDel = -1;

        bool bHasDataNodes = false;
        if (order.OperOrder != null)
        {
            if (order.OperOrder.Count != 0)
            {
                bHasDataNodes = true;
            }
        }

        if (bHasDataNodes == false)
        {
            order.OperOrder = new List<OperItem>();
            order.OperOrder.Add(new OperItem());
        }

        for (int nNode = 0; nNode < order.OperOrder.Count; nNode++)
        {
            order.OperOrder[nNode].bFold = EditorGUILayout.Foldout(order.OperOrder[nNode].bFold, nNode.ToString() + "-" + order.OperOrder[nNode].name);
            if (order.OperOrder[nNode].bFold)
            {
                int nButtonWidth = 60;

                bool bUpEnabled = nNode > 0;
                bool bDownEnabled = nNode < order.OperOrder.Count - 1;
                bool bDelEnabled =  order.OperOrder.Count > 1;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.Width(40));
                GUI.enabled = bUpEnabled;
                if (GUILayout.Button(new GUIContent("Up"), GUILayout.Width(nButtonWidth))) { nUp = nNode; }
                GUI.enabled = bDownEnabled;
                if (GUILayout.Button(new GUIContent("Down"), GUILayout.Width(nButtonWidth))) { nDown = nNode; }
                GUI.enabled = true;
                if (GUILayout.Button(new GUIContent("Add"), GUILayout.Width(nButtonWidth))) { nAdd = nNode; }
                GUI.enabled = bDelEnabled;
                if (GUILayout.Button(new GUIContent("Del"), GUILayout.Width(nButtonWidth))) { nDel = nNode; }
                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();

                EditorGUI.BeginChangeCheck();
                order.OperOrder[nNode].name = EditorGUILayout.TextField(new GUIContent("Operate Name:"), order.OperOrder[nNode].name);
                EditorGUI.EndChangeCheck();
                if (order.OperOrder[nNode].type != EOperType.WaitTime)
                {
                    EditorGUI.BeginChangeCheck();
                    GUI.contentColor = order.OperOrder[nNode].trans == null ? Color.red : GUI.contentColor;
                    order.OperOrder[nNode].trans = EditorGUILayout.ObjectField(new GUIContent("Operate GameObject:"), order.OperOrder[nNode].trans, typeof(Transform), true, GUILayout.ExpandWidth(true)) as Transform;
                    GUI.contentColor = v4GUIColor;
                    if (EditorGUI.EndChangeCheck())
                    {
                        order.OperOrder[nNode].tag = order.OperOrder[nNode].trans.tag;
                        order.OperOrder[nNode].transName = order.OperOrder[nNode].trans.name;
                    }
                }

                order.OperOrder[nNode].type = (EOperType)EditorGUILayout.EnumPopup(new GUIContent("Operate Type"), order.OperOrder[nNode].type, GUILayout.ExpandWidth(true));
                switch (order.OperOrder[nNode].type)
                {
                    case EOperType.SetTransform:
                        order.OperOrder[nNode].transTarget = EditorGUILayout.ObjectField(new GUIContent("target Transform:"),order.OperOrder[nNode].transTarget, typeof(Transform),true) as Transform;
                        order.OperOrder[nNode].speed = EditorGUILayout.FloatField(new GUIContent("Speed:"), order.OperOrder[nNode].speed, GUILayout.ExpandWidth(true));
                        order.OperOrder[nNode].precision = EditorGUILayout.Slider(new GUIContent("Precision:"), order.OperOrder[nNode].precision, 0.001f, 0.1f, GUILayout.ExpandWidth(true));
                        break;
                    case EOperType.SetParent:
                        order.OperOrder[nNode].parent = EditorGUILayout.ObjectField(new GUIContent("Parent:"), order.OperOrder[nNode].parent, typeof(Transform), true) as Transform;
                        break;
                    case EOperType.SetActive:
                        order.OperOrder[nNode].isActive = EditorGUILayout.Toggle(new GUIContent("Is Active:"), order.OperOrder[nNode].isActive);
                        break;
                    case EOperType.WaitTime:
                        order.OperOrder[nNode].time = EditorGUILayout.FloatField(new GUIContent("Wait Time:"), order.OperOrder[nNode].time);
                        break;
                }

                order.OperOrder[nNode].msg = EditorGUILayout.TextField(new GUIContent("Tip Messages:"), order.OperOrder[nNode].msg);
                order.OperOrder[nNode].errorMsg = EditorGUILayout.TextField(new GUIContent("Error Messages:"), order.OperOrder[nNode].errorMsg);

                EditorGUILayout.BeginHorizontal();
                order.OperOrder[nNode].group = EditorGUILayout.ToggleLeft(new GUIContent("Use Group:"), order.OperOrder[nNode].group);
                order.OperOrder[nNode].groupID = EditorGUILayout.TextField(order.OperOrder[nNode].groupID, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
                order.OperOrder[nNode].isFinishPause = EditorGUILayout.Toggle(new GUIContent("Is Finish Pause:"),order.OperOrder[nNode].isFinishPause);
            }
        }
        if (nUp != -1)
        {
            order.MoveNodeUp(nUp);
        }
        else if (nDown != -1)
        {
            order.MoveNodeDown(nDown);
        }
        else if (nAdd != -1)
        {
            order.CreateNewNode(nAdd);
        }
        else if (nDel != -1)
        {
            order.RemoveNode(nDel);
        }

        EditorGUILayout.Space();
    }
}
