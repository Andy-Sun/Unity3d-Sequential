/*
 * Author:      AndySun
 * Date:        2015-07-02
 * Description: The DataStruct for xml file & Unity Scripts Communication with each other.
 * ChangeLog：
 *      2015-07-28
 *          Added:
 *              1.保持物体的初始位置及角度信息，供操作后返回最初状态
 *      2015-07-27
 *          Added:
 *              1.添加空闲等待时间
 *      2015-07-25
 *          Added:
 *              1.添加拷贝构造函数
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
    SetActive,
    WaitTime
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
    /// 物体原始位置
    /// </summary>
    public Vector3 originPos;
    /// <summary>
    /// 物体原始旋转角度
    /// </summary>
    public Vector3 originRot;
    /// <summary>
    /// 操作物体的名称
    /// </summary>
    public string transName;
    /// <summary>
    /// 当前操作物体的tag
    /// </summary>
    public string tag;
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
    /// 要等待的时间
    /// </summary>
    public float time = 0.0f;
    /// <summary>
    /// 操作参数，父物体
    /// </summary>
    public Transform parent;
    /// <summary>
    /// 当前物体是否激活
    /// </summary>
    public bool isActive;
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
    /// <summary>
    /// 当前项执行完后，是否等待下一项的执行命令
    /// </summary>
    public bool isFinishPause = false;

    public OperItem()
    { }
    public OperItem(OperItem item)
    {
        this.name = item.name;
        this.trans = item.trans;
        this.transName = item.transName;
        this.tag = item.tag;
        this.type = item.type;
        this.transTarget = item.transTarget;
        this.speed = item.speed;
        this.precision = item.precision;
        this.parent = item.parent;
        this.isActive = item.isActive;
        this.msg = item.msg;
        this.errorMsg = item.errorMsg;
        this.group = item.group;
        this.groupID = item.groupID;
        this.isFinishPause = item.isFinishPause;
    }
}
