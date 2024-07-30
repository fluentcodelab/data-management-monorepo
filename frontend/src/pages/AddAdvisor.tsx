import React, { useState } from "react";
import { Form, Button } from "react-bootstrap";

const AddAdvisor: React.FC = () => {
  const [name, setName] = useState("");

  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault();
    console.log("New Advisor:", name);
    setName("");
  };

  return (
    <div>
      <h1 className="mb-4">Add Advisor</h1>
      <Form onSubmit={handleSubmit}>
        <Form.Group controlId="formAdvisorName">
          <Form.Label>Name</Form.Label>
          <Form.Control
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />
        </Form.Group>
        <Button variant="primary" type="submit" className="mt-3">
          Add Advisor
        </Button>
      </Form>
    </div>
  );
};

export default AddAdvisor;
