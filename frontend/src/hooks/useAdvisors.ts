import { useQuery, useMutation, useQueryClient } from "react-query";
import config from "../api/config";
import { AdvisorDto, AdvisorsApi } from "../api";

const advisorApi = new AdvisorsApi(config);

export const useAdvisors = () => {
  const queryClient = useQueryClient();

  const { data, error, isLoading } = useQuery("advisors", () =>
    advisorApi.apiV1AdvisorsGet(),
  );

  const addAdvisor = useMutation(
    (newAdvisor: AdvisorDto) =>
      advisorApi.apiV1AdvisorsPost({ advisorCreationDto: newAdvisor }),
    {
      onSuccess: () => {
        queryClient.invalidateQueries("advisors");
      },
    },
  );

  const updateAdvisor = useMutation(
    (updatedAdvisor: AdvisorDto) =>
      advisorApi.apiV1AdvisorsIdPut({
        id: updatedAdvisor.id!,
        advisorUpdateDto: updatedAdvisor,
      }),
    {
      onSuccess: () => {
        queryClient.invalidateQueries("advisors");
      },
    },
  );

  const deleteAdvisor = useMutation(
    (id: string) => advisorApi.apiV1AdvisorsIdDelete({ id: parseInt(id) }),
    {
      onSuccess: () => {
        queryClient.invalidateQueries("advisors");
      },
    },
  );

  return {
    advisors: data ?? [],
    isLoading,
    error,
    addAdvisor,
    updateAdvisor,
    deleteAdvisor,
  };
};
