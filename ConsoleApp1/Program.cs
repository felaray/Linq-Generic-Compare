using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ConsoleApp1
{
    public interface ITree<T>
    {
        string Test(T obj);
    }
    public class Tree<T>:ITree<T>
    {

        public List<T> Father { get; set; }
        public List<T> Child { get; set; }
        public List<T> Node { get; set; }

        public string Test(T obj)
        {
            throw new NotImplementedException();
        }
    }
    public class Result<T>
    {
        #region Basic
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

        public Tree<T> Tree { get; set; }

        private IEnumerable<Type> Property
        {
            get
            {
                return typeof(T).GetProperties(
                                         BindingFlags.DeclaredOnly |
                                         BindingFlags.Public |
                                         BindingFlags.Instance).ToList().Select(c => c.PropertyType);
            }
        }
        private IEnumerable<Type> CustomProperty
        {
            get
            {
                return typeof(T).GetProperties(
                                             BindingFlags.DeclaredOnly |
                                             BindingFlags.Public |
                                             BindingFlags.Instance)
                                             .Select(c => c.PropertyType)
                                             .Where(c => !c.FullName.Contains("System"));

            }
        }
        #endregion

        #region Method
        public Result<T> Compare(List<T> Source, List<T> Data, bool? deep = false)
        {
            if (deep == false)
            {
                var result = new Result<T>
                {
                    Add = Data.Except(Source).ToList(),
                    Update = Source.Intersect(Data).ToList(),
                    Delete = Source.Except(Data).ToList()
                };
                return result;
            }
            else
            {
                foreach (var Item in GetProperty(true))
                {

                }
                return null;
            }
        }

        public IEnumerable<Type> GetProperty(bool CustomOnly = false)
        {
            if (CustomOnly == false)
                return Property;
            else
                return CustomProperty;
        }

        #endregion
    }

    public class C<T> : Result<T>
    {
        //public Result<T> result { get; set; }
        private int count = 0;
        //public Dictionary<Type, T> Get(T Node)
        //{
        //    TargetClass = Node.GetType();
        //    //加入
        //    Book.Add(TargetClass, Node);
        //    //尋找
        //    if (CustomProperty.Count() > 0)
        //    {
        //        //遍尋子屬性
        //        foreach (var item in CustomProperty)
        //        {
        //            var O = Node.GetType().GetProperty(item.Name);
        //            var OV = O.GetValue(Node, null);
        //            //var OT = O.GetType();
        //            ////(O.GetType())Convert.ChangeType(O, O.GetType());
        //            //var R = Convert.ChangeType(O, OT);
        //            //var s = Tree < OT >
        //            Get(OV);
        //        }

        //    }

        //    return Book;
        //}



        public void Get(Result<T> Data)
        {

            var PropList = Data.GetProperty(true);
            if (PropList.Count() > 0)
            {

                foreach (var Item in PropList)
                {
                    var O = Data.GetType().GetProperty(Item.Name).GetValue(Data, null);
                    var OT = Convert.ChangeType(O, O.GetType());
                    //var R = Convert.ChangeType(O, OT);
                    //var s = Tree < OT >
                    //Get(OV);
                }


            }

        }
    }

    class Program
    {

        static void Lab()
        {
            var r = new C<Bank>();
            new C<Bank>().Get(r);


        }
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
