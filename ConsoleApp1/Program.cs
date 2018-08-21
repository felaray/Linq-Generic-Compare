using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ConsoleApp1
{
    public class Test<T>
    {
        private List<Type> CustomProperty
        {
            get
            {
                var result = typeof(T).GetProperties(
                                         BindingFlags.DeclaredOnly |
                                         BindingFlags.Public |
                                         BindingFlags.Instance)
                                         .Select(c => c.PropertyType)
                                         .Where(c => !c.FullName.Contains("System"));

                return result.ToList();
            }
        }
    }

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

        #region View
        public void CompareOnConsole(List<T> Source, List<T> Data)
        {
            var result = new Result<T>().Compare(Source, Data);
            Console.WriteLine("New");
            result.Add.ForEach(c => Console.Write(c + ","));
            Console.WriteLine();
            Console.WriteLine("Update");
            result.Update.ForEach(c => Console.Write(c + ","));
            Console.WriteLine();
            Console.WriteLine("Delete");
            result.Delete.ForEach(c => Console.Write(c + ","));
            Console.WriteLine();
            Console.WriteLine("All");
            result.NewList.ForEach(c => Console.Write(c + ","));
            Console.WriteLine();
            Console.WriteLine(string.Format("Property", typeof(T)));
            //foreach (var c in new Tree<T>().Property)
            //{
            //    Console.WriteLine(c + ",");
            //}
            Console.WriteLine();
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



    //public class Lab : SampleObject
    //{
    //    private List<int> result { get; set; } = new List<int>();
    //    private List<int> Go(int count)
    //    {
    //        if (count > 0)
    //        {
    //            result.Add(count);
    //            Go(count - 1);
    //        }

    //        return result;
    //    }
    //}

    class Program
    {
        #region Sample:Default class 
        static private void Keyin()
        {

            Console.WriteLine("input A array");
            var key1 = Console.ReadLine();
            var A = key1.Split(',').ToList();
            Console.WriteLine("input B array");
            var key2 = Console.ReadLine();
            var B = key2.Split(',').ToList();

            new Result<string>().CompareOnConsole(A, B);
        }
        static private void Sample_String()
        {
            var Source = new List<string> { "1", "2", "3" };
            var Data = new List<string> { "2", "4" };
            new Result<string>().CompareOnConsole(Source, Data);
        }
        static private void Sample_Date()
        {
            var Source = new List<DateTime>
            {
                DateTime.Now,
                DateTime.UtcNow,
            };
            var Data = new List<DateTime>
            {
                DateTime.Now,
                DateTime.UtcNow.AddDays(5),
            };
            new Result<DateTime>().CompareOnConsole(Source, Data);
        }
        #endregion
        #region Sample:Custom class 
        static private void Sample_CustomClass()
        {
            var updateItem = new SampleClass
            {
                Title = "hihi"
            };

            var Source = new List<SampleClass>
            {
                //dafault
                updateItem,
                //remove
                new SampleClass{ ID=Guid.NewGuid(), Title="haha"},
            };
            var Data = new List<SampleClass>
            {
                //dafault      
                updateItem,
                //new
                new SampleClass{ID=Guid.NewGuid(), Title="gogo"},
            };
            var result = new Result<SampleClass>().Compare(Source, Data);
        }
        static private void Sample_QuerySystemClass()
        {
            //var result = new Tree().Property;
            //foreach (var item in result)
            //{
            //    Console.WriteLine(item.FullName);
            //}
        }
        static private void Sample_QueryCustomClass()
        {

            //var result = new Tree().CustomProperty;
            //foreach (var item in result)
            //{
            //    Console.WriteLine(item.FullName);
            //}
        }


        #endregion

        static private void Lab()
        {
            //var test = new Lab().Go(5);
            var data = new Bank();
            var tree = new Tree().Get(data);
            foreach (var item in tree)
            {
                Console.WriteLine(string.Format("{0},{1}", item.Key, item.Value));
            }

            Console.Write("");
        }

        static void Main(string[] args)
        {
            Lab();
            bool escape = false;
            while (escape == false)
            {
                Console.WriteLine("=======Select Mode=======");
                Console.WriteLine("1 ) Keyin");
                Console.WriteLine("2 ) Sample:String");
                Console.WriteLine("3 ) Sample:Datetime");
                Console.WriteLine("4 ) Sample:SampleClass");
                //Console.WriteLine("5 ) Quert system class in T");
                //Console.WriteLine("6 ) Quert custom class in T");
                Console.WriteLine("7 ) Lab");
                Console.WriteLine("=======================");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        Keyin();
                        break;
                    case ConsoleKey.D2:
                        Sample_String();
                        break;
                    case ConsoleKey.D3:
                        Sample_Date();
                        break;
                    case ConsoleKey.D4:
                        Sample_CustomClass();
                        break;
                    //case ConsoleKey.D5:
                    //    Sample_QuerySystemClass();
                    //    break;
                    //case ConsoleKey.D6:
                    //    Sample_QueryCustomClass();
                    //    break;
                    case ConsoleKey.D7:
                        Lab();
                        break;
                    default:
                        escape = true;
                        break;
                }
            }

        }
    }
}
