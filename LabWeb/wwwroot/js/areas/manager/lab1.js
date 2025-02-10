﻿let datatable;
function loadDatatable() {
    datatable = $('#tbl1').DataTable({
        ajax: {
            url: "/manager/Lab1/GetAll",
            dataSrc: 'data',
            error: (xhr) => {
                //if (xhr.status === 401) {
                //    // Hiển thị thông báo cho người dùng
                //    toastr.info("Thông báo", xhr.message);

                    // Hoặc chuyển hướng đến trang đăng nhập
                    window.location.href = "/Customer/Account/Login"; // Thay đổi đường dẫn đến trang đăng nhập của bạn
                //} else {
                //    // Xử lý các lỗi khác nếu cần
                //    console.error("Có lỗi xảy ra: ", xhr);
                //}
            }
        },
        columns: [
            {
                data: 'maNhanVien',
                width: "15%",
                title: "ID"
            },
            {
                data: 'hoTen',
                width: "35%",
                title: "Họ tên"
            },
            {
                data: 'soDienThoai',
                width: "30%",
                title: "Số điện thoại"
            },
            {
                data: 'ngaySinh',
                width: "20%",
                title: "Ngày sinh",
                render: function (data) {
                    return new Date(data).toLocaleDateString("vi-VN");
                }
            }
        ],
        language: defaultLanguageDatatable
    });
};

$(document).ready(function () {
    loadDatatable();
})