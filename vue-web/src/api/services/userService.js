import apiClient from "@/api/axios";

export const fetchUsers = async () => {
  return apiClient.get("/users");
};

export const createUser = async (data) => {
  return apiClient.post("/users", data);
};

export const deleteUser = async (id) => {
  return apiClient.delete(`/users/${id}`);
};
