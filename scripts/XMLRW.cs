﻿
/*
 * Author:      AndySun
 * Date:        2015-07-06
 * Description: Read & Write XML file
 * ChangeLog:
 *      2015-08-02
 *          Added:
 *              1.XML文件的读取改为从Resources文件夹中读取和写入
 *      2015-07-28
 *          Added:
 *              1.保持物体的初始位置及角度信息，供操作后返回最初状态
 *      2015-07-23
 *          Added:
 *              1.添加物体对象的激活与隐藏控制
 *      2015-07-21
 *          Improvement:
 *              1.XML文件中只保存运动物体的名称和Tag用于定位特定的物体，为便于管理防止同名内容，Tranform变量的最终确定由Controller决定
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
using System;
using System.Collections.Generic;
using System.Xml;

public class XMLRW : MonoBehaviour
{
    
    /// <summary>
    /// 从XML文件中读取顺序信息
    /// </summary>
    [ExecuteInEditMode]
    public static List<OperItem> ReadXML(string fileName, Transform rootTrans)
    {
        /// <summary>
        /// 操作列表
        /// </summary>
        List<OperItem> oper = new List<OperItem>();

        XmlDocument doc = new XmlDocument();
        TextAsset textAsset = (TextAsset)Resources.Load(fileName, typeof(TextAsset));
        doc.LoadXml(textAsset.text);

        // doc.Load(Application.dataPath + "\\" + fileName);

        XmlNode root = doc.SelectSingleNode("root");
        XmlNodeList list = root.ChildNodes;
        foreach (XmlNode item in list)
        {
            OperItem o = new OperItem();
            XmlElement element = (XmlElement)item;
            o.name = element.GetAttribute("Name");
            o.transName = element.GetAttribute("transName");
            //获取要操作的物体对象
            o.tag = element.GetAttribute("tag");
            foreach (Transform trans in rootTrans.GetComponentsInChildren<Transform>())
            {
                if (o.transName == trans.name)
                {
                    o.trans = trans;
                    o.originPos = trans.position;
                    o.originRot = trans.localEulerAngles;
                    break;
                }
            }
            
            //根据操作类型填充对象值
            o.type = (EOperType)Enum.Parse(typeof(EOperType), element.GetAttribute("type"));
            switch (o.type)
            {
                case EOperType.SetTransform:
                    foreach (Transform trans in rootTrans.GetComponentsInChildren<Transform>())
                    {
                        if (element.GetAttribute("targetName") == trans.name)
                        {
                            o.transTarget = trans;
                            break;
                        }
                    }
                    o.speed = float.Parse(element.GetAttribute("speed"));
                    o.precision = float.Parse(element.GetAttribute("precision"));
                    break;
                case EOperType.SetParent:
                    foreach (Transform trans in rootTrans.GetComponentsInChildren<Transform>())
                    {
                        if (element.GetAttribute("parent") == trans.name)
                        {
                            o.parent = trans;
                            break;
                        }
                    }
                    break;
                case EOperType.SetActive:
                    o.isActive = !string.IsNullOrEmpty(element.GetAttribute("active"));
                    break;
                case EOperType.WaitTime:
                    o.time = float.Parse(element.GetAttribute("time"));
                    break;
            }
            o.msg = element.GetAttribute("msg");
            o.errorMsg = element.GetAttribute("errorMsg");
            o.groupID = element.GetAttribute("group");
            if (!String.IsNullOrEmpty(o.groupID))
            {
                o.group = true;
            }
            else
                o.group = false;
            o.isFinishPause = !string.IsNullOrEmpty(element.GetAttribute("pause"));
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
        string path = Application.dataPath + "\\Resources\\" + fileName + ".xml";
        XmlDocument doc = new XmlDocument();

        //每次保存编辑后的结果时，不保留以前的内容
        doc.Load(Application.dataPath + "\\Config.xml");
        foreach (OperItem item in operate)
        {
            XmlElement xmlNode = doc.CreateElement("Item");
            xmlNode.SetAttribute("Name", item.name);
            xmlNode.SetAttribute("transName", item.transName);
            xmlNode.SetAttribute("tag", item.tag);
            xmlNode.SetAttribute("type", item.type.ToString());
            xmlNode.SetAttribute("targetName", item.transTarget != null ? item.transTarget.name : "");
            xmlNode.SetAttribute("speed", item.speed.ToString());
            xmlNode.SetAttribute("precision", item.precision.ToString());
            xmlNode.SetAttribute("parent", item.parent == null ? "" : item.parent.name);
            xmlNode.SetAttribute("active", item.isActive ? "true" : "");
            xmlNode.SetAttribute("time", item.time.ToString());
            xmlNode.SetAttribute("msg", item.msg);
            xmlNode.SetAttribute("errorMsg", item.errorMsg);
            xmlNode.SetAttribute("group", item.group ? item.groupID : "");
            xmlNode.SetAttribute("pause", item.isFinishPause ? "true" : "");
            doc.DocumentElement.AppendChild(xmlNode);
        }
        doc.Save(path);
        Debug.Log(path);
    }
}
