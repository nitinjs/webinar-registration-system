using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkBack.Controllers;
using TalkBack.Webinars;
using TalkBack.Webinars.Dto;

namespace WMS.Web.Controllers
{
    public class ProjectsController : TalkBackControllerBase
    {
        public IWebinarAppService webinarAppService { get; set; }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(ProjectDto model)
        {
            try
            {
                var id = webinarAppService.AddProject(model);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }

        public IActionResult Edit(string id)
        {
            var model = webinarAppService.GetProject(new IdDto<int>(Convert.ToInt32(id)));
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ProjectDto model)
        {
            try
            {
                var id = webinarAppService.EditProject(model);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }
    }
}
