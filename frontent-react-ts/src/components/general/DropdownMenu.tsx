import * as React from "react";
import { useState } from "react";

interface DropdownMenuProps {
  label: string | React.ReactNode;
  icon: React.ReactNode;
  to?: string;
  children?: React.ReactNode;
  className?: string;
}

const DropdownMenu: React.FC<DropdownMenuProps> = ({ label, icon, to = "#", children, className = "" }) => {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <div className={`dropdown ${className}`} onMouseLeave={() => setIsOpen(false)}>
      <a
        href={to}
        className="dropdown-toggle"
        data-bs-toggle="dropdown"
        onClick={(e) => {
          e.preventDefault();
          setIsOpen(!isOpen);
        }}
      >
        <span className="me-2">{icon}</span>
        <span>{label}</span>
      </a>

      {isOpen && (
        <ul className="dropdown-menu dropdown-menu-end show">
          {children}
        </ul>
      )}
    </div>
  );
};

export default DropdownMenu;
