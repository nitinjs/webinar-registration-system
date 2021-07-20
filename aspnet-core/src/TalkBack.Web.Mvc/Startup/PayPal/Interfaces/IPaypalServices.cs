using Abp.Application.Services;
using PayPal.Api;

namespace TalkBack.Web
{
    public interface IPaypalServices : IApplicationService
    {
        PayPalAuthOptions Options { get; set; }
        Payment CreatePayment(decimal amount, string returnUrl, string cancelUrl, string intent);

        Payment ExecutePayment(string paymentId, string payerId);
    }
}