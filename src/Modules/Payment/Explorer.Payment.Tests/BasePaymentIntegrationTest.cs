using Explorer.BuildingBlocks.Tests;

namespace Explorer.Payment.Tests;

public class BasePaymentIntegrationTest : BaseWebIntegrationTest<PaymentTestFactory>
{
    public BasePaymentIntegrationTest(PaymentTestFactory factory) : base(factory) {}
}
