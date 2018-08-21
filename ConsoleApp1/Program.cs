using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ConsoleApp1
{

    public class Result<T>
    {

        #region Basic
        public Type FatherNode { get; set; }
        public T Type { get; }
        private List<T> Add { get; set; }
        private List<T> Update { get; set; }
        private List<T> Delete { get; set; }
        private List<T> NewList
        {
            get
            {
                return Add.Union(Update).ToList();
            }
        }

        #endregion

        #region Method
        public Result<T> Compare(List<T> Source, List<T> Data, bool? deep = false)
        {
            var result = new Result<T>
            {
                Add = Data.Except(Source).ToList(),
                Update = Source.Intersect(Data).ToList(),
                Delete = Source.Except(Data).ToList()
            };

            return result;

        }

        #endregion

        
    }



    public class Tree
    {
        private Type TargetClass { get; set; }
        private List<Type> Property
        {
            get
            {
                return TargetClass.GetProperties(
                                         BindingFlags.DeclaredOnly |
                                         BindingFlags.Public |
                                         BindingFlags.Instance).ToList().Select(c => c.PropertyType).ToList();
            }
        }
        private List<Type> CustomProperty
        {
            get
            {
                var result = TargetClass.GetProperties(
                                         BindingFlags.DeclaredOnly |
                                         BindingFlags.Public |
                                         BindingFlags.Instance)
                                         .Select(c => c.PropertyType)
                                         .Where(c => !c.FullName.Contains("System"));

                return result.ToList();
            }
        }
        //private T GetPropValue(object src, string propName)
        //{
        //return (T) Convert.ChangeType(readData, typeof(T));
        //    return src.GetType().GetProperty(propName).GetValue(src, null);
        //}
        private Dictionary<Type, object> Book { get; set; } = new Dictionary<Type, object>();
        public Dictionary<Type, object> Get(object Node)
        {
            TargetClass = Node.GetType();
            //加入
            Book.Add(TargetClass, Node);
            //尋找
            if (CustomProperty.Count() > 0)
            {
                //遍尋子屬性
                foreach (var item in CustomProperty)
                {
                    var O = Node.GetType().GetProperty(item.Name);
                    var OV = O.GetValue(Node, null);
                    //var OT = O.GetType();
                    ////(O.GetType())Convert.ChangeType(O, O.GetType());
                    //var R = Convert.ChangeType(O, OT);
                    //var s = Tree < OT >
                    Get(OV);
                }

            }

            return Book;
        }


    }



    class Program
    {




        static void Main(string[] args)
        {

            bool escape = false;
            while (escape == false)
            {
                Console.WriteLine("=======Select Mode=======");
                Console.WriteLine("1 ) Keyin");
                //Console.WriteLine("5 ) Quert system class in T");
                //Console.WriteLine("6 ) Quert custom class in T");
                Console.WriteLine("7 ) Lab");
                Console.WriteLine("=======================");
                switch (Console.ReadKey(true).Key)
                {
                    default:
                        escape = true;
                        break;
                }
            }

        }
    }
}
