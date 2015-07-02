/*
 * Author:      AndySun
 * Date:        2015-07-02
 * Description: The DataStruct for xml file & Unity Scripts Communication with each other.
 */
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
/// <summary>
/// 操作的类型
/// </summary>
public enum EOperType
{
    Trans,
    Rot,
    SetParent
}

public struct TransfromStruct
{
    float x, y, z;
}

/// <summary>
/// 操作对象
/// </summary>
[System.Serializable]
public class OperItem
{
    /// <summary>
    /// 操作名称
    /// </summary>
    public string name = "";
    /// <summary>
    /// 当前操作的物体
    /// </summary>
    public Transform go;
    /// <summary>
    /// 操作的类型
    /// </summary>
    public EOperType type;
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


/// <summary>
/// 下一步操作对象
/// </summary>
public class OperItemString
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

public class DataSource : MonoBehaviour
{
    /// <summary>
    /// 操作列表
    /// </summary>
    public List<OperItemString> oper = new List<OperItemString>();

    void Start()
    {
       // ReadXML("C:\\Config2.xml");
        Debug.Log(Application.dataPath);
        ReadXML(Application.dataPath + "\\Config.xml");
    }
   
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
}
