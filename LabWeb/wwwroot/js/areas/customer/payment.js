function makePayment() {
    $.ajax({
        url: "/Customer/Cart/MakePayment",
        method: 'GET',
        success: (response) => {
            if (response.success) {
                window.open(response.data);
            }
            else {
                toastr.info(response.message);
            }
        },
        error: (xhr) => {
            toastr.error("Hiện tại hành động bạn không thể xử lí.")
            console.log(xhr);
        }
    })
}