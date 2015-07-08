/*
 * Author:      AndySun
 * Date:        2015-07-08
 * Description: 通过界面操作XML文件进行读写操作
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
            order.OperOrder = XMLRW.ReadXML(PropXMLName.stringValue);
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

                bool bUpEnabled = order.FirstNodeIsCoil() ? nNode > 1 : nNode > 0;
                bool bDownEnabled = nNode < order.OperOrder.Count - 1;
                bool bDelEnabled = order.FirstNodeIsCoil() ? order.OperOrder.Count > 2 : order.OperOrder.Count > 1;

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

                GUI.contentColor =  order.OperOrder[nNode].go == null ? Color.red : GUI.contentColor;
                order.OperOrder[nNode].go = EditorGUILayout.ObjectField(new GUIContent("Operate GameObject:"), order.OperOrder[nNode].go, typeof(GameObject), GUILayout.ExpandWidth(true)) as GameObject;
                GUI.contentColor = v4GUIColor;

                order.OperOrder[nNode].type = (EOperType)EditorGUILayout.EnumPopup(new GUIContent("Operate Type"), order.OperOrder[nNode].type, GUILayout.ExpandWidth(true));
                if (order.OperOrder[nNode].type == EOperType.SetParent)
                {
                    order.OperOrder[nNode].parent = EditorGUILayout.ObjectField(new GUIContent("Parent:"), order.OperOrder[nNode].parent, typeof(Transform)) as Transform;
                }
                else
                    order.OperOrder[nNode].param = EditorGUILayout.Vector3Field(new GUIContent("Operate Parameter:"), order.OperOrder[nNode].param);

                order.OperOrder[nNode].msg = EditorGUILayout.TextField(new GUIContent("Tip Messages:"), order.OperOrder[nNode].msg);
                order.OperOrder[nNode].errorMsg = EditorGUILayout.TextField(new GUIContent("Error Messages:"), order.OperOrder[nNode].errorMsg);

                EditorGUILayout.BeginHorizontal();
                order.OperOrder[nNode].group = EditorGUILayout.ToggleLeft(new GUIContent("Use Group:"), order.OperOrder[nNode].group);
                order.OperOrder[nNode].groupID = EditorGUILayout.TextField(order.OperOrder[nNode].groupID,GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
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
