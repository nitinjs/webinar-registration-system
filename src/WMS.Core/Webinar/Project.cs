using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Authorization.Users;

namespace WMS
{
    public class Project : FullAuditedEntity<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [ForeignKey("CreatorUserId")]
        public User UserDetails { get; set; }
    }
}
