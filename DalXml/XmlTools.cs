//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;
//using System.Xml.Serialization;

//namespace Dal
//{
//    public static class XmlTools
//    {
//        static XmlTools() { }

//        #region XML element
//        public static void CreateXmlFromList<T>(List<T> list, string path, string name)
//        {
//            XElement root = new XElement(name);
//            foreach (var item in list)
//            {
//                root.Add(CreateElement(item));
//            }
//            root.Save(path);
//        }

//        public static XElement CreateElement<T>(T obj)
//        {
//            var res = new XElement(typeof(T).Name);
//            foreach (PropertyInfo prop in obj.GetType().GetProperties())
//            {
//                res.Add(new XElement(prop.Name, prop.GetValue(obj)));
//            }
//            return res;
//        }

//        public static void UpdateElement<T>(this XElement element, T obj)
//        {
//            foreach (var elem in element.Elements())
//            {
//                foreach (PropertyInfo prop in obj.GetType().GetProperties())
//                {
//                    if (prop.Name == elem.Name)
//                        elem.SetValue(prop.GetValue(obj));
//                }
//            }
//        }
//        #endregion

//        #region Save and Load With XElement
//        public static void SaveListToXMLElement(XElement rootElem, string filePath)
//        {
//            try
//            {
//                rootElem.Save(filePath);
//            }
//            catch (DO.XmlFileCreateException)
//            {
//                throw new DO.XmlFileCreateException(filePath);
//            }
//        }

//        public static XElement LoadListFromXMLElement(string filePath)
//        {
//            try
//            {
//                if (File.Exists(filePath))
//                {
//                    return XElement.Load(filePath);
//                }
//                else
//                {
//                    XElement rootElem = new XElement(filePath);
//                    rootElem.Save(filePath);
//                    return rootElem;
//                }
//            }
//            catch (DO.XmlFileLoadException)
//            {
//                throw new DO.XmlFileLoadException(filePath);
//            }
//        }
//        #endregion

//        #region Save and Load With XMLSerializer
//        public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
//        {
//            try
//            {
//                FileStream file = new FileStream(filePath, FileMode.Create);
//                XmlSerializer x = new XmlSerializer(list.GetType());
//                x.Serialize(file, list);
//                file.Close();
//            }
//            catch (DO.XmlFileCreateException)
//            {
//                throw new DO.XmlFileCreateException(filePath);
//            }
//        }

//        public static List<T> LoadListFromXMLSerializer<T>(string filePath)
//        {
//            try
//            {
//                if (File.Exists(filePath))
//                {
//                    List<T> list;
//                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
//                    FileStream file = new FileStream(filePath, FileMode.Open);
//                    list = (List<T>)x.Deserialize(file);
//                    file.Close();
//                    return list;
//                }
//                else
//                    return new List<T>();
//            }
//            catch (DO.XmlFileLoadException)
//            {
//                throw new DO.XmlFileLoadException(filePath);
//            }
//        }
//        #endregion
//    }
//}
