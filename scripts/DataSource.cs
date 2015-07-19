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

/// <summary>
/// 操作对象
/// </summary>
[System.Serializable]
public class OperItem
{
    /// <summary>
    /// 判断该项在面板中是否展开
    /// </summary>
    public bool bFold = false;
    /// <summary>
    /// 操作名称
    /// </summary>
    public string name = "";
    /// <summary>
    /// 当前操作的物体
    /// </summary>
    public GameObject go;
    /// <summary>
    /// 操作的类型
    /// </summary>
    public EOperType type;
    /// <summary>
    /// 操作数值
    /// </summary>
    public Vector3 param;
    /// <summary>
    /// 操作参数，父物体
    /// </summary>
    public Transform parent;
    /// <summary>
    /// 操作时的步骤提示
    /// </summary>
    public string msg = "";
    /// <summary>
    /// 操作错误信息
    /// </summary>
    public string errorMsg = "";
    /// <summary>
    /// 操作分组，组内动作可以同时执行，只有所有动作都完成后才可继续执行
    /// </summary>
    public bool group = false;
    public string groupID;
}
