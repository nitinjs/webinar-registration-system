using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Authorization;
using WMS.Authorization.Roles;
using WMS.Authorization.Users;
using WMS.Users.Dto;
using WMS.Webinars.Dto;

namespace WMS.Webinars
{
    public interface IWebinarAppService : IApplicationService
    {
        MyProjectsDto GetMyProjects();
        int AddProject(ProjectDto dto);
        bool EditProject(ProjectDto dto);
        bool DeleteProject(IdDto<int> dto);
        ProjectDto GetProject(IdDto<int> dto);

        WebinarsListDto GetWebinarsByUserId(IdDto<int> dto);
        WebinarsListDto GetWebinarsByProjectId(IdDto<int> dto);
        int AddWebinar(WebinarDto dto);
        bool EditWebinar(WebinarDto dto);
        bool DeleteWebinar(IdDto<int> dto);
        WebinarDto GetWebinar(IdDto<int> dto);

        List<TemplateDto> GetAllTemplates();
        TemplateDto GetTemplate(IdDto<int> dto);
        TemplateDto GetTemplateForWebinar(IdDto<int> dto);

        Task<UserDto> GetUserDetails(EntityDto<long> IdDto);
        Task<User> Update(UserDto input);

        PaymentListDto GetPayments(EntityDto<long> IdDto);
        bool IsAdmin(EntityDto<long> IdDto);
    }

    public class WebinarAppService : ApplicationService, IWebinarAppService
    {
        public LogInManager _logInManager { get; set; }
        public UserManager userManager { get; set; }
        public IObjectMapper objectMapper { get; set; }
        public UserRegistrationManager _userRegistrationManager { get; set; }

        public IRepository<Project> ProjectsRepo { get; set; }
        public IRepository<Webinar> WebinarRepo { get; set; }
        public IRepository<WebinarDesign> DesignRepo { get; set; }
        public IRepository<WebinarPayment> PaymentRepo { get; set; }
        public IRepository<User, long> UserRepo { get; set; }
        public IRepository<UserRole, long> UserRoleRepo { get; set; }
        public IRepository<Role> RoleRepo { get; set; }
        public async Task<UserDto> GetUserDetails(EntityDto<long> IdDto)
        {
            var user = await userManager.GetUserByIdAsync(IdDto.Id);
            return objectMapper.Map<UserDto>(user);
        }
        public async Task<User> Update(UserDto input)
        {
            var user = await userManager.GetUserByIdAsync(input.Id);
            MapToEntity(input, user);
            await userManager.UpdateAsync(user);

            return user;
        }
        void MapToEntity(UserDto input, User user)
        {
            objectMapper.Map(input, user);
            user.SetNormalizedNames();
        }
        public int AddProject(ProjectDto dto)
        {
            var p = new Project();
            p.CreationTime = DateTime.Now;
            p.CreatorUserId = AbpSession.UserId;
            p.Description = dto.Description;
            p.Name = dto.Name;
            var id = ProjectsRepo.InsertAndGetId(p);
            return id;
        }

        public int AddWebinar(WebinarDto dto)
        {
            var w = new Webinar();
            w.Cost = dto.Cost;
            w.CreationTime = DateTime.Now;
            w.CreatorUserId = AbpSession.UserId;
            w.Headline = dto.Headline;
            w.JoiningDetails = dto.JoiningDetails;
            w.Name = dto.Name;
            w.ProjectId = dto.ProjectId;
            w.RegistrationEndDate = dto.RegistrationEndDate;
            w.StartDateTime = dto.StartDateTime;
            w.EndDateTime = dto.EndDateTime;
            w.SubHeadline = dto.SubHeadline;
            w.VideoURL = dto.VideoURL;
            var wid = WebinarRepo.InsertAndGetId(w);

            WebinarDesign d = new WebinarDesign();
            d.Html = dto.DesignHtml;
            d.WebinarId = wid;
            DesignRepo.Insert(d);

            return wid;
        }

        public bool DeleteProject(IdDto<int> dto)
        {
            ProjectsRepo.Delete(p => p.Id == dto.Id);
            return true;
        }

        public bool DeleteWebinar(IdDto<int> dto)
        {
            WebinarRepo.Delete(x => x.Id == dto.Id);
            return true;
        }

        public bool EditProject(ProjectDto dto)
        {
            var p = ProjectsRepo.Get(dto.Id);
            p.Description = dto.Description;
            p.Name = dto.Name;
            p.LastModifierUserId = AbpSession.UserId;
            p.LastModificationTime = DateTime.Now;
            ProjectsRepo.Update(p);
            return true;
        }

        public bool EditWebinar(WebinarDto w)
        {
            var x = WebinarRepo.Get(w.Id);
            x.Name = w.Name;
            x.Cost = w.Cost;
            x.DiscountedCost = w.DiscountedCost;
            x.Headline = w.Headline;
            x.SubHeadline = w.SubHeadline;
            x.JoiningDetails = w.JoiningDetails;
            x.RegistrationEndDate = w.RegistrationEndDate;
            x.StartDateTime = w.StartDateTime;
            x.EndDateTime = w.EndDateTime;
            x.VideoURL = w.VideoURL;
            x.LastModificationTime = DateTime.Now;
            x.LastModifierUserId = AbpSession.UserId;
            WebinarRepo.Update(x);

            var design = DesignRepo.GetAll().Where(x => x.WebinarId == w.Id).FirstOrDefault();
            if (design != null)
            {
                design.Html = w.DesignHtml;
                DesignRepo.Update(design);
            }
            else {
                WebinarDesign d = new WebinarDesign();
                d.Html = w.DesignHtml;
                d.WebinarId = w.Id;
                DesignRepo.Insert(d);
            }

            return true;
        }

        public List<TemplateDto> GetAllTemplates()
        {
            var templates = from p in DesignRepo.GetAll()
                            where p.WebinarId == null
                            select new TemplateDto() { 
                                Id = p.Id,
                                Css = p.Css,
                                Html = p.Html,
                                JavaScript = p.JavaScript,
                                Name = p.Name,
                                WebinarId = p.WebinarId
                            };
            return templates.ToList();
        }

        public TemplateDto GetTemplate(IdDto<int> dto)
        {
            var templates = from p in DesignRepo.GetAll()
                            where p.Id == dto.Id
                            select new TemplateDto()
                            {
                                Id = p.Id,
                                Css = p.Css,
                                Html = p.Html,
                                JavaScript = p.JavaScript,
                                Name = p.Name,
                                WebinarId = p.WebinarId
                            };
            return templates.FirstOrDefault();
        }

        public TemplateDto GetTemplateForWebinar(IdDto<int> dto)
        {
            var templates = from p in DesignRepo.GetAll()
                            where p.WebinarId == dto.Id
                            select new TemplateDto()
                            {
                                Id = p.Id,
                                Css = p.Css,
                                Html = p.Html,
                                JavaScript = p.JavaScript,
                                Name = p.Name,
                                WebinarId = p.WebinarId
                            };
            return templates.FirstOrDefault();
        }

        public MyProjectsDto GetMyProjects()
        {
            var uid = AbpSession.UserId.Value;
            
            var webinarCount = (from p in ProjectsRepo.GetAll()
                                join w in WebinarRepo.GetAll() on p.Id equals w.ProjectId into joinT
                                from j in joinT.DefaultIfEmpty()
                                where p.CreatorUserId == uid
                                select new{ 
                                    ProjectId = p.Id,
                                    WebinarId = j==null?new Nullable<int>(): new Nullable<int>(j.Id)
                                }).ToList().GroupBy(x => x.ProjectId, (key, g) => new
                                {
                                    ProjectId = key,
                                    Count = g.Count(x=>x.WebinarId.HasValue)
                                }).ToList();
           
            /*var webinarCount = (from w in WebinarRepo.GetAll()
                                join p in ProjectsRepo.GetAll() on w.ProjectId equals p.Id into joinT
                                from j in joinT.DefaultIfEmpty()
                                where j.CreatorUserId == uid
                                select new { 
                                    j?.Id,
                                    w.
                                }).ToList().GroupBy(x => x.Id, (key, g) => new
                                {
                                    ProjectId = key,
                                    Count = g.Count()
                                }).ToList(); */
            var result = (from p in ProjectsRepo.GetAll().Where(p=> p.CreatorUserId == uid).AsEnumerable()
                    join q in webinarCount on p.Id equals q.ProjectId
                    select new ProjectDto()
                    {
                        Id = p.Id,
                        CreationTime = p.CreationTime,
                        Description = p.Description,
                        Name = p.Name,
                        NumberOfWebinars = q.Count
                    }).ToList();
            MyProjectsDto r = new MyProjectsDto();
            r.Items = result;
            r.TotalCount = result.Count;
            return r;
        }

        public ProjectDto GetProject(IdDto<int> dto)
        {
            var p = ProjectsRepo.Get(dto.Id);
            ProjectDto obj = new ProjectDto();
            obj.CreationTime = p.CreationTime;
            obj.Description = p.Description;
            obj.Id = p.Id;
            obj.Name = p.Name;
            obj.NumberOfWebinars = 0;
            return obj;
        }

        public WebinarDto GetWebinar(IdDto<int> dto)
        {
            var w = WebinarRepo.Get(dto.Id);
            WebinarDto obj = new WebinarDto();
            obj.Cost = w.Cost;
            obj.CreationTime = w.CreationTime;
            obj.DiscountedCost = w.DiscountedCost;
            obj.EndDateTime = w.EndDateTime;
            obj.Headline = w.Headline;
            obj.Id = w.Id;
            obj.JoiningDetails = w.JoiningDetails;
            obj.Name = w.Name;
            obj.ProjectId = w.ProjectId;
            obj.RegistrationEndDate = w.RegistrationEndDate;
            obj.StartDateTime = w.StartDateTime;
            obj.SubHeadline = w.SubHeadline;
            obj.VideoURL = w.VideoURL;
            return obj;
        }

        public WebinarsListDto GetWebinarsByProjectId(IdDto<int> dto)
        {
            var webinars = from w in WebinarRepo.GetAllIncluding(x => x.ProjectDetails)
                           join u in UserRepo.GetAll() on w.CreatorUserId equals u.Id
                           where w.IsDeleted == false && w.ProjectId == dto.Id && w.ProjectDetails.IsDeleted == false
                           select new WebinarDto()
                           {
                               Id = w.Id,
                               CreationTime = w.CreationTime,
                               CreatedBy = u.FullName,
                               Name = w.Name,
                               Cost = w.Cost,
                               DiscountedCost = w.DiscountedCost,
                               EndDateTime = w.EndDateTime,
                               Headline = w.Headline,
                               JoiningDetails = w.JoiningDetails,
                               RegistrationEndDate = w.RegistrationEndDate,
                               StartDateTime = w.StartDateTime,
                               SubHeadline = w.SubHeadline,
                               VideoURL = w.VideoURL,
                               ProjectId = w.ProjectId
                           };
            
            WebinarsListDto r = new WebinarsListDto();
            r.Items = webinars.ToList();
            r.TotalCount = r.Items.Count();
            return r;
        }

        public WebinarsListDto GetWebinarsByUserId(IdDto<int> dto)
        {
            var webinars = from w in WebinarRepo.GetAllIncluding(x=>x.ProjectDetails)
                           join p in PaymentRepo.GetAll() on w.Id equals p.WebinarId
                           join creator in UserRepo.GetAll() on w.CreatorUserId equals creator.Id
                           join payer in UserRepo.GetAll() on p.UserId equals payer.Id
                           where w.IsDeleted == false && p.UserId == dto.Id && w.ProjectDetails.IsDeleted == false
                           select new WebinarDto()
                           {
                               Id = w.Id,
                               CreationTime = w.CreationTime,
                               CreatedBy = creator.FullName,
                               Name = w.Name,
                               Cost = w.Cost,
                               DiscountedCost = w.DiscountedCost,
                               EndDateTime = w.EndDateTime,
                               Headline = w.Headline,
                               JoiningDetails = w.JoiningDetails.Replace(Environment.NewLine, "<br/>"),
                               RegistrationEndDate = w.RegistrationEndDate,
                               StartDateTime = w.StartDateTime,
                               SubHeadline = w.SubHeadline,
                               VideoURL = w.VideoURL,
                               ProjectId = w.ProjectId
                           };

            WebinarsListDto r = new WebinarsListDto();
            r.Items = webinars.ToList();
            r.TotalCount = r.Items.Count();
            return r;
        }

        public bool IsAdmin(EntityDto<long> IdDto)
        {
            var userRoles = from u in UserRepo.GetAll()
                            join ur in UserRoleRepo.GetAll() on u.Id equals ur.UserId
                            join r in RoleRepo.GetAll() on ur.RoleId equals r.Id
                            where u.Id == IdDto.Id && r.Name == "Admin"
                            select r.Name;
            return userRoles.Any();
        }

        public PaymentListDto GetPayments(EntityDto<long> IdDto)
        {
            PaymentListDto o = new PaymentListDto();
            if (IsAdmin(IdDto))
            {
                //all payments
                var webinars = from w in WebinarRepo.GetAllIncluding(x=>x.ProjectDetails)
                               join p in PaymentRepo.GetAll() on w.Id equals p.WebinarId
                               join creator in UserRepo.GetAll() on w.CreatorUserId equals creator.Id
                               join payer in UserRepo.GetAll() on p.UserId equals payer.Id
                               where w.IsDeleted == false && w.ProjectDetails.IsDeleted == false
                               orderby p.FeePaidOn descending
                               select new PaymentDto()
                               {
                                   WebinarId = w.Id,
                                   Name = w.Name,
                                   Cost = w.Cost,
                                   DiscountedCost = w.DiscountedCost,
                                   Headline = w.Headline,

                                   Id = p.Id,
                                   FeePaidOn = p.FeePaidOn,
                                   IsFeePaid = p.IsFeePaid,
                                   ReferenceId = p.ReferenceId,
                                   UserId = p.UserId,
                                   PayerName = payer.FullName
                               };
                o.Items = webinars.ToList();
                o.TotalCount = webinars.Count();
            }
            else {
                //my payments
                var webinars = from w in WebinarRepo.GetAllIncluding(x => x.ProjectDetails)
                               join p in PaymentRepo.GetAll() on w.Id equals p.WebinarId
                               join creator in UserRepo.GetAll() on w.CreatorUserId equals creator.Id
                               join payer in UserRepo.GetAll() on p.UserId equals payer.Id
                               where w.IsDeleted == false && p.UserId == IdDto.Id && w.ProjectDetails.IsDeleted == false
                               orderby p.FeePaidOn descending
                               select new PaymentDto()
                               {
                                   WebinarId = w.Id,
                                   Name = w.Name,
                                   Cost = w.Cost,
                                   DiscountedCost = w.DiscountedCost,
                                   Headline = w.Headline,

                                   Id = p.Id,
                                   FeePaidOn = p.FeePaidOn,
                                   IsFeePaid = p.IsFeePaid,
                                   ReferenceId = p.ReferenceId,
                                   UserId = p.UserId,
                                   PayerName = payer.FullName
                               };
                o.Items = webinars.ToList();
                o.TotalCount = webinars.Count();
            }
            return o;
        }
    }
}
