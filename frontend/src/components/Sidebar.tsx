import React from "react";
import { Link } from "react-router-dom";
import { Nav } from "react-bootstrap";

const Sidebar: React.FC = () => {
  return (
    <Nav className="flex-column">
      <Nav.Link as={Link} to="/">
        Home
      </Nav.Link>
      <Nav.Link as={Link} to="/add-advisor">
        Add Advisor
      </Nav.Link>
    </Nav>
  );
};

export default Sidebar;
