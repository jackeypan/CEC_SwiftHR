using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CEC_SwiftHR.ViewModel
{
    public class EmployeeViewModel
    {
        
        public System.Guid EmployeeId { get; set; }
        [DisplayName("姓名")]
        public string EmployeeName { get; set; }
        [DisplayName("英文姓名")]
        public string EmployeeNameEn { get; set; }
        [DisplayName("生日")]
        public Nullable<System.DateTime> BirthDate { get; set; }
        [DisplayName("身分證字號")]
        public string IdCardNum { get; set; }
        [DisplayName("性別")]
        public Gender Gender { get; set; }
        [DisplayName("血型")]
        public string BloodType { get; set; }
        [DisplayName("手機號碼")]
        public string MobilePhone { get; set; }
        [DisplayName("電子郵件")]
        public string Email { get; set; }
        [DisplayName("戶籍地址")]
        public Nullable<System.Guid> PermanentAddressId { get; set; }
        [DisplayName("居住地址")]
        public Nullable<System.Guid> ResidentialAddressId { get; set; }
        [DisplayName("戶籍電話")]
        public string PermanentTel { get; set; }
        [DisplayName("居住地電話")]
        public string ResidentialTel { get; set; }
        [DisplayName("照片")]
        public byte[] Photo { get; set; }
        [DisplayName("可上班日")]
        public Nullable<System.DateTime> OnBoardDate { get; set; }
        [DisplayName("員工編號")]
        public string EmpId { get; set; }
        [DisplayName("是否結婚")]
        public IsMarried IsMarried { get; set; }
        [DisplayName("建立日期")]
        public Nullable<System.DateTime> CreateOn { get; set; }
        [DisplayName("是否有小孩")]
        public HasChild HasChild { get; set; }
        [DisplayName("幾名小孩?")]
        public Nullable<int> NumberOfChild { get; set; }
        [DisplayName("修改日期")]
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        [DisplayName("狀態")]
        public Nullable<System.Guid> EmployeeStatusesId { get; set; }
        [DisplayName("是否領有殘障手冊")]
        public IsDisability IsDisability { get; set; }
        [DisplayName("是否為原住民身分")]
        public IsAboriginal IsAboriginal { get; set; }
    }

    public enum Gender
    {
        男=1,女=0
    }
    public enum IsMarried
    {
        是=1,否=0
    }
    public enum HasChild
    {
        是 = 1, 否 = 0
    }
    public enum IsDisability
    {
        是 = 1, 否 = 0
    }
    public enum IsAboriginal
    {
        是 = 1, 否 = 0
    }

}