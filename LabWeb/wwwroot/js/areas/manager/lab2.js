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
            dataSrc: '',
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
                width: "30%",
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
            }
        ],
        language: defaultLanguageDatatable
    });
}

// Setup button click to refresh the DataTable with filters
$(document).ready(function () {
    loadDatatable();
    $('#btnFilter').on('click', function () {
        loadDatatable();
    });
});
