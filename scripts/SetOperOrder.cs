/*
 * Author:      AndySun
 * Date:        2015-07-06
 * Description: Set The Operation Order
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SetOperOrder : MonoBehaviour
{
    public string fileName = "OrderConfig.xml";
    public List<OperItem> OperOrder = new List<OperItem>();
    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "WirteInto"))
        {
            XMLRW.WriteXML(OperOrder, Application.dataPath + "\\" + fileName);
        }
    }

    void OnEnable()
    {
        OperOrder = XMLRW.ReadXML(Application.dataPath + "\\" + fileName);
    }
}
