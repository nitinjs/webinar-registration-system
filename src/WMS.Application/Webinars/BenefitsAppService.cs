using Abp.Application.Services;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Webinars.Dto;

namespace WMS.Webinars
{
    public interface IBenefitsAppService : IApplicationService
    {
        int Add(Benefit emp);
        [HttpPost]
        void DeleteBenefit(IdDto<int> obj);
        List<Benefit> ListAll(IdDto<int> obj);
        int Update(Benefit emp);
        Benefit GetbyID(IdDto<int> obj);
    }
    public class BenefitsAppService : ApplicationService, IBenefitsAppService
    {
        public IRepository<Benefit> benefitRepo { get; set; }
        public int Add(Benefit emp)
        {
            return benefitRepo.InsertAndGetId(emp);
        }

        public void DeleteBenefit(IdDto<int> obj)
        {
            benefitRepo.Delete(obj.Id);
        }

        public Benefit GetbyID(IdDto<int> obj)
        {
            return benefitRepo.GetAll().Where(x => x.Id == obj.Id).FirstOrDefault();
        }

        public List<Benefit> ListAll(IdDto<int> obj)
        {
            return benefitRepo.GetAll().Where(x => x.WebinarId == obj.Id).ToList();
        }

        public int Update(Benefit emp)
        {
            var b = benefitRepo.GetAll().Where(x => x.Id == emp.Id).FirstOrDefault();
            if (b != null)
            {
                b.Name = emp.Name;
                b.Description = emp.Description;
                benefitRepo.Update(b);
            }
            return emp.Id;
        }
    }
}
