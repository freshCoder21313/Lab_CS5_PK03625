let datatable;
function loadDatatable() {
    datatable = $('#tbl1').DataTable({
        ajax: {
            url: "https://localhost:7081/manager/Lab1/getall",
            dataSrc: 'data'
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
                title: "Ngày sinh"
            }
        ]
    });
};

$(document).ready(function () {
    loadDatatable();
})