﻿namespace Application.Services.PayPal.Http;

public static class Requests
{
    public record PaypalAuthorizePayment;

    public record PaypalCancelPayment;

    public record PaypalRefundPayment;
}