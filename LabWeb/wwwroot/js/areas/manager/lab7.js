// Setup button click to refresh the DataTable with filters
$(document).ready(function () {

    $.ajax({
        url: '/manager/lab7/getall',
        method: 'GET',
        success: (response) => {
            var responseJs = handleJsonData(response);
            if (responseJs.status === 200 && responseJs.success) {
                // Gọi hàm để vẽ biểu đồ với dữ liệu lấy từ API
                drawChart(responseJs.data);
            } else {
                console.error('Không tìm thấy dữ liệu từ API.');
            }
        },
        error: (xhr) => {
            console.log(xhr);
            toastr.error("Hiện không thể truy vấn dữ liệu tạo Chart");
        }
    });

    // Gửi yêu cầu lấy dữ liệu từ API
    /*fetch('/manager/lab7') // Thay bằng URL API phù hợp
        .then(response => response.json())
        .then(result => {
            if (result.status === 200 && result.success) {
                // Gọi hàm để vẽ biểu đồ với dữ liệu lấy từ API
                drawChart(result.data);
            } else {
                console.error('Không tìm thấy dữ liệu từ API.');
            }
        })
        .catch(error => {
            console.error('Lỗi khi lấy dữ liệu từ API:', error);
        });    */
});

// Hàm xử lý vẽ biểu đồ
function drawChart(data) {
    // Lọc top 10 sản phẩm có donGia giảm dần
    const top10Products = data
        .sort((a, b) => b.donGia - a.donGia)
        .slice(0, 10);

    // Tách riêng dữ liệu
    const productNames = top10Products.map(product => product.tenSanPham);
    const productPrices = top10Products.map(product => product.donGia);
    const productQuantities = top10Products.map(product => product.soLuong);

    // Tạo biểu đồ bằng Chart.js
    const ctx = document.getElementById('productsChart').getContext('2d');
    new Chart(ctx, {
        type: 'bar', // Biểu đồ dạng cột
        data: {
            labels: productNames,
            datasets: [
                {
                    label: 'Số Lượng (Cột)',
                    type: 'bar',
                    data: productQuantities,
                    backgroundColor: 'rgba(54, 162, 235, 0.2)',
                    borderColor: 'rgba(54, 162, 235, 1)',
                    borderWidth: 1,
                    yAxisID: 'y',
                },
                {
                    label: 'Đơn Giá (Sparkline)',
                    type: 'line',
                    data: productPrices,
                    backgroundColor: 'rgba(255, 206, 86, 0.2)',
                    borderColor: 'rgba(255, 206, 86, 1)',
                    borderWidth: 2,
                    yAxisID: 'y1',
                },
            ],
        },
        options: {
            responsive: true,
            scales: {
                y: {
                    type: 'linear',
                    display: true,
                    position: 'left',
                    title: {
                        display: true,
                        text: 'Số Lượng',
                    },
                },
                y1: {
                    type: 'linear',
                    display: true,
                    position: 'right',
                    grid: {
                        drawOnChartArea: false, // Không vẽ grid ở trên đồ thị cột
                    },
                    title: {
                        display: true,
                        text: 'Đơn Giá',
                    },
                },
            },
            plugins: {
                tooltip: {
                    callbacks: {
                        label: function (tooltipItem) {
                            if (tooltipItem.datasetIndex === 0) {
                                return `Số Lượng: ${tooltipItem.raw}`;
                            }
                            if (tooltipItem.datasetIndex === 1) {
                                return `Đơn Giá: ${tooltipItem.raw.toLocaleString()} VND`;
                            }
                        },
                    },
                },
            },
        },
    });
}
