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

const InputField = ({ control, label, inputName, Value, Disabled, inputType = 'text', Placeholder, error, ClassName="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:focus:border-primary", Required, ...rest }: IProps) => {
  // const renderTopRow = () => {
  //   if (error) {
  //     return <span className='text-danger'>{error}</span>;
  //   }
  //   if (label) {
  //     return <label className='  font-semibold '>{label} {Required && <span className='text-red-500'>*</span>}</label>;
  //   }
  //   return null;
  // };

  const dynamicClassName = error ? ClassName + ' border-red rounded-lg ' : ClassName + ' border-[#754eb477] ';

  return (
    <div className='w-full mb-3'>
      {/* {renderTopRow()} */}
      {label &&   <label className='mb-3 block text-black dark:text-white'>{label} {Required && <span className='text-red-500'>*</span>}</label>}

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
