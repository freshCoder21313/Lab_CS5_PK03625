import Swal from "sweetalert2";

export const successNotification = (message) => {
  Swal.fire({
    icon: "success",
    title: "Thành công!",
    text: message,
    timer: 2000,
    timerProgressBar: true,
    showConfirmButton: false,
  });
};

export const errorNotification = (message) => {
  Swal.fire({
    icon: "error",
    title: "Lỗi!",
    text: message,
    timer: 2000,
    timerProgressBar: true,
    showConfirmButton: false,
  });
};

export const warningNotification = (message) => {
  Swal.fire({
    icon: "warning",
    title: "Cảnh báo!",
    text: message,
    timer: 2000,
    timerProgressBar: true,
    showConfirmButton: false,
  });
};

export const confirmAction = (message, onConfirm, onCancel) => {
  Swal.fire({
    title: "Xác nhận",
    text: message,
    icon: "question",
    showCancelButton: true,
    confirmButtonText: "Đồng ý",
    cancelButtonText: "Hủy",
  }).then((result) => {
    if (result.isConfirmed) {
      onConfirm && onConfirm();
    } else if (result.dismiss === Swal.DismissReason.cancel) {
      onCancel && onCancel();
    }
  });
};
  