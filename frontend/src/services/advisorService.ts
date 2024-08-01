import axios from "axios";
import {Advisor, AdvisorCreationDto, AdvisorUpdateDto} from "../models";

const baseURL = import.meta.env.VITE_API_URL as string;
const API_URL = `${baseURL}/advisors`;

export const fetchAdvisors = async (): Promise<Advisor[]> => {
  const response = await axios.get(API_URL);
  return response.data;
};

export const fetchAdvisor = async (id: number): Promise<Advisor> => {
  const response = await axios.get(`${API_URL}/${id}`);
  return response.data;
};

export const createAdvisor = async (
  advisor: AdvisorCreationDto,
): Promise<Advisor> => {
  const response = await axios.post(API_URL, advisor);
  return response.data;
};

export const updateAdvisor = async (id: number, advisor: AdvisorUpdateDto): Promise<void> => {
  await axios.put(`${API_URL}/${id}`, advisor);
};
export const deleteAdvisor = async (id: number): Promise<void> => {
  await axios.delete(`${API_URL}/${id}`);
};
