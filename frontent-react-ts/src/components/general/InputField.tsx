import { Control, Controller } from 'react-hook-form';

interface IProps {
  control?: Control<any, any>;
  label?: string;
  inputName: string;
  inputType?: string;
  error?: string;
  ClassName?: string;
  Placeholder?: string;
  Required?: boolean;
  Value?: any;
  Disabled?: boolean
}

const InputField = ({ control, label, inputName, Value, Disabled, inputType = 'text', Placeholder, error, ClassName, Required, ...rest }: IProps) => {
  ClassName = " w-full bg-white placeholder:text-slate-400 text-slate-700 text-sm border border-slate-200 rounded-md px-3 py-2 transition duration-300 ease focus:outline-none focus:border-slate-400 hover:border-slate-300 shadow-sm focus:shadow";
  // const renderTopRow = () => {
  //   if (error) {
  //     return <span className='text-danger'>{error}</span>;
  //   }
  //   if (label) {
  //     return <label className='  font-semibold '>{label} {Required && <span className='text-red-500'>*</span>}</label>;
  //   }
  //   return null;
  // };

  const dynamicClassName = error ? ClassName + ' border-danger rounded-lg ' : ClassName + ' border-[#754eb477] ';

  return (
    <div className='w-full mb-3'>
      {/* {renderTopRow()} */}
      <label className='  font-semibold '>{label} {Required && <span className='text-red-500'>*</span>}</label>

      <Controller
        name={inputName}
        control={control}
        defaultValue={Value}
        render={({ field }) => <input placeholder={Placeholder} disabled={Disabled}  {...rest}
          {...field} autoComplete='off' type={inputType} className={dynamicClassName} />}
      />
      {error && <span className='text-danger'>{error}</span>}

    </div>
  );
};

export default InputField;
