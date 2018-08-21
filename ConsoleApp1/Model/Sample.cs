using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Title { get; set; } = DateTime.UtcNow.ToShortDateString();
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public SampleClass sampleClass { get; set; }

    }

    #region SampleData
    public class Bank
    {
        //public string Class { get; } = "Bank";
        public string Name { get; } = "City";
        public Member Member { get; } = new Member();
    }

    public class Member
    {
        //public Bank bank { get; } = new Bank();
        //public string Class { get; } = "Bank";
        public string Name { get; } = "A";
        public Money Money { get; } = new Money();
        public Data Data { get; } = new Data();
    }

    public class Money
    {
        //public Member member { get; } = new Member();
        //public string Class { get; } = "Bank";
        public int Value { get; } = 1000;
    }

    public class Data
    {
        //public Member member { get; } = new Member();
        //public string Class { get; } = "Bank";
        public string Phone { get; } = "0912345678";
    }

    #endregion


    #endregion
}
