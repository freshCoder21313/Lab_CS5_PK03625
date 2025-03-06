import toastr from "toastr";
import "toastr/build/toastr.min.css";

export default {
  install(app) {
    toastr.options = {
      closeButton: true,
      progressBar: true,
      timeOut: "3000",
      positionClass: "toast-top-right",
    };

    app.config.globalProperties.$toast = {
      success: (message) => toastr.success(message, "Thành công"),
      error: (message) => toastr.error(message, "Lỗi"),
      warning: (message) => toastr.warning(message, "Cảnh báo"),
      info: (message) => toastr.info(message, "Thông tin"),
    };
  },
};
