$(document).ready(function () {
    loadClientProductContent();
});
function loadClientProductContent() {
    const minPrice = +(document.getElementById('minPriceProduct')?.value ?? 0);
    const maxPrice = +(document.getElementById('maxPriceProduct')?.value ?? 999999);

    console.log(`Min Price: ${minPrice}, Max Price: ${maxPrice}`); // Thêm log

    $.ajax({
        url: "/Customer/ClientProduct/SearchContent",
        data: {
            giaNhoNhat: minPrice,
            giaLonNhat: maxPrice
        },
        method: 'GET',
        success: (response) => {
            $('#frameContentProduct').html(response);
        }, error: (xhr) => {
            console.log(xhr);
        }
    });
}
