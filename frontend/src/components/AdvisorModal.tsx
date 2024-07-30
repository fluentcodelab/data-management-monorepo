import React, { useEffect, useState } from "react";
import { Button, Form, Modal } from "react-bootstrap";
import { Advisor, HealthStatus } from "../models";

interface AddAdvisorModalProps {
  show: boolean;
  handleClose: () => void;
  handleSave: (advisor: Advisor) => void;
  advisorToEdit?: Advisor | null;
}

const newAdvisor: Advisor = {
  firstName: "",
  lastName: "",
  email: "",
  sin: "",
  address: "",
  phone: "",
  healthStatus: HealthStatus.Green,
};

const AdvisorModal: React.FC<AddAdvisorModalProps> = ({
  show,
  handleClose,
  handleSave,
  advisorToEdit,
}) => {
  const [advisor, setAdvisor] = useState<Advisor>(newAdvisor);

  useEffect(() => {
    if (advisorToEdit) {
      setAdvisor(advisorToEdit);
    } else {
      setAdvisor(newAdvisor);
    }
  }, [advisorToEdit]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    handleSave(advisor);
    handleClose();
  };

  return (
    <Modal show={show} onHide={handleClose}>
      <Modal.Header closeButton>
        <Modal.Title>Create Advisor</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form onSubmit={handleSubmit}>
          <Form.Group controlId="formAdvisorFirstName">
            <Form.Label>First Name</Form.Label>
            <Form.Control
              type="text"
              value={advisor.firstName}
              onChange={(e) =>
                setAdvisor({ ...advisor, firstName: e.target.value })
              }
              required
            />
          </Form.Group>
          <Form.Group controlId="formAdvisorLastName" className="mt-3">
            <Form.Label>Last Name</Form.Label>
            <Form.Control
              type="text"
              value={advisor.lastName}
              onChange={(e) =>
                setAdvisor({ ...advisor, lastName: e.target.value })
              }
              required
            />
          </Form.Group>
          <Form.Group controlId="formAdvisorEmail" className="mt-3">
            <Form.Label>Email</Form.Label>
            <Form.Control
              type="email"
              value={advisor.email}
              onChange={(e) =>
                setAdvisor({ ...advisor, email: e.target.value })
              }
              required
            />
          </Form.Group>
          <Form.Group controlId="formAdvisorSIN" className="mt-3">
            <Form.Label>SIN</Form.Label>
            <Form.Control
              type="text"
              value={advisor.sin}
              onChange={(e) => setAdvisor({ ...advisor, sin: e.target.value })}
              required
            />
          </Form.Group>
          <Form.Group controlId="formAdvisorAddress" className="mt-3">
            <Form.Label>Address</Form.Label>
            <Form.Control
              type="text"
              value={advisor.address}
              onChange={(e) =>
                setAdvisor({ ...advisor, address: e.target.value })
              }
              required
            />
          </Form.Group>
          <Form.Group controlId="formAdvisorPhone" className="mt-3">
            <Form.Label>Phone</Form.Label>
            <Form.Control
              type="text"
              value={advisor.phone}
              onChange={(e) =>
                setAdvisor({ ...advisor, phone: e.target.value })
              }
              required
            />
          </Form.Group>
          <Button variant="primary" type="submit" className="mt-3">
            Save
          </Button>
        </Form>
      </Modal.Body>
    </Modal>
  );
};

export default AdvisorModal;
