import { getFromApi } from "@/api/axiosClient";
import ConfigsRequest from "@/models/ConfigsRequest";

export function isAccess() {
  return !!localStorage.getItem("accessToken");
}

export async function logout() {
  await getFromApi("/TruyCap/Logout", ConfigsRequest.takeAuth());
  localStorage.removeItem("accessToken");
  localStorage.removeItem("refreshToken");
}
