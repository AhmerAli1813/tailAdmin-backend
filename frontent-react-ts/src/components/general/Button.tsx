import { ReactNode } from "react";
import BtnSpinner from "./btnSpinner";

interface IProps {
  variant?: 'primary' | 'secondary' | 'danger' | 'light';
  type: 'submit' | 'button';
  label?: string;
  ClassName?: string;
  onClick: () => void;
  loading?: boolean;
  disabled?: boolean;
  Children?:ReactNode
}

const Button = ({ variant, type, label, onClick, loading, disabled,ClassName ,Children}: IProps) => {
  const primaryClasses = ' btn btn-primary ';

  const secondaryClasses = ' btn btn-secondary ';

  const dangerClasses = ' btn btn-danger ';

  const lightClasses = ' btn btn-light ';

  const classNameCreator = (): string => {
    let finalClassName =' ';
    if (variant === 'primary') {
      finalClassName += primaryClasses;
    } else if (variant === 'secondary') {
      finalClassName += secondaryClasses;
    } else if (variant === 'danger') {
      finalClassName += dangerClasses;
    } else if (variant === 'light') {
      finalClassName += lightClasses;
    }
    finalClassName += ClassName+' disabled:shadow-none disabled:bg-gray-300 disabled:border-gray-300';
    return finalClassName;
  };

  const loadingIconCreator = () => {
    return <BtnSpinner/>
  };

  return (
    <button type={type} onClick={onClick} className={classNameCreator()} disabled={disabled}>
      {Children}
      {loading ? loadingIconCreator() : label}
    </button>
  );
};

export default Button;
