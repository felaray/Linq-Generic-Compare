﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ConsoleApp1
{
    #region Class
    public class SampleClass
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }

    public class SampleObject
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public SampleClass sampleClass { get; set; }

    }

    #endregion

    public class QueryClassProperty<T>
    {
        public int GetSystemPropertyCount
        {
            get
            {
                return GetSystemProperty.Count();
            }
        }
        public int GetCustomPropertyCount
        {
            get
            {
                return GetCustomProperty.Count();
            }
        }
        public List<Type> GetSystemProperty
        {
            get
            {
                return typeof(T).GetProperties(
                                         BindingFlags.DeclaredOnly |
                                         BindingFlags.Public |
                                         BindingFlags.Instance).ToList().Select(c => c.PropertyType).ToList();
            }
        }

        public List<Type> GetCustomProperty
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

        public List<T> Add { get; set; }
        public List<T> Update { get; set; }
        public List<T> Delete { get; set; }
        public List<T> NewList
        {
            get
            {
                return Add.Union(Update).ToList();
            }
        }

        public Result<T> Compare(List<T> Source, List<T> Data)
        {

            return new Result<T>
            {
                Add = Data.Except(Source).ToList(),
                Update = Source.Intersect(Data).ToList(),
                Delete = Source.Except(Data).ToList()
            };
        }
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
            foreach (var c in new QueryClassProperty<T>().GetSystemProperty)
            {
                Console.WriteLine(c + ",");
            }
            Console.WriteLine();
        }
    }


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
            var result = new QueryClassProperty<SampleObject>().GetSystemProperty;
            foreach (var item in result)
            {
                Console.WriteLine(item.FullName);
            }
        }
        static private void Sample_QueryCustomClass()
        {

            var result = new QueryClassProperty<SampleObject>().GetCustomProperty;
            foreach (var item in result)
            {
                Console.WriteLine(item.FullName);
            }
        }
        #endregion


        static void Main(string[] args)
        {

            bool escape = false;
            while (escape == false)
            {
                Console.WriteLine("=======Select Mode=======");
                Console.WriteLine("1 ) keyin");
                Console.WriteLine("2 ) Sample:String");
                Console.WriteLine("3 ) Sample:Datetime");
                Console.WriteLine("4 ) Sample:SampleClass");
                Console.WriteLine("5 ) Quert system class in T");
                Console.WriteLine("6 ) Quert custom class in T");
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
                    case ConsoleKey.D5:
                        Sample_QuerySystemClass();
                        break;
                    case ConsoleKey.D6:
                        Sample_QueryCustomClass();
                        break;
                    default:
                        escape = true;
                        break;
                }
            }

        }
    }
}
