$(document).ready(function () {
    loadDatatable();

    //#region Event handlers for showing detail
    $('#tbl1 tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = datatable.row(tr);  // Use 'datatable' instead of 'table'

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        } else {
            row.child(format(row.data())).show();
            tr.addClass('shown');
        }
    });

    function format(rowData) {
        // Tạo một div để hiển thị chi tiết
        var div = $('<div/>').addClass('loading').text('Loading...');

        // Gửi yêu cầu AJAX để lấy thông tin chi tiết
        $.ajax({
            url: '/Manager/Lab1/Get',
            data: { id: rowData.maNhanVien },
            dataType: 'json',
            success: function (json) {
                // Kiểm tra nếu có dữ liệu trong response
                if (json && json.data) {
                    div.removeClass('loading'); // Xóa lớp loading

                    // Tạo HTML cho các cột hiển thị
                    var detailsHtml = `
                <div class="container">
                    <div class="row">
                        <p>
                            Thông tin chi tiết của nhân viên: <strong>${json.data.hoTen} [ID: ${json.data.maNhanVien}] </strong>
                        <p>
                    </div>
                    <div class="row">
                        <div class="col-6">
                            <p class="form-control d-flex justify-content-between"><strong>Số điện thoại:</strong> ${json.data.soDienThoai}</p>
                        </div>
                        <div class="col-6">
                            <p class="form-control d-flex justify-content-between"><strong>Ngày sinh:</strong> ${new Date(json.data.ngaySinh).toLocaleDateString('vi-VN')}</p>
                        </div>
                        <div class="col-6">
                            <p class="form-control d-flex justify-content-between"><strong>Tên đăng nhập:</strong> ${json.data.tenDangNhap}</p>
                        </div>
                    </div>
                </div>`;

                    // Thêm HTML chi tiết vào div
                    div.html(detailsHtml);
                } else {
                    // Nếu không có dữ liệu
                    div.html('<p>Không có thông tin chi tiết để hiển thị.</p>');
                }
            },
            error: (xhr) => {
                toastr.error('Hiện tại không thể xử lí yêu cầu của bạn.');
            }
        });

        return div;
    }


    //#endregion
});

let datatable;

function loadDatatable() {
    datatable = $('#tbl1').DataTable({
        ajax: {
            url: "/manager/Lab1/GetAll",
            dataSrc: 'data',
            error: (xhr) => {
                // Redirect to login page if the request fails
                window.location.href = "/Customer/Account/Login";
            }
        },
        columns: [
            {
                className: 'details-control',
                orderable: false,
                data: null,
                defaultContent: '<button class="btn btn-info">+</button>'
            },
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
                    return `
                        <button onclick="deleteStaffRecord(${data})" class="btn btn-danger">🗑️</button>
                        <button onclick="loadViewUpsertStaff(${data})" data-bs-toggle="modal" data-bs-target="#upsertModal" class="btn btn-warning">📝</button>`;
                }
            }
        ],
        order: [[1, 'asc']],
        language: defaultLanguageDatatable
    });
}



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

            if (data == undefined) {
                const htmlWithValidate = response.htmlWithValidate;

                $('#upsertModal .modal-content').html(htmlWithValidate); // Cập nhật lại form nếu có lỗi
            }

            if (data.success) {
                toastr.success(data.message); // Hiển thị thông báo thành công
                if (dataTable) {
                    dataTable.ajax.reload(); // Reload bảng dữ liệu nếu tồn tại
                    loadViewUpsertStaff();
                }
                loadViewUpsertStaff();
            } else {
                toastr.error(data.message); // Hiển thị thông báo lỗi
            }
        },
        error: (xhr) => {
            toastr.error("Có lỗi xảy ra: " + xhr.responseText); // Hiển thị lỗi từ server
            console.error("Error:", xhr.responseText); // Log lỗi
        }
    });
}