using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Validation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TalkBack.Authorization.Users;
using TalkBack.Controllers;
using TalkBack.Users;
using TalkBack.Web.Models;
using TalkBack.Webinars;

namespace TalkBack.Web.Controllers
{
    public class PaymentController : TalkBackControllerBase
    {
        public SmtpClient smtpClient { get; set; }
        public IHostingEnvironment _hostingEnvironment { get; set; }
        public IConfiguration iConfig { get; set; }
        public IWebinarAppService webinarAppService { get; set; }
        public IRepository<User, long> UserRepo { get; set; }
        public IRepository<WebinarPayment> paymentRepo { get; set; }
        //public IUserAppService userAppService { get; set; }
        public UserRegistrationManager _userRegistrationManager { get; set; }
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public PaymentController(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public IActionResult Index()
        {
            ViewBag.Id = AbpSession.UserId;
            ViewBag.Payments = webinarService.GetPayments(new Abp.Application.Services.Dto.EntityDto<long>(AbpSession.UserId.Value));
            return View();
        }

        public async Task<IActionResult> Pay(string id)//webinar id
        {
            Response.Cookies.Delete("uid");
            try
            {
                //user.Id + "-" + model.WebinarId
                string[] splitted = id.Split("-", StringSplitOptions.RemoveEmptyEntries);
                string webinarid = splitted[1];
                string uid = splitted[0];
                var webinar = webinarAppService.GetWebinar(new Webinars.Dto.IdDto<int>(Convert.ToInt32(webinarid)));
                ViewBag.Fee = webinar.Cost;
                ViewBag.WebinarId = webinarid;
                if (!string.IsNullOrWhiteSpace(uid))
                {
                    long UserId = Convert.ToInt64(uid);
                    var user = await webinarAppService.GetUserDetails(new Abp.Application.Services.Dto.EntityDto<long>(UserId));
                    Response.Cookies.Append("uid", uid);

                    return View(user);
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message, ex);
                ViewBag.Message = ex.Message;
            }
            return View("Fail");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisableValidation]
        public async Task<ActionResult> Pay(string webinarid, string uid, string cardNumber, string cardExpiry, string cardCVC, string CardName)
        {
            Response.Cookies.Delete("uid");
            try
            {
                var webinar = webinarAppService.GetWebinar(new Webinars.Dto.IdDto<int>(Convert.ToInt32(webinarid)));
                if (!string.IsNullOrWhiteSpace(uid))
                {
                    long UserId = Convert.ToInt64(uid);
                    var user = await webinarAppService.GetUserDetails(new Abp.Application.Services.Dto.EntityDto<long>(UserId));

                    StripeConfiguration.ApiKey = iConfig.GetSection("StripeApiKey").Value;

                    long applicationfee = (long)webinar.Cost;
                    string customerEmail = user.EmailAddress;
                    string customerName = user.FullName;
                    string[] exp = cardExpiry.Split(new char[] { '/' });

                    //create customer
                    var cardCreation = new TokenCreateOptions
                    {
                        Card = new AnyOf<string, TokenCardOptions>(new TokenCardOptions
                        {
                            Number = cardNumber,
                            ExpYear = Convert.ToInt64(exp[1]),
                            ExpMonth = Convert.ToInt64(exp[0]),
                            Cvc = cardCVC,
                            Name = CardName
                        })
                    };

                    var service = new TokenService();
                    Token stripeToken = service.Create(cardCreation);

                    //create 
                    var options = new CustomerCreateOptions
                    {
                        Description = user.FullName,
                        Source = stripeToken.Id,
                        Email = customerEmail,
                        Name = customerName
                    };

                    var customerservice = new CustomerService();
                    Customer customer = customerservice.Create(options);//get or create by customer id
                    var charges = new ChargeService();

                    var charge = charges.Create(new ChargeCreateOptions
                    {
                        Amount = applicationfee * 100,
                        Description = "Webinar Charge",
                        Currency = "usd",
                        Customer = customer.Id
                    });

                    WebinarPayment objPayment = new WebinarPayment();
                    objPayment.WebinarId = webinar.Id;
                    if (charge.Status == "succeeded")
                    {
                        objPayment.ReferenceId = charge.Id;

                        string to = user.EmailAddress;
                        string template = _hostingEnvironment.WebRootPath + @"\report\activationMail.html";
                        string body = System.IO.File.ReadAllText(template);
                        body = body.Replace("[USER]", user.FullName);
                        body = body.Replace("[DETAILS]", webinar.JoiningDetails.Replace("\n", "<br/>"));
                        SendMail(to, "WMS: Webinar Details", body);

                        user.CustomerId = customer.Id;
                        await webinarAppService.Update(user);
                        //user.IsTrial = false;
                        //user.TrialExpiredOn = null;
                        objPayment.IsFeePaid = true;
                        objPayment.FeePaidOn = DateTime.Now;
                        objPayment.UserId = user.Id;
                        objPayment.CreatorUserId = user.Id;
                        //user.IsAnnualFeePaid = true;
                        paymentRepo.Insert(objPayment);

                        ViewBag.Message = "Payment successful";
                        return View("Success", user);
                    }
                    else
                    {
                        objPayment.IsFeePaid = false;
                        objPayment.FeePaidOn = null;
                        paymentRepo.Insert(objPayment);

                        ViewBag.Message = "Invalid payment details";
                        return View("Fail");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message, ex);
                ViewBag.Message = ex.Message;
                ViewBag.Errors = ex.Message;
                return View("Fail");
            }
            return View("Fail");
        }

        [NonAction]
        public void SendMail(string to, string subject, string body)
        {
            var msg = new MailMessage()
            {
                From = new MailAddress("wms-do-not-reply@workflow.com", "WMS Admin"),
                Subject = subject,
                Body = body
            };
            string[] mails = to.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var m in mails)
                msg.To.Add(m);
            msg.IsBodyHtml = true;
            //msg.Attachments.Add(new Attachment());

            this.smtpClient.Send(msg);
        }

        [HttpPost]
        public ActionResult Webhook()
        {
            var json = new StreamReader(HttpContext.Request.Body).ReadToEnd();

            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);

                // Handle the event
                if (stripeEvent.Type == Events.InvoicePaymentFailed)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    var cid = paymentIntent.CustomerId;

                    var payment = (from u in paymentRepo.GetAll()
                                   where u.ReferenceId == paymentIntent.InvoiceId
                                   select u).FirstOrDefault();
                    if (payment != null)
                    {
                        payment.IsFeePaid = false;
                        payment.FeePaidOn = null;
                        paymentRepo.Update(payment);
                    }
                }
                else if (stripeEvent.Type == Events.InvoicePaid)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    var cid = paymentIntent.CustomerId;

                    var payment = (from u in paymentRepo.GetAll()
                                   where u.ReferenceId == paymentIntent.InvoiceId
                                   select u).FirstOrDefault();
                    if (payment != null)
                    {
                        payment.IsFeePaid = true;
                        payment.FeePaidOn = DateTime.Now;
                        paymentRepo.Update(payment);
                    }
                }
                return Content("Success");
            }
            catch (StripeException e)
            {
                Logger.Fatal(e.Message, e);
                return Content(e.Message);
            }
        }

        public IActionResult Register(string id)
        {
            WebinarRegisterDto model = new WebinarRegisterDto();
            model.WebinarId = Convert.ToInt32(id);
            var webinar = webinarAppService.GetWebinar(new Webinars.Dto.IdDto<int>(model.WebinarId));
            model.WebinarFee = webinar.Cost;
            return View(model);
        }
        [NonAction]
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        [HttpPost]
        [DisableValidation]
        [UnitOfWork]
        public async Task<IActionResult> Register(WebinarRegisterDto model)
        {
            //using (_unitOfWorkManager.Current.SetTenantId(1))
            {
                string password = CreatePassword(5);
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                var userQuery = UserRepo.GetAll().Where(x => x.EmailAddress.ToLower() == model.Email.ToLower());
                if (userQuery.Any() == false)
                {
                    var user = await _userRegistrationManager.RegisterAsync(
                        model.FirstName,
                        model.LastName,
                        model.Email,
                        model.Email,
                        password,
                        true // Assumed email address is always confirmed. Change this if you want to implement email confirmation.
                    );

                    string to = model.Email;
                    string template = _hostingEnvironment.WebRootPath + @"\report\registerMail.html";
                    string body = System.IO.File.ReadAllText(template);
                    body = body.Replace("[USER]", user.FullName);
                    body = body.Replace("[DETAILS]", "Username: " + model.Email + "<br/>Password: " + password + "<br/>");
                    SendMail(to, "WMS: Registration Details", body);
                }

                var u = UserRepo.GetAll().Where(x => x.EmailAddress.ToLower() == model.Email.ToLower()).FirstOrDefault();
                return RedirectToAction("Pay", "Payment", new { id = u.Id + "-" + model.WebinarId });
            }
        }
    }
}
