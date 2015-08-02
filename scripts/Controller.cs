/*
 * Author:          AndySun
 * Date:            2015-07-20
 * Description:     控制XML存储操作的顺序执行。
 * ChangeLog:
 *      2015-08-02
 *          Added:
 *              1.添加默认不激活的物体列表，以防查找物体时无法查询到被禁用的物体
 *      2015-07-27
 *          Improvement:
 *              1.设置要加载的xml文件名称，使不同的操作可以操作不同的xml文件
 *          Added:
 *              1.添加空闲等待时间
 *      2015-07-23
 *          Added:
 *              1.添加物体对象的激活与隐藏控制
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
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public string operateXML = "OrderConfig";
    /// <summary>
    /// 默认不激活的物体列表
    /// </summary>
    public List<Transform> defaultUnActive;
    /// <summary>
    /// 所有操作的列表
    /// </summary>
    List<OperItem> list = new List<OperItem>();
    static OperItem currentItem;//当前操作项
    static int id = -1;//操作项编号
    static bool b_pause = true;//暂停操作

    List<OperItem> groupList = new List<OperItem>();

    void Start()
    {
        foreach (Transform item in defaultUnActive)
        {
            item.gameObject.SetActive(false);
        }
        list = XMLRW.ReadXML(operateXML, transform);
        NextStep();
        orderPanel.SetActive(false);
    }

    void Update()
    {
        SetOrderContent();
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
    /// 继续执行下一步操作
    /// </summary>
    public static void Continue()
    {
        b_pause = false;
    }
    /// <summary>
    /// 获取下一次操作的物体对象
    /// </summary>
    /// <returns></returns>
    public static Transform GetNextTransform()
    {
        return currentItem.trans;
    }
    /// <summary>
    /// 单项执行
    /// </summary>
    void StepInto()
    {
        Debug.Log("单项执行物体名称：" + currentItem.transName);

        switch (currentItem.type)
        {
            case EOperType.SetParent:
                currentItem.trans.SetParent(currentItem.parent);
                NextStep();
                break;
            case EOperType.SetActive:
                if (!currentItem.trans)
                {
                    foreach (Transform item in defaultUnActive)
                    {
                        if (currentItem.transName == item.name)
                        {
                            currentItem.trans = item;
                            currentItem.originPos = item.position;
                            currentItem.originRot = item.localEulerAngles;
                            break;
                        }
                    }
                }
                currentItem.trans.gameObject.SetActive(currentItem.isActive);
                NextStep();
                break;
            case EOperType.SetTransform:
                if (Vector3.Distance(currentItem.trans.position, currentItem.transTarget.position) > currentItem.precision || Vector3.Distance(currentItem.trans.localEulerAngles, currentItem.transTarget.localEulerAngles) > currentItem.precision)
                {
                    currentItem.trans.position = Vector3.Lerp(currentItem.trans.position, currentItem.transTarget.position, currentItem.speed);
                    currentItem.trans.localEulerAngles = Vector3.Lerp(currentItem.trans.localEulerAngles, currentItem.transTarget.localEulerAngles, currentItem.speed);
                }
                else
                    NextStep();
                break;
            case EOperType.WaitTime:
                if (!isWaiting)
                    StartCoroutine(WaitTime(currentItem.time));
                break;
        }
    }

    bool isWaiting = false;
    IEnumerator WaitTime(float time)
    {
        isWaiting = true;
        yield return new WaitForSeconds(time);
        NextStep();
        isWaiting = false;
    }
    /// <summary>
    /// 组合执行
    /// </summary>
    void Group()
    {
        for (int i = 0; i < groupList.Count; i++)
        {
            OperItem item = groupList[i];
            currentItem = item;
            Debug.Log("组合运动物体名称：" + item.transName);
            switch (item.type)
            {
                case EOperType.SetParent:
                    item.trans.SetParent(item.parent);
                    groupList.Remove(item);
                    break;
                case EOperType.SetActive:
                    item.trans.gameObject.SetActive(item.isActive);
                    groupList.Remove(item);
                    break;
                case EOperType.SetTransform:
                    if (Vector3.Distance(item.trans.position, item.transTarget.position) > item.precision)
                    {
                        item.trans.position = Vector3.Lerp(item.trans.position, item.transTarget.position, item.speed);
                        item.trans.localEulerAngles = Vector3.Lerp(item.trans.localEulerAngles, item.transTarget.localEulerAngles, item.speed);
                    }
                    else
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
        SetOrderContent();
        if (currentItem != null && currentItem.isFinishPause)
        {
            b_pause = true;
        }
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
            Debug.Log("当前运动对象编号： " + list.Count + "--" + id + "--" + list[id].msg);

            currentItem = list[id];
        }
    }

    string content = "";
    public string lastMsg = "";
    public GameObject orderPanel;
    public Text _order;
    /// <summary>
    /// 显示操作序列
    /// </summary>
    void SetOrderContent()
    {
        content = "";
        foreach (OperItem item in list)
        {
            //if (item.msg == lastMsg)
            //    continue;

            if (item == currentItem)
                content += "<color=red>" + item.msg + "</color>\n";
            else
                content += item.msg + "\n";
            lastMsg = item.msg;
        }
        //  Debug.Log(content);
        _order.text = content;
    }

    public void ShowOrder()
    {
        if (orderPanel.activeSelf)
            orderPanel.SetActive(false);
        else
        {
            SetOrderContent();
            orderPanel.SetActive(true);
        }
    }
}
