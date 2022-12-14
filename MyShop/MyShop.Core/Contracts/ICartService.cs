using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Core.Contracts
{
    public interface ICartService
      { 
        void AddToCart(HttpContextBase httpContext, string productId);
        void RemoveFromCart(HttpContextBase httpContext, string itemId);
        List<CartItemViewModel> GetCartItems(HttpContextBase httpContext);
        CartSummaryViewModel GetCartSummary(HttpContextBase httpContext);
        void ClearCart(HttpContextBase httpContext);
    }
}
