using Microsoft.AspNetCore.Authorization;
using Stripe;
using Talabat.Core.Order_Aggregate;
namespace Talabat.APIs.Controllers;

public class PaymentController(
    IPaymentService paymentService,
    IConfiguration configuration,
    ILogger<PaymentController> logger)
    : BaseApiController
{
    [Authorize]
    [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [HttpPost("{basketId}")]
    public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
    {
        var basket = await paymentService.CreateOrUpdatePaymentIntent(basketId);
        if (basket == null) return BadRequest(new ApiResponse(400, "Problem with your basket"));
        return basket;
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var stripeEvent = EventUtility.ConstructEvent(
            json,
            Request.Headers["Stripe-Signature"],
            configuration["StripeSettings:WebhookSecret"]
        );
        
        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        Order order;
        switch (stripeEvent.Type)
        {
            case "payment_intent.succeeded":
                order = await paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, true);
                await HandlePaymentIntentSucceeded(paymentIntent);
                logger.LogInformation("Payment succeeded: {PaymentId}", paymentIntent.Id); 
                break;

            case "payment_intent.payment_failed":
                order = await paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, false);
                logger.LogInformation("Payment failed: {PaymentId}", paymentIntent.Id); 
                await HandlePaymentIntentFailed(paymentIntent);
                break;

            default:
                logger.LogWarning("Unhandled event type: {EventType}", stripeEvent.Type); 
                break;
        }

        return Ok();
    }

    private async Task HandlePaymentIntentSucceeded(PaymentIntent paymentIntent)
    {
        Console.WriteLine($"Payment succeeded: {paymentIntent.Id}");
        await Task.CompletedTask;
    }

    private async Task HandlePaymentIntentFailed(PaymentIntent paymentIntent)
    {
        Console.WriteLine($"Payment failed: {paymentIntent.Id}");
        await Task.CompletedTask;
    }
}
