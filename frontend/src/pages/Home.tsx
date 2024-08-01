import React, { useState } from "react";
import { Button, Table } from "react-bootstrap";
import { PencilSquare, Trash } from "react-bootstrap-icons";
import AdvisorModal from "../components/AdvisorModal.tsx";
import { Advisor, AdvisorCreationDto, AdvisorUpdateDto } from "../models";
import ConfirmDeleteModal from "../components/ConfirmDeleteModal.tsx";
import { useAdvisors } from "../hooks/useAdvisors.ts";
import { AxiosError } from "axios";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const Home: React.FC = () => {
  const {
    advisors,
    isLoading,
    error,
    createAdvisor,
    updateAdvisor,
    deleteAdvisor,
  } = useAdvisors();
  const [showAdvisorModal, setShowAdvisorModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [selectedAdvisor, setSelectedAdvisor] = useState<Advisor | null>(null);
  const [advisorToDelete, setAdvisorToDelete] = useState<Advisor | null>(null);

  const handleCreate = async (advisor: AdvisorCreationDto) => {
    try {
      await createAdvisor({
        ...advisor,
        sin: advisor.sin.replace(/-/g, ""),
        phone: advisor.phone ? advisor.phone.replace(/-/g, "") : advisor.phone,
      });
    } catch (err) {
      if (err instanceof AxiosError) {
        toast.error(err!.response!.data[0].message || "An error occurred");
      }
    }
  };

  const handleUpdate = async (advisor: AdvisorUpdateDto) => {
    try {
      if (selectedAdvisor) {
        const updatedAdvisor = {
          ...advisor,
          phone: advisor.phone
            ? advisor.phone.replace(/-/g, "")
            : advisor.phone,
        };
        await updateAdvisor({
          id: selectedAdvisor.id,
          advisor: updatedAdvisor,
        });
      }
    } catch (err) {
      if (err instanceof AxiosError) {
        toast.error(err!.response!.data[0].message || "An error occurred");
      }
    }
  };

  const handleDelete = async () => {
    try {
      if (advisorToDelete) {
        await deleteAdvisor(advisorToDelete.id);
        setAdvisorToDelete(null);
      }
      setShowDeleteModal(false);
    } catch (err) {
      if (err instanceof AxiosError) {
        toast.error(err!.response!.data[0].message || "An error occurred");
      }
    }
  };

  const openAdvisorModal = (advisor: Advisor | null = null) => {
    setSelectedAdvisor(advisor);
    setShowAdvisorModal(true);
  };

  const closeAdvisorModal = () => {
    setShowAdvisorModal(false);
    setSelectedAdvisor(null);
  };

  const openDeleteModal = (advisor: Advisor) => {
    setAdvisorToDelete(advisor);
    setShowDeleteModal(true);
  };

  const closeDeleteModal = () => {
    setShowDeleteModal(false);
    setAdvisorToDelete(null);
  };

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error: {error.message}</div>;

  return (
    <div>
      <h1 className="mb-4">Advisors</h1>
      <Button
        variant="primary"
        onClick={() => openAdvisorModal()}
        className="mb-4"
      >
        Add Advisor
      </Button>
      {advisors && advisors.length == 0 && <div>No data available</div>}
      {advisors && advisors.length > 0 && (
        <Table striped bordered hover>
          <thead>
            <tr>
              <th>Name</th>
              <th>SIN</th>
              <th>Address</th>
              <th>Phone</th>
              <th>Health Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {advisors!.map((advisor) => (
              <tr key={advisor.id}>
                <td>
                  {advisor.lastName}, {advisor.firstName}
                </td>
                <td>{advisor.sin}</td>
                <td>{advisor.address}</td>
                <td>{advisor.phone}</td>
                <td style={{ color: advisor.healthStatus.toLowerCase() }}>
                  {advisor.healthStatus}
                </td>
                <td>
                  <Button
                    variant="outline-primary"
                    size="sm"
                    onClick={() => openAdvisorModal(advisor)}
                    className="me-2"
                  >
                    <PencilSquare />
                  </Button>
                  <Button
                    variant="outline-danger"
                    size="sm"
                    onClick={() => openDeleteModal(advisor)}
                  >
                    <Trash />
                  </Button>
                </td>
              </tr>
            ))}
          </tbody>
        </Table>
      )}

      <AdvisorModal
        show={showAdvisorModal}
        handleClose={closeAdvisorModal}
        handleSave={selectedAdvisor ? handleUpdate : handleCreate}
        advisorToEdit={selectedAdvisor}
      />

      <ConfirmDeleteModal
        show={showDeleteModal}
        handleClose={closeDeleteModal}
        handleConfirm={handleDelete}
        advisorName={
          advisorToDelete
            ? `${advisorToDelete.firstName} ${advisorToDelete.lastName}`
            : ""
        }
      />
      <ToastContainer />
    </div>
  );
};

export default Home;
