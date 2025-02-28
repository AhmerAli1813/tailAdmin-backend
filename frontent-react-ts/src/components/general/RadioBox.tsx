import { Control, Controller } from 'react-hook-form';

interface IRadioBoxProps {
  control: Control<any, any>;
  label?: string;
  inputName: string;
  error?: string;
  ClassName?: string;
  options: { value: string | number; label: string }[]; // Options for the radio group
}

const RadioBox = ({ control, label, inputName, error, ClassName, options }: IRadioBoxProps) => {
  const renderTopRow = () => {
    if (error) {
      return <span className='text-danger'>{error}</span>;
    }
    if (label) {
      return <label className='form-label font-semibold'>{label}</label>;
    }
    return null;
  };

  const dynamicClassName = error ? `${ClassName} border-danger rounded-lg` : `${ClassName} border-[#754eb477]`;

  return (
    <div className=''>
      {renderTopRow()}
      <Controller
        name={inputName}
        control={control}
        render={({ field }) => (
          <div>
            {options.map((option) => (
              <label key={option.value} className="flex items-center space-x-2">
                <input
                  type="radio"
                  value={option.value}
                  checked={field.value === option.value}
                  onChange={() => field.onChange(option.value)}
                  className={`form-radio ${dynamicClassName}`}
                />
                <span>{option.label}</span>
              </label>
            ))}
          </div>
        )}
      />
    </div>
  );
};

export default RadioBox;
