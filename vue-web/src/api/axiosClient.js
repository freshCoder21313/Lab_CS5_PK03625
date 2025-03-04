import axios from "axios";
import jwtDecode from "jwt-decode"; // Cần cài đặt thư viện này: npm install jwt-decode
import ResponseAPI from "@/models/ResponseAPI";

// Base Axios Client
const axiosClient = axios.create({
  baseURL: "https://localhost:7094/api", // Thay bằng base URL của API bạn
  timeout: 10000, // Giới hạn timeout (ms)
  headers: {
    "Content-Type": "application/json",
  },
});

// Kiểm tra token hết hạn
function isTokenExpired(token) {
  try {
    const decoded = jwtDecode(token);
    return decoded.exp * 1000 < Date.now(); // Thời gian hết hạn (exp) là milli-seconds
  } catch {
    return true;
  }
}

// Hàm refresh token
async function refreshAccessToken() {
  const refreshToken = localStorage.getItem("refreshToken");

  if (!refreshToken) {
    throw new Error("Refresh Token không tồn tại!");
  }

  try {
    const response = await axios.post("https://localhost:7094/api/TruyCap/RefreshToken", {
      RefreshToken: refreshToken,
    });

    if (response.status === 200 && response.data) {
      const { accessToken, refreshToken: newRefreshToken } = response.data;

      // Lưu lại token mới
      localStorage.setItem("accessToken", accessToken);
      localStorage.setItem("refreshToken", newRefreshToken);

      return accessToken;
    }
  } catch (error) {
    console.error("Không thể refresh access token:", error);
    throw new Error("Refresh Token đã hết hạn");
  }
}

// Middleware (interceptors) thêm Authorization header
axiosClient.interceptors.request.use(
  async (config) => {
    const accessToken = localStorage.getItem("accessToken");

    // Nếu cần Authorization và có accessToken
    if (accessToken) {
      if (isTokenExpired(accessToken)) {
        // Nếu token hết hạn, refresh token
        const newToken = await refreshAccessToken();
        config.headers.Authorization = `Bearer ${newToken}`;
      } else {
        config.headers.Authorization = `Bearer ${accessToken}`;
      }
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Xử lý lỗi trong phản hồi API
axiosClient.interceptors.response.use(
  (response) => {
    return response.data; // Trả về data từ API nếu thành công
  },
  (error) => {
    if (error.response) {
      console.error(`API Error: ${error.response.status}`, error.response.data);
      throw new Error(error.response.data?.message || "Lỗi không xác định");
    }
    throw error;
  }
);

// Hàm xử lý response API
const handleResponse = async (callback) => {
  try {
    const result = await callback();
    return new ResponseAPI(200, true, "Thành công", result);
  } catch (error) {
    const status = error.response?.status || 500;
    const message = error.response?.data?.message || "Lỗi không xác định";
    return new ResponseAPI(status, false, message, null);
  }
};

// Hàm GET
async function getFromApi(url) {
  return handleResponse(() => axiosClient.get(url));
}

// Hàm POST
async function postToApi(url, data) {
  return handleResponse(() => axiosClient.post(url, data));
}

// Hàm PUT
async function putToApi(url, data) {
  return handleResponse(() => axiosClient.put(url, data));
}

// Hàm PATCH
async function patchToApi(url, data) {
  return handleResponse(() => axiosClient.patch(url, data));
}

// Hàm DELETE
async function deleteFromApi(url) {
  return handleResponse(() => axiosClient.delete(url));
}

export { getFromApi, postToApi, putToApi, patchToApi, deleteFromApi };
