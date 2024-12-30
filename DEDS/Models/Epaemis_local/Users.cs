using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DEDS.Models.Epaemis_local
{
    public class Users
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Pwd { get; set; }

        [StringLength(50)]
        public string VoicePwd { get; set; }

        [StringLength(30)]
        public string Duty { get; set; }

        [StringLength(20)]
        public string City { get; set; }

        [StringLength(30)]
        public string Town { get; set; }

        [StringLength(50)]
        public string MobilePhone { get; set; }

        [StringLength(50)]
        public string HumanType { get; set; }

        [StringLength(2)]
        public string MainContacter { get; set; }

        [StringLength(2)]
        public string ReportPriority { get; set; }

        public int? DepartmentId { get; set; }

        public int? PositionId { get; set; }

        [StringLength(50)]
        public string OfficePhone { get; set; }

        [StringLength(50)]
        public string FaxNumber { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(1000)]
        public string Remark { get; set; }

        [StringLength(50)]
        public string HomeNumber { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? CityId { get; set; }

        public int? TownId { get; set; }

        public int? DutyId { get; set; }

        public DateTime? ConfirmTime { get; set; }

        public bool? isadmin { get; set; }

        public int? ContactManualDuty { get; set; }

        public int? ContactManualDepartmentId { get; set; }

        [StringLength(2)]
        public string ISEnvironmentalProtectionAdministration { get; set; }

        [StringLength(2)]
        public string ISEnvironmentalProtectionDepartment { get; set; }

        [StringLength(2)]
        public string ISBook { get; set; }
    }
}