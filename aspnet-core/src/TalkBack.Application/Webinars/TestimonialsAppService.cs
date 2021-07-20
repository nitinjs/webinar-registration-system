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
    public interface ITestimonialsAppService : IApplicationService
    {
        int Add(Testimonial emp);
        //[HttpPost]
        void DeleteTestimonial(IdDto<int> obj);
        List<Testimonial> ListAll(IdDto<int> obj);
        int Update(Testimonial emp);
        Testimonial GetbyID(IdDto<int> obj);
    }
    public class TestimonialsAppService : ApplicationService, ITestimonialsAppService
    {
        public IRepository<Testimonial> benefitRepo { get; set; }
        public int Add(Testimonial emp)
        {
            return benefitRepo.InsertAndGetId(emp);
        }

        public void DeleteTestimonial(IdDto<int> obj)
        {
            benefitRepo.Delete(obj.Id);
        }

        public Testimonial GetbyID(IdDto<int> obj)
        {
            return benefitRepo.GetAll().Where(x => x.Id == obj.Id).FirstOrDefault();
        }

        public List<Testimonial> ListAll(IdDto<int> obj)
        {
            return benefitRepo.GetAll().Where(x => x.WebinarId == obj.Id).ToList();
        }

        public int Update(Testimonial emp)
        {
            var b = benefitRepo.GetAll().Where(x => x.Id == emp.Id).FirstOrDefault();
            if (b != null)
            {
                b.Name = emp.Name;
                b.Negative = emp.Negative;
                b.NumberOfLikes = emp.NumberOfLikes;
                b.NumberOfReviews = emp.NumberOfReviews;
                b.NumberOfStars = emp.NumberOfStars;
                b.Positive = emp.Positive;
                b.Review = emp.Review;
                benefitRepo.Update(b);
            }
            return emp.Id;
        }
    }
}
