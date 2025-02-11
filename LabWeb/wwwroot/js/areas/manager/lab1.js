$(document).ready(function () {
    loadDatatable();
});

let datatable;
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
                width: "20%",
                title: "Số điện thoại"
            },
            {
                data: 'ngaySinh',
                width: "15%",
                title: "Ngày sinh",
                render: function (data) {
                    return new Date(data).toLocaleDateString("vi-VN");
                }
            },
            {
                data: 'maNhanVien',
                width: "20%",
                title: "Hành động",
                render: function (data) {
                    return `<button onclick="deleteStaffRecord(${data})" class="btn btn-danger">🗑️</button>
                            <button onclick="loadViewUpsertStaff(${data})" data-bs-toggle="modal" data-bs-target="#upsertModal" class=btn btn-warning">📝</button>`
                }
            }
        ],
        language: defaultLanguageDatatable
    });
};

function deleteStaffRecord(staffId) {
    $.ajax({
        url: `/Manager/Lab1/Delete/`,
        data: { id: staffId },
        method: 'DELETE',
        success: (response) => {
            handleResponse(handleJsonData(response.jsonResponse));
            datatable.ajax.reload();
        },
        error: (xhr) => {
            toastr.error("Lỗi", "Hiện không thể xử lí yêu cầu của bạn.");
            console.log(xhr);
        }
    })
}

function loadViewUpsertStaff(staffId) {
    // Gửi yêu cầu AJAX
    $.ajax({
        url: `/Manager/Lab1/Upsert/`,
        data: {id: staffId},
        method: 'GET',
        success: (data) => {
            $('#upsertModal .modal-content').html(data);
        },
        error: (xhr) => {
            toastr.error("Có lỗi xảy ra: " + xhr.responseText); // Hiển thị lỗi từ server
            console.error("Error:", xhr.responseText); // Log lỗi
        }
    });
}

function actionUpsertStaff(event) {
    event.preventDefault();

    var formData = new FormData(document.getElementById('formUpsertStaff'));
    console.log(formData);
    // Gửi yêu cầu AJAX
    $.ajax({
        url: `/Manager/Lab1/Upsert/`, // URL API
        data: formData, // Dữ liệu form
        method: 'POST', // Phương thức HTTP
        processData: false, // Không xử lý dữ liệu form
        contentType: false, // Để content-type mặc định của FormData
        success: (response) => {
            const data = handleJsonData(response.jsonResponse);
            if (data.success) {
                toastr.success(data.message); // Hiển thị thông báo thành công
                if (dataTable) {
                    dataTable.ajax.reload(); // Reload bảng dữ liệu nếu tồn tại
                    loadViewUpsertStaff();
                }
                loadViewUpsertStaff();
            } else {
                $('#upsertModal .modal-content').html(data.data); // Cập nhật lại form nếu có lỗi
                toastr.error(data.message); // Hiển thị thông báo lỗi
            }
        },
        error: (xhr) => {
            toastr.error("Có lỗi xảy ra: " + xhr.responseText); // Hiển thị lỗi từ server
            console.error("Error:", xhr.responseText); // Log lỗi
        }
    });
}