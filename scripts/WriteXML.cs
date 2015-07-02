/*
 * Author:      AndySun
 * Date:        2015-07-02
 * Description: Operate info will be store into oper array,and this info will be write into xml file.
 */
using UnityEngine;
using System.Collections;
using System.IO;

public class WriteXML : MonoBehaviour {
    public string fileName = "Config.xml";
    public OperItem[] oper;

    void FindXMLFile(string name)
    {
        if (!File.Exists(name))
        {
            File.Create(name);
        }
    }
}
