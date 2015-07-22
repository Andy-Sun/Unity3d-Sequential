/*
 * Author:      AndySun
 * Date:        2015-07-02
 * Description: The DataStruct for xml file & Unity Scripts Communication with each other.
 * ChangeLog：
 *      2015-07-20
 *          Added:
 *              1.添加物体运动与旋转时的速度及最终的检测精度
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
    public bool bFold = true;
    /// <summary>
    /// 操作名称
    /// </summary>
    public string name = "";
    /// <summary>
    /// 当前操作的物体
    /// </summary>
    public Transform trans;
    /// <summary>
    /// 操作物体的名称
    /// </summary>
    public string transName;
    /// <summary>
    /// 当前操作物体的tag
    /// </summary>
    public string tag;
    /// <summary>
    /// 当前物体的唯一标识
    /// </summary>
    public int instanceID;
    /// <summary>
    /// 操作的类型
    /// </summary>
    public EOperType type;
    /// <summary>
    /// 操作空间坐标系
    /// </summary>
    public Space space;
    /// <summary>
    /// 操作数值
    /// </summary>
    public Vector3 target;
    /// <summary>
    /// 操作速度
    /// </summary>
    public float speed = 0.01f;
    /// <summary>
    /// 操作精度
    /// </summary>
    public float precision = 0.01f;
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
