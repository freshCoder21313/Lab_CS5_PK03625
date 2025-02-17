$(document).ready(function () {
    loadDatatable();
});

let datatable;

function loadDatatable() {
    datatable = $('#tbl1').DataTable({
        ajax: {
            url: "/Customer/Cart/GetAll",
            dataSrc: '',
            error: (xhr) => {
                // Redirect to login page if the request fails
                toastr.error("Không thể tải dữ liệu. Vui lòng kiểm tra lại.");
            }
        },
        columns: [
            {
                data: 'maSanPham',
                width: "15%",
                title: "ID"
            },
            {
                data: 'tenSanPham',
                width: "35%",
                title: "Tên sản phẩm"
            },
            {
                data: 'soLuong',
                width: "20%",
                title: "Số lượng"
            },
            {
                data: 'donGia',
                width: "15%",
                title: "Đơn giá",
                render: function (data) {
                    return data;
                }
            },
            {
                data: 'maSanPham',
                width: "20%",
                title: "Hành động",
                render: function (data) {
                    return `
                        <button onclick="deleteCart(${data})" class="btn btn-danger">🗑️</button>`;
                }
            }
        ],
        language: defaultLanguageDatatable
    });
}

function deleteCart(id) {
    $.ajax({
        url: "/Customer/Cart/DeleteCart",
        data: { id: id },
        method: 'GET',
        success: (response) => {
            if (response.success) {
                datatable.ajax.reload();
                toastr.success(response.message);
            } else {
                toastr.info(response.message);
            }
        }, error: (xhr) => {
            toastr.error("Bạn hiện không thể thực hiện hành động này")
        }
    })
}