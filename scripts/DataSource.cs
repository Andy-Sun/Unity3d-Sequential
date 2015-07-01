using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

public class DataSource : MonoBehaviour
{
    /// <summary>
    /// 操作列表
    /// </summary>
    public List<OperItem> oper = new List<OperItem>();

    void Start()
    {
       // ReadXML("C:\\Config2.xml");
        Debug.Log(Application.dataPath);
        ReadXML(Application.dataPath + "\\Config.xml");
    }
    /// <summary>
    /// 操作的类型
    /// </summary>
    public enum EOperType
    {
        Trans,
        Rot,
        SetParent
    }
    /// <summary>
    /// 下一步操作对象
    /// </summary>
    public class OperItem
    {
        /// <summary>
        /// 当前操作的物体
        /// </summary>
        public string go;
        /// <summary>
        /// 操作的类型
        /// </summary>
        public string type = "";
        /// <summary>
        /// 操作参数，对应于操作类型
        /// </summary>
        public string OperParam = "";
        /// <summary>
        /// 操作时的步骤提示
        /// </summary>
        public string msg = "";
        /// <summary>
        /// 操作错误信息
        /// </summary>
        public string errorMsg = "";
    }

    public void ReadXML(string path)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(path);

        XmlNode root = doc.SelectSingleNode("root");
        XmlNodeList list = root.ChildNodes;
        foreach (XmlNode item in list)
        {
            OperItem o = new OperItem();
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
}
