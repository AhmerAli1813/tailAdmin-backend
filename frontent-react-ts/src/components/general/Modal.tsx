import * as React from "react";
import { ReactNode } from "react";
import ReactModal from "react-modal";

interface ModalProps {
  isOpen: boolean;
  size?: "xs" | "sm" | "md" | "lg" | "xl" | "xxl";  // <--- size prop
  onClose: () => void;
  children?: ReactNode;   // Make children optional
  title?: string;
  ClassName?: string;
  HeaderClassName?: string;
  BodyClassName?: string;
  overlayClassName?: string;
}

const sizeClasses: Record<NonNullable<ModalProps["size"]>, string> = {
  xs: "max-w-xs",
  sm: "max-w-sm",
  md: "max-w-md",
  lg: "max-w-lg",    // Default
  xl: "max-w-xl",
  xxl: "max-w-2xl",
};

const Modal: React.FC<ModalProps> = ({
  isOpen,
  size = "lg",             // Default size
  onClose,
  children,
  title,
  overlayClassName = "",
  ClassName = "",
  BodyClassName = "",
  HeaderClassName = "",
}) => {
  // Pick the corresponding size class
  const sizeClass = sizeClasses[size];

  return (
    <ReactModal
      isOpen={isOpen}
      onRequestClose={onClose}
      contentLabel={title || "Modal"}
      ariaHideApp={false}
      // Combine user-defined classes + size class
      className={`${ClassName} modal-content ${sizeClass}`}
      // Combine user-defined overlay classes
      overlayClassName={`${overlayClassName} modal-overlay`}
    >
      <div className={`${HeaderClassName} modal-header`}>
        {title && <h5 className="modal-title">{title}</h5>}
        <button type="button" className="close-btn" onClick={onClose}>
          &times;
        </button>
      </div>
      <div className={`${BodyClassName} modal-body`}>{children}</div>
    </ReactModal>
  );
};

export default Modal;
