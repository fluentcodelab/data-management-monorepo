import React, { useState } from "react";
import { Button, Table } from "react-bootstrap";
import { PencilSquare, Trash } from "react-bootstrap-icons";
import AdvisorModal from "../components/AdvisorModal.tsx";
import { Advisor, HealthStatus } from "../models";
import ConfirmDeleteModal from "../components/ConfirmDeleteModal.tsx";

const initialAdvisors = [
  {
    id: 1,
    firstName: "John",
    lastName: "Doe",
    sin: "123-456-789",
    address: "123 Main St, City, Country",
    phone: "123-456-7890",
    healthStatus: HealthStatus.Green,
  },
  {
    id: 2,
    firstName: "Jane",
    lastName: "Smith",
    sin: "987-654-321",
    address: "456 Elm St, City, Country",
    phone: "098-765-4321",
    healthStatus: HealthStatus.Yellow,
  },
  {
    id: 3,
    firstName: "Alice",
    lastName: "Johnson",
    sin: "555-666-777",
    address: "789 Oak St, City, Country",
    phone: "555-666-7777",
    healthStatus: HealthStatus.Red,
  },
];

const Home: React.FC = () => {
  const [advisors, setAdvisors] = useState<Advisor[]>(initialAdvisors);
  const [showModal, setShowModal] = useState(false);
  const [advisorToEdit, setAdvisorToEdit] = useState<Advisor | null>(null);
  const [showConfirmDeleteModal, setShowConfirmDeleteModal] = useState(false);
  const [advisorToDelete, setAdvisorToDelete] = useState<Advisor | null>(null);

  const handleSave = (advisor: Advisor) => {
    if (advisor.id) {
      setAdvisors(advisors.map((a) => (a.id === advisor.id ? advisor : a)));
    } else {
      const newId = advisors.length
        ? Math.max(...advisors.map((a) => a.id!)) + 1
        : 1;
      setAdvisors([...advisors, { id: newId, ...advisor }]);
    }
  };
  const handleEdit = (advisor: Advisor) => {
    setAdvisorToEdit(advisor);
    setShowModal(true);
  };

  const handleDelete = () => {
    if (advisorToDelete) {
      setAdvisors(advisors.filter((a) => a.id !== advisorToDelete.id));
      setAdvisorToDelete(null);
    }
    setShowConfirmDeleteModal(false);
  };

  const confirmDelete = (advisor: Advisor) => {
    setAdvisorToDelete(advisor);
    setShowConfirmDeleteModal(true);
  };

  return (
    <div>
      <h1 className="mb-4">Advisors</h1>
      <Button
        variant="primary"
        onClick={() => {
          setAdvisorToEdit(null);
          setShowModal(true);
        }}
        className="mb-4"
      >
        Add Advisor
      </Button>
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
              <td>{advisor.lastName}, {advisor.firstName}</td>
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
                  onClick={() => handleEdit(advisor)}
                  className="me-2"
                >
                  <PencilSquare />
                </Button>
                <Button
                  variant="outline-danger"
                  size="sm"
                  onClick={() => confirmDelete(advisor)}
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
        handleClose={() => setShowModal(false)}
        handleSave={handleSave}
        advisorToEdit={advisorToEdit}
      />

      <ConfirmDeleteModal
        show={showConfirmDeleteModal}
        handleClose={() => setShowConfirmDeleteModal(false)}
        handleConfirm={handleDelete}
        advisorName={advisorToDelete ? `${advisorToDelete.firstName} ${advisorToDelete.lastName}` : ""}
      />
    </div>
  );
};

export default Home;
