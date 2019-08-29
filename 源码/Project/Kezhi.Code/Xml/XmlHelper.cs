/*******************************************************************************
 * Copyright © 2016 科致电气
 * Author: Kezhi
 * Description: 上海科致MES系统
 * Website：www.kezhi-electric.com
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Kezhi.Code
{
    /// <summary>
    /// XML 文件操作类
    /// </summary>
    public class XmlHelper
    {
        string strFile = string.Empty;
        XmlDocument xmlDOC = null;
        const string strNotFound = "Not Found";

        public XmlHelper(string xmlFile)
        {
            strFile = xmlFile;
            xmlDOC = new XmlDocument();
            xmlDOC.Load(xmlFile);
        }
        /// <summary>
        /// 读取节点值
        /// </summary>
        /// <param name="key"节点名称></param>
        /// <returns></returns>
        public string ReadNoteValue(string key)
        {
            XmlNodeList nodeList = xmlDOC.GetElementsByTagName(key);
            if (nodeList.Count == 0)
            {
                return strNotFound;
            }
            else
            {
                XmlNode mNode = nodeList[0];
                return mNode.InnerText;
            }
        }
        public void AddNoteValue(string nkey, string mValue)
        {
            if (ReadNoteValue(nkey) == strNotFound)
            {
                XmlNodeList nodeList = xmlDOC.GetElementsByTagName("SQLConfig");
                XmlNode mNode = nodeList[0];
                XmlElement nElement = xmlDOC.CreateElement(nkey);
                nElement.InnerText = mValue;
                mNode.AppendChild(nElement);
                WriteXml();
            }

        }
        public void UpdateNoteValue(string nkey, string mValue)
        {
            if (ReadNoteValue(nkey) != strNotFound)
            {
                XmlNodeList nodeList = xmlDOC.GetElementsByTagName(nkey);
                XmlNode mNode = nodeList[0];
                mNode.InnerText = mValue;
                WriteXml();
            }

        }
        public void DeleteNoteValue(string nKey)
        {
            if (ReadNoteValue(nKey) != strNotFound)
            {
                XmlNodeList mParentnodeList = xmlDOC.GetElementsByTagName("SQLConfig");
                XmlNode mParentNode = mParentnodeList[0];
                XmlNodeList nodeList = xmlDOC.GetElementsByTagName(nKey);
                XmlNode node = nodeList[0];
                mParentNode.RemoveChild(node);
                WriteXml();
            }

        }
        /// <summary>
        /// 写内容到Xml
        /// </summary>
        private void WriteXml()
        {
            XmlTextWriter xw = new XmlTextWriter(new System.IO.StreamWriter(strFile));
            xw.Formatting = Formatting.Indented;
            xmlDOC.WriteTo(xw);
            xw.Close();
        }
    }
}
