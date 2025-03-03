$(document).ready(function () {
    loadDatatable();

    //#region Event handlers for showing detail
    $('#tbl1 tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var tdi = tr.find("i.fa");
        var row = datatable.row(tr);

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
            tdi.first().removeClass('fa-minus-square');
            tdi.first().addClass('fa-plus-square');
        } else {
            row.child(format(row.data())).show();
            tr.addClass('shown');
            tdi.first().removeClass('fa-plus-square');
            tdi.first().addClass('fa-minus-square');
        }
    });

    function format(rowData) {
        var div = $('<div/>').addClass('loading').text('Loading...');

        $.ajax({
            url: '/Manager/Lab1/Get',
            data: { id: rowData.Id },
            dataType: 'json',
            success: function (json) {
                if (json && json.success && json.data) {
                    div.removeClass('loading');

                    var detailsHtml = `
                <div class="container">
                    <div class="row">
                        <p>
                            Thông tin chi tiết của nhân viên: <strong>${json.data.hoTen} [ID: ${json.data.Id}] </strong>
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

                    div.html(detailsHtml);
                } else {
                    div.html('<p>Không có thông tin chi tiết để hiển thị.</p>');
                }
            },
            error: (xhr) => {
                div.html('<p>Không thể lấy thông tin chi tiết.</p>');
                console.error('Error fetching details:', xhr);
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
            url: "/Manager/Lab1/GetAll",
            dataSrc: 'data',
            error: (xhr) => {
                // Redirect to login page if the request fails
                toastr.error("Không thể tải dữ liệu. Vui lòng kiểm tra lại.");
                window.location.href = "/Customer/Account/Login";
            }
        },
        columns: [
            defaultTdToShowDetail,
            {
                data: 'Id',
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
                data: 'Id',
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
    if (confirm("Bạn có chắc chắn muốn xóa nhân viên này không?")) {
        $.ajax({
            url: `/Manager/Lab1/Delete/`,
            data: { id: staffId },
            method: 'DELETE',
            success: (response) => {
                if (response.success) {
                    toastr.success(response.message);
                    datatable.ajax.reload();
                } else {
                    toastr.error(response.message);
                }
            },
            error: (xhr) => {
                toastr.error("Có lỗi xảy ra khi xóa bản ghi.");
                console.log(xhr);
            }
        });
    }
}

function loadViewUpsertStaff(staffId) {
    $.ajax({
        url: `/Manager/Lab1/Upsert/`,
        data: { id: staffId },
        method: 'GET',
        success: (response) => {
            $('#upsertModal .modal-content').html(response);
        },
        error: (xhr) => {
            toastr.error("Có lỗi xảy ra: " + xhr.responseText);
            console.error("Error:", xhr.responseText);
        }
    });
}

function actionUpsertStaff(event) {
    event.preventDefault();

    var formData = new FormData(document.getElementById('formUpsertStaff'));

    logValueForm(formData);

    $.ajax({
        url: `/Manager/Lab1/Upsert/`, // URL API
        data: formData, // Dữ liệu form
        method: 'POST', // Phương thức HTTP
        processData: false, // Không xử lý dữ liệu form
        contentType: false, // Để content-type mặc định của FormData
        success: (response) => {

            console.log(response);

            if (response.success) {
                toastr.success(response.message); // Hiển thị thông báo thành công

                datatable.ajax.reload(); 
                loadViewUpsertStaff();
            } else {
                if (response.htmlWithValidate) {
                    $('#upsertModal .modal-content').html(response.htmlWithValidate ?? null); // Cập nhật lại form nếu có lỗi
                }
                toastr.error(response.message); // Hiển thị thông báo lỗi
            }
        },
        error: (xhr) => {
            toastr.error("Có lỗi xảy ra: " + xhr.statusText); // Hiển thị thông báo lỗi
            console.error("Error:", xhr.responseText);
        }
    });
}