using Lab.Models;
using Lab.Models.ViewModels;
using Lab.Services.VnPay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lab.API.Areas.Customer.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	//[Authorize]
	public class PaymentController : ControllerBase
	{
		private readonly IVnPayService _vnPayService;

		public PaymentController(IVnPayService vnPayService)
		{
			_vnPayService = vnPayService;
		}

		[HttpGet]
		public IActionResult GetPaymentUrl()
		{
			// Tạo giỏ hàng mẫu để tính tổng tiền
			var cartItems = new List<GioHang>
			{
				new GioHang { MaSanPham = 1, TenSanPham = "Sản phẩm 1", SoLuong = 2, DonGia = 10000 },
				new GioHang { MaSanPham = 2, TenSanPham = "Sản phẩm 2", SoLuong = 1, DonGia = 20000 }
			};

			// Tính tổng tiền
			var totalAmount = cartItems.Sum(c => c.SoLuong * c.DonGia);

			// Tạo URL thanh toán qua VNPay
			var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, new VnPaymentRequestModel
			{
				Amount = Convert.ToInt32(totalAmount),
				OrderId = Guid.NewGuid().ToString(), // Tạo mã tạm thời để theo dõi thanh toán
				CreatedDate = DateTime.Now
			});

			return Ok(new { success = true, paymentUrl });
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
			// Xử lý callback thanh toán (giả định hoàn tất thành công)
			var order = new DonHang
			{
				NguoiDungId = "sampleUserId",
				NgayDatHang = DateTime.Now,
				TongTienDonHang = 30000,
				TrangThaiThanhToan = "Đã thanh toán",
				TenNguoiNhan = "Nguyễn Văn A",
				Duong = "Đường 1",
				ThanhPho = "TP.HCM",
				SoDienThoai = "0123456789",
				MaPhienThanhToan = "sampleTransactionId"
			};

			// Trả về thông tin đơn hàng đã thanh toán thành công
			return Ok(order);
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