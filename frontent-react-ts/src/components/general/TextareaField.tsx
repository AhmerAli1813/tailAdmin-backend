import { Control, Controller } from 'react-hook-form';

interface ITextareaFieldProps {
  control: Control<any, any>;
  label?: string;
  inputName: string;
  error?: string;
  ClassName?: string;
  Placeholder?: string;
  rows?: number;
  Required?: boolean
}

const TextareaField = ({ control, label, inputName, error, ClassName, Placeholder, Required, rows = 3 }: ITextareaFieldProps) => {
  ClassName = "w-full bg-white placeholder:text-slate-400 text-slate-700 text-sm border border-slate-200 rounded-md px-3 py-2 transition duration-300 ease focus:outline-none focus:border-slate-400 hover:border-slate-300 shadow-sm focus:shadow";

  // const renderTopRow = () => {
  //   if (error) {
  //     return <span className='text-danger'>{error}</span>;
  //   }
  //   if (label) {
  //     return <label className='form-label font-semibold'>{label} {Required && <span className='text-red-500'>*</span>}</label>;
  //   }
  //   return null;
  // };

  const dynamicClassName = error ? `${ClassName} border-danger rounded-lg` : `${ClassName} border-[#754eb477]`;

  return (
    <div className='w-full mb-3'>
      {/* {renderTopRow()} */}
      <label className='form-label font-semibold'>{label} {Required && <span className='text-red-500'>*</span>}</label>
      <Controller
        name={inputName}
        control={control}
        render={({ field }) => (
          <textarea
            {...field}
            rows={rows}
            placeholder={Placeholder}
            className={`form-textarea ${dynamicClassName}`}
          ></textarea>

        )}
      />
      {error && <span className='text-danger'>{error}</span>}

    </div>
  );
};

export default TextareaField;
