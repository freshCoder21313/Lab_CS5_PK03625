// Setup button click to refresh the DataTable with filters
$(document).ready(function () {
    loadDatatable();
    $('#btnFilter').on('click', function () {
        loadDatatable();
    });
});

let datatable;

function loadDatatable() {
    const minPrice = parseInt(document.getElementById("minPrice").value) || 0; // Default to 0
    const maxPrice = parseInt(document.getElementById("maxPrice").value) || 1000000000; // Default to a large value

    // If a DataTable already exists, destroy it before creating a new one
    if ($.fn.DataTable.isDataTable('#tbl1')) {
        $('#tbl1').DataTable().clear().destroy();
    }

    datatable = $('#tbl1').DataTable({
        ajax: {
            url: "/manager/lab2/HandFilterByPriceAndManualSortByPrice",
            data: { giaNhoNhat: minPrice, giaLonNhat: maxPrice }, // Use colon instead of equal sign
            dataSrc: 'data',
            error: (xhr) => {
                // Redirect to login page if the request fails
                window.location.href = "/Customer/Account/Login";
            }
        },
        columns: [
            {
                data: 'maSanPham',
                width: "15%",
                title: "ID",
                className: "text-center"
            },
            {
                data: 'tenSanPham',
                width: "35%",
                title: "Tên sản phẩm",
                className: "text-right"
            },
            {
                data: 'soLuong',
                width: "10%",
                title: "Số lượng",
                className: "text-center"
            },
            {
                data: 'donGia',
                width: "20%",
                title: "Đơn giá",
                className: "text-right",
                render: function (data) {
                    if (typeof +data === 'number' && !isNaN(+data)) {
                        return (+data).toLocaleString("vi-VN", { style: "currency", currency: "VND" });
                    }
                    return "N/A"; // Return a default value or a placeholder
                }
            },
            {
                data: 'maSanPham',
                width: "20%",
                title: "Hành động",
                render: function (data) {
                    return `
                        <button onclick="deleteProductRecord(${data})" class="btn btn-danger">🗑️</button>
                        <button onclick="loadViewUpsertProduct(${data})" data-bs-toggle="modal" data-bs-target="#upsertModal" class="btn btn-warning">📝</button>`;
                }
            }
        ],
        language: defaultLanguageDatatable
    });
}



function deleteProductRecord(ProductId) {
    if (confirm("Bạn có chắc chắn muốn xóa nhân viên này không?")) {
        $.ajax({
            url: `/Manager/Lab2/Delete/`,
            data: { id: ProductId },
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

function loadViewUpsertProduct(ProductId) {
    $.ajax({
        url: `/Manager/Lab2/Upsert/`,
        data: { id: ProductId },
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

function actionUpsertProduct(event) {
    event.preventDefault();

    var formData = new FormData(document.getElementById('formUpsertProduct'));

    logValueForm(formData);

    $.ajax({
        url: `/Manager/Lab2/Upsert/`, // URL API
        data: formData, // Dữ liệu form
        method: 'POST', // Phương thức HTTP
        processData: false, // Không xử lý dữ liệu form
        contentType: false, // Để content-type mặc định của FormData
        success: (response) => {

            console.log(response);

            if (response.success) {
                toastr.success(response.message); // Hiển thị thông báo thành công

                datatable.ajax.reload();
                loadViewUpsertProduct();
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