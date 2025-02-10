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

const defaultPath = "https://localhost:7094/api/";
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