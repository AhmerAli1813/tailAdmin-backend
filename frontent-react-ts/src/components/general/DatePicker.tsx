import * as React from "react";
import { Control, Controller } from "react-hook-form";
import InputField from "./InputField";

interface DatePickerProps {
  control?: Control<any, any>; // Control from react-hook-form
  ClassName?: string;
  inputName: string; // Made this required to align with react-hook-form requirements
  label?: string; // Optional label for the date picker
  placeholder?: string; // Optional placeholder
  Disabled?: boolean; // Disables the date picker
  error?: string; // Optional error message
  Required?: boolean;
}

const DatePicker: React.FC<DatePickerProps> = ({
  ClassName = "custom-input-date custom-input-date-1 w-full rounded border-[1.5px] border-stroke bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary dark:border-form-strokedark dark:bg-form-input dark:focus:border-primary",
  inputName,
  control,
  label,
  Disabled = false,
  error,
  Required
}) => {
  // Dynamic classes for the input field
  const inputClassName = `${ClassName} ${
    error ? "border-danger border-red-500" : "border-gray-300"
  } rounded-lg focus:outline-none focus:ring-2 ${
    error ? "focus:ring-red-500" : "focus:ring-blue-500"
  }`;

  return (
    <div className="w-full mb-3">
      {/* Label */}
      <label className='mb-3 block text-black dark:text-white"'>{label} {Required && <span className='text-red-500'>*</span>}</label>
  
      {/* ReactDatePicker Component */}
      <div className="relative">
        <Controller
          name={inputName}
          control={control}
          render={({ field }) => (
            <InputField
            inputType="date"
              inputName={inputName}
              {...field}
              error={error}
              Disabled={Disabled}
              ClassName={inputClassName}
              control={control}
            />
          )}
        />
      </div>
      {error && <span className='text-danger'>{error}</span>}
    </div>
  );
};

export default DatePicker;
