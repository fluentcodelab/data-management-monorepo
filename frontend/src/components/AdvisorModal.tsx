import React, { useEffect, useState } from "react";
import { Button, Form, Modal } from "react-bootstrap";
import { Advisor } from "../models";
import { z } from "zod";

const advisorSchema = z.object({
  firstName: z
    .string()
    .min(1, "first name is required")
    .max(255, "first name must be less than 255 characters"),
  lastName: z
    .string()
    .min(1, "last name is required")
    .max(255, "last name must be less than 255 characters"),
  sin: z
    .string()
    .regex(
      /^\d{3}-\d{3}-\d{3}$/,
      "SIN must be exactly 9 digits formatted as xxx-xxx-xxx",
    ),
  address: z
    .string()
    .max(255, "Address must be less than 255 characters")
    .optional()
    .or(z.literal("")),
  phone: z
    .string()
    .regex(
      /^\d{3}-\d{3}-\d{2}$/,
      "Phone must be exactly 8 digits formatted as xxx-xxx-xx",
    )
    .optional()
    .or(z.literal("")),
});

type FormData = z.infer<typeof advisorSchema>;

interface AddAdvisorModalProps {
  show: boolean;
  handleClose: () => void;
  handleSave: (advisor: Advisor) => void;
  advisorToEdit?: Advisor | null;
}

const newAdvisor: FormData = {
  firstName: "",
  lastName: "",
  sin: "",
  address: "",
  phone: "",
};

const AdvisorModal: React.FC<AddAdvisorModalProps> = ({
  show,
  handleClose,
  handleSave,
  advisorToEdit,
}) => {
  const [advisor, setAdvisor] = useState<FormData>(newAdvisor);
  const [errors, setErrors] = useState<z.ZodIssue[]>([]);

  useEffect(() => {
    if (advisorToEdit) {
      setAdvisor(advisorToEdit);
    } else {
      setAdvisor(newAdvisor);
    }
  }, [advisorToEdit]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    if (name === "sin") {
      const formattedValue = value.replace(/\D/g, "").slice(0, 9);
      const maskedValue = formattedValue.replace(
        /(\d{3})(\d{3})(\d{3})/,
        "$1-$2-$3",
      );
      setAdvisor({ ...advisor, sin: maskedValue });
    } else if (name === "phone") {
      const formattedValue = value.replace(/\D/g, "").slice(0, 8);
      const maskedValue = formattedValue.replace(
        /(\d{3})(\d{3})(\d{2})/,
        "$1-$2-$3",
      );
      setAdvisor({ ...advisor, phone: maskedValue });
    } else {
      setAdvisor({ ...advisor, [name]: value });
    }
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const result = advisorSchema.safeParse(advisor);
    if (result.success) {
      handleSave(advisor as Advisor);
      handleClose();
      setAdvisor(newAdvisor)
    } else {
      setErrors(result.error.issues);
    }
  };

  const getError = (path: string) => {
    return errors.find((error) => error.path.includes(path))?.message;
  };

  return (
    <Modal show={show} onHide={handleClose}>
      <Modal.Header closeButton>
        <Modal.Title>
          {advisorToEdit ? "Edit Advisor" : "Create Advisor"}
        </Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form onSubmit={handleSubmit}>
          <Form.Group controlId="formAdvisorFirstName">
            <Form.Label>First Name</Form.Label>
            <Form.Control
              type="text"
              name="firstName"
              value={advisor.firstName}
              onChange={handleChange}
              isInvalid={!!getError("firstName")}
            />
            <Form.Control.Feedback type="invalid">
              {getError("firstName")}
            </Form.Control.Feedback>
          </Form.Group>
          <Form.Group controlId="formAdvisorLastName" className="mt-3">
            <Form.Label>Last Name</Form.Label>
            <Form.Control
              type="text"
              name="lastName"
              value={advisor.lastName}
              onChange={handleChange}
              isInvalid={!!getError("lastName")}
            />
            <Form.Control.Feedback type="invalid">
              {getError("lastName")}
            </Form.Control.Feedback>
          </Form.Group>
          <Form.Group controlId="formAdvisorSIN" className="mt-3">
            <Form.Label>SIN</Form.Label>
            <Form.Control
              type="text"
              name="sin"
              value={advisor.sin}
              onChange={handleChange}
              isInvalid={!!getError("sin")}
            />
            <Form.Control.Feedback type="invalid">
              {getError("sin")}
            </Form.Control.Feedback>
          </Form.Group>
          <Form.Group controlId="formAdvisorAddress" className="mt-3">
            <Form.Label>Address</Form.Label>
            <Form.Control
              type="text"
              name="address"
              value={advisor.address}
              onChange={handleChange}
              isInvalid={!!getError("address")}
            />
          </Form.Group>
          <Form.Group controlId="formAdvisorPhone" className="mt-3">
            <Form.Label>Phone</Form.Label>
            <Form.Control
              type="text"
              name="phone"
              value={advisor.phone}
              onChange={handleChange}
              isInvalid={!!getError("phone")}
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
