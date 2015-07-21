/*
 * Author:          AndySun
 * Date:            2015-07-20
 * Description:     控制XML存储操作的顺序执行。
 * ChangeLog:
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

    void Start()
    {
        list = XMLRW.ReadXML("OrderConfig.xml");
        NextStep();
    }

    void Update()
    {
        if (id < list.Count)
        {
            Debug.Log(currentItem.trans);
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
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// 执行下一步操作
    /// </summary>
    void NextStep()
    {
        ++id;
        if (id != list.Count)
        {
            currentItem = list[id];
            //确定要运动的物体Tranform对象
            foreach (Transform item in transform.GetComponentsInChildren<Transform>())
            {
                if (currentItem.transName == item.name)
                {
                    currentItem.trans = item;
                    break;
                }
            }
        }
    }

}
