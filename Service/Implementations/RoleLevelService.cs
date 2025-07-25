
using Microsoft.AspNetCore.Http;
using Model.Domain;
using Service.Data;
using Service.ServiceContracts;
using System.Security.Claims;

namespace Service.Implementations
{
    public class RoleLevelService : IRoleLevelService
    {
        private readonly List<ActionRoleData> ActionRoleLevels = new List<ActionRoleData>();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleLevelService(IHttpContextAccessor httpContextAccessor)
        {
            AddPermissions();
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthorizedToUseAction(string controllerName, string action)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var role = (eRole)Convert.ToInt32(claimsIdentity.FindFirst("Role").Value);

            return ActionRoleLevels.Any(arl => arl.Controller.ToLower() == controllerName.ToLower() && arl.Action == action && arl.Roles.Any(r => r == role));
        }

        private void AddPermissions()
        {
            #region ShoppingCartController
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "ShoppingCart",
                Action = "Index",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager,
                    eRole.Purchasing
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "ShoppingCart",
                Action = "Grid",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager,
                    eRole.Purchasing
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "ShoppingCart",
                Action = "GetProductDetail",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager,
                    eRole.Purchasing
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "ShoppingCart",
                Action = "GetProductPriceByQuantity",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager,
                    eRole.Purchasing
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "ShoppingCart",
                Action = "Cart",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager,
                    eRole.Purchasing
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "ShoppingCart",
                Action = "AddProductToCart",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager,
                    eRole.Purchasing
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "ShoppingCart",
                Action = "ChangeProductCartQuantity",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager,
                    eRole.Purchasing
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "ShoppingCart",
                Action = "DeleteProductFromCart",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager,
                    eRole.Purchasing
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "ShoppingCart",
                Action = "Create",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager,
                    eRole.Purchasing
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "ShoppingCart",
                Action = "Orders",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager,
                    eRole.Purchasing
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "ShoppingCart",
                Action = "OrdersGrid",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager,
                    eRole.Purchasing
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "ShoppingCart",
                Action = "OrderDetail",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager,
                    eRole.Purchasing
                }
            });
            #endregion
            #region SalesFileController
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "SalesFile",
                Action = "Index",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "SalesFile",
                Action = "Grid",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "SalesFile",
                Action = "Create",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "SalesFile",
                Action = "UpdateOrder",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "SalesFile",
                Action = "Delete",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "SalesFile",
                Action = "Display",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager,
                    eRole.Purchasing
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "SalesFile",
                Action = "GetPdf",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager,
                    eRole.Purchasing
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "SalesFile",
                Action = "LoadPrevious",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager,
                    eRole.Purchasing
                }
            });
            #endregion
            #region DownloadFileController
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "DownloadFile",
                Action = "Index",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "DownloadFile",
                Action = "Grid",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "DownloadFile",
                Action = "Create",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "DownloadFile",
                Action = "UpdateOrder",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "DownloadFile",
                Action = "Delete",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "DownloadFile",
                Action = "Display",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager,
                    eRole.Purchasing
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "DownloadFile",
                Action = "LoadPrevious",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager,
                    eRole.Purchasing
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "DownloadFile",
                Action = "GetFile",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager,
                    eRole.Purchasing
                }
            });
            #endregion
            #region OrderController
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Order",
                Action = "Index",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Order",
                Action = "Grid",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Order",
                Action = "Edit",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Order",
                Action = "GetProductsByTerm",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Order",
                Action = "AddProductToOrder",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Order",
                Action = "UpdateOrder",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Order",
                Action = "DeleteProduct",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Order",
                Action = "Detail",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                    eRole.StockManager
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Order",
                Action = "Approve",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Order",
                Action = "Reject",
                Roles = new List<eRole>
                {
                    eRole.Administrator,
                }
            });
            #endregion
            #region ClientController
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Client",
                Action = "Index",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Client",
                Action = "Grid",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Client",
                Action = "Create",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Client",
                Action = "Edit",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Client",
                Action = "Detail",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            #endregion
            #region ProductController
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Product",
                Action = "Index",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Product",
                Action = "Grid",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Product",
                Action = "Create",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Product",
                Action = "Edit",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Product",
                Action = "Detail",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Product",
                Action = "AddOffer",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "Product",
                Action = "DeleteOffer",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            #endregion
            #region UserController
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "User",
                Action = "Index",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "User",
                Action = "Grid",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            ActionRoleLevels.Add(new ActionRoleData
            {
                Controller = "User",
                Action = "ResetPassword",
                Roles = new List<eRole>
                {
                    eRole.Administrator
                }
            });
            #endregion
        }
    }
}
