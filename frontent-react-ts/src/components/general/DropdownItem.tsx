import React from "react";
import { Link } from "react-router-dom";

interface DropdownItemProps {
  label: string | React.ReactNode;
  icon?: React.ReactNode;
  to: string;
  className?: string;
  onClick?: () => void;
}

const DropdownItem: React.FC<DropdownItemProps> = ({ label, icon, to, className = "", onClick }) => {
  return (
    <li>
      <Link to={to} className={`dropdown-item ${className}`} onClick={onClick}>
        {icon && <span className="me-2">{icon}</span>}
        {label}
      </Link>
    </li>
  );
};

export default DropdownItem;
