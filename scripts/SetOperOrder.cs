/*
 * Author:      AndySun
 * Date:        2015-07-06
 * Description: Set The Operation Order
 * ChangeLog:
 *      2015-07-08:
 *          Added:
 *              1.区分读取XML文件和写入XML文件的文件名
 *              2.动态调整xml文件列表中的操作顺序
 *              3.添加或删除xml文件中的操作项
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[ExecuteInEditMode]
public class SetOperOrder : MonoBehaviour
{
    public string fileName = "OrderConfig.xml";
    public string saveName = "OrderConfig.xml";
    public List<OperItem> OperOrder = new List<OperItem>();

    public void MoveNodeUp(int nNode)
    {
        if (OperOrder != null)
        {
            if (nNode > 0 && nNode < OperOrder.Count)
            {
                OperItem old = OperOrder[nNode];
                OperOrder[nNode] = OperOrder[nNode - 1];
                OperOrder[nNode - 1] = old;
            }
        }
    }

    public void MoveNodeDown(int nNode)
    {
        if (OperOrder != null)
        {
            if (nNode >= 0 && nNode < OperOrder.Count - 1)
            {
                OperItem old = OperOrder[nNode];
                OperOrder[nNode] = OperOrder[nNode + 1];
                OperOrder[nNode + 1] = old;
            }
        }
    }

    public void CreateNewNode(int nNode)
    {
        if (OperOrder == null)
        {
            OperOrder = new List<OperItem>();
        }
        OperItem item = new OperItem();
        item = OperOrder[nNode];
        OperOrder.Insert(nNode + 1, item);
    }

    public void RemoveNode(int nNode)
    {
        if (OperOrder == null)
        {
            return;
        }

        OperOrder.RemoveAt(nNode);
    }
}
