import axios from "axios";
import { jwtDecode } from "jwt-decode"; // Thư viện cần: npm install jwt-decode
import ResponseAPI from "@/models/ResponseAPI";

// Base Axios Client
const axiosClient = axios.create({
  baseURL: "http://localhost:5273/api", // Thay bằng base URL của API bạn
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
    return true; // Nếu không decode được token, xem như nó đã hết hạn
  }
}

// Hàm refresh token
async function refreshAccessToken() {
  const refreshToken = localStorage.getItem("refreshToken");

  if (!refreshToken) {
    throw new Error("Refresh Token không tồn tại!");
  }

  try {
    const response = await axios.post(
      axiosClient.baseURL + "/TruyCap/RefreshToken",
      {
        RefreshToken: refreshToken,
      }
    );

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
    // Kiểm tra xem request này có cần Authorization hay không
    const requiresAuth = !config.headers.skipAuth; // Mặc định là true nếu skipAuth không tồn tại
    if (!requiresAuth) {
      return config; // Nếu không cần Authorization, trả về config gốc
    }

    const accessToken = localStorage.getItem("accessToken");

    // Nếu cần Authorization và có accessToken
    if (accessToken) {
      if (isTokenExpired(accessToken)) {
        // Nếu token hết hạn, refresh token
        try {
          const newToken = await refreshAccessToken();
          config.headers.Authorization = `Bearer ${newToken}`;
        } catch (error) {
          console.error("Lỗi khi refresh token:", error.message);
          throw error;
        }
      } else {
        config.headers.Authorization = `Bearer ${accessToken}`;
      }
    }

    return config;
  },
  (error) => {
    return Promise.reject(error); // Xử lý lỗi nếu request bị gián đoạn
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
    return new ResponseAPI(result);
  } catch (error) {
    const status = error.response?.status || 500;
    const message = error.response?.data?.message || "Lỗi không xác định";
    return new ResponseAPI(status, false, message, null);
  }
};

// Hàm GET
async function getFromApi(url, config = { headers: { skipAuth: true } }) {
  return handleResponse(() => axiosClient.get(url, config));
}

// Hàm POST
async function postToApi(url, data, config = { headers: { skipAuth: true } }) {
  return handleResponse(() => axiosClient.post(url, data, config));
}

// Hàm PUT
async function putToApi(url, data, config = { headers: { skipAuth: true } }) {
  return handleResponse(() => axiosClient.put(url, data, config));
}

// Hàm PATCH
async function patchToApi(url, data, config = { headers: { skipAuth: true } }) {
  return handleResponse(() => axiosClient.patch(url, data, config));
}

// Hàm DELETE
async function deleteFromApi(url, config = { headers: { skipAuth: true } }) {
  return handleResponse(() => axiosClient.delete(url, config));
}

export { getFromApi, postToApi, putToApi, patchToApi, deleteFromApi };
