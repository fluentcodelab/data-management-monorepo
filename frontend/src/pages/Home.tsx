import React, { useState } from "react";
import { Button, Table } from "react-bootstrap";
import { PencilSquare, Trash } from "react-bootstrap-icons";
import AdvisorModal from "../components/AdvisorModal.tsx";
import { useAdvisors } from "../hooks/useAdvisors.ts";
import { AdvisorDto } from "../api";

const Home: React.FC = () => {
  const {
    advisors,
    isLoading,
    addAdvisor,
    updateAdvisor,
    deleteAdvisor,
  } = useAdvisors();

  const [showModal, setShowModal] = useState(false);
  const [selectedAdvisor, setSelectedAdvisor] = useState<AdvisorDto | null>(
    null,
  );
  // const [showDeleteModal, setShowDeleteModal] = useState(false);
  // const [advisorToDelete, setAdvisorToDelete] = useState(null);

  const handleSave = (advisor: AdvisorDto) => {
    if (selectedAdvisor) {
      updateAdvisor.mutate(advisor);
    } else {
      addAdvisor.mutate(advisor);
    }
    // setShowModal(false);
    // setSelectedAdvisor(null);
  };

  // const handleDelete = () => {
  //   if (advisorToDelete) {
  //     deleteAdvisor.mutate(advisorToDelete.id);
  //     setShowDeleteModal(false);
  //     setAdvisorToDelete(null);
  //   }
  // };

  return (
    <div>
      <h1 className="mb-4">Advisors</h1>
      <Button
        variant="primary"
        onClick={() => {
          // setAdvisorToEdit(null);
          setShowModal(true);
        }}
        className="mb-4"
      >
        Add Advisor
      </Button>
      {isLoading && <p>Loading...</p>}
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
          {advisors.map((advisor) => (
            <tr key={advisor.id}>
              <td>
                {advisor.lastName}, {advisor.firstName}
              </td>
              <td>{advisor.sin}</td>
              <td>{advisor.address}</td>
              <td>{advisor.phone}</td>
              <td style={{ color: advisor.healthStatus!.toLowerCase() }}>
                {advisor.healthStatus}
              </td>
              <td>
                <Button
                  variant="outline-primary"
                  size="sm"
                  onClick={() => {
                    setSelectedAdvisor(advisor);
                    setShowModal(true);
                  }}
                  className="me-2"
                >
                  <PencilSquare />
                </Button>
                <Button
                  variant="outline-danger"
                  size="sm"
                  onClick={() => deleteAdvisor.mutate(advisor.id!.toString())}
                >
                  <Trash />
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

      <AdvisorModal
        show={showModal}
        handleClose={() => {
          setShowModal(false);
          setSelectedAdvisor(null);
        }}
        handleSave={handleSave}
      />

      {/*<ConfirmDeleteModal*/}
      {/*  show={showConfirmDeleteModal}*/}
      {/*  handleClose={() => setShowConfirmDeleteModal(false)}*/}
      {/*  handleConfirm={handleDelete}*/}
      {/*  advisorName={*/}
      {/*    advisorToDelete*/}
      {/*      ? `${advisorToDelete.firstName} ${advisorToDelete.lastName}`*/}
      {/*      : ""*/}
      {/*  }*/}
      {/*/>*/}
    </div>
  );
};

export default Home;
