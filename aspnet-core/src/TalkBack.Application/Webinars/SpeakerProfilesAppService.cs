using Abp.Application.Services;
using Abp.Domain.Repositories;
//using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkBack.Webinars.Dto;

namespace TalkBack.Webinars
{
    public interface ISpeakerProfilesAppService : IApplicationService
    {
        int Add(SpeakerPofile emp);
        //[HttpPost]
        void DeleteSpeakerProfile(IdDto<int> obj);
        List<SpeakerPofile> ListAll(IdDto<int> obj);
        int Update(SpeakerPofile emp);
        SpeakerPofile GetbyID(IdDto<int> obj);
    }
    public class SpeakerProfilesAppService : ApplicationService, ISpeakerProfilesAppService
    {
        public IRepository<SpeakerPofile> benefitRepo { get; set; }
        public int Add(SpeakerPofile emp)
        {
            return benefitRepo.InsertAndGetId(emp);
        }

        public void DeleteSpeakerProfile(IdDto<int> obj)
        {
            benefitRepo.Delete(obj.Id);
        }

        public SpeakerPofile GetbyID(IdDto<int> obj)
        {
            return benefitRepo.GetAll().Where(x => x.Id == obj.Id).FirstOrDefault();
        }

        public List<SpeakerPofile> ListAll(IdDto<int> obj)
        {
            return benefitRepo.GetAll().Where(x => x.WebinarId == obj.Id).ToList();
        }

        public int Update(SpeakerPofile emp)
        {
            var b = benefitRepo.GetAll().Where(x => x.Id == emp.Id).FirstOrDefault();
            if (b != null)
            {
                b.Name = emp.Name;
                b.Description = emp.Description;
                b.Website = emp.Website;
                b.PhotoPath = emp.PhotoPath;
                benefitRepo.Update(b);
            }
            return emp.Id;
        }
    }
}
