function changeCart(id) {
    $.ajax({
        url: "/customer/cart/changecart",
        data: { maSanPham: id },
        method: 'GET',
        success: (response) => {
            if (response.success) {
                toastr.success(response.message);
            } else {
                toastr.info(response.message);
            }
        }, error: (xhr) => {
            toastr.error("Bạn hiện không thể thực hiện hành động này")
        }
    })
}