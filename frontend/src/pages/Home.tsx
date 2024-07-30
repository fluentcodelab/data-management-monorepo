import React from "react";
import { Table } from "react-bootstrap";

const Home: React.FC = () => {
  const advisors = [
    { id: 1, name: "John Doe" },
    { id: 2, name: "Jane Smith" },
    { id: 3, name: "Alice Johnson" },
  ];

  return (
    <div>
      <h1 className="mb-4">Advisors</h1>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
          </tr>
        </thead>
        <tbody>
          {advisors.map((advisor) => (
            <tr key={advisor.id}>
              <td>{advisor.id}</td>
              <td>{advisor.name}</td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  );
};

export default Home;
