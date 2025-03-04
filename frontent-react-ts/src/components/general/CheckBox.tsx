import { Control, Controller } from 'react-hook-form';

interface ICheckBoxProps {
  control: Control<any, any>;
  label?: string;
  inputName: string;
  error?: string;
  ClassName?: string;
}

const CheckBox = ({ control, label, inputName, error, ClassName }: ICheckBoxProps) => {
  const renderTopRow = () => {
    if (error) {
      return <span className='ms-1 text-danger'>{error}</span>;
    }
    if (label) {
      return <label htmlFor={inputName}  className='flex cursor-pointer select-none items-center'>{label}</label>;
    }
    return null;
  };

  const dynamicClassName = error ? `${ClassName} border-danger rounded-lg` : `${ClassName} border-[#754eb477]`;

  return (
    <div className='form-check form-check-inline'>
      <Controller
        name={inputName}
        control={control}
        render={({ field }) => (
          
          <input
          type="checkbox"
          id={inputName}
          {...field}
          className={`form-check-input ${dynamicClassName}`}
          />
        )}
        />
        {renderTopRow()}
    </div>
  );
};

export default CheckBox;
