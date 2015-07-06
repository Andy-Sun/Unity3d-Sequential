/*
 * Author:      AndySun
 * Date:        2015-07-06
 * Description: Read & Write XML file
 */
using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;

public class XMLRW : MonoBehaviour {
    /// <summary>
    /// 操作列表
    /// </summary>
    public List<OperItemString> oper = new List<OperItemString>();
    /// <summary>
    /// 从XML文件中读取顺序信息
    /// </summary>
    public void ReadXML(string path)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(path);

        XmlNode root = doc.SelectSingleNode("root");
        XmlNodeList list = root.ChildNodes;
        foreach (XmlNode item in list)
        {
            OperItemString o = new OperItemString();
            XmlElement element = (XmlElement)item;
            o.go = element.GetAttribute("gameobject").ToString();
            o.type = element.GetAttribute("type").ToString();
            o.OperParam = element.GetAttribute("param").ToString();
            o.msg = element.GetAttribute("msg").ToString();
            o.errorMsg = element.GetAttribute("errorMsg").ToString();
            oper.Add(o);
        }
        Debug.Log(oper.Count);
    }

    /// <summary>
    /// 将操作顺序信息写入到XML文件
    /// </summary>
    public void WriteXML()
    {

    }

    public string fileName = "Config.xml";

    void FindXMLFile(string name)
    {
        if (!File.Exists(name))
        {
            File.Create(name);
        }
    }
}
