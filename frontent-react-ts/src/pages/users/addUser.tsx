import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {  IAccountDto, UserNameListDto } from "../../types/auth.types";
import toast from "react-hot-toast";
import { PATH_USER } from "../../routes/paths";
import axiosInstance from "../../utils/axiosInstance";
import Spinner from "../../components/general/Spinner";
import { Regional_RelationShip_user_list__URL, REGISTER_URL } from "../../utils/globalConfig";
import Button from "../../components/general/Button";
import * as Yup from 'yup';
import { useForm } from "react-hook-form";
import { yupResolver } from '@hookform/resolvers/yup';
import InputField from "../../components/general/InputField";
import { IResponseDto, ISelectDropDownDto } from "../../types/App.types";
import Select2 from "../../components/general/Select2";
import TextareaField from "../../components/general/TextareaField";

const addUser = () => {
  // const { user: loggedInUser } = useAuth();
  // const [role, setRole] = useState<string>();

  // const roles = allowedRolesForUpdateArray(loggedInUser)
  // const options = roles.map(role => ({
  //   value: role,
  //   label: role,
  // }));
  //const { userName } = useParams();
  //Define Properties 

  const [loading, setLoading] = useState<boolean>(false);
  const navigate = useNavigate();
  const [RegionalUserOptions, setRegionalUserOptions] = useState<{ label: string; value: string|number }[]>([]);
  const [RelationShipUserOptions, setRelationShipUserOptions] = useState<{ label: string; value: string|number }[]>([]);
  const [regionOptions, setRegionOptions] = useState<{ label: string; value: string|number }[]>([]);
  
  const GetUserNameList = async () => {

    try {
      setLoading(true);
      const response = await axiosInstance.get<IResponseDto>(Regional_RelationShip_user_list__URL);
      const Result = response.data.result as UserNameListDto;
      const regionalUser = Result.regionalUser as ISelectDropDownDto[];
      const RegionalUserOptions = [
        { value: "", label: "Select a Region Head" }, // Null option
        ...regionalUser.map((user) => ({
          value: user.id || "",
          label: user.name || "",
        })),
      ];
      setRegionalUserOptions(RegionalUserOptions);
      
      const RelationShipUser = Result.relationShipUser as ISelectDropDownDto[];
      const RelationShipUserOptions = [
        { value: "", label: "Select a line Manager" }, // Null option
        ...RelationShipUser.map((user) => ({
          value: user.id || "",
          label: user.name || "",
        })),
      ];
      setRelationShipUserOptions(RelationShipUserOptions);
      
      const RegionsD = Result.regions as ISelectDropDownDto[];
      const RegionsOptions = [
        { value: "", label: "Select a line Manager" }, // Null option
        ...RegionsD.map((item) => ({
          value: item.id || "",
          label: item.name || "",
        })),
      ];
      setRegionOptions(RegionsOptions)
    } catch (error) {
      console.log("Error", error)
    } finally {
      setLoading(false);
    }

  }

  const RegisterSchema = Yup.object().shape({
    userName: Yup.string().required("User name is required."),
    address: Yup.string(),
    email: Yup.string().email().required("email is required."),
    fullName: Yup.string().required("fullName is required."),
    regionId: Yup.number().required("Region is required."),
    fatherName: Yup.string(),
    phoneNumber: Yup.string()
    .matches(/^\d{10}$/, "Phone number must be exactly 10 digits")
    .required("Phone number is required"),
    dC_Code: Yup.string().required("DC Code is Required."),
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
  useEffect(() => {
    GetUserNameList();
  }, [])

  if (loading) {
    return (
      <div className='w-full'>
        <Spinner />
      </div>
    );
  }
  return (
    <div>
      <form id="formAuthentication" onSubmit={handleSubmit(onSubmitForm)}>
        <div className="row mt-5">
          {/* Full Name */}
          <div className="col-md-4 col-sm-6 col-12 mb-3">
            <InputField control={control} label='Full Name' Placeholder="Enter Full Name" Required ClassName='form-control' inputName="fullName" // Ensure this corresponds to the name in the schema 
              error={errors?.fullName?.message} />

          </div>
          {/* User Name */}
          <div className="col-md-4 col-sm-6 col-12 mb-3">
            <InputField control={control} label='Username'Placeholder="Enter UserName" Required ClassName='form-control' inputName='userName' error={errors?.userName?.message} />

          </div>
          {/* Email */}
          <div className="col-md-4 col-sm-6 col-12 mb-3">
            <InputField control={control} label='Email' Required Placeholder="abc@xyz.com" ClassName='form-control' inputName='email' error={errors?.email?.message} />
          </div>
          {/* Father's Name */}
          {/* <div className="col-md-4 col-sm-6 col-12 mb-3">
            <InputField control={control} label='Father Name' ClassName='form-control' inputName='fatherName' error={errors?.fatherName?.message} />
          </div> */}
          {/* Phone Number */}
          <div className="col-md-4 col-sm-6 col-12 mb-3">
            <InputField control={control} label='Phone Number' Placeholder="30000000000" Required ClassName='form-control' inputName='phoneNumber' error={errors?.phoneNumber?.message} />
          </div>


          {/* Dc Code */}
          <div className="col-md-4 col-sm-6 col-12 mb-3">
            <InputField control={control} label='DC Code' Placeholder="Enter a DC code" Required ClassName='form-control' inputName='dC_Code' error={errors?.dC_Code?.message} />
          </div>

          {/* Designation */}
          <div className="col-md-4 col-sm-6 col-12 mb-3">
            <InputField control={control} label='Designation' Placeholder="Enter a Designation" ClassName='form-control' inputName='designation' error={errors?.designation?.message} />
          </div>
          {/* Region Head */}
          <div className="col-md-4 col-sm-6 col-12 mb-3">
            <Select2
              label="Region Head"

              control={control}
              placeholder="Select a Region Head"
              inputName="regionHeadId"
              error={errors.regionHeadId?.message}
              options={RegionalUserOptions}
            />
          </div>
          {/* Region Head */}
          <div className="col-md-4 col-sm-6 col-12 mb-3">
            <Select2
              label="Line Manager"
              
              control={control}
              placeholder="Select a line Manager"
              inputName="lineManagerId"
              error={errors.lineManagerId?.message}
              options={RelationShipUserOptions}
            />
          </div>
          {/* Region Head */}
          <div className="col-md-4 col-sm-6 col-12 mb-3">
            <Select2
              label="Region"
              Required
              control={control}
              placeholder="Select a region"
              inputName="regionId"
              error={errors.regionId?.message}
              options={regionOptions}
            />
          </div>

          {/* Address */}
          <div className="col-md-4 col-sm-6 col-12 mb-3">
            <TextareaField Placeholder="Enter a Address" control={control} label='Address' ClassName='form-control' inputName='address' error={errors?.address?.message} />
          </div>
          {/* Role */}

        </div>
        {/* Submit Button */}
        <div className='grid grid-cols-2 gap-4 mt-12'>
          <Button
            label='Cancel'
            onClick={() => navigate(PATH_USER.user)}
            type='button'
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

export default addUser
