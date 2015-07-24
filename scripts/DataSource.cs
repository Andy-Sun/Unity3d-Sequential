/*
 * Author:      AndySun
 * Date:        2015-07-02
 * Description: The DataStruct for xml file & Unity Scripts Communication with each other.
 * ChangeLog：
 *      2015-07-23
 *          Added:
 *              1.添加物体对象的激活与隐藏控制
 *      2015-07-20
 *          Added:
 *              1.添加物体运动与旋转时的速度及最终的检测精度
 */
using UnityEngine;
/// <summary>
/// 操作的类型
/// </summary>
public enum EOperType
{
    SetTransform,
    SetParent,
    SetActive
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
    /// 操作目标状态
    /// </summary>
    public Transform transTarget;
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
    /// 当前物体是否激活
    /// </summary>
    public bool active;
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
