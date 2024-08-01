import { useQuery, useMutation, useQueryClient } from "react-query";
import { Advisor, AdvisorUpdateDto } from "../models";
import {
  createAdvisor,
  deleteAdvisor,
  fetchAdvisor,
  fetchAdvisors,
  updateAdvisor,
} from "../services/advisorService.ts";

export const useAdvisors = () => {
  const queryClient = useQueryClient();

  const {
    data: advisors,
    error,
    isLoading,
  } = useQuery<Advisor[], Error>("advisors", fetchAdvisors);

  const createAdvisorMutation = useMutation(createAdvisor, {
    onSuccess: () => {
      queryClient.invalidateQueries("advisors");
    },
  });

  const updateAdvisorMutation = useMutation(
    (data: { id: number; advisor: AdvisorUpdateDto }) =>
      updateAdvisor(data.id, data.advisor),
    {
      onSuccess: () => {
        queryClient.invalidateQueries("advisors");
      },
    },
  );

  const deleteAdvisorMutation = useMutation(deleteAdvisor, {
    onSuccess: () => {
      queryClient.invalidateQueries("advisors");
    },
  });

  return {
    advisors,
    error,
    isLoading,
    createAdvisor: createAdvisorMutation.mutateAsync,
    updateAdvisor: updateAdvisorMutation.mutateAsync,
    deleteAdvisor: deleteAdvisorMutation.mutateAsync,
  };
};

export const useAdvisor = (id: number) => {
  const {
    data: advisor,
    error,
    isLoading,
  } = useQuery<Advisor, Error>(["advisor", id], () => fetchAdvisor(id));

  return {
    advisor,
    error,
    isLoading,
  };
};
