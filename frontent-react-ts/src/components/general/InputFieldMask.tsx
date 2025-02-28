import { forwardRef } from "react";
import { Control, Controller } from "react-hook-form";
import InputMask, { Props as InputMaskProps } from "react-input-mask";

interface IProps {
  control?: Control<any, any>;
  label?: string;
  inputName: string;
  mask: string; // Mask pattern (e.g., "99999-9999999-9")
  error?: string;
  ClassName?: string;
  Placeholder?: string;
  Required?: boolean;
  Value?: any;
  Disabled?: boolean;
}

// Fix findDOMNode warning by using forwardRef
const InputMaskWrapper = forwardRef<HTMLInputElement, InputMaskProps>(
  ({ mask, ...props }, ref) => <InputMask {...props} mask={mask} inputRef={ref} />
);
InputMaskWrapper.displayName = "InputMaskWrapper";

const InputFieldMask = ({
  control,
  label,
  inputName,
  mask,
  Value,
  Disabled,
  Placeholder,
  error,
  ClassName,
  Required,
  ...rest
}: IProps) => {
  const baseClass =
    "w-full bg-white placeholder:text-slate-400 text-slate-700 text-sm border border-slate-200 rounded-md px-3 py-2 transition duration-300 ease focus:outline-none focus:border-slate-400 hover:border-slate-300 shadow-sm focus:shadow";

  const dynamicClassName = `${baseClass} ${error ? "border-danger rounded-lg" : "border-[#754eb477]"
    }`;
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
    <div className="w-full mb-3">
      {/* {renderTopRow()} */}
      <label className='  font-semibold '>{label} {Required && <span className='text-red-500'>*</span>}</label>

      <Controller
        name={inputName}
        control={control}
        defaultValue={Value}
        render={({ field }) => (
          <InputMaskWrapper
            {...field}
            {...rest}
            mask={mask} // Ensure mask is passed correctly
            disabled={Disabled}
            placeholder={Placeholder}
            autoComplete="off"
            className={dynamicClassName}
          />
        )}
      />
      {error && <span className='text-danger'>{error}</span>}

    </div>
  );
};

export default InputFieldMask;
