import React, { ReactNode, useRef, useState } from "react";
import { useNavigate } from "react-router-dom";

interface MenuItemProps {
  label: string | ReactNode; // Label for the menu item
  icon: ReactNode; // Icon to display
  to: string; // Navigation path
  children?: ReactNode; // Sub-menu items or additional content
  Access?: string[]; // Roles that have access to this menu item
  userRoles?: string[]; // Current user's roles
  ClassName?: string;
}

const MenuItem: React.FC<MenuItemProps> = ({ label, icon, to = "#", children, Access, userRoles }) => {
  const navigate = useNavigate();
  const [isOpen, setIsOpen] = useState(false);
  const menuRef = useRef<HTMLUListElement | null>(null);

  // Check if the user has access to this menu item
  const hasAccess = !Access || (userRoles && Access.some((role) => userRoles.includes(role)));

  const toggleMenu = (e: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => {
    e.preventDefault();

    if (children) {
      setIsOpen((prev) => !prev);

      if (menuRef.current) {
        if (isOpen) {
          menuRef.current.style.maxHeight = "0px";
        } else {
          menuRef.current.style.maxHeight = `${menuRef.current.scrollHeight}px`;
        }
      }
    } else {
      navigate(to);
    }
  };

  if (!hasAccess) {
    return null; // Hide the menu item if the user doesn't have access
  }

  return (
    <li className={`slide ${isOpen ? "is-expanded" : ""}`}>
      {/* Main Menu Item */}
      <a className="side-menu__item" data-bs-toggle="slide" href="#" onClick={toggleMenu}>
        {icon && <span className="side-menu__icon me-2">{icon}</span>}
        <span className="side-menu__label">{label}</span>
        {children && <i className={`angle fa fa-angle-${isOpen ? "down" : "right"}`} />}
      </a>

      {/* Sub-menu */}
      {children && (
        <ul
          ref={menuRef}
          className="slide-menu"
          style={{ maxHeight: isOpen ? "500px" : "0px", overflow: "hidden", transition: "max-height 0.3s ease-in-out" }}
        >
          {children}
        </ul>
      )}
    </li>
  );
};

export default MenuItem;



