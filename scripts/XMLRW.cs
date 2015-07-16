﻿/*
 * Author:      AndySun
 * Date:        2015-07-06
 * Description: Read & Write XML file
 * ChangeLog:
 *      2015-07-06
 *          Improvement：
 *          1.每次保存编辑后的结果时，不保留编辑前的内容。即每次保存时，都以空xml文件为基准
 *      2015-07-08:
 *          Added:
 *          1.所有文件路径均在assets目录下，读写xml文件时，只传递文件名
 *      2015-07-07:
 *      Added:
 *          1.
 *      
 */
using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System;

public class XMLRW : MonoBehaviour
{
    /// <summary>
    /// 从XML文件中读取顺序信息
    /// </summary>
    [ExecuteInEditMode]
    public static List<OperItem> ReadXML(string fileName)
    {
        Debug.Log("Read:" + Application.dataPath + "\\" + fileName);
        /// <summary>
        /// 操作列表
        /// </summary>
        List<OperItem> oper = new List<OperItem>();

        XmlDocument doc = new XmlDocument();
        doc.Load(Application.dataPath +"\\"+ fileName);

        XmlNode root = doc.SelectSingleNode("root");
        XmlNodeList list = root.ChildNodes;
        foreach (XmlNode item in list)
        {
            OperItem o = new OperItem();
            XmlElement element = (XmlElement)item;
            o.name = element.GetAttribute("Name");
            o.go = GameObject.Find(element.GetAttribute("gameobject"));
            o.type = (EOperType)Enum.Parse(typeof(EOperType), element.GetAttribute("type"));
            o.param = new Vector3(float.Parse(element.GetAttribute("x")), float.Parse(element.GetAttribute("y")), float.Parse(element.GetAttribute("z")));
            if (o.type == EOperType.SetParent)
                o.parent = GameObject.Find(element.GetAttribute("parent")).transform;
            o.msg = element.GetAttribute("msg");
            o.errorMsg = element.GetAttribute("errorMsg");
            o.groupID = element.GetAttribute("group");
            oper.Add(o);
        }
        return oper;
    }

    /// <summary>
    /// 将操作顺序信息写入到XML文件
    /// </summary>
    [ExecuteInEditMode]
    public static void WriteXML(List<OperItem> operate, string fileName)
    {
        
        string path = Application.dataPath + "\\" + fileName;
        XmlDocument doc = new XmlDocument();
        //if (!File.Exists(path))
        //{
        //    doc.Load(Application.dataPath + "\\Config.xml");
        //}
        //else
        //    doc.Load(path);
        //每次保存编辑后的结果时，不保留以前的内容
        doc.Load(Application.dataPath + "\\Config.xml");
        foreach (OperItem item in operate)
        {
            XmlElement xmlNode = doc.CreateElement("Item");
            xmlNode.SetAttribute("Name", item.name);
            xmlNode.SetAttribute("gameobject", item.go.transform.name);
            xmlNode.SetAttribute("type", item.type.ToString());
            xmlNode.SetAttribute("x", item.param.x.ToString());
            xmlNode.SetAttribute("y", item.param.y.ToString());
            xmlNode.SetAttribute("z", item.param.z.ToString());
            xmlNode.SetAttribute("parent", item.parent == null ? "" : item.parent.name);
            xmlNode.SetAttribute("msg", item.msg);
            xmlNode.SetAttribute("errorMsg", item.errorMsg);
            xmlNode.SetAttribute("group", item.group ? item.groupID : "");
            doc.DocumentElement.AppendChild(xmlNode);
        }
        doc.Save(path);
        Debug.Log(path);
    }
}
