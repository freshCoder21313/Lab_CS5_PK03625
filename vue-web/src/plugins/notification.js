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
