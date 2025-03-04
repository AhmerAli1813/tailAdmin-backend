import {  useState } from "react";
import { useNavigate } from "react-router-dom";
import { IAccountDto } from "../../types/auth.types";
import toast from "react-hot-toast";
import { PATH_USER } from "../../routes/paths";
import axiosInstance from "../../utils/axiosInstance";
import Spinner from "../../common/Loader/index";
import {  REGISTER_URL } from "../../utils/globalConfig";
import Button from "../../components/general/Button";
import * as Yup from 'yup';
import { useForm } from "react-hook-form";
import { yupResolver } from '@hookform/resolvers/yup';
import InputField from "../../components/general/InputField";
import TextareaField from "../../components/general/TextareaField";


const addUser = () => {
  const [loading, setLoading] = useState<boolean>(false);
  const navigate = useNavigate();


  const RegisterSchema = Yup.object().shape({
    userName: Yup.string().required("User name is required."),
    address: Yup.string(),
    email: Yup.string().email().required("email is required."),
    fullName: Yup.string().required("fullName is required."),
    fatherName: Yup.string(),
    phoneNumber: Yup.string()
      .matches(/^\d{10}$/, "Phone number must be exactly 10 digits")
      .required("Phone number is required"),
  });
  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<IAccountDto>({
    resolver: yupResolver(RegisterSchema),
    defaultValues: {
      userName: '',
      address: '',
      fullName: '',
      email: '',
      phoneNumber: '',
    },
  });

  const onSubmitForm = async (data: IAccountDto) => {
    try {
      setLoading(true);
      await axiosInstance.post(REGISTER_URL, data); // Use correct REGISTER_URL here
      reset();
      setLoading(false);
      navigate(PATH_USER.user)
      toast.success("Add Successfully")

    } catch (error) {
      console.log("ApI", error)
      setLoading(false);
      const err = error as { data: string, Status: number };
      const { Status } = err;
      if (Status == 401) {
        toast.error("Authorized Person")
      }
      else if (Status == 409) {
        toast.error("User name Already Exit")
      }
      else {
        toast.error(err.data)
      }

    }
  };

  if (loading) {
    return (
      <div className='w-full'>
        <Spinner />
      </div>
    );
  }
  return (
    <div>
      <form id="formAuthentication" className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark pb-5" onSubmit={handleSubmit(onSubmitForm)}>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 mt-5  p-6.5">
          {/* Full Name */}
          <div className="mb-3">
            <InputField control={control} label='Full Name' Placeholder="Enter Full Name" Required  inputName="fullName" // Ensure this corresponds to the name in the schema 
              error={errors?.fullName?.message} />
          </div>
          {/* User Name */}
          <div className="mb-3">
            <InputField control={control} label='Username' Placeholder="Enter UserName" Required  inputName='userName' error={errors?.userName?.message} />
          </div>
          {/* Email */}
          <div className="mb-3">
            <InputField control={control} label='Email' Required Placeholder="abc@xyz.com"  inputName='email' error={errors?.email?.message} />
          </div>
          {/* Phone Number */}
          <div className="mb-3">
            <InputField control={control} label='Phone Number' Placeholder="30000000000" Required  inputName='phoneNumber' error={errors?.phoneNumber?.message} />
          </div>
      
          {/* Designation */}
          <div className="mb-3">
            <InputField control={control} label='Designation' Placeholder="Enter a Designation"  inputName='designation' error={errors?.designation?.message} />
          </div>
        
          {/* Address */}
          <div className="mb-3">
            <TextareaField Placeholder="Enter a Address" control={control} label='Address'  inputName='address' error={errors?.address?.message} />
          </div>
        </div>
        {/* Submit Button */}
        <div className='grid grid-cols-2 gap-4 mt-12'>
          <Button
            label='Cancel'
            onClick={() => navigate(PATH_USER.user)}
            type='button'
            variant="secondary"
            ClassName="btn btn-dark me-1"
          />
          <Button
            label='Register'
            onClick={() => { }}
            loading={loading}
            type="submit" // This ensures the form submission is triggered 
            variant='primary'
          />
        </div>
      </form>
    </div>
  )
}

export default addUser;
