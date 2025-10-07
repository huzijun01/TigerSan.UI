using System.Windows;
using TigerSan.UI.Controls;

namespace Test.WPF.Models
{
    [Table(Name = "员工信息")]
    public class EmployeeInfo
    {
        [TableHeader(
            Title = "ID",
            IsReadOnly = true,
            IsAllowResize = false,
            TextAlignment = TextAlignment.Center)]
        public int Id { get; set; }

        [TableHeader(Title = "姓名", TextAlignment = TextAlignment.Center)]
        public string Name { get; set; } = string.Empty;

        [TableHeader(Title = "年龄", TextAlignment = TextAlignment.Center)]
        public int Age { get; set; }

        [TableHeader(Title = "性别", TextAlignment = TextAlignment.Center)]
        public bool Gender { get; set; }

        [TableHeader(Title = "薪资", TextAlignment = TextAlignment.Center)]
        public double Salary { get; set; }

        [TableHeader(Title = "加入日期", TextAlignment = TextAlignment.Center)]
        public DateTime JoinDate { get; set; } = DateTime.Now;
    }
}
