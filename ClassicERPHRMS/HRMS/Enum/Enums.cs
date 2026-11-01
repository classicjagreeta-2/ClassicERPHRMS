using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace HRMS.Enum
{
    public enum EnumVoucherType
    {
        [Description("Contra")]
        CashPayment = 0,

        [Description("Cash Payment/Cash Receipt")]
        Cash = 1,

        [Description("Bank Payment/Bank Receipt")]
        Bank = 2,

        [Description("Journal Voucher")]
        Journal = 3,
    }

    public enum EnumPaymentType
    {
        [Description("Auto")]
        Auto = 0,

        [Description("Payment")]
        Payment = 1,

        [Description("Receipt")]
        Receipt = 2,

        [Description("Other")]
        Other = 3,
    }
}