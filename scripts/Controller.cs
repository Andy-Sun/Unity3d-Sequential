/*
 * Author:          AndySun
 * Date:            2015-07-20
 * Description:     控制XML存储操作的顺序执行。
 * ChangeLog:
 *      2015-07-22
 *          Added:
 *              1.添加控制逻辑的暂停操作
 *              2.添加多项操作整合执行
 *      2015-07-21
 *          Added:
 *              1.由于同一Scene中可能会包含多个同名且同tag的物体，所以将该脚本附于最顶层物体上，要操作的物体都在该物体下。
 *              2.具体的物体确定条件由FindTransform确定，可根据不同需求重新定义该方法的实现。
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour
{
    /// <summary>
    /// 所有操作的列表
    /// </summary>
    List<OperItem> list = new List<OperItem>();
    static OperItem currentItem;//当前操作项
    static int id = -1;//操作项编号
    static bool b_pause = false;//暂停操作

    List<OperItem> groupList = new List<OperItem>();

    void Start()
    {
        list = XMLRW.ReadXML("OrderConfig.xml", transform);
        NextStep();
    }

    void Update()
    {
        if (b_pause)
            return;
        if (groupList.Count != 0)
        {
            Group();
            return;
        }
        if (id < list.Count)
        {
            StepInto();
        }
    }

    /// <summary>
    /// 单项执行
    /// </summary>
    void StepInto()
    {
        Debug.Log("单项执行物体名称：" + currentItem.transName);

        switch (currentItem.type)
        {
            case EOperType.Trans:
                if (Vector3.Distance(currentItem.trans.position, currentItem.target) > currentItem.precision)
                {
                    currentItem.trans.position = Vector3.Lerp(currentItem.trans.position, currentItem.target, currentItem.speed);
                }
                else
                    NextStep();
                break;
            case EOperType.Rot:
                if (Vector3.Distance(currentItem.trans.localEulerAngles, currentItem.target) > currentItem.precision)
                {
                    currentItem.trans.localEulerAngles = Vector3.Lerp(currentItem.trans.localEulerAngles, currentItem.target, currentItem.speed);
                }
                else
                    NextStep();
                break;
            case EOperType.SetParent:
                currentItem.trans.SetParent(currentItem.parent);
                NextStep();
                break;
        }
    }
    /// <summary>
    /// 组合执行
    /// </summary>
    void Group()
    {
        for (int i = 0; i < groupList.Count; i++)
        {
            OperItem item = groupList[i];
            Debug.Log("组合运动物体名称："+item.transName);
            switch (item.type)
            {
                case EOperType.Trans:
                    if (Vector3.Distance(item.trans.position, item.target) > item.precision)
                    {
                        item.trans.position = Vector3.Lerp(item.trans.position, item.target, item.speed);
                    }
                    else
                        groupList.Remove(item);
                    break;
                case EOperType.Rot:
                    if (Vector3.Distance(item.trans.localEulerAngles, item.target) > item.precision)
                    {
                        item.trans.localEulerAngles = Vector3.Lerp(item.trans.localEulerAngles, item.target, item.speed);
                    }
                    else
                        groupList.Remove(item);
                    break;
                case EOperType.SetParent:
                    item.trans.SetParent(item.parent);
                    groupList.Remove(item);
                    break;
            }
        }
        if (groupList.Count == 0)
        {
            NextStep();
        }
    }
    /// <summary>
    /// 执行下一步操作
    /// </summary>
    void NextStep()
    {
        if (++id < list.Count)
        {
            if (list[id].group)
            {
                string groupid = list[id].groupID;
                for (; id < list.Count; ++id)
                {
                    if (list[id].groupID == groupid)
                    {
                        groupList.Add(list[id]);
                    }
                    else
                    {
                        --id;
                        return;
                    }
                }
            }
            Debug.Log("当前运动对象编号： " + id);
            currentItem = list[id];
        }
    }

}
