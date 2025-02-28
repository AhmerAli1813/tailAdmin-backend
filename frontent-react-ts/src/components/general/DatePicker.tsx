import React from "react";
import ReactDatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { Control, Controller } from "react-hook-form";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {  faCalendarDay } from '@fortawesome/free-solid-svg-icons';

interface DatePickerProps {
  control?: Control<any, any>; // Control from react-hook-form
  ClassName?: string;
  inputName: string; // Made this required to align with react-hook-form requirements
  selectedDate?: Date | null; // The currently selected date
  onChange?: (date: Date | null) => void; // Callback when the date changes
  label?: string; // Optional label for the date picker
  placeholder?: string; // Optional placeholder
  minDate?: Date; // Minimum selectable date
  maxDate?: Date; // Maximum selectable date
  isClearable?: boolean; // Allows clearing the selected date
  disabled?: boolean; // Disables the date picker
  error?: string; // Optional error message
  Required?:boolean;
}

const DatePicker: React.FC<DatePickerProps> = ({
  ClassName = "w-full bg-white placeholder:text-slate-400 text-slate-700 text-sm border border-slate-200 rounded-md px-3 py-2 transition duration-300 ease focus:outline-none focus:border-slate-400 hover:border-slate-300 shadow-sm focus:shadow",
  inputName,
  control,
  selectedDate,
  onChange,
  label,
  placeholder = "dd/MM/yyyy",
  minDate,
  maxDate,
  isClearable = false,
  disabled = false,
  error,
  Required
}) => {
  // Dynamic classes for the input field
  const inputClassName = `${ClassName} mt-2 form-control ${
    error ? "border-danger border-red-500" : "border-gray-300"
  } rounded-lg focus:outline-none focus:ring-2 ${
    error ? "focus:ring-red-500" : "focus:ring-blue-500"
  }`;
  // const renderTopRow = () => {
  //   if (error) {
  //     return <span className='text-danger'>{error}</span>;
  //   }
  //   if (label) {
  //     return <label className='  font-semibold  '>{label} {Required && <span className='text-red-500'>*</span>}</label>;
  //   }
  //   return null;
  // };
  return (
    <div className="w-full mb-3">
      {/* Label */}
      {/* {renderTopRow()} */}
      <label className='  font-semibold '>{label} {Required && <span className='text-red-500'>*</span>}</label>
  
      {/* ReactDatePicker Component */}
      <div className="position-relative">
        <Controller
          name={inputName}
          control={control}
          render={({ field }) => (
            <ReactDatePicker
              id={inputName}
              {...field}
              selected={selectedDate || field.value} // Ensure controlled input
              onChange={(date) => {
                field.onChange(date); // Update react-hook-form field value
                if (onChange) onChange(date); // Trigger external onChange callback
              }}
              placeholderText={placeholder}
              minDate={minDate}
              maxDate={maxDate}
              dateFormat="dd/MM/yyyy"
              disabled={disabled}
              isClearable={isClearable}
              className={inputClassName}
            />
          )}
        />
        {/* Calendar Icon */}
        <span className="input-group-addon">
          <FontAwesomeIcon icon={faCalendarDay} className="text-gray-400" />
        </span>
      </div>
      {error && <span className='text-danger'>{error}</span>}

 
    </div>
  );
};

export default DatePicker;
