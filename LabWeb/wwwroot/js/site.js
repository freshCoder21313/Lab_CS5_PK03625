//#region Format toastr
toastr.options = {
    "closeButton": true,            // Hiển thị nút đóng
    "debug": false,                  // Debug mode
    "newestOnTop": true,             // Hiển thị thông báo mới nhất trên cùng
    "progressBar": true,             // Thanh tiến trình
    "positionClass": "toast-top-right", // Vị trí
    "preventDuplicates": true,       // Ngăn thông báo trùng lặp
    "showDuration": "300",           // Thời gian xuất hiện (ms)
    "hideDuration": "1000",          // Thời gian ẩn (ms)
    "timeOut": "5000",               // Tự động đóng sau (ms)
    "extendedTimeOut": "1000",       // Thời gian chờ thêm khi di chuột vào
    "showEasing": "swing",           // Hiệu ứng xuất hiện
    "hideEasing": "linear",          // Hiệu ứng biến mất
    "showMethod": "fadeIn",          // Phương thức hiển thị
    "hideMethod": "fadeOut"          // Phương thức ẩn
};
//#endregion

//#region Handling api
const defaultPathAPI = "https://localhost:7094/api/";

function handleResponse(response) {
    if (response.success) {
        toastr.success(response.message, "Thông báo");
    } else {
        console.log(response)
        toastr.info(response.message ?? "Lỗi không xác định. Vui lòng liên hệ nhà phát triển để được hỗ trợ.", "Thông báo");
    }
}
class ResponseAPI {
    constructor(status, success, message, htmlWithValidate, data) {
        this.status = status;
        this.success = success;
        this.message = message;
        this.htmlWithValidate = htmlWithValidate;
        this.data = data;
    }
}

function logValueForm(formData) {
    // Log dữ liệu để kiểm tra
    for (var pair of formData.entries()) {
        console.log(pair[0] + ', ' + pair[1]);
    }
}

function changeValueForm(formData) {
    var data = {};
    formData.forEach((value, key) => {
        data[key.replace(/^Data\./, '')] = value;
    });
    return data;
}

function convertFromJson(jsonResponse) {
    try {
        const responseObject = JSON.parse(jsonResponse);
        return new ResponseAPI(
            responseObject.status,
            responseObject.success,
            responseObject.message,
            responseObject.htmlWithValidate,
            responseObject.data
        );
    } catch (error) {
        console.error("Error converting JSON to ResponseAPI object: ", error);
        return null;
    }
}


function handleJsonData(data) {
    if (typeof data === 'string') {
        return JSON.parse(data);
    }
    return data;
}

function attachDetailsControl(tableSelector, format) {
    $(tableSelector + ' tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var tdi = tr.find("i.fa");
        var row = $(tableSelector).DataTable().row(tr);

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
}
//#endregion

//#region Default value for Datatable
const defaultLanguageDatatable = {
    "sSearch": "Tìm kiếm:",
    "lengthMenu": "Hiển thị _MENU_ mục",
    "info": "Hiển thị _START_ đến _END_ trong tổng số _TOTAL_ mục",
    "paginate": {
        "first": "<<",
        "last": ">>",
        "next": ">",
        "previous": "<"
    },
    "zeroRecords": `<div style="text-align: center;">Không tìm thấy kết quả nào.</div>`,
    "infoEmpty": "Không có mục nào để hiển thị",
    "infoFiltered": "(lọc từ _MAX_ mục)"
}

const defaultTdToShowDetail =
{
    "className": 'details-control',
    "orderable": false,
    "data": null,
    "defaultContent": '',
    "render": function () {
        return '<i class="fa fa-plus-square" aria-hidden="true"></i>';
    },
    "width": "15px"
}
//#endregion