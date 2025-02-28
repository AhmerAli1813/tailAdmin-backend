import React from "react";
import Select, { Props as SelectProps } from "react-select";
import { Control, Controller } from "react-hook-form";

export interface Option {
  value: string | number;
  label: string;
}

interface Select2Props extends SelectProps<Option> {
  control?: Control<any, any>; // Control from react-hook-form
  options: Option[]; // Array of dropdown options
  isMulti?: boolean; // Support for multiple selections
  placeholder?: string; // Placeholder text
  error?: string; // Error message to display
  label?: string; // Label for the dropdown
  inputName: string; // Name of the input field
  ClassName?: string; // Additional class names
  Required?: boolean;
  DefaultValue?: string;
}

const Select2: React.FC<Select2Props> = ({
  control,
  options,
  isMulti = false,
  placeholder = "Select an option",
  error,
  label,
  Required,
  inputName,
  DefaultValue,
  ClassName,
  ...rest
}) => {
  const dynamicClassName = error ? `${ClassName} border-danger border-red-500` : `${ClassName} border-[#754eb477]`;

  // const renderTopRow = () => {
  //   if (error) {
  //     return <span className='text-danger'>{error}</span>;
  //   }
  //   if (label) {
  //     return <label className='  font-semibold '>{label} {Required && <span className='text-red-500'>*</span>}</label>;
  //   }
  //   return null;
  // };

  return (
    <div className="">
      {/* {renderTopRow()} */}
      <label className='  font-semibold '>{label} {Required && <span className='text-red-500'>*</span>}</label>

      <Controller
        name={inputName}
        control={control}
        defaultValue={DefaultValue}
        render={({ field }) => (
          <Select
            {...field}
            options={options}
            isMulti={isMulti}
            placeholder={placeholder}
            classNamePrefix={"react-select "}
            className={"mt-2" + dynamicClassName}
            onChange={(selectedOption) => {
              // Transform selected value
              const value = isMulti
                ? (selectedOption as Option[]).map((option) => option.value) // Return array of values for multi-select
                : (selectedOption as Option)?.value; // Return a single value for single-select
              field.onChange(value);
            }}
            value={
              isMulti
                ? options.filter((option) => (field.value || []).includes(option.value)) // Multi-select value
                : options.find((option) => option.value === field.value) // Single-select value
            }
            {...rest}
          />
        )}
      />
      {error && <span className='text-danger'>{error}</span>}

    </div>
  );
};

export default Select2;

