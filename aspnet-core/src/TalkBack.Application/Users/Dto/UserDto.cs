using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using TalkBack.Authorization.Users;

namespace TalkBack.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public bool IsTrial { get; set; }
        public DateTime? TrialExpiredOn { get; set; }
        public string PayPalReferenceId { get; set; }
        public bool? IsAnnualFeePaid { get; set; }
        public DateTime? AnnualFeePaidOn { get; set; }
        public DateTime? AppFeePaidOn { get; set; }

        public string DeviceId { get; set; }

        public string SecurityStamp { get; set; }

        public string CustomerId { get; set; }

        public string FullName { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime CreationTime { get; set; }

        public string[] RoleNames { get; set; }

        public string CompanyName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email2 { get; set; }
    }

    public class UserProfileDto
    {
        public int id { get; set; }
        public string CompanyName { get; set; }
        public string streetAddress { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipCode { get; set; }
        public string phoneNumber { get; set; }
        public string Email2 { get; set; }

        public string name { get; set; }
        public string surname { get; set; }
    }
}
