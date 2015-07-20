/*
 * Author:          AndySun
 * Date:            2015-07-20
 * Description:     控制XML存储操作的顺序执行。
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
    static int id = 0;//操作项编号

    void Start()
    {
        list = XMLRW.ReadXML("OrderConfig.xml");
        currentItem = list[id];
    }

    void Update()
    {
        if (id < list.Count)
        {
            switch (currentItem.type)
            {
                case EOperType.Trans:
                    if (Vector3.Distance(currentItem.trans.position, currentItem.param) > currentItem.precision)
                    {
                        currentItem.trans.position = Vector3.Lerp(currentItem.trans.position, currentItem.param, currentItem.speed);
                    }
                    else
                        NextStep();
                    break;
                case EOperType.Rot:
                    if (Vector3.Distance(currentItem.trans.localEulerAngles, currentItem.param) > currentItem.precision)
                    {
                        currentItem.trans.localEulerAngles = Vector3.Lerp(currentItem.trans.localEulerAngles, currentItem.param, currentItem.speed);
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
        }
    }
}
