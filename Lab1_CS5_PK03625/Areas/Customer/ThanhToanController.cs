using Lab.DataAccess.Repository;
using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Lab.Models.ViewModels;
using Lab.Services.VnPay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lab.API.Areas.Customer
{
	[Area("Customer")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	[Authorize]
	public class ThanhToanController : ControllerBase
	{
		private readonly IVnPayService _vnPayService;
		private readonly IUnitOfWork _unit;

		public ThanhToanController(IVnPayService vnPayService, IUnitOfWork unit)
		{
			_vnPayService = vnPayService;
            _unit = unit;
        }

		[HttpPost]
		public async Task<IActionResult> GetPaymentUrl([FromBody]List<GioHang> gioHangs)
		{
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (String.IsNullOrEmpty(userId))
			{
				return Ok(new ResponseAPI<dynamic>
				{
					Message = "Lỗi truy vấn dữ liệu."
				});
			}
			var responseCreatePayment = await _unit.Payments.CreatePayment(userId, gioHangs);


            // Tạo URL thanh toán qua VNPay
            responseCreatePayment.Data = _vnPayService.CreatePaymentUrl(HttpContext, new VnPaymentRequestModel
            {
                Amount = Convert.ToInt32(responseCreatePayment.Data),
                OrderId = Guid.NewGuid().ToString(), // Tạo mã tạm thời để theo dõi thanh toán
                CreatedDate = DateTime.Now
            });

            return Ok(responseCreatePayment);
		}

		[HttpPost]
		public IActionResult ProcessVNPayPayment()
		{
			// Trong api này, bạn không cần nhập thông tin, vì vậy đã bỏ
			return Ok(new { success = false, message = "Vui lòng sử dụng phương thức Get để lấy link thanh toán." });
		}

		[HttpGet]
		public IActionResult PaymentCallBack()
		{
			return Ok();
		}

		[HttpPost]
		public IActionResult CancelOrder()
		{
			return Ok(new { success = false, message = "Hủy đơn hàng không hỗ trợ trong API này." });
		}

		[HttpGet]
		public IActionResult OrderComplete()
		{
			// Giả định trả về thông tin đơn hàng. Bạn có thể tạo Ok "OrderComplete" để hiển thị
			return Ok();
		}

		[HttpGet]
		public IActionResult TrackOrder()
		{
			// Giả định trả về trạng thái đơn hàng. Bạn có thể tạo Ok "TrackOrder" để hiển thị
			return Ok();
		}
	}
}