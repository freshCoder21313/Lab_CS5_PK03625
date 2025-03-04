class ResponseAPI {
  constructor(status = null, success = false, message = "Phản hồi không xác định", data = null) {
    this.status = status; // Mã trạng thái của phản hồi (status code)
    this.success = success; // Trạng thái thành công/chưa thành công
    this.message = message; // Thông báo phản hồi từ backend
    this.data = data; // Payload dữ liệu trả về
  }

  static empty() {
    return new ResponseAPI(null, false, "Phản hồi rỗng", null);
  }
}

export default ResponseAPI;
