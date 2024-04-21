using System.Collections.Generic;
using MoonlightShadow.Models.Transaction;

namespace MoonlightShadow.ViewModels.Account
{
    public class AccountViewModel
    {
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }
        public ShippingDataViewModel ShippingDataViewModel { get; set; }
        public UserDataViewModel UserDataViewModel { get; set; }
        public List<BoughtOrder> BoughtOrders { get; set; }
        public string TypeOfModel { get; set; }
    }
}